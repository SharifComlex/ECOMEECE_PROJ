using Microflake.Core.Domain;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.DealOfTheWeeks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.Core.Application.DealOfTheWeeks
{
  public  interface IDealOfTheWeekService
    {

        Task<ServiceResponse<List<ListDealOfTheWeek>>> List();
        IEnumerable<Product> List(string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy);

        Task<ServiceResponse<SelectList>> SelectList(long? Id);

        Task<ServiceResponse<Product>> Get(long Id);

        Task<ServiceResponse<ListDealOfTheWeek>> Create(CreateDealOfTheWeek model, string userId);

        Task<ServiceResponse<EditDealOfTheWeek>> Edit(long id);

        Task<ServiceResponse<ListDealOfTheWeek>> Update(EditDealOfTheWeek model, string userId);


        Task<ServiceResponse<long>> Delete(long Id);

        //int ProductCount(string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy);
        //double GetMaximumPrice();
    }
}
