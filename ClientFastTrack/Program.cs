using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ClientFastTrack
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://apps.fasttrack.com.co:48136/services/FTPagoElectronicoService?wsdl";
            Console.WriteLine(url);

            try
            {
                FTPagoElectronicoService.FTPagoElectronicoServicePortTypeClient client = ServicesFacade.ObtenerClienteFTPagoElectronico(url);

                FTPagoElectronicoService.procesarPago procesarPago = new FTPagoElectronicoService.procesarPago();
                procesarPago.PagoRequest = new FTPagoElectronicoService.PagoRequest();
                procesarPago.PagoRequest.fecha_canal = DateTime.Now.ToShortDateString();
                procesarPago.PagoRequest.fecha_transaccion = DateTime.Now.ToShortDateString();
                procesarPago.PagoRequest.id_comercio = "2575";
                procesarPago.PagoRequest.id_producto = "377";
                procesarPago.PagoRequest.ip_cliente = "ip_cliente";
                procesarPago.PagoRequest.pwd_apps = "QSH7Z4SF";
                procesarPago.PagoRequest.usr_apps= "SQA_VNP_MEDMETRO";

                FTPagoElectronicoService.procesarPagoResponse1 r= client.procesarPagoAsync(procesarPago).Result;


                XmlSerializer xsSubmit = new XmlSerializer(typeof(FTPagoElectronicoService.procesarPagoResponse1));
                var subReq = new FTPagoElectronicoService.procesarPagoResponse1();
                var xml = "";

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, subReq);
                        xml = sww.ToString(); // Your XML
                    }
                }

                Console.WriteLine(xml);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.ReadKey();


        }
    }
}
