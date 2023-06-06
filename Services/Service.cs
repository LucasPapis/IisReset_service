using IisReset.Interfaces;
using IisReset.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;

namespace IisReset.Services
{
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

        public Response ReiniciarIIS()
        {
            ProcessStartInfo ProcessInfo;
            string Command = "IISRESET";

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
            //ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            Process.Start(ProcessInfo);
            return new Response { response = "IIS Reiniciando..." };
        }
    }
}
