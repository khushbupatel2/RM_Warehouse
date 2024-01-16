using DAL.CRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace RM_Warehouse.Pages
{
    // THIS CLASS IS FOR 0 INVENTORY - REPORT PAGE.
    public class _0_InventoryModel : BasePageModel
    {
        [BindProperty]
        public static DataTable IVT { get; set; }
        public IActionResult OnGet()
        {

           
            // THIS CODE CALLS INHAND_INVENORY.CS FUNCTION Get_0_Inventory TO GET 0 INVENTORY ITEMS. 

            Inhand_Inventory inv = new Inhand_Inventory();
            string warehouse = BaseWarehouse;

            IVT = inv.Get_0_Inventory(warehouse);
            
            return Page();

        }
    }
}
