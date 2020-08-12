using System;
using Microsoft.Extensions.Configuration;

namespace CacheManagerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile("cache.json");
                var configurationRoot = builder.Build();
                var cacheConfiguration = configurationRoot.GetCacheConfiguration();
                Console.WriteLine($"MaxRetries = {cacheConfiguration.MaxRetries}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
