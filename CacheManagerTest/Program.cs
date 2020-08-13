using System;
using System.Collections.Generic;
using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace CacheManagerTest
{
    class Program
    {
        private static string cacheName1 = "test1";
        private static string cacheName2 = "test2";

        private static readonly Dictionary<string, ICacheManager<object>> dictionary =
            new Dictionary<string, ICacheManager<object>>();

        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile("cache.json");
                var configurationRoot = builder.Build();
                var cacheConfiguration = configurationRoot.GetCacheConfiguration();
                Console.WriteLine($"MaxRetries = {cacheConfiguration.MaxRetries}");

                Test1(cacheConfiguration);
                Test2(cacheConfiguration);
                Test3();
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

        static void Test1(ICacheManagerConfiguration configuration)
        {
            var cache = CacheFactory.FromConfiguration<object>(cacheName1, configuration);
            dictionary.Add(cacheName1, cache);
            cache.Add("keyA", "valueA");
            cache.Put("keyB", 23);
            cache.Update("keyB", v => 42);

            Console.WriteLine("KeyA is " + cache.Get("keyA")); // should be valueA
            Console.WriteLine("KeyB is " + cache.Get("keyB")); // should be 42
            cache.Remove("keyA");

            Console.WriteLine("KeyA removed? " + (cache.Get("keyA") == null).ToString());

            Console.WriteLine("We are done Test1 ...");
        }

        static void Test2(ICacheManagerConfiguration configuration)
        {
            var cache = CacheFactory.FromConfiguration<object>(cacheName2, configuration);
            dictionary.Add(cacheName2, cache);
            cache.Add("keyC", "valueC");
            cache.Put("keyD", 23);
            cache.Update("keyC", v => 24);

            Console.WriteLine("KeyC is " + cache.Get("keyC")); // should be valueC
            Console.WriteLine("KeyD is " + cache.Get("keyD")); // should be 24
            cache.Remove("keyC");

            Console.WriteLine("KeyC removed? " + (cache.Get("keyC") == null).ToString());

            Console.WriteLine("We are done Test2 ...");
        }

        static void Test3()
        {
            var cache1 = dictionary[cacheName1]; 
            var valueB = cache1.Get("keyB");
            Console.WriteLine($"KeyB is {valueB}"); // should be 42

            var cache2 = dictionary[cacheName2];
            var valueD = cache2.Get("keyD");
            Console.WriteLine($"KeyD is {valueD}"); // should be 24
        }
    }
}
