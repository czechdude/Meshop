using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;

namespace Meshop.Framework.Translation
{

    public class TranslateNameAttribute : DisplayNameAttribute
    {

        public TranslateNameAttribute(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                    return (string)HttpContext.GetGlobalResourceObject("Global", ResourceKey);
                
            }
        }

    }

    public class TranslateRequiredAttribute : RequiredAttribute
    {
        public new string ErrorMessage { 
            get { return base.ErrorMessage; }
            set
            {
                base.ErrorMessage = (string)HttpContext.GetGlobalResourceObject("Global", value);
            } 
        }
    }

    public class TranslateRequiredAttributeAdapter : DataAnnotationsModelValidator<TranslateRequiredAttribute>
    {
        private readonly string _message;

        public TranslateRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, TranslateRequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            _message = attribute.ErrorMessage;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRequiredRule(_message);
            return new[] { rule };
        }

    }

    public class TranslateRegularExpressionAttributeAdapter : DataAnnotationsModelValidator<TranslateRegularExpressionAttribute>
    {
        private string _message;
        private string _pattern;

        public TranslateRegularExpressionAttributeAdapter(ModelMetadata metadata, ControllerContext context, TranslateRegularExpressionAttribute attribute) : base(metadata, context, attribute)
        {
            _pattern = attribute.Pattern;
            _message = attribute.ErrorMessage;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRegexRule(_message,_pattern);

            return new[] {rule};

        }
    }



    public class TranslateRegularExpressionAttribute : RegularExpressionAttribute
    {
        public TranslateRegularExpressionAttribute(string pattern) : base(pattern)
        {
        }

        public new string ErrorMessage
        {
            get { return base.ErrorMessage; }
            set
            {
                base.ErrorMessage = (string)HttpContext.GetGlobalResourceObject("Global", value);
            }
        }

    }

}
