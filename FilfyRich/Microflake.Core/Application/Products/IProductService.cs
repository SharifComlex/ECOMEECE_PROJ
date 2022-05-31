using Microflake.Core.Domain;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.Core.Application.Products
{
   public interface IProductService
    {
       
        Task<ServiceResponse<List<ListProduct>>> List();
        IEnumerable<Product> List(int? subcategoryID, string searchTerm, int Page, int recordSize,int? categoryID, int? minimumPrice, int? maximumPrice  ,int? sortBy);

        Task<ServiceResponse<SelectList>> SelectList(long? Id);

        Task<ServiceResponse<Product>> Get(long Id);

        Task<ServiceResponse<ListProduct>> Create(CreateProduct model, string userId);

        Task<ServiceResponse<EditProduct>> Edit(long id);

        Task<ServiceResponse<ListProduct>> Update(EditProduct model, string userId);


        Task<ServiceResponse<long>> Delete(long Id);
        
        int ProductCount(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy);
        double GetMaximumPrice();
        int ProductCountOfDeal(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy);
        
       int FeaturedProductCount(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy);

    }
}
