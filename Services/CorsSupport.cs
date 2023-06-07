using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace IisReset.Services
{
    public class CorsSupport : IDispatchMessageInspector
    {

        Dictionary<string, string> requiredHeaders;
        public CorsSupport(Dictionary<string, string> headers)
        {
            requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var httpRequest = request.Properties["httpRequest"] as HttpRequestMessageProperty;
            if (httpRequest.Method.ToLower() == "options")
                instanceContext.Abort();
            return httpRequest;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var httpResponse = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
            var httpRequest = correlationState as HttpRequestMessageProperty;

            foreach (var item in requiredHeaders)
            {
                httpResponse.Headers.Add(item.Key, item.Value);
            }
            var origin = httpRequest.Headers["origin"];
            if (origin != null)
                httpResponse.Headers.Add("Access-Control-Allow-Origin", origin);

            var method = httpRequest.Method;
            if (method.ToLower() == "options")
                httpResponse.StatusCode = System.Net.HttpStatusCode.NoContent;

        }
    }

    // Simply apply this attribute to a DataService-derived class to get  
    // CORS support in that service  
    [AttributeUsage(AttributeTargets.Class)]
    public class CORSSupportBehaviorAttribute : Attribute, IServiceBehavior
    {

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            Dictionary<string, string> requiredHeaders = new Dictionary<string, string>
            {
                //Chrome doesn't accept wildcards when authorization flag is true  
                //requiredHeaders.Add("Access-Control-Allow-Origin", "*");  
                { "Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS" },
                { "Access-Control-Allow-Headers", "Accept, Origin, Authorization, X-Requested-With,Content-Type" },
                { "Access-Control-Allow-Credentials", "true" }
            };
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    ed.DispatchRuntime.MessageInspectors.Add(new CorsSupport(requiredHeaders));
                }
            }
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}