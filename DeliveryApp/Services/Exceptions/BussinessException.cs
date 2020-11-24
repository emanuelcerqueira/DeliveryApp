namespace DeliveryApp.Services.Exceptions
{
    [System.Serializable]
    public class BussinessException : System.Exception
    {
        public BussinessException() { }
        public BussinessException(string message) : base(message) { }
        public BussinessException(string message, System.Exception inner) : base(message, inner) { }
        protected BussinessException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}