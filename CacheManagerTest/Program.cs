using System;
using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

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

                var cache = CacheFactory.FromConfiguration<object>(cacheConfiguration);
                cache.Add("keyA", "valueA");
                cache.Put("keyB", 23);
                cache.Update("keyB", v => 42);

                Console.WriteLine("KeyA is " + cache.Get("keyA"));      // should be valueA
                Console.WriteLine("KeyB is " + cache.Get("keyB"));      // should be 42
                cache.Remove("keyA");

                Console.WriteLine("KeyA removed? " + (cache.Get("keyA") == null).ToString());

                Console.WriteLine("We are done...");

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
