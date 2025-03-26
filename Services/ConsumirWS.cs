using Microsoft.OpenApi.Models;
using Modelos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ConsumirWS
    {

        public static HttpStatusCode WSInsertUpdate(string json)
        {
            HttpStatusCode retorno = new();
            try
            {
                retorno = RequestPOSTJSON(json);
                return retorno;
            }
            catch (Exception)
            {
                Logger.Erro(retorno.ToString());
                return HttpStatusCode.BadRequest;
            }
        }

        public static HttpStatusCode RequestPOSTJSON(string jsonObj)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("http://191.253.80.67:5555");
            var request = new RestRequest("/Cliente", Method.Post)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response.StatusCode;
        }




    }
}

