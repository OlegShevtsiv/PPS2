using BookLibrary.ViewModels.Sorting.States;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace SortApp.TagHelpers
{
    public class SortHeaderTagHelper : TagHelper
    {
        public SortEnum Property { get; set; }
        public SortEnum Current { get; set; }
        public string Action { get; set; }
        public bool Up { get; set; }

        private IUrlHelperFactory urlHelperFactory;
        public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            
            string url = urlHelper.Action(Action, new { sortOrder = Property });
            output.Attributes.SetAttribute("href", url);

            switch (Current)
            {
                case SortEnum.NAME_DESC:
                    Current = SortEnum.NAME_ASC;
                    break;
                case SortEnum.SURNAME_ASC:
                    Current = SortEnum.SURNAME_DESC;
                    break;
                case SortEnum.SURNAME_DESC:
                    Current = SortEnum.SURNAME_ASC;
                    break;
                default:
                    Current = SortEnum.NAME_DESC;
                    break;
            }
            if (Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");

                if (Up == true)
                    tag.AddCssClass("fas fa-chevron-up");
                else
                    tag.AddCssClass("fas fa-chevron-down");
                output.PreContent.AppendHtml(tag);
            }
        }
    }
}