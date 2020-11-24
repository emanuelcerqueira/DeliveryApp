using System; 

namespace DeliveryApp.Util
{

    //https://www.geeksforgeeks.org/program-distance-two-points-earth/
    public static class Haversine 
    { 
        public static double ToRadians( 
            double angleIn10thofaDegree) 
        { 
            // Angle in 10th 
            // of a degree 
            return (angleIn10thofaDegree * 
                        Math.PI) / 180; 
        } 
        public static double Distance(double lat1, 
                            double lat2, 
                            double lon1, 
                            double lon2) 
        { 

            // The math module contains 
            // a function named toRadians 
            // which converts from degrees 
            // to radians. 
            lon1 = ToRadians(lon1); 
            lon2 = ToRadians(lon2); 
            lat1 = ToRadians(lat1); 
            lat2 = ToRadians(lat2); 

            // Haversine formula 
            double dlon = lon2 - lon1; 
            double dlat = lat2 - lat1; 
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + 
                    Math.Cos(lat1) * Math.Cos(lat2) * 
                    Math.Pow(Math.Sin(dlon / 2),2); 
                
            double c = 2 * Math.Asin(Math.Sqrt(a)); 

            // Radius of earth in 
            // kilometers. Use 3956 
            // for miles 
            double r = 6371; 

            // calculate the result 
            return Math.Round((c * r), 2); 
        } 

    } 


}
