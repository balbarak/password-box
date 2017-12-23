using PasswordBox.Core.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordBox.Web.TagHelpers
{
    public class MetronicEmptyNoteTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "note note-info");

            var h1 = new TagBuilder("h4");
            h1.AddCssClass("block");
            h1.AddCssClass("text-center");
            h1.InnerHtml.AppendHtml($"{MessageText.EmptyData}");

            output.Content.AppendHtml(h1);
        }
    }
}
