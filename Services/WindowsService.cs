using IisReset.Interfaces;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceProcess;

namespace IisReset.Services
{
    public class WindowsService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public WindowsService(ServiceHost selfHost)
        {
            // Name the Windows Service
            ServiceName = "WCFService";
            serviceHost = selfHost;
        }
        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            try
            {
                serviceHost.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "")
                    .EndpointBehaviors.Add(new WebHttpBehavior());
                serviceHost.Open();
            }
            catch (CommunicationException ce)
            {
                serviceHost.Abort();
            }
        }
        protected override void OnStop()
        {
            serviceHost.Close();
        }
    }
}
