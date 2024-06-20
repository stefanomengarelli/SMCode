using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMCode;
using System.Text;

namespace SMFrontSample.Pages
{
    public class TestModel : PageModel
    {
        private readonly ILogger<TestModel> _logger;

        private SMApplication SM = new SMApplication();

        public TestModel(ILogger<TestModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

    }
}
