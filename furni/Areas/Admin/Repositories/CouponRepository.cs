using AutoMapper;
using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace furni.Areas.Admin.Repositories
{
    // Implements the IGenericRepository for CouponModel entities and IDisposable to manage resource cleanup.
    public class CouponRepository : IGenericRepository<CouponModel, int>, IDisposable
    {
        private readonly ApplicationDbContext _context; // Database context for Entity Framework operations.
        private readonly IMapper _mapper; // AutoMapper to map between CouponModel and Coupon entities.

        // Constructor initializes the repository with necessary dependencies.
        public CouponRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Adds a new coupon entity to the database asynchronously.
        public async Task<int> AddAsync(CouponModel entity)
        {
            var couponEntity = _mapper.Map<Coupon>(entity); // Maps the CouponModel to a Coupon entity.
            await _context.Coupons.AddAsync(couponEntity); // Adds the entity to the DbContext.
            return await SaveAsync(); // Saves changes to the database.
        }

        // Retrieves all inactive coupons asynchronously.
        public async Task<List<CouponModel>> GetAllAsync()
        {
            var coupons = await _context.Coupons
                                   .Where(coupon => coupon.IsActive) // Filters out active coupons.
                                   .ToListAsync();
            return _mapper.Map<List<CouponModel>>(coupons);
        }

        // Retrieves a single coupon by its ID if it's active.
        public async Task<CouponModel> GetByIdAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id); // Attempts to find a coupon by ID.
            if (coupon == null || !coupon.IsActive) // Checks if the coupon is found and active.
            {
                return null; // Returns null if not found or inactive.
            }
            return _mapper.Map<CouponModel>(coupon); // Maps and returns the found coupon.
        }

        // Placeholder for a future method to get a coupon by ID with related entities.
        public Task<CouponModel> GetByIdWithIncludesAsync(int id)
        {
            throw new NotImplementedException();
        }

        // "Deactivates" a coupon by setting its IsActive flag to true, instead of deleting it.
        public async Task RemoveAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id); // Finds the coupon by ID.
            if (coupon != null)
            {
                coupon.IsActive = false; // Sets the coupon as active (Note: This might be intended to deactivate).
                _context.Entry(coupon).State = EntityState.Modified; // Marks the entity as modified.
                await SaveAsync(); // Saves changes to the database.
            }
        }

        // Saves changes to the database asynchronously.
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync(); // Saves all changes made in this context to the database.
        }

        // Placeholder for a method to select coupons based on a predicate.
        public Task<CouponModel> SelectAsync(Expression<Func<CouponModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        // Updates an existing coupon's data.
        public async Task UpdateAsync(int id, CouponModel entity)
        {
            var coupon = await _context.Coupons.FindAsync(id); // Finds the existing coupon by ID.
            if (coupon != null)
            {
                _mapper.Map(entity, coupon); // Maps the updated model data onto the existing entity.
                await SaveAsync(); // Saves the updated entity to the database.
            }
            else
            {
                throw new KeyNotFoundException($"Coupon with ID {id} not found."); // Throws if the coupon doesn't exist.
            }
        }

        // Implementing the Dispose pattern to clean up the DbContext resource.
        private bool _disposed = false; // To track whether Dispose has been called.

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose(); // Disposes the DbContext.
                }
                _disposed = true; // Marks that the object has been disposed.
            }
        }

        // Public implementation of Dispose method.
        public void Dispose()
        {
            Dispose(true); // Calls the protected Dispose method.
            GC.SuppressFinalize(this); // Suppresses finalization to prevent the destructor from being called.
        }
    }
}
