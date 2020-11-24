using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeliveryApp.Models
{
    [JsonConverter(typeof(StringEnumConverter))]    
    public enum DeliveryStatus
    {
        [EnumMember(Value = "Requested")]
        Requested,
        
        [EnumMember(Value = "Canceled")]
        Canceled,

        [EnumMember(Value = "Accepted")]
        Accepted,

        [EnumMember(Value = "On Carriage")]
        OnCarriage,

        [EnumMember(Value = "Delivered")]
        Delivered

    }
}