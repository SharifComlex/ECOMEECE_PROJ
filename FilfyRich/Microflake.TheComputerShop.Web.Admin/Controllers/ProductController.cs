using Microflake.TheComputerShop.Application.Categories;
using Microflake.TheComputerShop.Application.Products;
using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _entityService;

        private readonly ICategoryService _CategoryService;

        public ProductController(ApplicationDbContext context, ICategoryService CategoryService, IProductService entityService)
        {
            _context = context;
            _entityService = entityService;

            _CategoryService = CategoryService;

          
        }
        public async Task<ActionResult> ProductQuickView(int id)
        {
            var result = await _entityService.Edit(id);
            if (Currency != null && Currency != "")
            {
                ViewBag.Currency = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Currency_Rate;
                ViewBag.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Simbal;
              

            }
            else
            {
                ViewBag.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Base_Currency == true).Simbal;

                ViewBag.Currency = 1;
            }


            return PartialView(result.Data);
        }
    }
}