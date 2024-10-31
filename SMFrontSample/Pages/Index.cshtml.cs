using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMCodeSystem;
using SMFrontSystem;
using System.Text;

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
