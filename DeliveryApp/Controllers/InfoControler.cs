using System.Threading.Tasks;
using DeliveryApp.Controllers.Models;
using DeliveryApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controller
{
    [Route("infos")]
    [ApiController]
    public class InfoController : ControllerBase
    {

        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IUserRepository _userRepository;
        
        public InfoController(IDeliveryRepository deliveryRepository, IUserRepository userRepository)
        {
            _deliveryRepository = deliveryRepository;
            _userRepository = userRepository;
        }
        
        /// <summary>
        /// Gets the number of deliverymen, customers and successful deliveries
        /// </summary>
        /// <returns>Gets the number of deliverymen, customers and successful deliveries</returns>
        /// <response code="200">Returns statics about the app</response>
        [HttpGet]
        public async Task<ActionResult<InfoResponse>> GetInfos()
        {
            int successfulDeliveries = await _deliveryRepository.GetNumberOfSuccessfulDeliveries();
            int customers = await _userRepository.GetNumberOfCustomers();
            int deliverymen = await _userRepository.GetNumberOfDeliverymen();

            return Ok(new InfoResponse(customers, deliverymen, successfulDeliveries));
        }
    }
}