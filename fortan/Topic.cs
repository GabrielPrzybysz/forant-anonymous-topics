using System;

namespace fortan
{
    [Serializable]
    public class Topic
    {

        public Topic(string id, string author, string title, string text)
        {
            Id = id;
            Author = author;
            Title = title;
            Text = text;
        }

        public string Id { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}