using IisReset.Models;
using IisReset.Services;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace IisReset.Interfaces
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", 
            UriTemplate = "/resetiis", 
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json)]
        Response ReiniciarIIS();

        [OperationContract]
        [WebInvoke(
           Method = "GET",
           UriTemplate = "/services",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        List<WinServices> All();

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/servicekill/{service}",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        Response Servicekill(string service);

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/servicestart/{service}",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        Response ServiceStart(string service);

    }
}
