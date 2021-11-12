using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace fortan.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _services = new Services();
            _logger = logger;
        }

        [BindProperty]
        public string Nickname { get; set; } = "";
        
        [BindProperty]
        public string Title { get; set; } = "";
        
        [BindProperty]
        public string Text { get; set; } = "";

        public List<Topic> AllTopics;
        
        private Services _services;

        
        public async Task OnGet()
        {
            AllTopics = await _services.GetAllTopics();
        }

        public async Task<ActionResult> OnPost()
        {
            string id = Guid.NewGuid().ToString("N");
            await _services.PutTopic(new Topic(id, Nickname, Title, Text, new List<string>()));

            return RedirectToPage("/Index");
        }

        public async Task<ActionResult> OnPostTopicSelected(int index)
        {
            AllTopics = await _services.GetAllTopics();
            return RedirectToPage("/TopicView",  new {id = AllTopics[index].Id});
        }
    }
}
