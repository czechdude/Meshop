using System.Collections.Generic;
using System.Web.Mvc;
using Meshop.Framework.Model;

namespace Meshop.Framework.Services
{
    public interface ICommon
    {
        TempDataDictionary TempData { get; set; }
        /*translation of error data messages shown on sumbit and transfered over to next request via TempData property (originally transfered from Controller class)*/
        string Message { get; set; }
        /*loaded settings dynamic dictionary from database*/
        dynamic Settings { get; }
    }
}