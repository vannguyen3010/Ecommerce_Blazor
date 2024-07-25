﻿using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;
using Ecommerce_Server.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce_Server.Repositories
{
    public class CategoryRepository(AppDbContext appDbContext) : ICategory
    {
        private readonly AppDbContext appDbContext = appDbContext;

        public async Task<ServiceResponse> AddCategory(Category model)
        {
            if (model is null) return new ServiceResponse(false, "Model is null");
            var (flag, message) = await CheckName(model.Name!);
            if (flag)
            {
                appDbContext.Categories.Add(model);
                await Commit();
                return new ServiceResponse(true, "Category Saved");
            }
            return new ServiceResponse(flag, message);
        }

        public async Task<List<Category>> GetAllCategories() => await appDbContext.Categories.ToListAsync();

        private async Task<ServiceResponse> CheckName(string name)
        {
            var category = await appDbContext.Categories
                .FirstOrDefaultAsync(x => x.Name!.ToLower()!.Equals(name.ToLower()));
            return category is null ? new ServiceResponse(true, null!) : new ServiceResponse(false, "Product already exits");
        }
        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
