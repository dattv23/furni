using furni.Areas.Admin.Models;
using furni.Interfaces;
using System.Linq.Expressions;
using System;
using Microsoft.EntityFrameworkCore;
using furni.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace furni.Areas.Admin.Repositories
{
    public class UserRepository : IGenericRepository<UserModel, string>, IDisposable
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        // Constructor
        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(UserModel model)
        {
            // Convert UserModel to ApplicationUser
            var userEntity = _mapper.Map<ApplicationUser>(model);
            Guid guid = Guid.NewGuid();
            userEntity.Id = guid.ToString();
            userEntity.UserName = model.Email;
            userEntity.NormalizedUserName = model.Email.ToUpper();

            // Use UserManager to create the user asynchronously
            var result = await _userManager.CreateAsync(userEntity, "Furni@2024");

            // Handle result
            if (result.Succeeded)
            {
                // Optionally, you could also set the Id of the UserModel to the newly created user's Id if needed
                model.Id = userEntity.Id;

                // Return a positive value indicating success
                return 1;
            }
            else
            {
                // Return a negative value indicating failure
                return -1;
            }
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            var users = await _context.Users
                                      .Where(user => user.IsDeleted == false || user.IsDeleted == null)
                                      .ToListAsync();
            return _mapper.Map<List<UserModel>>(users);
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null || userEntity.IsDeleted == true)
            {
                return null; // or handle as appropriate
            }
            return _mapper.Map<UserModel>(userEntity);
        }

        public Task<UserModel> GetByIdWithIncludesAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;

                _context.Entry(user).State = EntityState.Modified;

                await SaveAsync();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<UserModel> SelectAsync(Expression<Func<UserModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(string id, UserModel model)
        {
            // Retrieve the existing user
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity != null)
            {
                // Map the updated values onto the retrieved user entity
                _mapper.Map(model, userEntity);

                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the situation where the user wasn't found, which could involve throwing an exception
                // or returning some kind of error code, depending on your application's needs.
                throw new KeyNotFoundException($"User with ID {id} not found.");
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
