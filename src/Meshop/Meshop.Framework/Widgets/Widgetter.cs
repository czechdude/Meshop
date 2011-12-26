using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Meshop.Framework.DI;

namespace Meshop.Framework.Widgets
{
    public delegate MvcHtmlString Widgetter(PagePosition position);

    public class DefaultWidgetter
    {
        private readonly HtmlHelper _html;
        private readonly List<ChildAction> _childActions; 

        public DefaultWidgetter(HtmlHelper html)
        {
            _html = html;
            _childActions = new List<ChildAction>();

            //prozkoumat registrovany kontrollery v zavislosti na atributech akci nahazet do seznamu-> ten vytvorit jen jednou
            
            var ctrlers = Modules.Controllers;
            foreach (var ctrler in ctrlers)
            {
                foreach (var method in ctrler.GetMethods())
                {
                    var att = (PlacementAttribute) method.GetCustomAttributes(typeof(PlacementAttribute), false).FirstOrDefault();
                    if (att == null) continue;
                    var childAction = new ChildAction {Controller = ctrler.Name.Remove(ctrler.Name.IndexOf("Controller")),
                                                        Method = method.Name,
                                                        Position = att.Position,
                                                        Template = att.Template};
                    
                    _childActions.Add(childAction);
                }
            }
        }


        public MvcHtmlString Compose(PagePosition position)
        {
            
            var action = _html.ViewContext.RouteData.Values["Action"] as string;

            //
            foreach (var child in _childActions)
            {
                if (child.Position == position && (child.Template.ToString() == action || child.Template == PageTemplate.All))
                {
                    _html.RenderAction(child.Method, child.Controller);
                }
            }

            return MvcHtmlString.Create("");
        }
    }

    class ChildAction
    {
        public string Controller { get; set; }
        public string Method { get; set; }
        public PagePosition Position { get; set; }
        public PageTemplate Template { get; set; }
    }

}
