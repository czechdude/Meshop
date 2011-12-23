using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel;
using Meshop.Framework.Model;
using Meshop.Framework.Services;

namespace Meshop.Core.DefaultServices
{
    public class DefaultCommon : ICommon
    {
        private readonly DatabaseConnection2 _db = new DatabaseConnection2();
        private dynamic _dynamicDictionary;


        public TempDataDictionary TempData { get; set; }

        public string Message
        {
            get
            {
                if (TempData == null) throw new ComponentNotFoundException("The TempData property not set in DefaultCommonService");
                if(TempData["message"] != null)
                    return (string)HttpContext.GetGlobalResourceObject("Global", (string)TempData["message"]);
                return "";
            }
            set
            {
                if(TempData == null) throw new ComponentNotFoundException("The TempData property not set in DefaultCommonService");
                TempData["message"] = value;
            }
        }

        public dynamic Settings
        {
            get
            {
                if (_dynamicDictionary == null)
                {
                    var setting = _db.Settings.ToDictionary(s => s.Key, s => Convert.ChangeType(s.Value,Type.GetType("System."+s.Type)));
                    _dynamicDictionary = new DynamicDictionary<object>(setting);
                }
                return _dynamicDictionary;
            }
        }
    }

    public class DynamicDictionary<TValue> : DynamicObject
    {
        private IDictionary<string, TValue> dictionary;

        public DynamicDictionary(IDictionary<string, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            if (dictionary.ContainsKey(key))
            {
                result = dictionary[key];
                return true;
            }
            throw new KeyNotFoundException(string.Format("Key \"{0}\" was not found in the given dictionary", key));
        }
    }
}