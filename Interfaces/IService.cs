using IisReset.Models;
using System.Collections.Generic;
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

    }
}
