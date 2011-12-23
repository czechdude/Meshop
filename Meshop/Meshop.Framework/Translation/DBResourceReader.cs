using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Resources;

namespace Meshop.Framework.Translation
{
    public class DBResourceReader : DisposableBaseType, IResourceReader, IEnumerable<KeyValuePair<string, object>>
    {
        private ListDictionary m_resourceDictionary;
        public DBResourceReader(ListDictionary resourceDictionary)
        {
            this.m_resourceDictionary = resourceDictionary;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }


        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return this.m_resourceDictionary.GetEnumerator();
        }

        // other methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}