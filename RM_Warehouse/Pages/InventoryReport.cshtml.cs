using DAL.CRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace RM_Warehouse.Pages
{
    public class InventoryReportModel : BasePageModel
    {
        [BindProperty]
        public DateTime SearchDate { get; set; }
        [BindProperty]
        public DateTime To_Date { get; set; }
        [BindProperty]
        public string Search_Value { get; set; }
        public string Msg_Search { get; set; }
        public int WarehouseId { get; set; }
        [BindProperty]
        public string Search_Criteria { get; set; }
        public static DataTable? dt_wh_all { get; set; }

        [BindProperty]
        [ValidateNever]
        public static List<WarehouseDetails> warehouseList { get; set; }
        public readonly IConfiguration _configuration;
        public InventoryReportModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            ViewData["ReportURL"] = _configuration.GetValue<string>("ReportURL");
            DateTime dt = DateTime.Now.AddMonths(-1);
            string s2 = dt.ToString("yyyy-MM-ddTHH:mm:ss");
            SearchDate = DateTime.Parse(s2);
            FillWarehouseList();
        }
        public void FillWarehouseList()
        {
            Inhand_Inventory in_inv = new Inhand_Inventory();
            dt_wh_all = in_inv.GetAll_Warehouses();

            warehouseList = new List<WarehouseDetails>();
            for (int i = 0; i < dt_wh_all.Rows.Count; i++)
            {
                WarehouseDetails warehouse = new();
                warehouse.Warehouse_ID = (int)dt_wh_all.Rows[i]["Warehouse_ID"];
                warehouse.Warehouse = dt_wh_all.Rows[i]["Name"].ToString();
                warehouseList.Add(warehouse);
            }

        }
    }
}
