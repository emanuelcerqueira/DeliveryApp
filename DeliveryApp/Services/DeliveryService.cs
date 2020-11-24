using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeliveryApp.Controller.Models;
using DeliveryApp.Models;
using DeliveryApp.Repository;
using DeliveryApp.Service.Exception;
using DeliveryApp.Services;
using DeliveryApp.Services.Exceptions;
using DeliveryApp.Services.Models;
using DeliveryApp.Util;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Service
{
    public interface IDeliveryService
    {
        Task<DeliveryInfoDTO> SimulateDelivery(Location initialLocation, Location deliveryLocation);
        Task<Delivery> RequestDelivery(DeliveryRequest deliveryRequest);

        Task<List<Delivery>> FindCurrentUserDeliveries();
        List<Delivery> FindRequestedDeliveriesByLocationAndRadius(Location location, int radius);
        Task<Delivery> AcceptDeliveryRequestById(Guid id);
        Task<Delivery> OnCarriageDelivery(Guid id);
        Task<Delivery> CancelDelivery(Guid id);
        Task<Delivery> DeliverDelivery(Guid id);

        Task<Delivery> FindDeliveryById(Guid id);
        Task DeleteDeliveryById(Guid id);
        Task<Delivery> FindDeliveryByIdWithRouteInfo(Guid id);
        Task<Delivery> UpdateDelivery(Guid id, DeliveryRequest deliveryRequest);
    }

    public class DeliveryService : IDeliveryService
    {
        private readonly IOpenRouteService _openRouteService;
        private readonly IOpenWatherMapService _openWatherMapService;

        private readonly IEnumerable<IAdditionalFeeRules> _feeRules;
        private readonly ISecurityUtil _securityUtil;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOpenCageDataService _openCageDataService;

        private RoutingInfoDTO routingInfo;


        public DeliveryService(IOpenRouteService openRouteService,
                               IOpenWatherMapService openWatherMapService,
                               IEnumerable<IAdditionalFeeRules> feeRules,
                               ISecurityUtil securityUtil,
                               IDeliveryRepository repository,
                               IOpenCageDataService openCageDataService)
        {
            _openRouteService = openRouteService;
            _openWatherMapService = openWatherMapService;
            _feeRules = feeRules;
            _securityUtil = securityUtil;
            _deliveryRepository = repository;
            _openCageDataService = openCageDataService;
        }

        public async Task<DeliveryInfoDTO> SimulateDelivery(Location initialLocation, Location deliveryLocation)
        {
            decimal totalPrice = await GetTotalPriceByInitialAndDeliveryLocation(initialLocation, deliveryLocation);

            return new DeliveryInfoDTO(routingInfo, totalPrice);
        }

        private async Task<decimal> GetTotalPriceByInitialAndDeliveryLocation(Location initialLocation, Location deliveryLocation)
        {
            var routePrice = await GetRoutePriceByInitialAndDeliveryLocationAsync(initialLocation, deliveryLocation);
            var additionalFee = await GetAdditionalFeePriceByInitialLocationAsync(initialLocation);
            var totalPrice = routePrice + additionalFee;
            return totalPrice;
        }

        private async Task<decimal> GetRoutePriceByInitialAndDeliveryLocationAsync(Location initialLocation, Location deliveryLocation)
        {
            var routeResponse = await _openRouteService.findRoutingInfoAsync(initialLocation, deliveryLocation);
            routingInfo = new RoutingInfoDTO(routeResponse);

            return (routingInfo.Distance / 500) * Constants.PRICE_PER_500_METERS;
        }

        private async Task<decimal> GetAdditionalFeePriceByInitialLocationAsync(Location initialLocation)
        {
            var weatherResponse = await _openWatherMapService.FindWeatherInfoAsync(initialLocation);
            List<string> weathersOfTheInitialLocation = weatherResponse.Weather.Select(weather => weather.Icon.Substring(0, 2)).ToList();

            return _feeRules
                .Where(feeRule => weathersOfTheInitialLocation.Contains(feeRule.WeatherCondition))
                .Select(feeRule => feeRule.fee)
                .Aggregate((a, b) => a + b);
        }

        public async Task<Delivery> RequestDelivery(DeliveryRequest deliveryRequest)
        {
            decimal totalPrice = await GetTotalPriceByInitialAndDeliveryLocation(
                deliveryRequest.InitialLocation,
                deliveryRequest.DeliveryLocation);
                
            var currentUser = _securityUtil.CurrentUser;

            await SetLocationNameAsync(deliveryRequest.InitialLocation);
            await SetLocationNameAsync(deliveryRequest.DeliveryLocation);

            var newDelivery = new Delivery(currentUser,
                                           deliveryRequest.Object,
                                           deliveryRequest.InitialLocation,
                                           deliveryRequest.DeliveryLocation,
                                           totalPrice,
                                           totalPrice * Constants.DELIVERYMAN_EARNINGS_PERCENT,
                                           routingInfo.Distance,
                                           deliveryRequest.Notes, 
                                           routingInfo.Route);
            
            return await _deliveryRepository.Save(newDelivery);
        }

        private async Task SetLocationNameAsync(Location location)
        {
            var initalLocationResponse = await _openCageDataService.ReverseGeocodingAsync(location);  
            location.Name = initalLocationResponse.Results[0].Formatted;
        }

        public async Task<List<Delivery>> FindCurrentUserDeliveries()
        {
            var currentUser = _securityUtil.CurrentUser;

            if (currentUser.IsCustomer())
                return await _deliveryRepository.FindDeliveriesByCustomer(currentUser);

            if (currentUser.IsDeliveryman())
                return await _deliveryRepository.FindDeliveriesByDeliveryman(currentUser);

            return new List<Delivery>();        
        }

        public List<Delivery> FindRequestedDeliveriesByLocationAndRadius(Location location, int radius)
        {
            return _deliveryRepository.FindRequestedDeliveriesByLocationAndRadius(location, radius);
        }

        public async Task<Delivery> AcceptDeliveryRequestById(Guid id)
        {
            var delivery = await FindDeliveryById(id);
            var currentDeliveryman = _securityUtil.CurrentUser;
            delivery.Accept(currentDeliveryman);
            return await _deliveryRepository.Update(delivery);
        }

        public async Task<Delivery> OnCarriageDelivery(Guid id)
        {
            var delivery = await FindDeliveryById(id);
            var currentDeliveryman = _securityUtil.CurrentUser;
            delivery.OnCarriage(currentDeliveryman);
            return await _deliveryRepository.Update(delivery);
        }

        public async Task<Delivery> CancelDelivery(Guid id)
        {
            var delivery = await FindDeliveryById(id);
            var currentUser = _securityUtil.CurrentUser;
            delivery.Cancel(currentUser);
            return await _deliveryRepository.Update(delivery);
        }

        public async Task<Delivery> DeliverDelivery(Guid id)
        {
            var delivery = await FindDeliveryById(id);
            var currentDeliveryman = _securityUtil.CurrentUser;
            delivery.Deliver(currentDeliveryman);
            return await _deliveryRepository.Update(delivery);
        }

        public async Task DeleteDeliveryById(Guid id)
        {
            var delivery = await FindDeliveryById(id);

            if (!delivery.CanDelete())
                throw new BussinessException($"It's not possible to delete a delivery that it status is '{delivery.Status.ToString().ToLower()}'");

            await _deliveryRepository.Remove(delivery);
        }

        public async Task<Delivery> FindDeliveryById(Guid id)
        {
            var delivery = await _deliveryRepository.FindDeliveryById(id);

            if (delivery == null)
            {
                throw new ObjectNotFoundException("Delivery not found");
            }

            return delivery;
        }

        public async Task<Delivery> FindDeliveryByIdWithRouteInfo(Guid id)
        {
            var delivery = await FindDeliveryById(id);

            var response = await _openRouteService
                .findRoutingInfoAsync(delivery.InitialLocation, delivery.DeliveryLocation);

            var info = new RoutingInfoDTO(response);

            delivery.Route = info.Route;

            return delivery;
        }

        public async Task<Delivery> UpdateDelivery(Guid id, DeliveryRequest deliveryRequest)
        {
            var deliveryToUpdate = await FindDeliveryById(id);

            if (!deliveryToUpdate.IsStatusRequested())
                throw new BussinessException($"Is not possible to update a delivery that it status is '{deliveryToUpdate.Status.ToString().ToLower()}'");
            
            if (!deliveryToUpdate.isDeliveryAssociatedWith(_securityUtil.CurrentUser))
                throw new BussinessException("It's not possible update delivery that is not associated with you");

            decimal totalPrice = await GetTotalPriceByInitialAndDeliveryLocation(
                deliveryRequest.InitialLocation,
                deliveryRequest.DeliveryLocation);
                
            await SetLocationNameAsync(deliveryRequest.InitialLocation);
            await SetLocationNameAsync(deliveryRequest.DeliveryLocation);

            deliveryToUpdate.RequestDate = DateTime.Now;
            deliveryToUpdate.TransportedObject = deliveryRequest.Object;
            deliveryToUpdate.InitialLocation = deliveryRequest.InitialLocation;
            deliveryToUpdate.DeliveryLocation = deliveryRequest.DeliveryLocation;
            deliveryToUpdate.Price = totalPrice;
            deliveryToUpdate.DeliverymanEarnings = totalPrice * Constants.DELIVERYMAN_EARNINGS_PERCENT;
            deliveryToUpdate.Distance = routingInfo.Distance;
            deliveryToUpdate.Notes = deliveryRequest.Notes;
            deliveryToUpdate.Route = routingInfo.Route;

            return await _deliveryRepository.Update(deliveryToUpdate);
        }
    }
}