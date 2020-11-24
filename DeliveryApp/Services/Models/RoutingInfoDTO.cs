using System.Collections.Generic;
using DeliveryApp.Models;
using System.Linq;

namespace DeliveryApp.Services.Models
{
    public class RoutingInfoDTO
    {
        public long Distance { get; set;}
        public LocationDTO InitialLocation { get; set;} 
        public LocationDTO EndLocation { get; set;}
        public List<LocationDTO> Route { get; set;}
        public RoutingInfoDTO(OpenRouteServiceResponse response)
        {
            this.Distance = response.Features[0].Properties.Summary.Distance;
            this.InitialLocation = new LocationDTO(response.Metadata.Query.Coordinates[0][1],response.Metadata.Query.Coordinates[0][0]);
            this.EndLocation = new LocationDTO(response.Metadata.Query.Coordinates[1][1],response.Metadata.Query.Coordinates[1][0]);
            
            this.Route = response.Features[0].Geometry.Coordinates.Select(coordinate => {
                var lat = coordinate[1];
                var lon = coordinate[0];

                return new LocationDTO(lat, lon);
            }).ToList();
        }

    }
}