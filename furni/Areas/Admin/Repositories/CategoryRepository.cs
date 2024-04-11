using AutoMapper;
using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace furni.Areas.Admin.Repositories
{
    public class CategoryRepository : IGenericRepository<CategoryModel, int>, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<int> AddAsync(CategoryModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            var categories = await _context.Categories
                                    .Where(category => !category.IsDeleted)
                                    .ToListAsync();
            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public Task<CategoryModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryModel> GetByIdWithIncludesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryModel> SelectAsync(Expression<Func<CategoryModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, CategoryModel entity)
        {
            throw new NotImplementedException();
        }

        // Dispose Methods
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
