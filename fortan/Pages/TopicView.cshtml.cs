using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fortan.Pages
{
    public class TopicView : PageModel
    {
        public Topic SelectedTopic;

        [BindProperty] 
        public string Comment { get; set; }

        public void OnGet()
        {
            SelectedTopic = Services.SelectedTopic;
        }
    }
}