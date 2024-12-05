using Microsoft.AspNetCore.Mvc.RazorPages;
using SMFrontSystem;

namespace SMFrontSample.Pages
{

    public class IndexModel : PageModel
    {

        private SMFront SM;

        public IndexModel()
        {
            //
        }

        private void Initialize()
        {
            SM = new SMFront(HttpContext);
        }

        public void OnGet()
        {
            Initialize();
        }

        public void OnPost()
        {
            Initialize();
        }

    }

}
