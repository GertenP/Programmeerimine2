﻿using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public interface ICategoryItemService
    {
        Task<PagedResult<Category>> List(int page, int pageSize);

        Task<Category> Get(int? Id);

        Task Save(Category category);

        Task Delete(int Id);

        Task<bool> Includes(int Id);
    }
}

