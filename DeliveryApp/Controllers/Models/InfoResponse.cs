namespace DeliveryApp.Controllers.Models
{
    public class InfoResponse
    {
        public int NumberOfCustomers { get; set; }
        public int NumberOfDeliverymen { get; set; }
        public int NumberOfSuccessfulDeliveries { get; set; }

        public InfoResponse(int numberOfCustomers, int numberOfDeliverymen, int numberOfSuccessfulDeliveries)
        {
            NumberOfCustomers = numberOfCustomers;
            NumberOfDeliverymen = numberOfDeliverymen;
            NumberOfSuccessfulDeliveries = numberOfSuccessfulDeliveries;
        }
    }
}