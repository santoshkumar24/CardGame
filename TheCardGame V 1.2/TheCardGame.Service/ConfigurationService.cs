using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TheCardGame.ServiceInterface;

namespace TheCardGame.Service
{
    public class ConfigurationService:IConfigurationService
    {
        private readonly IConfiguration _configuration;
        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string[] ReadDelimitedValue(string key, string delimiter)
        {
            IConfigurationSection section = _configuration.GetSection(key);
            string value = section.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] values = value.Split(delimiter);
                return values;
            }
            else
            {
                return null;
            }
        }
    }
}
