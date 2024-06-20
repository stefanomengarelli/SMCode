using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMCodeSystem;
using System.Text;

namespace SMFrontSample.Pages
{
    public class TestModel : PageModel
    {
        private readonly ILogger<TestModel> _logger;

        private SMCode SM = new SMCode();

        public TestModel(ILogger<TestModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

    }
}
