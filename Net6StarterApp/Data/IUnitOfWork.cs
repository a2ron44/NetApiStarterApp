using System;
namespace Net6StarterApp.Data
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}

