using Microsoft.AspNetCore.Mvc;
using Onatrix.Services;
using Onatrix.ViewModels;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Onatrix.Controllers;

public class FormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, FormSubmissionsService formSubmissions) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    private readonly FormSubmissionsService _formSubmissions = formSubmissions;

    public IActionResult HandleCallbackForm(CallbackFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }
        var result = _formSubmissions.SaveCallbackRequest(model);
        if (!result)
        {
           TempData["FormError"] = "There was an error submitting your request. Please try again later.";
            return RedirectToCurrentUmbracoPage();
        }

        TempData["FormSuccess"] = "Your callback request has been submitted successfully.";
        return RedirectToCurrentUmbracoPage();
    }
}
