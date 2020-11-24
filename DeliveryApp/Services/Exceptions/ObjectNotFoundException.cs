using System;
using System.Runtime.Serialization;

namespace DeliveryApp.Service.Exception
{
    [Serializable]
    internal class ObjectNotFoundException : System.Exception
    {
        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message) : base(message)
        {
        }

        public ObjectNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}