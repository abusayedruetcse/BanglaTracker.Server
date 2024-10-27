namespace BanglaTracker.Core.Utils
{
    public class LocationDistanceCalculator
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double R = unit == 'M' ? 3958.8 : 6371.0; // Radius of Earth in miles or kilometers

            // Convert degrees to radians
            double lat1Rad = DegreesToRadians(lat1);
            double lon1Rad = DegreesToRadians(lon1);
            double lat2Rad = DegreesToRadians(lat2);
            double lon2Rad = DegreesToRadians(lon2);

            // Haversine formula
            double dlat = lat2Rad - lat1Rad;
            double dlon = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            return R * c; // Distance in the specified unit (default is kilometers)
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }

    public class TestDistance
    {
        double lat1 = 40.748817; // Latitude for location 1
        double lon1 = -73.985428; // Longitude for location 1
        double lat2 = 34.052235; // Latitude for location 2
        double lon2 = -118.243683; // Longitude for location 2

        //double distanceKm = new LocationDistanceCalculator().CalculateDistance(lat1, lon1, lat2, lon2, 'K');        
    }
}
