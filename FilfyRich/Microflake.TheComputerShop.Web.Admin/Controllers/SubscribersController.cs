using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class SubscribersController : BaseController
    {
        private readonly ApplicationDbContext _context;
       
        public SubscribersController(ApplicationDbContext context)
        {
            _context = context;
          

        }

        [HttpPost]
        public async Task<ActionResult> Create(string email)
        {
            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }

            if (email != null)
            {
                var entity = new Subscriber();
                entity.email = email;
                _context.Subscribers.Add(entity);
                _context.SaveChanges();

            }
            return Json(new
            {
                status = true,


            }, JsonRequestBehavior.AllowGet);
        }

    }
}