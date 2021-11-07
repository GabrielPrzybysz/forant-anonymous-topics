using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace fortan
{
    public static class Services
    {
        public static async Task<List<string>> GetAllTitles()
        {
            WebRequest request = WebRequest.Create ("url");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key","key");
            
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                string json = "{\"Id\":\"/\"}";
                await streamWriter.WriteAsync(json);
            }

            WebResponse response = await request.GetResponseAsync();

            List<string> titles;
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                string responseString = await reader.ReadToEndAsync();
                titles = JsonConvert.DeserializeObject<List<string>>(responseString);
            }
            
            response.Close();

            return titles;
        }
    }
}