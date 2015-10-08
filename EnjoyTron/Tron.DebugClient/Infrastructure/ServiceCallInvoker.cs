using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Prism.Logging;
using Tron.DebugClient.ClientService;

namespace Tron.DebugClient.Infrastructure
{
    public static class BaseResponseExtensions
    {
        public static bool IsOk(this BaseResp resp)
        {
            if (ReferenceEquals(resp, null))
                return false;

            if (resp.Status != "OK")
                return false;

            return true;
        }
    }


    public class ServiceCallInvoker : IServiceCallInvoker
    {
        private readonly ILoggerFacade _logger;

        public ServiceCallInvoker(ILoggerFacade logger)
        {
            _logger = logger;
        }
        
        public async Task<TResp> InvokeAsync<TReq, TResp>(TReq req, Func<TReq, Task<TResp>> action) 
            where TReq : BaseReq 
            where TResp : BaseResp, new()
        {
            string requestString;
            using (var stringStream = new StringWriter())
            {
                var requestSerializer = new XmlSerializer(typeof(TReq));
                requestSerializer.Serialize(stringStream, req);
                requestString = stringStream.ToString();
            }

            var reqArgs = new InvokeEventArgs
            {
                Request = requestString,
            };

            OnInvokeBegan(reqArgs);

            _logger.Log(requestString, Category.Info, Priority.Medium);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            TResp resp = null;
            try
            {
                resp = await action(req);
            }
            catch (Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine(e.Message);
                sb.AppendLine();
                sb.AppendLine(e.StackTrace);

                var errorArgs = new InvokeEventArgs
                {
                    Response = sb.ToString(),
                    OperationTime = stopwatch.ElapsedMilliseconds,
                };

                _logger.Log(sb.ToString(), Category.Info, Priority.Medium);

                OnInvokeFinished(errorArgs);

                return resp;
            }

            stopwatch.Stop();


            string responseString;
            using (var stringStream = new StringWriter())
            {
                var responseSerializer = new XmlSerializer(typeof(TResp));
                responseSerializer.Serialize(stringStream, resp);
                responseString = stringStream.ToString();
            }

            var respArgs = new InvokeEventArgs
            {
                Response = responseString,
                OperationTime = stopwatch.ElapsedMilliseconds,
            };

            _logger.Log(responseString, Category.Info, Priority.Medium);

            OnInvokeFinished(respArgs);

            return resp;
        }

        public event InvokedEventHandler InvokeBegan;
        public event InvokedEventHandler InvokeFinished;

        protected virtual void OnInvokeBegan(InvokeEventArgs e)
        {
            var handler = InvokeBegan;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnInvokeFinished(InvokeEventArgs e)
        {
            var handler = InvokeFinished;
            if (handler != null)
                handler(this, e);
        }
    }
}