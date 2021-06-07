using PingWrapperSoapService.PingSoapService;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace PingWrapperSoapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class PingWrapperService : IPingWrapperService
    {
        public PingWrapperResponse Ping(PingWrapperRequest request)
        {
            var pingServiceClient = new PingServiceClient();

            var response = pingServiceClient.Ping(DeserializeRequest(request.Payload));

            var cdataSection = new System.Xml.XmlDocument().CreateCDataSection(this.SerializeResponse(response));
            return new PingWrapperResponse
            {
                Payload = cdataSection.OuterXml,
            };
        }

        private PingRequest DeserializeRequest(string request)
        {
            var cdataSection = new System.Xml.XmlDocument().CreateCDataSection(request);

            XmlSerializer serializer = new XmlSerializer(typeof(PingRequest));
            using (TextReader reader = new StringReader(cdataSection.InnerXml))
            {
                var pingRequest = (PingRequest)serializer.Deserialize(reader);
                return pingRequest;
            }
        }

        private string SerializeResponse(PingResponse pingResponse)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(PingResponse));

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    xsSubmit.Serialize(writer, pingResponse);
                    return sw.ToString(); // Your XML
                }
            }
        }
    }
}
