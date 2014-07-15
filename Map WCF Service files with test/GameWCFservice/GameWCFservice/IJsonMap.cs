using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GameWCFservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGetJsonMap" in both code and config file together.
    [ServiceContract]
    public interface IJsonMap
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "sendmap")]
        bool Sendmap(byte[] MapByteArray);      //returns TRUE if succeeded saving map

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Getmap")]
        byte[] Getmap(int mapId);      //returns a byte array that is a map in json format

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetmapsFromUser")]
        byte[] GetmapsFromUser(int userId);      //returns a byte array that is maps created by a user in json format

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetMultipleMapsByMapIDs")]
        byte[] GetMultipleMapsByMapIDs(int[] mapIds);      //returns a byte array that is multiple maps in json format

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetAllMapsID")]
        byte[] GetAllMapsID();      //returns a byte array that is multiple maps in json format

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "SendmapWithImage")]
        string SendmapWithImage(byte[] mapByteArray, byte[] imageByteArray, string mapName, int finalVersion, int userId);      //returns "success" if succeeded saving map

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "UpdateMapInProgress")]
        string UpdateMapInProgress(byte[] mapByteArray, byte[] imageByteArray, string mapName, int finalVersion, int mapId, int userId);
    }
}
