using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Meshop.Framework.Translation
{
    public class TranslationResolver
    {


            public static Translator Resolve(ViewContext viewContext)
            {
                var defaultTranslator = new DefaultTranslator(viewContext);
                return defaultTranslator.Translated;
            }
        }
    
}
