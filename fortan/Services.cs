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
        
        public static List<Topic> AllTopics;
        
        public static Topic SelectedTopic;
        public static async Task<List<Topic>> GetAllTopics()
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

            List<Topic> topics;
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                string responseString = await reader.ReadToEndAsync();
                topics = JsonConvert.DeserializeObject<List<Topic>>(responseString);
            }
            
            response.Close();

            return topics;
        }
        
        public static async Task PutTopic(Topic newTopic)
        {
            WebRequest request = WebRequest.Create ("url");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key","key");
            
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                string json = JsonConvert.SerializeObject(newTopic);
                await streamWriter.WriteAsync(json);
            }

            await request.GetResponseAsync();
        }
        
        public static async Task Comment()
        {
            WebRequest request = WebRequest.Create ("URL");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key","KEY");
            
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                string json = "{\"Id\":\"" + SelectedTopic.Id + "\", \"Comments\":" + JsonConvert.SerializeObject(SelectedTopic.Comments) + "}";
                await streamWriter.WriteAsync(json);
            }

            await request.GetResponseAsync();
        }
    }
    
}