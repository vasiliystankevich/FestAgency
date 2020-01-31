using System;
using System.Threading.Tasks;
using Backend.Web.Core.Backend.Interfaces;

namespace Backend.Web.Core.Backend
{
    public class AwaitTaskCreator: IAwaitTaskCreator
    {
        public async Task<TResult> Create<TData, TResult>(TData data, Func<TData, TResult> functor)
        {
            return await Task<TResult>.Factory.StartNew(() => functor(data));
        }

        public async Task<TResult> Create<TDataFirst, TDataSecond, TResult>(TDataFirst dataFirst, TDataSecond dataSecond, Func<TDataFirst, TDataSecond, TResult> functor)
        {
            return await Task<TResult>.Factory.StartNew(() => functor(dataFirst, dataSecond));
        }
    }
}