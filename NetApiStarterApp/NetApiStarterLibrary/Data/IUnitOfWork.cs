using System;
using Microsoft.EntityFrameworkCore;
using NetApiStarterLibrary.Models;
using System.Security.Claims;

namespace NetApiStarterLibrary.Data
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }

    //Need to implement this function to auto-save meta data

    //public async Task CompleteAsync()
    //{
    //    // will set metadata on all saves

    //    var now = DateTime.Now;
    //    var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    //    foreach (var changedEntity in _context.ChangeTracker.Entries())
    //    {
    //        if (changedEntity.Entity is IBaseObject entity)
    //        {
    //            entity.ModifiedSource = "app";
    //            entity.LastModifiedDate = now;
    //            entity.LastModifiedBy = currentUser;
    //            switch (changedEntity.State)
    //            {
    //                case EntityState.Added:
    //                    entity.CreateDate = now;
    //                    entity.CreatedBy = currentUser;
    //                    break;
    //                case EntityState.Modified:
    //                    _context.Entry(entity).Property(x => x.CreateDate).IsModified = false;
    //                    _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
    //                    break;
    //            }
    //        }
    //    }
    //    await _context.SaveChangesAsync();
    //}
}

