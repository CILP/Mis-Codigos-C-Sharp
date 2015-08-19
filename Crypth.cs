using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.IO;

namespace RSACrypth
{
    public class RSAAlgorithm //: IAsimetrico
    {

        private static RSACryptoServiceProvider rsa;
        private const string _NOMBRE_CONTENEDOR = "ContenedorRSA";
        private static string _rutaLlavePrivada;
        private static string _rutaLlavePublica;

        public static void SetKeys(string rutaLlavePrivada, string rutaLlavePublica)
        {
            _rutaLlavePrivada = rutaLlavePrivada;
            _rutaLlavePublica = rutaLlavePublica;
        }

        private static void ConstructorRSA()
        {
            CspParameters cspParametros;
            cspParametros = new CspParameters(1);
            cspParametros.Flags = CspProviderFlags.UseDefaultKeyContainer;
            cspParametros.KeyContainerName = _NOMBRE_CONTENEDOR;
            rsa = new RSACryptoServiceProvider(cspParametros);
        }
        
        static RSAAlgorithm()
        {
            ConstructorRSA();
        }

        public static string Desencriptar(string input)
        {
            string mensajeDescifrado = String.Empty;
            byte[] textoCifradoBytes = Convert.FromBase64String(input);

            // Se carga la llave para desencriptar
            CargarLlavePrivada();

            byte[] textoPlanoBytes = rsa.Decrypt(textoCifradoBytes, false);

            mensajeDescifrado = System.Text.Encoding.UTF8.GetString(textoPlanoBytes);

            return mensajeDescifrado;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string Encriptar(string input)
        {
            string mensajeCifrado = String.Empty;

            byte[] textoPlanoBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // byte[] textoPlanoBytes = GetBytes(input);

            // Cargar llave publica
            CargarLlavePublica();

            byte[] textoCifradoBytes = rsa.Encrypt(textoPlanoBytes, false);
            
            mensajeCifrado = Convert.ToBase64String(textoCifradoBytes);
            Console.WriteLine(mensajeCifrado);

            return mensajeCifrado;
        }

        // TODO: Definir la función
        public static bool GenerarLlavePrivada()
        {
            return true;
        }

        // TODO: Definir la función
        public static bool GenerarLlavePublica()
        {
            return true;
        }

        // TODO: Agregar referencia de lectura por web.config
        private static bool CargarLlavePrivada()
        {
            bool success = false;

            try
            {
                ConstructorRSA();
                StreamReader readerPrivada = new StreamReader(_rutaLlavePrivada);
                string publicPrivateKeyXML = readerPrivada.ReadToEnd();
                rsa.FromXmlString(publicPrivateKeyXML);
                readerPrivada.Close();
                success = true;
            }
            catch(Exception)
            {
                success = false;
            }

            return success;
        }

        // TODO: Agregar referencia de lectura por web.config
        private static bool CargarLlavePublica()
        {
            bool success = false;

            try
            {
                ConstructorRSA();
                StreamReader readerPublica = new StreamReader(_rutaLlavePublica);
                string publicOnlyKeyXML = readerPublica.ReadToEnd();
                rsa.FromXmlString(publicOnlyKeyXML);
                readerPublica.Close();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }
    }
}

