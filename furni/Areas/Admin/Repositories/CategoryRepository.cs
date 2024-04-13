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

        public async Task<int> AddAsync(CategoryModel model)
        {
            var CategoryEntity = _mapper.Map<Category>(model);
            await _context.Categories.AddAsync(CategoryEntity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            var categories = await _context.Categories
                                    .Where(category => !category.IsDeleted)
                                    .ToListAsync();
            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            var CategoryEntity = await _context.Categories.FindAsync(id);
            if (CategoryEntity == null || CategoryEntity.IsDeleted == true)
            {
                return null; // or handle as appropriate
            }
            return _mapper.Map<CategoryModel>(CategoryEntity);
        }

        public async Task<CategoryModel> GetByIdWithIncludesAsync(int id)
        {
            var CategoryEntity = await _context.Categories
                                                .Where(category => category.IsDeleted == false)
                                                .Include(category => category.Parent)
                                                .FirstOrDefaultAsync();
            if (CategoryEntity == null || CategoryEntity.IsDeleted == true)
            {
                return null; // or handle as appropriate
            }
            return _mapper.Map<CategoryModel>(CategoryEntity);
        }

        public async Task RemoveAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;

                _context.Entry(category).State = EntityState.Modified;

                await SaveAsync();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<CategoryModel> SelectAsync(Expression<Func<CategoryModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, CategoryModel model)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory != null)
            {
                // Map the updated values onto the retrieved Category entity
                _mapper.Map(model, existingCategory);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Category not found.");
            }
            
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
