using UnityEngine;
using System;
using System.Net;
using System.IO;

namespace TentuPlay
{
    public class TentuPlayEditorRestApi
    {
        private static string url = "https://api.tentuplay.io/v2021.2/unity/clientkey";


        // Returns null if error, else return client key
        public static string GetClientKey(string apiKey, string secret)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                request.Headers.Add("TPAuthMode", "api_key_plain");
                request.Headers.Add("TPApiKey", apiKey);
                request.Headers.Add("TPSecretKey", secret);

                string responseText = string.Empty;
                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.Log("TPError||Error while getting client key:" + resp.StatusCode.ToString());
                        return null;
                    }
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                TPClientKeyContainer clientKey = JsonUtility.FromJson<TPClientKeyContainer>(responseText);

                if (clientKey.status == 200 || clientKey.status == 201)
                {
                    return clientKey.data.client_key.ToString();
                }
                else
                {
                    Debug.Log("TPError||Error while creating client key:" + clientKey.reason);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.Log("TPError||Error while creating client key:" + e.ToString());
                return null;
            }
        }
    }

    [Serializable]
    public class TPClientKeyContainer
    {
        public int status;
        public string reason;
        public TPClientKey data;
    }
}