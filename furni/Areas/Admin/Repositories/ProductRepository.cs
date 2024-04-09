using AutoMapper;
using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace furni.Areas.Admin.Repositories
{
    public class ProductRepository : IGenericRepository<ProductModel, int>, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            var products = await _context.Products
                                     .Where(product => product.IsDeleted == false || product.IsDeleted == null)
                                     .ToListAsync();
            return _mapper.Map<List<ProductModel>>(products);
        }

        public Task<ProductModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetByIdWithIncludesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(ProductModel entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, ProductModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> SelectAsync(Expression<Func<ProductModel, bool>> predicate)
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
