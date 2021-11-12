using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fortan.Pages
{
    public class TopicView : PageModel
    {
        [BindProperty] 
        public string Comment { get; set; }
        
        public async Task<ActionResult> OnPost()
        {
            Services.SelectedTopic.Comments.Add(Comment);
            await Services.Comment();

            return RedirectToPage("TopicView");
        }
    }
}