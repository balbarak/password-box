using PasswordBox.Core.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordBox.Web.TagHelpers
{
    [HtmlTargetElement("bootstrap-modal")]
    public class BootstrapModalTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-click-hide")]
        public bool IsHideOnClick { get; set; } = true;

        [HtmlAttributeName("asp-content-id")]
        public string ContentId { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("class", "modal fade");
            output.Attributes.Add("tabindex", "-1");
            output.Attributes.Add("aria-hidden", "true");

            if (IsHideOnClick)
                output.Attributes.Add("data-backdrop", "static");

            TagBuilder modalDialog = new TagBuilder("div");
            modalDialog.AddCssClass("modal-dialog");
            modalDialog.TagRenderMode = TagRenderMode.StartTag;

            TagBuilder endDiv = new TagBuilder("div");
            endDiv.TagRenderMode = TagRenderMode.EndTag;



            TagBuilder modalContent = new TagBuilder("div");
            modalContent.AddCssClass("modal-content");

            if (!String.IsNullOrWhiteSpace(ContentId))
                modalContent.Attributes.Add("id", ContentId);

            modalContent.TagRenderMode = TagRenderMode.StartTag;


            output.PreContent.AppendHtml(modalDialog);
            output.PreContent.AppendHtml(modalContent);
            output.PostContent.AppendHtml(endDiv);
            output.PostContent.AppendHtml(endDiv);


            base.Process(context, output);
        }
    }

    [HtmlTargetElement("bootstrap-modal-header")]
    public class BootstrapModalHeaderTagHelper : TagHelper
    {

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "modal-header");

            TagBuilder closeButton = new TagBuilder("button");
            closeButton.Attributes.Add("type", "button");
            closeButton.Attributes.Add("class", "close");
            closeButton.Attributes.Add("data-dismiss", "modal");
            closeButton.Attributes.Add("aria-hidden", "true");

            output.PreContent.AppendHtml(closeButton);

            base.Process(context, output);
        }
    }

    [HtmlTargetElement("bootstrap-modal-body")]
    public class BootstrapModalBodyTagHelper : TagHelper
    {

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "modal-body");

            base.Process(context, output);
        }
    }

    [HtmlTargetElement("bootstrap-modal-footer")]
    public class BootstrapModalFooterTagHelper : TagHelper
    {

        [HtmlAttributeName("asp-show-close-button")]
        public bool ShowCloseButton { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "modal-footer");

            if (ShowCloseButton)
            {
                TagBuilder closeButton = new TagBuilder("button");
                closeButton.AddCssClass("btn btn-danger");
                closeButton.Attributes.Add("data-dismiss", "modal");
                closeButton.InnerHtml.Append($"{CommonText.Close}");

                output.PostContent.AppendHtml(closeButton);
            }

            base.Process(context, output);
        }
    }
}
