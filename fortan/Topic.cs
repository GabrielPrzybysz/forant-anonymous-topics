using System;
using System.Collections.Generic;

namespace fortan
{
    [Serializable]
    public class Topic
    {

        public Topic(string id, string author, string title, string text , List<string> comments)
        {
            Id = id;
            Author = author;
            Title = title;
            Text = text;
            Comments = comments;
        }
        
        public string Id { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public List<string> Comments { get; set; }
    }
}