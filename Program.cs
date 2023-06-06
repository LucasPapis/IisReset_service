using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System;
using IisReset.Services;

namespace IisReset
{
    public class Program
    {
        static void Main(string[] args)
        {
			try
			{
                ServiceHost selfHost = new ServiceHost(typeof(Service), new Uri("http://localhost:44315"));
                var processo = Process.GetProcessesByName("WCFService");
                if (processo != null)
                {
                    ServiceBase.Run(new WindowsService(selfHost));
                }
                else

                {
                    throw new Exception("Falha ao criar o serviço");
                }
            }
			catch (System.Exception)
			{

				throw;
			}
        }
    }
}
