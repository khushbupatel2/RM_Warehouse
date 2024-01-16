using Irony.Parsing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RM_Warehouse.Pages
{
    public class WelcomeModel : BasePageModel
    {
        // THIS FUNCTION IS CALLED FROM LEFT MENU BAR'S LOGOUT OPTION.IT CLEARS SESSIONS.IT REDIRECTS TO LOGIN PAGE.

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("JWToken");
            
            HttpContext.Session.Clear();
            
            return RedirectToPage("Index");
        }

        // THIS PAGE IS REDIRECTED FROM LOGIN PAGE.IT CHECKS SESSION PARAMETER username.IF FOUND SHOWS THIS WELCOME
        // PAGE.ELSE,REDIRECT BACK TO LOGIN PAGE.

        public IActionResult OnGet()
        {
           
            return Page();
        }
    }
}

