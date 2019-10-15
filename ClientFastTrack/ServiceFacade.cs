using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace ClientFastTrack
{
    public static class ServicesFacade
    {
        private static FTPagoElectronicoService.FTPagoElectronicoServicePortTypeClient cliente;
        private static WSHttpBinding _binding;
        private static EndpointAddress _address;
        //private static Context _context;

        private static void EstablecerAtributosConexion(string dirServicio)
        {
            _binding = new WSHttpBinding();

            if (dirServicio.ToLower().Contains("https"))
            {


                _binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
                //_binding.Security.Mode = SecurityMode.Message;
                _binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            }
            else
            {
                _binding.Security.Mode = SecurityMode.None;
            }


            _binding.MaxBufferPoolSize = 2147483647;
            //_binding.MaxBufferSize = 2147483647;
            _binding.MaxReceivedMessageSize = 2147483647;

            _binding.ReaderQuotas.MaxArrayLength = 2147483647;
            _binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            _binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            _binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            _binding.ReaderQuotas.MaxDepth = 32;

            _address = new EndpointAddress(dirServicio);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FTPagoElectronicoService.FTPagoElectronicoServicePortTypeClient ObtenerClienteFTPagoElectronico(string urlConsumo)
        {
            EstablecerAtributosConexion(urlConsumo);
            cliente = new FTPagoElectronicoService.FTPagoElectronicoServicePortTypeClient(_binding, _address);
            EstablecerSeguridadHttps(urlConsumo);
            return cliente;
        }


        private static void EstablecerSeguridadHttps(string dirServicio)
        {

            if (dirServicio.ToLower().Contains("https"))
            {

                X509Certificate certificate;
                ///Busqueda del certificado en el almacen de certificados de Windows.
                using (X509Store store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine))
                {
                    store.Open(OpenFlags.OpenExistingOnly);
                    var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, "0c04c7e505534431bc77fff35951438c", true);
                    certificate = certs.OfType<X509Certificate>().FirstOrDefault();
                }


                ///Set certificado al objeto cliente de comunicación.
                cliente.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(@"C:\Users\852038\Desktop\70-6\apps.fasttrack.com.co.cer");
                cliente.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(certificate);


                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, change, error) =>
                {
                    return true;
                };

            }


        }

        //private static void Auten(X509Certificate2 certificate, string address)
        //{
        //    X509Certificate[] X509Certificates = { certificate };
        //    X509CertificateCollection certsCollection = new X509CertificateCollection(X509Certificates);

        //    SslStream.AuthenticateAsClient(address, certsCollection, SslProtocols.Default, false);
        //}

        //public static bool ValidateCertificate(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors errors)
        //{
        //    if (errors == SslPolicyErrors.None)
        //        return true;
        //    if (certificate != null)
        //    {
        //        string SendingCertificateName = "";
        //        //List<string> Subject = CommaText(certificate.Subject); // decode commalist
        //        // SendingCertificateName = ExtractNameValue(Subject, "CN"); // get the CN= value
        //        report = string.Format(CultureInfo.InvariantCulture, "certificatename : {0}, SerialNumber: {1}, {2}, {3}", certificate.Subject, certificate.GetSerialNumberString(), SendingCertificateName, ServerName);
        //        Console.WriteLine(report);
        //    }

        //    string error= "Certificate error: {0}", errors;
        //    int allow = AllowPolicyErrors << 1;  // AllowPolicyErrors property allowing you to pass certain errors
        //    return (allow & (int)sslPolicyErrors) == (int)sslPolicyErrors;  // or just True if you dont't mind.
        //}
    }

}
