using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Business
{
    public class ddlApiGELBusiness
    {
        public void InsertCiudadanoDBGEL(CiudadanoGELEntities oCiudadano, string CodigoEntidad, string Tramite)
        {
            try
            {
                DetalleContribuyente detalle = new DetalleContribuyente()
                {
                    NombreCompleto = oCiudadano.NombreCompleto,
                    RazonSocial = null,
                    DV = null
                };
                Correo correo = new Correo
                {
                    Email = oCiudadano.Email,
                };
                List<Correo> Correos = new List<Correo>();
                Correos.Add(correo);
                Address direccion = new Address
                {
                    Direccion = oCiudadano.Direccion,
                };
                List<Address> Direcciones = new List<Address>();
                Direcciones.Add(direccion);
                Telefono telefono = new Telefono
                {
                    Numero = oCiudadano.Telefono,
                };
                List<Telefono> Telefonos = new List<Telefono>();
                Telefonos.Add(telefono);
                Contribuyente contribuyente = new Contribuyente
                {
                    Id_TipoDocumento = 1,
                    NroDocumento = oCiudadano.Identificacion,
                    DetalleContribuyente = detalle,
                    Tramite = Tramite,
                    Entidad = CodigoEntidad,
                    Correos = Correos,
                    Telefonos = Telefonos,
                    Direcciones = Direcciones
                };

                Login login = new Login()
                {
                    Username = "Tramites",
                    Password = "1Cero12020$/*",
                };

                string token = login.GetToken(login);
                string Respuesta = string.Empty;
                if (!String.IsNullOrEmpty(token))
                {
                    Respuesta = contribuyente.Guardar(contribuyente, token);
                }

            }
            catch (Exception ex)
            {
            }

        }


        public class Login
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string GetToken(Login login)
            {
                string uri = "http://201.184.190.108:9892/api/";
                try
                {
                    string json = JsonConvert.SerializeObject(login);
                    string respuesta = null;
                    HttpWebRequest cliente = (HttpWebRequest)WebRequest.Create(String.Concat(uri, "Login/authenticate"));
                    cliente.ContentType = "application/json";
                    cliente.Method = "POST";
                    using (StreamWriter sw = new StreamWriter(cliente.GetRequestStream()))
                    {
                        sw.Write(json);
                        sw.Flush();
                        sw.Close();
                    }
                    HttpWebResponse response = (HttpWebResponse)cliente.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        respuesta = sr.ReadToEnd();
                    }
                    return respuesta.Replace("\"", "");

                }
                catch (Exception ex)
                {
                    //throw ex;
                    return "Error en el servicio " + ex.Message;
                }

            }
        }
        public class DetalleContribuyente
        {
            public string NombreCompleto { get; set; }
            public string PrimerNombre { get; set; }
            public string SegundoNombre { get; set; }
            public string PrimerApellido { get; set; }
            public string SegundoApellido { get; set; }
            public string RazonSocial { get; set; }
            public string DV { get; set; }
        }
        public class Correo
        {
            public int Id { get; set; }
            public string Email { get; set; }
        }
        public class Telefono
        {
            public int Id { get; set; }
            public string Numero { get; set; }
            public int Indicativo { get; set; }
        }
        public class Address
        {
            public int Id { get; set; }
            public string Direccion { get; set; }
            public int Id_Municipio { get; set; }
        }
        public class Contribuyente
        {
            public int Id { get; set; }
            public int Id_TipoDocumento { get; set; }
            public string NroDocumento { get; set; }
            public DetalleContribuyente DetalleContribuyente { get; set; }
            public string Tramite { get; set; }
            public string Entidad { get; set; }
            public List<Correo> Correos { get; set; }
            public List<Telefono> Telefonos { get; set; }
            public List<Address> Direcciones { get; set; }
            public string Guardar(Contribuyente c, string token)
            {
                string uri = "http://201.184.190.108:9892/api/";

                string json = JsonConvert.SerializeObject(c);
                string respuesta = null;
                HttpWebRequest cliente = (HttpWebRequest)WebRequest.Create(String.Concat(uri, "Contribuyente/GuardarContribuyente"));
                cliente.Headers.Add("Authorization", "Bearer " + token);
                cliente.ContentType = "application/json";
                cliente.Method = "POST";
                using (StreamWriter sw = new StreamWriter(cliente.GetRequestStream()))
                {
                    sw.Write(json);
                    sw.Flush();
                    sw.Close();
                }
                HttpWebResponse response = (HttpWebResponse)cliente.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    respuesta = sr.ReadToEnd();
                }
                return respuesta;

            }

        }

        public class CiudadanoGELEntities
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public string Identificacion { get; set; }
            public int TipoDocumento { get; set; }
        }
    }
}