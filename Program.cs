
using System.Net.Http.Headers;
using System.Xml;

namespace SOAPClientExample
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            //URL Base de la prueba SOAP
            //http://www.dneonline.com/calculator.asmx
            // Configura la dirección del servicio SOAP
            string serviceUrl = "http://www.dneonline.com/calculator.asmx?op=Add";

   
            // Crea el cliente HTTP
            using (HttpClient client = new HttpClient())
            {
                // Crea el cuerpo de la solicitud SOAP
                string soapRequest =@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <soap:Body>
                            <Add xmlns=""http://tempuri.org/"">
                            <intA>10</intA>
                            <intB>10</intB>
                            </Add>
                        </soap:Body>
                        </soap:Envelope>";
                
                // Realiza la llamada SOAP
                try
                {
                HttpClient httpClient = new HttpClient();
                HttpContent httpContent = new StringContent(soapRequest);
                HttpResponseMessage response;

                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, serviceUrl);
                req.Headers.Add("SOAPAction", "http://tempuri.org/Add");
                req.Method = HttpMethod.Post;
                req.Content = httpContent;
                req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

                // Here you will get the Reponse from service
                response = await httpClient.SendAsync(req);
                // Converting the response into text format
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBodyAsText);
                // Parsea la respuesta XML
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(responseBodyAsText);
                
                // Maneja la respuesta como desees
                XmlNodeList resultNodes = xmlDocument.GetElementsByTagName("AddResult");
                string result = resultNodes[0].InnerText;
                
                Console.WriteLine("SOAP response:");
                Console.WriteLine(result);
                var builder = WebApplication.CreateBuilder(args);
                var app = builder.Build();

                app.MapGet("/", () => "El resultado de la Operacion es "+ result);

                app.Run();
                }
                catch (System.Exception ex)
                {
                    
                    throw ex;
                }

            }
        }
    }
}

