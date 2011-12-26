using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Meshop.Framework.Caching;
using Meshop.Framework.Model;

namespace Meshop.Framework.Translation
{
    public class DBResourcesModel
    {
        private readonly ResourcesConnection _db = new ResourcesConnection();
        private Cache _cache = new Cache();

        protected string m_defaultResourceCulture
        {
            get { return "en"; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetResourceByCultureAndKey(CultureInfo culture, string resourceKey)
        {
            if (culture == null || culture.Name.Length == 0)
            {
                culture = new CultureInfo(this.m_defaultResourceCulture);
            }

            //Cache query
            var key = resourceKey + "_" + culture.Name;
            var resourceValue = _cache[key] as string;

            if (resourceValue == null)
            {
                try
                {
                        resourceValue = (from r in _db.Resources
                                                     where r.cultureCode == culture.Name && r.resourceKey == resourceKey
                                                     select r.resourceValue).SingleOrDefault(); 
                }
                catch (EntityCommandExecutionException)
                {
                    resourceValue = null;
                }
  
                if (resourceValue != null)
                {
                    _cache.Add(key, resourceValue);
                    System.Console.WriteLine("caching: " + key +" value: " + resourceValue);
                }
            }

            if (resourceValue == null)
            {
                resourceValue = resourceKey;
            }

            return resourceValue;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public ListDictionary GetResourcesByCulture(CultureInfo cultureInfo)
        {
            var list = new ListDictionary();

            if (cultureInfo == null || cultureInfo.Name.Length == 0)
            {
                cultureInfo = new CultureInfo(this.m_defaultResourceCulture);
            }

            Dictionary<string, string> xlist = (from r in _db.Resources
                                                where r.cultureCode == cultureInfo.Name
                                                select r).ToDictionary(k => k.resourceKey, k => k.resourceValue);

            foreach (var kvp in xlist)
            {
                list.Add(kvp.Key, kvp.Value);
            }

            return list;
        }
    }
}