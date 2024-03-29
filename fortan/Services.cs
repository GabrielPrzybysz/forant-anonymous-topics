﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace fortan
{
    public class Services
    {
        public async Task<List<Topic>> GetAllTopics()
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
        
        public async Task<Topic> GetSingleTopic(string id)
        {
            WebRequest request = WebRequest.Create ("url");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key","key");
            
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                string json = "{\"Id\":\"" + id + "\"}";
                await streamWriter.WriteAsync(json);
            }

            WebResponse response = await request.GetResponseAsync();

            Topic topic;
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                string responseString = await reader.ReadToEndAsync();
                topic = JsonConvert.DeserializeObject<Topic>(responseString);
            }
            
            response.Close();

            return topic;
        }
        
        public async Task PutTopic(Topic newTopic)
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
        
        public async Task Comment(string id, List<string> comments)
        {
            WebRequest request = WebRequest.Create ("url");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key","key");
            
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                string json = "{\"Id\":\"" + id + "\", \"Comments\":" + JsonConvert.SerializeObject(comments) + "}";
                await streamWriter.WriteAsync(json);
            }

            await request.GetResponseAsync();
        }
    }
    
}