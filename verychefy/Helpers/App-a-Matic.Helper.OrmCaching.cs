using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Web;
using App_a_matic;
using App_a_matic.Orm;
namespace App_a_matic.Helper {
   public static class OrmCaching {

        /// <summary>
        /// Returns the CacheStatus of the Cache by CacheName
        /// </summary>
        /// <remarks>
        /// Returns False, Meaning, do not utilize Caching.
        /// Return True, Meaning, Application is set to use Cache and Cache is not empty.
        /// </remarks>
        /*[Extension()]*/

       //application cache status, mostly for testing state of app.
       static public bool CacheStatus(this IOrmModel me) {
               return Convert.ToBoolean(ConfigurationManager.AppSettings["caching"]);
       }

       static public bool CacheStatus(this IOrmModel me, string CacheName ) {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["caching"])) {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["caching"]);
            }
            if (HttpRuntime.Cache[CacheName] == null) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Set Cache Looks at the configuration setting for application caching and based on the setting, 
        /// sets the value into cache.
        /// </summary>
        /// <param name="CacheName"></param>
        /// <param name="Value"></param>
        /// <remarks></remarks>
        static public void SetCache(string CacheName, object Value) {
            HttpRuntime.Cache.Remove(CacheName);
            if (ConfigurationManager.AppSettings["caching"] == "true") {
                HttpRuntime.Cache.Insert(CacheName, Value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
        }

        static public object GetCache(string CacheName ) {
            if (ConfigurationManager.AppSettings["caching"] == "true") {
                return System.Web.HttpRuntime.Cache[CacheName];
            }
            return null; 
        }

        static public void ClearCacheForProcessType(string ProcessTypeFullName) {
            System.Collections.IDictionaryEnumerator Cached = HttpRuntime.Cache.GetEnumerator();
            List<string> Keys = new List<string>();
            while (Cached.MoveNext()) {
                if (Cached.Key.ToString().IndexOf(ProcessTypeFullName) > 0) {
                    Keys.Add(Cached.Key.ToString());
                }
            }
            foreach (string key in Keys) {
                HttpRuntime.Cache.Remove(key);
            }
        }
    }
    
}