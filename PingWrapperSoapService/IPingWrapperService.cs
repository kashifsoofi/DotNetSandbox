using System.Runtime.Serialization;
using System.ServiceModel;

namespace PingWrapperSoapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPingWrapperService
    {
        [OperationContract]
        PingWrapperResponse Ping(PingWrapperRequest request);
    }

    [DataContract]
    public class PingWrapperRequest
    {
        [DataMember()]
        public string Payload { get; set; }
    }

    [DataContract]
    public class PingWrapperResponse
    {
        [DataMember]
        public string Payload { get; set; }
    }
}
