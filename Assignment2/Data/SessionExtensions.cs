﻿/*Borrow code from Microsoft official tutorial ASP.NET Core 2.0 Fundamentals -> Session and app state
 * See the following URL for more information:
 * https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.0&tabs=aspnetcore2x
*/
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Assignment2.Data
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }
}
