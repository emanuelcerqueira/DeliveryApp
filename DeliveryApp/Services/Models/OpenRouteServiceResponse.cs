using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeliveryApp.Services.Models
{
    // classes generated using https://app.quicktype.io/
    public partial class OpenRouteServiceResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }

        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }

        [JsonProperty("summary")]
        public Summary Summary { get; set; }

        [JsonProperty("way_points")]
        public List<long> WayPoints { get; set; }
    }

    public partial class Segment
    {
        [JsonProperty("distance")]
        public long Distance { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }
    }

    public partial class Step
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("instruction")]
        public string Instruction { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("way_points")]
        public List<long> WayPoints { get; set; }
    }

    public partial class Summary
    {
        [JsonProperty("distance")]
        public long Distance { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("attribution")]
        public string Attribution { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("engine")]
        public Engine Engine { get; set; }
    }

    public partial class Engine
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("build_date")]
        public DateTimeOffset BuildDate { get; set; }

        [JsonProperty("graph_date")]
        public DateTimeOffset GraphDate { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [JsonProperty("profile")]
        public string Profile { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }

}