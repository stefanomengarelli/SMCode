using Microsoft.AspNetCore.Mvc.RazorPages;
using SMFrontSystem;

namespace SMFrontSample.Pages
{
    public class TestModel : PageModel
    {
        private readonly ILogger<TestModel> _logger;

        private SMFront SM;

        private void Initialize()
        {
            SM = new SMFront();
        }

        public TestModel(ILogger<TestModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Initialize();
            string s = SM.Configuration.Get("AllowedHosts");
            s = s + ";" + SM.Configuration.Get("Logging.LogLevel.Default");
        }

        public void OnPost()
        {
            Initialize();
        }

    }
}
