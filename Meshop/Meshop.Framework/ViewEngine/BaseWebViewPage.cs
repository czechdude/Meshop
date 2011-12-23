using System.Web.Mvc;
using Meshop.Framework.DI;
using Meshop.Framework.Services;
using Meshop.Framework.Translation;
using Meshop.Framework.Widgets;

namespace Meshop.Framework.ViewEngine
{

    /*
     web page base - do not inherit alone!
     */
    public abstract class BaseWebViewPage<TModel> : WebViewPage<TModel>
    {
        private Translator _translator = NullTranslator.Instance;
        private Widgetter _widgetter;

        public ICommon CommonService { get; private set; }

        public Translator T { get { return _translator; } }

        public Widgetter WidgetPlace { get { return _widgetter; } }

        protected string _layout
        {
            get
            {
                return base.Layout;
            }
            set
            {
                base.Layout = value;
            }
        }

        protected void SetLayout(string layout)
        {
            if (!Context.Items.Contains("layoutIsSet"))
            {
                _layout = layout;
                Context.Items.Add("layoutIsSet",true);
            }
        }

        public override void InitHelpers()
        {
            CommonService = Modules.Container.Resolve<ICommon>();
            CommonService.TempData = TempData;
            base.InitHelpers();
            _widgetter = WidgetterResolver.Resolve(Html);
            _translator = TranslationResolver.Resolve(ViewContext);
        }
    }

    public abstract class BaseWebViewPage : BaseWebViewPage<dynamic>
    {
        
    }

    /*
     Front web page base
     */

    public abstract class FrontWebViewPage<TModel> : BaseWebViewPage<TModel>
    {

        // set this modifier as protected, to make it accessible from view-pages


        public IFront FrontService { get; private set; }

        protected override void InitializePage()
        {
            FrontService = Modules.Container.Resolve<IFront>();
            SetLayout("~/Views/Shared/_Layout.cshtml");
            base.InitializePage();
        }



    }

    public abstract class FrontWebViewPage : FrontWebViewPage<dynamic>
    {

    }


    /*
     Admin web page base
     */
    public abstract class AdminWebViewPage<TModel> : BaseWebViewPage<TModel>
    {
        

        public IAdmin AdminService { get; private set; }

        protected override void InitializePage()
        {
            AdminService = Modules.Container.Resolve<IAdmin>();
            SetLayout("~/Areas/Admin/Views/Shared/_Layout.cshtml");
            base.InitializePage();
        }


    }

    public abstract class AdminWebViewPage : AdminWebViewPage<dynamic>
    {

    }

}
