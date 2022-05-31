using Microflake.Core.Application.Categories;
using Microflake.Core.Application.Currencies;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _entityService;
        private readonly ICategoryService _categoryService;


        public CurrencyController(ICurrencyService entityService , ICategoryService categoryService)
        {
            _entityService = entityService;
            _categoryService = categoryService;
        }
        // GET: Currency
        public async Task<ActionResult> CurrencyList()
        {
            var url = "http://data.fixer.io/api/latest?access_key={fixer_apikey}&base=EUR";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Accept = "application/json";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            //Console.WriteLine(httpResponse.StatusCode);
            var list = await _categoryService.ToList();
            return View(list.Data);
        }
       
        //[Route("home/Currency")]
        public ActionResult SetCurrency(string Name, string url)
        {
           

            HttpCookie cookie = new HttpCookie("Currency");
            cookie.Value = Name;
            Response.Cookies.Add(cookie);

            if (!string.IsNullOrEmpty(url))
            {
                return Redirect(url);
            }

            return RedirectToAction("Index");
        }

    }
}