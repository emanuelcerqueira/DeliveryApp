

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeliveryApp.Models
{
    [JsonConverter(typeof(StringEnumConverter))]    
    public enum Role
    {
        [EnumMember(Value = "Customer")]
        Customer,
        [EnumMember(Value = "Deliveryman")]
        Deliveryman
    }
}