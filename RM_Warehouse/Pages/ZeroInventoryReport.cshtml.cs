using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RM_Warehouse.Pages
{
    public class ZeroInventoryReportModel : BasePageModel
    {
        public readonly IConfiguration _configuration;
        public ZeroInventoryReportModel(IConfiguration configuration) {
            _configuration = configuration;

        }
        [BindProperty]
        public string ItemCode { get; set; }
        public void OnGet()
        {
            ViewData["ReportURL"] = _configuration.GetValue<string>("ReportURL");
        }
    }
}
