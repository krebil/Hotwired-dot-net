using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotwiredDotNet.Core.Helpers;

public static class PageHelpers
{
    public static PageResult UnprocessableEntity(this PageResult pageResult)
    {
        pageResult.StatusCode = 422;
        return pageResult;
    }
    public static PageResult SeeOther(this PageResult pageResult)
    {
        pageResult.StatusCode = 303;
        return pageResult;
    }
    public static ViewResult UnprocessableEntity(this ViewResult viewResult)
    {
        viewResult.StatusCode = 422;
        return viewResult;
    }
    public static ViewResult SeeOther(this ViewResult viewResult)
    {
        viewResult.StatusCode = 303;
        return viewResult;
    }
}