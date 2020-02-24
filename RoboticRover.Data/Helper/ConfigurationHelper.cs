using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticRover.Data.Helper
{
    public static class ConfigurationHelper
    {
        private static string GetParameter(string key, string defaultValue = "")
        {
            var returnValue = defaultValue;

            try
            {
                if (!string.IsNullOrEmpty(key) && ConfigurationManager.AppSettings[key] != null)
                    return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {

                throw;
            }
            

            return returnValue;

           
        }

        public static string DataPath
        {
            get {
                return GetParameter("jsonDataPath");
            }
        }

    }
}
