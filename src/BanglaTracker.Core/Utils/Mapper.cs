using System.Reflection;

namespace BanglaTracker.Core.Utils
{    
    public static class Mapper<TSource, TDestination>
        where TSource : new()
        where TDestination : new()
    {
        public static TDestination Map(TSource source)
        {
            if (source == null) return default;

            TDestination destination = new TDestination();
            MapProperties(source, destination);
            return destination;
        }

        private static void MapProperties(object source, object destination)
        {
            var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinationProperties = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProp in sourceProperties)
            {
                var destProp = destinationProperties.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                if (destProp != null && destProp.CanWrite)
                {
                    var value = sourceProp.GetValue(source, null);
                    destProp.SetValue(destination, value, null);
                }
            }
        }
    }

}
