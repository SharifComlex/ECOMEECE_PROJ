using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Microflake.Web.Models
{
    public class GlobalizationLang
    {
        protected bool _language = false;
        public GlobalizationLang()
        {
            try
            {
                var cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "ar")
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");

                        _language = true;
                        Thread.CurrentThread.CurrentCulture.DateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    }
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                }
                //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

                //if (cookie != null && cookie.Value != null)
                //{
                //    if (cookie.Value == "ar")
                //    {
                //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar");
                //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");
                //    }
                //}
            }
            catch (Exception)
            {
            }
        }
    }
}