using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace OrderDispatcher.CatalogService.Dal
{
    public static class Config
    {
        public static string GetConnectionString()
        {
            var path = Directory.GetCurrentDirectory();
            //For example purpose only, try to move this to a right place like configuration manager class
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                               .SetBasePath(path)
                                               .AddJsonFile("appsettings.json")
                                               .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}