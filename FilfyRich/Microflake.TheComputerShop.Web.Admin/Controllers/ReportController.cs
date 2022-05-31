using Microflake.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microflake.Core.Application.Categories;
using Microflake.Web.Models;

namespace Microflake.Web.Controllers
{
    public class ReportController : BaseController
    {
        ApplicationDbContext _context = new ApplicationDbContext();


        private ICategoryService _categoryService;
        public ReportController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            new GlobalizationLang();
        }

        #region Category wise List
        // GET: Report
        public async Task<ActionResult> CategoryWise()
        {
            var categoryResult = await _categoryService.ToList();
            //ViewBag.CategoryId = new SelectList(categoryResult.Data, "Id", "Name");

            if (_language)
            {
                ViewBag.CategoryId = new MultiSelectList(categoryResult.Data.ToList(), "Id", "Arabic");
            }
            else
            {
                ViewBag.CategoryId = new MultiSelectList(categoryResult.Data.ToList(), "Id", "English");
            }
            return View();
        }
        public async Task<ActionResult> CategoryWiseProductList(long ProductId)
        {
            //var tenants = await _reportsService.GetTenantList();

            var Products = await _context.OrderDetals
                   .Include(x => x.Order)
                   .Include(x => x.Product)
                   .Where(x => x.Order.Status == "Completed" 
                   && 
                   x.Product.SubCategory.Category.Id== ProductId).ToListAsync();
            return View(Products);
        }
        #endregion


        #region Weekly ProfitLoss Report
        public async Task<ActionResult> WeeklyReport()
        {
            
            return View();
        }
        public async Task<ActionResult> WeeklyReportVView()
        {
            //var tenants = await _reportsService.GetTenantList();
            DateTime Date = DateTime.UtcNow;
            DateTime StartDate = Date.AddDays(-7);
            var Products = await _context.OrderDetals
                   .Include(x => x.Order)
                   .Include(x => x.Product)
                   .Where(x => x.Order.Status == "Completed").ToListAsync();

            var product = Products.Where(x => x.Order.CreatedAt >= StartDate && x.Order.CreatedAt <= Date).ToList();
            return View(product);
        }

        #endregion


        #region Daily ProfitLoss Report
        public async Task<ActionResult> DailyReport()
        {

            return View();
        }
        public async Task<ActionResult> DailyReportView(DateTime date)
        {
            //var tenants = await _reportsService.GetTenantList();
            DateTime Date = DateTime.UtcNow;
          
            var Products = await _context.OrderDetals
                   .Include(x => x.Order)
                   .Include(x => x.Product)
                   .Where(x => x.Order.Status == "Completed").ToListAsync();

            var product = Products.Where(x => x.Order.CreatedAt.Date == date.Date).ToList();
            return View(product);
        }

        #endregion


        #region Yearly Profitloss Report
        public async Task<ActionResult> ProductListYearly()
        {
            var years = new List<int>();

            for (int i = 2017; i <= DateTime.UtcNow.Year; i++)
            {
                years.Add(i);
            }
            ViewBag.Years = new SelectList(years.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString()
            }), "Value", "Text", DateTime.UtcNow.Year);
            return View();
        }

        public async Task<ActionResult> ProductListYearlyreport(long SelectYear)
        {
            //var tenants = await _reportsService.GetTenantList();
            ViewBag.Year = SelectYear;
            var Products = await _context.OrderDetals
                 .Include(x => x.Order)
                 .Include(x => x.Product)
                 .Where(x => x.Order.Status == "Completed").ToListAsync();
            var Product= Products.Where(x => x.Order.CreatedAt.Year == SelectYear).ToList();

            return View(Product);
        }


        #endregion
    }
}