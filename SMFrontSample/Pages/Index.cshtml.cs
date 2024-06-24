using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMCodeSystem;
using SMFrontSystem;
using System.Text;

namespace SMFrontSample.Pages
{

    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private SMFront SM;
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            SM = new SMFront();
        }

        public void OnGet()
        {

        }

    }

}
