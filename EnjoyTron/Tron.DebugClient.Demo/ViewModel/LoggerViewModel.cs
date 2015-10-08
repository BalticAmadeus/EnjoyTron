using Prism.Mvvm;
using Tron.DebugClient.Demo.Infrastructure;

namespace Tron.DebugClient.Demo.ViewModel
{
    public class LoggerViewModel : BindableBase
    {
        public LoggerViewModel(IServiceCallInvoker serviceCallInvoker)
        {
            serviceCallInvoker.InvokeBegan += (sender, args) =>
            {
                Request = args.Request;
            };

            serviceCallInvoker.InvokeFinished += (sender, args) =>
            {
                Response = args.Response;
                OperationTime = args.OperationTime;
            };
        }

        private string _request;
        public string Request
        {
            get { return _request; }
            set { SetProperty(ref _request, value); }
        }

        private string _response;
        public string Response
        {
            get { return _response; }
            set { SetProperty(ref _response, value); }
        }

        private long _operationTime;
        public long OperationTime
        {
            get { return _operationTime; }
            set { SetProperty(ref _operationTime, value); }
        }
    }
}