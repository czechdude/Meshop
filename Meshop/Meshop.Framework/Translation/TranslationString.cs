using System;
using System.Web;
using System.Web.Mvc;

namespace Meshop.Framework.Translation
{
    

    public class TranslationString: IHtmlString
    {
        private readonly string _string;

   

        public TranslationString(string translation)
        {
            _string = translation;

        }


        public string ToHtmlString()
        {
            return _string;
        }


        public string Text
        {
            get { return _string; }
        }

        public override string ToString()
        {
            return _string;
        }

    }
}
