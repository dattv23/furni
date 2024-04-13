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

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var productEntity = await _context.Products.FindAsync(id);
            if (productEntity == null || productEntity.IsDeleted == true)
            {
                return null; // or handle as appropriate
            }
            return _mapper.Map<ProductModel>(productEntity);
        }

        public Task<ProductModel> GetByIdWithIncludesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;

                _context.Entry(product).State = EntityState.Modified;

                await SaveAsync();
            }
        }

        public async Task<int> AddAsync(ProductModel model)
        {
            var productEntity = _mapper.Map<Product>(model);
            // Create default Specification
            var defaultSpecification = new Specification
            {
                Width = 100,  // Default width
                Height = 50,  // Default height
                Depth = 20,   // Default depth
                Weight = 5,   // Default weight
                QualityChecking = true,  // Assume quality checking passed
                FreshnessDuration = 0    // Default to no freshness concern
            };

            // Assuming productEntity has a Specification property
            productEntity.Specification = defaultSpecification;
            await _context.Products.AddAsync(productEntity);
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProductModel entity)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (entity.ImageUrl == null)
            {
                entity.ImageUrl = existingProduct.ImageUrl;
            }
            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            // Map the properties from the provided entity to the existing product entity
            _mapper.Map(entity, existingProduct);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
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
