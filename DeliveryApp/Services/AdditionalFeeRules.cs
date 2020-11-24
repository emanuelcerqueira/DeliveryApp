using System;
using DeliveryApp.Services.Exceptions;

namespace DeliveryApp.Service
{
    public interface IAdditionalFeeRules
    {
        string WeatherCondition { get;}
        decimal fee {get;}
    }

    // see https://openweathermap.org/weather-conditions icon codes

    public class ClearSkyFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "01";

        public decimal fee => 0;

    }

    public class FewCloudsFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "02";

        public decimal fee => 0;

    }

    public class ScatteredCloudsSkyFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "03";

        public decimal fee => 0;

    }

    public class BrokenCloudsSkyFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "04";

        public decimal fee => 4;

    }

    public class ShowerRainFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "09";

        public decimal fee => 5;

    }

    public class RainFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "10";

        public decimal fee => 10;

    }

   public class ThunderstormFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "11";

        public decimal fee => throw new BussinessException("It's not possible to perform delivery in a thunderstorm");

    }

    public class SnowFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "13";

        public decimal fee => 12;

    }

    public class MistFee : IAdditionalFeeRules
    {
        public string WeatherCondition => "50";

        public decimal fee => 8;

    }


}