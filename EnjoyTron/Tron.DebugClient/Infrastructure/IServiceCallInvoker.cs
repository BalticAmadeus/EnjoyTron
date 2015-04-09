using System;
using System.Threading.Tasks;
using Tron.DebugClient.ClientService;

namespace Tron.DebugClient.Infrastructure
{
    public interface IServiceCallInvoker
    {
        Task<TResp> InvokeAsync<TReq, TResp>(TReq req, Func<TReq, Task<TResp>> action) 
            where TReq : BaseReq
            where TResp : BaseResp, new();

        event InvokedEventHandler InvokeBegan;
        event InvokedEventHandler InvokeFinished;
    }

    public delegate void InvokedEventHandler(object sender, InvokeEventArgs args);
}
