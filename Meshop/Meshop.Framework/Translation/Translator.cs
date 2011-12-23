

using System;
using System.Web.Mvc;

namespace Meshop.Framework.Translation
{
    public delegate TranslationString Translator(string text);

    public class DefaultTranslator
    {
        private readonly ViewContext _viewContext;

        public DefaultTranslator(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }


        public TranslationString Translated(string text)
        {
            var translated = _viewContext.HttpContext.GetGlobalResourceObject("Global", text);      
           
            return new TranslationString(translated != null ? translated.ToString() : text);
        }
    }

    public static class NullTranslator
    {
        static readonly Translator _instance;
        
        static NullTranslator()
        {
            _instance = (text) => new TranslationString( text );
        }

        public static Translator Instance { get { return _instance; } }
    }
}
