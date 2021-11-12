using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace fortan.Pages
{
    public class TopicView : PageModel
    {
        
        private readonly ILogger<TopicView> _logger;

        public TopicView(ILogger<TopicView> logger)
        {
            _services = new Services();
            _logger = logger;
        }
        
        [BindProperty] 
        public string Comment { get; set; }

        public Topic SelectedTopic;

        private Services _services;
        
        public async Task OnGet(string id)
        {
            SelectedTopic = await _services.GetSingleTopic(id);
        }

        public async Task<ActionResult> OnPost(string id)
        {
            SelectedTopic = await _services.GetSingleTopic(id);
            SelectedTopic.Comments.Add(Comment);
            await _services.Comment(SelectedTopic.Id, SelectedTopic.Comments);

            return RedirectToPage("TopicView", new {id = SelectedTopic.Id});
        }
    }
}