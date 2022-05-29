﻿using Microflake.TheComputerShop.Domain;
using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Category;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Application.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResponse<SelectList>> SelectList(long? Id);

        Task<ServiceResponse<List<ListCategory>>> ToList();
        
        Task<ServiceResponse<ListCategory>> Create(CreateCategory model, string userId);
        
        Task<ServiceResponse<EditCategory>> Edit(long id);
        
        Task<ServiceResponse<ListCategory>> Update(EditCategory model, string userId);
        
        Task<ServiceResponse<long>> Remove(long Id);
    }
}
