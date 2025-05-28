using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace RedisExchangeApi.Web.Helpers
{
    public class ControllerHelper
    {
        public static List<string> GetControllerNames(Assembly assembly)
        {
            var controllerTypes = assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)
                               && !type.IsAbstract)
                .ToList();

            return controllerTypes.Select(c => c.Name.Replace("Controller", "")).ToList();
        }
    }
}
