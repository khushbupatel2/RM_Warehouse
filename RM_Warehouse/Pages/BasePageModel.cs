using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.DirectoryServices;
using System.Security.Claims;

namespace RM_Warehouse.Pages
{
   
    public class BasePageModel : PageModel
    {
        public BasePageModel()
        {

        }
        string _UserName;
        public string BaseUserName
        {
            get { return _UserName; }
        }
        string _warehouse;
        public string BaseWarehouse
        {
            get { return _warehouse; } 
        }
        string _EmailId;
        public string BaseEmailId
        { get { return _EmailId; } }
		string _password;
		public string BasePassword
		{ get { return _password; } }

		public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }
        public async override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var data = context.HttpContext.User.Identity;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            var unth = false;
           
            //var identity = identitysdata as ClaimsIdentity;
            if (identity.Claims.Count() > 0)
            {
                var userClaims = identity.Claims;
                var pass = "";
                var role = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Anonymous)?.Value != null)
                {
                    pass = EncryptDecrypt.Decrypt(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Anonymous)?.Value);
                }
                bool isAuthenticated = false;
                
                isAuthenticated = AuthorizeUser(EncryptDecrypt.Decrypt(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value), pass);

               
                if (isAuthenticated)
                {
                   
                        if (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value != null)
                        {
                            ViewData["BaseUserName"] = EncryptDecrypt.Decrypt(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                            _UserName = EncryptDecrypt.Decrypt(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                        }
                        if (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value != null)
                        {
                            _EmailId = EncryptDecrypt.Decrypt(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
                        }
                        if (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != null)
                        {
                            ViewData["BaseRole"] = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                            _warehouse = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                        }
					if (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Anonymous)?.Value != null)
					{
						_password =userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Anonymous)?.Value;
					}
				

					await next.Invoke();
				}
                else
                {
                    if (isAjax)
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        context.HttpContext.Response.Headers.Add("user-agent", "Index");
                    }
                    else
                    {
                        //context.Result = RedirectToPage("/Index", new { area = "BusinessHost" });
                        context.Result = new RedirectResult("Index");
                    }
                }
            }
            else
            {
                if (isAjax)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.HttpContext.Response.Headers.Add("user-agent", "Index");
                }
                else
                {
                    //context.Result = RedirectToPage("/Index", new { area = "BusinessHost" });
                    context.Result = new RedirectResult("Index");
                }
                //context.Result= context.HttpContext.Response.Redirect("Index", true);
                //context.HttpContext.Response.Redirect("Index",true);
            }
        }

        public bool AuthorizeUser(string Username, string pass)
        {
            string domainAdnUserName = @"HEI\" + Username;
#pragma warning disable CA1416 // Validate platform compatibility
            DirectoryEntry entry = new("LDAP://local.hunterexpress.ca", domainAdnUserName, pass, AuthenticationTypes.None);

            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + Username + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("memberOf");
                SearchResult result = search.FindOne();
                string key = "username";


                if (null == result)
                {
                    return false;

                }
                else
                {
                    foreach (string GroupPath in result.Properties["memberOf"])
                    {
                        if (GroupPath.Contains("RM RECORD")) //if (GroupPath.Contains("RM RECORD"))
                        {
                            return true;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.ToUpper());
                return false;
            }
            return false;
        }

    }
}
