using System.Runtime.Serialization;
using System.ServiceModel;

namespace PingSoapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPingService
    {
        [OperationContract]
        PingResponse Ping(PingRequest request);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class PingRequest
    {
        [DataMember]
        public bool BoolValue { get; set; }

        [DataMember]
        public string StringValue { get; set; }
    }

    [DataContract]
    public class PingResponse
    {
        [DataMember]
        public string Value { get; set; }
    }
}
