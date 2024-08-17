using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlphaVisa.Web.Areas.Identity.Pages
{
    [AllowAnonymous]
    public class NotFoundModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
