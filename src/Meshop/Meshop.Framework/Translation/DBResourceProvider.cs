using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Resources;
using System.Web.Compilation;


namespace Meshop.Framework.Translation
{
    
    class DBResourceProvider: IResourceProvider
    {
        private string classKey;
        private DBResourcesModel m_dalc = new DBResourcesModel();

        
        public DBResourceProvider(string classKey)
        {
            
            this.classKey = classKey;
        }

        public object GetObject(string resourceKey, CultureInfo culture)
        {

            if (string.IsNullOrEmpty(resourceKey))
            {
                throw new ArgumentNullException("resourceKey");
            }

            if (culture == null)
            {
                culture = CultureInfo.CurrentUICulture;
            }

            string resourceValue = m_dalc.GetResourceByCultureAndKey(culture, resourceKey);
            return resourceValue;
        }

        public IResourceReader ResourceReader
        {
            get
            {
                ListDictionary resourceDictionary = this.m_dalc.GetResourcesByCulture(CultureInfo.InvariantCulture);

                return new DBResourceReader(resourceDictionary);
            }
        }
    }
}
