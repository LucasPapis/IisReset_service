using IisReset.Interfaces;
using IisReset.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;

namespace IisReset.Services
{
    [CORSSupportBehavior]
    public class Service : IService
    {
        public List<WinServices> All()
        {
            ServiceController[] services = ServiceController.GetServices();
            List<WinServices> list = new List<WinServices>();

            foreach (ServiceController service in services)
            {
                RegistryKey regKey1 = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\services\{service.ServiceName}");
                list.Add(new WinServices
                {
                    nomeServico = service.ServiceName.ToString(),
                    status = service.Status.ToString(),
                    localizacao = regKey1.ToString()
                });
                regKey1.Close();
            }
            return list;
        }

        //Bolar um endpoint para stopar o serviço..
        public Response Servicekill(string service)
        {
            ServiceController serviceToKill = new ServiceController(service);
            try
            {
                serviceToKill.Refresh();
                if (serviceToKill.Status == ServiceControllerStatus.Running)
                {
                    serviceToKill.Stop();
                    return new Response { response = $"O serviço {service} está sendo finalizado..." };
                    //service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                else
                {
                    throw new Exception($"O serviço {service} já está parado...");
                }
            }
            catch
            {
                throw;
            }
            
        }
        public Response ServiceStart(string service)
        {
            ServiceController serviceToKill = new ServiceController(service);
            try
            {
                serviceToKill.Refresh();
                if (serviceToKill.Status == ServiceControllerStatus.Stopped)
                {
                    serviceToKill.Start();
                    return new Response { response = $"O serviço {service} está sendo iniciado..." };
                    //service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                else
                {
                    throw new Exception($"O serviço {service} já está rodando...");
                }
            }
            catch
            {
                throw;
            }
        }

        public Response ReiniciarIIS()
        {
            ProcessStartInfo ProcessInfo;
            string Command = "IISRESET";
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
            //ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            Process.Start(ProcessInfo);
            return new Response { response = "IIS Reiniciado." };
        }

    }
}

//HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
//if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
//{
//    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, PUT, DELETE");

//    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
//    HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
//    HttpContext.Current.Response.End();
//}
