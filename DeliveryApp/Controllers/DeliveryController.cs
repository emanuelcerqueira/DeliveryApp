using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeliveryApp.Controllers.Model;
using DeliveryApp.Service;
using DeliveryApp.Models;
using System.Threading.Tasks;
using DeliveryApp.Services.Models;
using DeliveryApp.Controller.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DeliveryApp.Controller
{
    [Route("deliveries")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        /// <summary>
        /// Simulates a delivery
        /// </summary>
        /// <param name="queryParams">Query Params</param>
        /// <returns>Returns a simulation of a delivery</returns>
        /// <response code="200">Returns a simulation of a delivery</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="400">If One or more validation errors occur</response>
        [Produces("application/json")]
        [HttpGet]
        [Route("simulate")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<DeliveryInfoDTO>> SimulateDelivery([FromQuery] DeliveryParams queryParams)
        {
            if (ModelState.IsValid)
            {
                var initialLocation = new Location(queryParams.InitialLat, queryParams.InitialLon);
                var deliveryLocation = new Location(queryParams.DeliveryLat, queryParams.DeliveryLon);
                
                var deliveryInfo = await _deliveryService.SimulateDelivery(initialLocation, deliveryLocation);
                return Ok(deliveryInfo);
            }
            
            return BadRequest();
        }

        /// <summary>
        /// Requests a delivery
        /// </summary>
        /// <param name="deliveryRequest">Delivery request</param>
        /// <returns>Returns a newly created delivery</returns>
        /// <response code="201">Returns a newly created delivery</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="400">If One or more validation errors occur</response>
        [Produces("application/json")]
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<DeliveryResponse>> RequestDelivery([FromBody] DeliveryRequest deliveryRequest)
        {
            if (ModelState.IsValid)
            {
                var delivery = await _deliveryService.RequestDelivery(deliveryRequest);
                
                return CreatedAtRoute(
                    "GetDeliveryById",
                    new { id = delivery.Id },
                    new DeliveryResponse(delivery));

            }
            return BadRequest();

        }

        /// <summary>
        /// List all deliveries based upon deliveryman's informed location and radius (km)
        /// </summary>
        /// <param name="queryParams">Latitude, Longitude and Radius (km)</param>
        /// <returns>Returns a list of delivery requests</returns>
        /// <response code="200">Returns a list of nearby delivery requests</response>
        /// <response code="400">If One or more validation errors occur</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        [Produces("application/json")]
        [HttpGet]
        [Route("query")]
        [Authorize(Roles = "Deliveryman")]
        public ActionResult<List<DeliveryResponseDeliverymanQuery>> FindRequestedDeliveriesByLocationAndRadius([FromQuery] LocationAndRadiusQuery queryParams)
        {
            if (ModelState.IsValid)
            {
                var deliverymanCurrentLocation = new Location(queryParams.Lat, queryParams.Lon);
                var deliveries = _deliveryService
                    .FindRequestedDeliveriesByLocationAndRadius(deliverymanCurrentLocation, queryParams.Radius);
                var response = deliveries.Select(delivery => new DeliveryResponseDeliverymanQuery(delivery, deliverymanCurrentLocation));
                return Ok(response);
            }
            return BadRequest();
        }


        /// <summary>
        /// Gets a delivery by Id with route info
        /// </summary>
        /// <param name="id">Delivery's Id</param>
        /// <returns>Returns a delivery</returns>
        /// <response code="200">Returns a delivery</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="400">If One or more validation errors occur</response>
        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetDeliveryById")]
        [Authorize]
        public async Task<ActionResult<DeliveryResponse>> GetDeliveryById([FromRoute] Guid id)
        {
            var delivery = await _deliveryService.FindDeliveryByIdWithRouteInfo(id);
            return Ok(new DeliveryResponse(delivery));
        }

        /// <summary>
        /// A customer can update a delivery that it status is requested
        /// </summary>
        /// <param name="id">Delivery's Id</param>
        /// <param name="deliveryRequest">Delivery's payload</param>
        /// <returns>Returns a delivery</returns>
        /// <response code="200">Returns a delivery</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="400">If One or more validation errors occur</response>
        [Produces("application/json")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<DeliveryResponse>> UpdateDelivery([FromRoute] Guid id, [FromBody] DeliveryRequest deliveryRequest)
        {
            if (ModelState.IsValid)
            {
                var updateDelivery = await _deliveryService.UpdateDelivery(id, deliveryRequest);
                return Ok(new DeliveryResponse(updateDelivery));
            }
            return BadRequest();
        }

        /// <summary>
        /// A customer can deletes a delivery that it status is requested or canceled
        /// </summary>
        /// <param name="id">Delivery's id</param>
        /// <response code="204">The delivery was deleted successfully</response>
        /// <response code="400">If One or more validation errors occur</response>
        /// <response code="404">If One or more validation errors occur</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> DeleteDeliveryById([FromRoute] Guid id)
        {
            await _deliveryService.DeleteDeliveryById(id);
            return NoContent();
        }

        /// <summary>
        /// A deliveryman can accepts a delivery
        /// </summary>
        /// <param name="id">Id of the Delivery</param>
        /// <returns>Returns a delivery</returns>
        /// <response code="200">Returns a delivery with the associated deliveryman</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="404">Delivery not found</response>
        [Produces("application/json")]
        [HttpPost("accept/{id}")]
        [Authorize(Roles = "Deliveryman")]
        public async Task<ActionResult<DeliveryResponse>> AcceptDeliveryRequest([FromRoute] Guid id)
        {
            var acceptedDelivery = await _deliveryService.AcceptDeliveryRequestById(id);
            return Ok( new DeliveryResponse(acceptedDelivery));
        }

        /// <summary>
        /// Both customer and deliveryman can cancel a delivery that it status is accepted, and customer can cancel a requested delivery
        /// </summary>
        /// <param name="id">Id of the Delivery</param>
        /// <returns>Returns a delivery</returns>
        /// <response code="200">Returns the canceled delivery </response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="404">Delivery not found</response>
        [Produces("application/json")]
        [HttpPost("cancel/{id}")]
        [Authorize]
        public async Task<ActionResult<DeliveryResponse>> CancelDelivery([FromRoute] Guid id)
        {
            var canceledDelivery = await _deliveryService.CancelDelivery(id);
            return Ok( new DeliveryResponse(canceledDelivery));
        }

        /// <summary>
        /// A deliveryman informs that the delivery is on carriage
        /// </summary>
        /// <param name="id">Id of the Delivery</param>
        /// <returns>Returns a delivery</returns>
        /// <response code="200">Returns a delivery with the associated deliveryman</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="404">Delivery not found</response>
        [Produces("application/json")]
        [HttpPost("on-carriage/{id}")]
        [Authorize(Roles = "Deliveryman")]
        public async Task<ActionResult<DeliveryResponse>> OnCarriageDelivery([FromRoute] Guid id)
        {
            var OnCarriageDelivery = await _deliveryService.OnCarriageDelivery(id);
            return Ok(new DeliveryResponse(OnCarriageDelivery));
        }

        /// <summary>
        /// A deliveryman informs that the delivery was delivered with success
        /// </summary>
        /// <param name="id">Id of the Delivery</param>
        /// <returns>Returns a delivery</returns>
        /// <response code="200">Returns a delivery with the associated deliveryman</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="403">If user is forbidden to access this resource</response>
        /// <response code="404">Delivery not found</response>
        [Produces("application/json")]
        [HttpPost("deliver/{id}")]
        [Authorize(Roles = "Deliveryman")]
        public async Task<ActionResult<DeliveryResponse>> Deliver([FromRoute] Guid id)
        {
            var OnCarriageDelivery = await _deliveryService.DeliverDelivery(id);
            return Ok(new DeliveryResponse(OnCarriageDelivery));
        }

        /// <summary>
        /// List all deliveries associated if the logged in user
        /// </summary>
        /// <returns>Returns a list of delivery</returns>
        /// <response code="200">Returns a list of delivery</response>
        /// <response code="401">If user is not logged in</response>
        [Produces("application/json")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserDeliveriesResponse>>> FindCurrentUserDeliveries()
        {
            var deliveries = await _deliveryService.FindCurrentUserDeliveries();
            var customerDeliveriesResponse = deliveries
                .Select(delivery => new UserDeliveriesResponse(delivery)).ToList();

            return Ok(customerDeliveriesResponse);
        }

    }
}