using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.ContactUss;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.ContactUs
{
   public class ContactUsService : IContactUsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public ContactUsService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }

        public async Task<ServiceResponse<List<ListContactUs>>> List()
        {
            try
            {
                var list = await _context
                    .ContactUss
                    .Select(x => new ListContactUs
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Message = x.Message,
                        subject = x.subject,
                        firstname = x.firstname ,
                        lastname = x.lastname


                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListContactUs>());
            }
        }

    }
}
