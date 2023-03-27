using HotwiredDotNet.Core.ActionResults;
using HotwiredDotNet.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Components;
using WebApp.Helpers;

namespace WebApp.Pages;

public class SeeOtherPage : PageModel
{
    public void OnGet()
    {
        
    }

    public IActionResult OnPostPageResult()
    {
        //You can pass data between pages using the TempData dictionary as normal
        AlertHelper.SetAlert(TempData, "I'm an alert from page handler");
        AlertHelper.SetAlertType(TempData, AlertType.Warning);
        /*var page = Page();
        page.StatusCode = 303;
        return page;*/
        
        //same as above
        //there is also a version of SetUnprocessableEntityStatusCode for ViewResult
        return Page().SetSeeOtherStatusCode();
    }
 
    public IActionResult OnPost()
    {
        //You can pass data between pages using the TempData dictionary as normal
        AlertHelper.SetAlert(TempData, "I'm an alert from before the redirect");
        AlertHelper.SetAlertType(TempData, AlertType.Warning);
        
        //The see other result allows ud to use the Post/Redirect/Get pattern https://en.wikipedia.org/wiki/Post/Redirect/Get
        //This is necessary since turbo expects form redirects to be 303 and not 301 or 302
        return new SeeOtherResult("SeeOtherPage");
    }
}