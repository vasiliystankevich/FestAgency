using System;
using System.Threading.Tasks;

namespace Backend.Web.Core.Backend.Interfaces
{
    public interface IAwaitTaskCreator
    {
        Task<TResult> Create<TData, TResult>(TData data, Func<TData, TResult> functor);
        Task<TResult> Create<TDataFirst, TDataSecond, TResult>(TDataFirst dataFirst, TDataSecond dataSecond, Func<TDataFirst, TDataSecond, TResult> functor);
    }
}
