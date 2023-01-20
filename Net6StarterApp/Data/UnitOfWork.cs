using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Net6StarterApp.Models;

namespace Net6StarterApp.Data
{
	public class UnitOfWork : IUnitOfWork
	{
        private readonly ApiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork(ApiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CompleteAsync()
        {
            // will set metadata on all saves

            var now = DateTime.Now;
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            foreach (var changedEntity in _context.ChangeTracker.Entries())
            {
                if (changedEntity.Entity is IBaseObject entity)
                {
                    entity.ModifiedSource = "app";
                    entity.LastModifiedDate = now;
                    entity.LastModifiedBy = currentUser;
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreateDate = now;
                            entity.CreatedBy = currentUser;
                            break;
                        case EntityState.Modified:
                            _context.Entry(entity).Property(x => x.CreateDate).IsModified = false;
                            _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                            break;
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}

