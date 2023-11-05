using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_153501_Antilevskaya.TagHelpers;
public class Pager:TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly HttpContext _httpContext;

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Category { get; set; }
    public string? CurrentCategory { get; set; }
    public bool Admin { get; set; }

    private RouteValueDictionary GetRouteValues(int page)
    {
        RouteValueDictionary routeValues = null;

        if (Admin)
        {
            routeValues = new RouteValueDictionary() { {"page", page}, };
        }
        else
        {
            routeValues = new RouteValueDictionary() { {"category", CurrentCategory }, 
                                                       { "page", page } };

        }
        return routeValues;
    }

    private string GetUrl(int page)
    {
        return _linkGenerator.GetPathByPage(_httpContext, values: GetRouteValues(page));
    }

    public Pager(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";

        var ulTag = new TagBuilder("ul");
        ulTag.AddCssClass("pagination");

        var previousAvailable = CurrentPage > 1;

        var liPrevious = new TagBuilder("li");
        liPrevious.AddCssClass("page-item");

        var aPagePrevious = new TagBuilder("a");
        aPagePrevious.AddCssClass("page-link");
        aPagePrevious.Attributes["aria-label"] = "Previous";
        if(previousAvailable)
        {
            var previousUrl = GetUrl(CurrentPage - 1);
            aPagePrevious.Attributes["href"] = previousUrl;
        }

        var spanPrevious = new TagBuilder("span");
        spanPrevious.InnerHtml.Append("\u00AB");

        aPagePrevious.InnerHtml.AppendHtml(spanPrevious);
        liPrevious.InnerHtml.AppendHtml(aPagePrevious);
        ulTag.InnerHtml.AppendHtml(liPrevious);

        for (int i = 1; i<=3; i++) 
        { 
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (CurrentPage == i)
            {
                li.AddCssClass("active");
            }

            var aPage = new TagBuilder("a");
            aPage.AddCssClass("page-link");
            var url = GetUrl(i);
            aPage.Attributes["href"] = url;

            aPage.InnerHtml.Append(i.ToString());
            li.InnerHtml.AppendHtml(aPage);
            ulTag.InnerHtml.AppendHtml(li);
        }

        var liNext = new TagBuilder("li");
        liNext.AddCssClass("page-item");

        var nextAvailable = CurrentPage < TotalPages;

        var aPageNext = new TagBuilder("a");
        aPageNext.AddCssClass("page-link");
        aPageNext.Attributes["aria-label"] = "Next";
        if (nextAvailable)
        {
            var nextUrl = GetUrl(CurrentPage + 1);
            aPageNext.Attributes["href"] = nextUrl;
        }
        var spanNext = new TagBuilder("span");
        spanNext.InnerHtml.Append("\u00BB");

        aPageNext.InnerHtml.AppendHtml(spanNext);
        liNext.InnerHtml.AppendHtml(aPageNext);
        ulTag.InnerHtml.AppendHtml(liNext);

        output.Content.AppendHtml(ulTag);
    }
}
