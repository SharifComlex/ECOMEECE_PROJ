using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Utilities.Logger;
using Microflake.TheComputerShop.Utilities.Response;
using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Subscribers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Application.Subscribers
{
  public  class SubsribersService : ISubscribersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        

        private readonly ILogger _logger;

        public SubsribersService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }

        public async Task<ServiceResponse<List<ListSubscriber>>> ToList()
        {
            try
            {
                var list = await _context
                    .Subscribers
                    .Select(x => new ListSubscriber
                    {
                        Id = x.Id,
                        email = x.email 

                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListSubscriber>());
            }
        }

    }
}
