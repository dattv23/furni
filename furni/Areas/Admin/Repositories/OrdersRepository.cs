using furni.Data;
using furni.Areas.Admin.Models;
using furni.Interfaces;
using AutoMapper;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace furni.Areas.Admin.Repositories
{
    public class OrdersRepository : IGenericRepository<OrdersModel, int>, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<UserModel, string> _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersRepository(ApplicationDbContext context, IMapper mapper, IGenericRepository<UserModel, string> userRepo, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        public async Task<int> AddAsync(OrdersModel model)
        {
            var OrdersEntity = _mapper.Map<Order>(model);
            await _context.Orders.AddAsync(OrdersEntity);
            return await _context.SaveChangesAsync();

        }

        public async Task<List<OrdersModel>> GetAllAsync()
        {
            var Order = await _context.Orders
                                     .Where(Order => !Order.IsDeleted)
                                     .ToListAsync();
            return _mapper.Map<List<OrdersModel>>(Order);
        }

        public async Task<OrdersModel> GetByIdAsync(int id)
        {
            var OrderEntity = await _context.Orders.FindAsync(id);
            if (OrderEntity == null || OrderEntity.IsDeleted == true)
            {
                return null; // or handle as appropriate
            }
            return _mapper.Map<OrdersModel>(OrderEntity);
        }

        public Task<OrdersModel> GetByIdWithIncludesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(int id)
        {
            var Order = await _context.Orders.FindAsync(id);
            if (Order != null)
            {
                Order.IsDeleted = true;

                _context.Entry(Order).State = EntityState.Modified;

                await SaveAsync();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<OrdersModel> SelectAsync(Expression<Func<OrdersModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, OrdersModel model)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder != null)
            {
                // Map the updated values onto the retrieved user entity
                _mapper.Map(model, existingOrder);

                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the situation where the user wasn't found, which could involve throwing an exception
                // or returning some kind of error code, depending on your application's needs.
                throw new InvalidOperationException("Orders not found.");
            }
        }
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
