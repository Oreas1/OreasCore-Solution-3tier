
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore.Custom_Classes
{
    public enum MyCSSClassName
    {
        RowInForm,
        RowInForm_FormGroup12,
        RowInForm_FormGroup9,
        RowInForm_FormGroup8,
        RowInForm_FormGroup6,
        RowInForm_FormGroup5,
        RowInForm_FormGroup4,
        RowInForm_FormGroup3,
        RowInForm_FormGroup2,
        RowInForm_FormGroup1,
        RowInForm_FormGroupSubmit,
        RowInForm_FormGroup_Label,
        RowInForm_FormGroup_Label_Req,
        RowInForm_FormGroup_Input,
        RowInForm_FormGroup_Select,
        RowInForm_FormGroup_Range,
        RowInForm_FormGroupSubmit_btn,
        RowInForm_FormGroupSubmit_btn_Modal,
        //---------------------Main Page table which have operations----------------//
        Table_Div,
        Table_Table,
        Table_OptColDiv,
        Table_NavDiv,
        Table_OperationBtn,
        //---------------------Sub Page table which acts as parent data----------------//
        Table_DivSubPage,
        Table_TableSubPage

    }


    [HtmlTargetElement(Attributes = "mycssclass")]
    public class setCustomClassOnHTML : TagHelper
    {
        //----------those elements which have attribute MyCSSClassName="@MyCSSClassName.BtnAddNew"-----------//
        public MyCSSClassName mycssclass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("class");
            if (mycssclass == MyCSSClassName.RowInForm)
                output.Attributes.SetAttribute("class", "row col-lg-12 mimWidthDiv250 mt-2");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup12)
                output.Attributes.SetAttribute("class", "form-group col-lg-12 col-md-12");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup9)
                output.Attributes.SetAttribute("class", "form-group col-lg-9 col-md-9");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup8)
                output.Attributes.SetAttribute("class", "form-group col-lg-8 col-md-8");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup6)
                output.Attributes.SetAttribute("class", "form-group col-lg-6 col-md-6");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup5)
                output.Attributes.SetAttribute("class", "form-group col-lg-5 col-md-5");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup4)
                output.Attributes.SetAttribute("class", "form-group col-lg-4 col-md-6");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup3)
                output.Attributes.SetAttribute("class", "form-group col-lg-3 col-md-6 col-sm-6");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup2)
                output.Attributes.SetAttribute("class", "form-group col-lg-2 col-md-4 col-sm-4");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup1)
                output.Attributes.SetAttribute("class", "form-group col-lg-1 col-md-2 col-sm-2");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroupSubmit)
                output.Attributes.SetAttribute("class", "form-group");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup_Label)
                output.Attributes.SetAttribute("class", "input-group-text LabelTextNotRequired");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup_Label_Req)
                output.Attributes.SetAttribute("class", "input-group-text LabelTextRequired");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup_Input)
                output.Attributes.SetAttribute("class", "form-control");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup_Select)
                output.Attributes.SetAttribute("class", "form-select");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroup_Range)
                output.Attributes.SetAttribute("class", "form-range form-control");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroupSubmit_btn)
                output.Attributes.SetAttribute("class", "btn btn-dark");
            else if (mycssclass == MyCSSClassName.RowInForm_FormGroupSubmit_btn_Modal)
                output.Attributes.SetAttribute("class", "btn btn-primary float-end");
            else if (mycssclass == MyCSSClassName.Table_Div)
                output.Attributes.SetAttribute("class", "table-responsive mt-2");
            else if (mycssclass == MyCSSClassName.Table_Table)
                output.Attributes.SetAttribute("class", "table pagedData_table");
            else if (mycssclass == MyCSSClassName.Table_OptColDiv)
                output.Attributes.SetAttribute("class", "d-flex align-items-center justify-content-center");
            else if (mycssclass == MyCSSClassName.Table_NavDiv)
                output.Attributes.SetAttribute("class", "btn-group");
            else if (mycssclass == MyCSSClassName.Table_OperationBtn)
                output.Attributes.SetAttribute("class", "btn");
            else if (mycssclass == MyCSSClassName.Table_DivSubPage)
                output.Attributes.SetAttribute("class", "table-responsive mt-2");
            else if (mycssclass == MyCSSClassName.Table_TableSubPage)
                output.Attributes.SetAttribute("class", "table masterData_table");
        }

    }

    [HtmlTargetElement("MyFieldSet")]
    public class TagMyFieldSet : TagHelper
    {
        //----------New HTML Tag name <MyFieldSet><MyFieldSet/>-----------//
        public string legendtext { get; set; }

        public bool child { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "fieldset";
            output.TagMode = TagMode.StartTagAndEndTag;
            if(child)
                output.Attributes.SetAttribute("class", "border p-2 mt-2 FieldSetChild");
            else
                output.Attributes.SetAttribute("class", "border p-2 mt-2 FieldSetParent");
            // output.Attributes.SetAttribute("style", "margin-bottom: 5px; margin-top: 5px;");

            var legend = new TagBuilder("legend");
            legend.Attributes.Add("class", "w-auto");
            legend.InnerHtml.Append(legendtext);
            output.PreContent.AppendHtml(legend);
        }
    }

    [HtmlTargetElement("MyAntiforgeryToken")]
    public class TagMyAntiforgeryToken : TagHelper
    {
        //----------New HTML Tag name <MyAntiforgeryToken><MyAntiforgeryToken/>-----------//
        private readonly string _token;
        public TagMyAntiforgeryToken(Microsoft.AspNetCore.Antiforgery.IAntiforgery Xsrf, Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor)
        {
            _token = Xsrf.GetAndStoreTokens(contextAccessor.HttpContext).RequestToken;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("type", "hidden");
            output.Attributes.SetAttribute("id", "antiForgeryToken");
            output.Attributes.SetAttribute("data-ng-model", "antiForgeryToken");            
            output.Attributes.SetAttribute("data-ng-init", $"antiForgeryToken='{_token}'");
        }
    }

    [HtmlTargetElement("MyViewName")]
    public class TagMyViewName : TagHelper
    {
        public bool child { get; set; }
        //----------New HTML Tag name <MyViewName><MyViewName/>-----------//

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (child)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.Add("style", "font-size:small;");
                output.PreContent.AppendHtml(@"<span class=""fa fa-arrow-right""></span>");
            }
            else
            {
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.Add("style", "font-size:large;");
            }
            
        }
    }


    [HtmlTargetElement("MyValidationSpan")]
    public class TagMyValidationSpan : TagHelper
    {
        //----------New HTML Tag name <MyFieldSet><MyFieldSet/>-----------//
        public string formname { get; set; }
        public string fieldname { get; set; }
        public bool required { get; set; }
        public int maxlength { get; set; }
        public int minlength { get; set; }
        public string pattern { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {           
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("style", "color:red;");
            output.Attributes.SetAttribute("ng-show",
                formname + "['" + fieldname + "']" + ".$dirty" + " && " + formname + "['" + fieldname + "']" + ".$invalid"
                );
            output.PreContent.AppendHtml("Invalid: ");    

            if (required)
            { 
                output.PreContent.AppendHtml(
                    "<span ng-show=\""+ formname + "['" + fieldname + "']" + ".$error.required" + "\">"+ "Field is Required" 
                    + "</span>");   
            }
            if (maxlength>0)
            {
                output.PreContent.AppendHtml(
                    "<span ng-show=\"" + formname + "['" + fieldname + "']" + ".$error.maxlength" + "\">" + "Field should be max: " + maxlength.ToString() + " Characters" 
                    + "</span>");
            }
            if (minlength>0)
            {
                output.PreContent.AppendHtml(
                    "<span ng-show=\"" + formname + "['" + fieldname + "']" + ".$error.minlength" + "\">" + "Field should be min: " + minlength.ToString() + " Characters"
                    + "</span>");
            }
            if (!string.IsNullOrEmpty(pattern))
            {
                output.PreContent.AppendHtml(
                    "<span ng-show=\"" + formname + "['" + fieldname + "']" + ".$error.pattern" + "\">" + "Combination; " + pattern
                    + "</span>");
            }

        }
    }

    ////--------------------------Custom Input-------------------------------------////

    public enum MyButtonOperation
    {
        Add,
        CancelOfAdd,
        AddDetail,
        Edit,
        Delete,
        View,
        Copy,
        first,
        last,
        next,
        back,
        RecordNo,
        ModalItemSelection
    }

    [HtmlTargetElement("MyButton")]
    public class TagMyInputButton : TagHelper
    {
        //----------New HTML Tag name <MyInputEdit><MyInputEdit/>-----------//

        public MyButtonOperation operation { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (operation == MyButtonOperation.Add)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Add OR Cancel Record");
                output.Attributes.SetAttribute("class", "btn btn-primary ngHideCustomAnimation");

                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanAdd && ng_entryPanelBtnText!='Cancel'");
                
                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");
                
                if (!output.Attributes.Any(w => w.Name == "ng-click"))
                    output.Attributes.SetAttribute("ng-click", @"showEntryPanel(); myform.$setPristine();");
                
                output.Content.AppendHtml(@"{{ng_entryPanelBtnText}}");
            }
            else if (operation == MyButtonOperation.CancelOfAdd)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Cancel Record");
                output.Attributes.SetAttribute("class", "btn btn-primary no-animate");

                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"ng_entryPanelBtnText!='Cancel'");

                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");
                
                if (!output.Attributes.Any(w => w.Name == "ng-click"))
                    output.Attributes.SetAttribute("ng-click", @"showEntryPanel(); myform.$setPristine();");
                
                output.Content.AppendHtml(@"{{ng_entryPanelBtnText}}");
            }
            else if (operation == MyButtonOperation.AddDetail)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Add Record");
                output.Attributes.SetAttribute("class", "btn");
            

                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");

                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanAdd");
                else
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanAdd"+ " || ("+ output.Attributes.Where(w => w.Name == "ng-hide").FirstOrDefault().Value + ")");
               
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@" <i class='fa fa-plus' style='color: steelblue;'></i>");
            
                
            }
            else if (operation == MyButtonOperation.Edit)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Edit Record");
                output.Attributes.SetAttribute("class", "btn");

                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");

                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanEdit");
                else
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanEdit" + " || (" + output.Attributes.Where(w => w.Name == "ng-hide").FirstOrDefault().Value + ")");

                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-edit' style='color: steelblue;'></i>");
            }
            else if (operation == MyButtonOperation.Delete)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Delete Record");
                output.Attributes.SetAttribute("class", "btn");

                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");
                
                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanDelete");
 
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-xmark-square' style='color: darkred;'></i>");
            }
            else if (operation == MyButtonOperation.View)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "View Record");
                output.Attributes.SetAttribute("class", "btn");

                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");

                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanView");

                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-eye' style='color: darkgreen;'></i>");
            }
            else if (operation == MyButtonOperation.Copy)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Copy Record");
                output.Attributes.SetAttribute("class", "btn");

                if (!output.Attributes.Any(w => w.Name == "ng-disabled"))
                    output.Attributes.SetAttribute("ng-disabled", "ng_DisabledBtnAll");

                if (!output.Attributes.Any(w => w.Name == "ng-hide"))
                    output.Attributes.SetAttribute("ng-hide", @"!Privilege.CanAdd");

                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-copy' style='color: darkgoldenrod;'></i>");
            }
            else if (operation == MyButtonOperation.first)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Move First");
                output.Attributes.SetAttribute("class", "btn btn-primary");
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-backward-fast'></i> First");
            }
            else if (operation == MyButtonOperation.back)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Move Back");
                output.Attributes.SetAttribute("class", "btn btn-primary");
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-backward-step'></i> Back");
            }
            else if (operation == MyButtonOperation.next)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Move Next");
                output.Attributes.SetAttribute("class", "btn btn-primary");
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-forward-step'></i> Next");
            }
            else if (operation == MyButtonOperation.last)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Move Last");
                output.Attributes.SetAttribute("class", "btn btn-primary");
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@"<i class='fa fa-forward-fast'></i> Last");
            }
            else if (operation == MyButtonOperation.RecordNo)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "Record No");
                output.Attributes.SetAttribute("class", "btn btn-light");
            }
            else if (operation == MyButtonOperation.ModalItemSelection)
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.SetAttribute("title", "click to select item");
                output.Attributes.SetAttribute("class", "btn focusBtn");
                output.Attributes.SetAttribute("style", "max-width:inherit;");
                output.Content = output.GetChildContentAsync().Result.AppendHtml(@" <i class='fa fa-hand-point-left'></i>");



            }
        }
    }


    [HtmlTargetElement("MyButtonModal")]
    public class TagMyButtonModal : TagHelper
    {
        //----------New HTML Tag name <MyInputEdit><MyInputEdit/>-----------//

        public string fieldname { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("type", "button");
            output.Attributes.SetAttribute("title", "click to search");
            output.Attributes.SetAttribute("class", "form-control text-start");
            output.Attributes.SetAttribute("id", fieldname);
            output.Attributes.SetAttribute("name", fieldname);
            output.Attributes.SetAttribute("value", "{{" + fieldname + "}}");
            output.Attributes.SetAttribute("data-ng-model", fieldname);
        }
    }


    ////---------------------------------***************************************----------------------------------////
    ///
    //var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;
    //output.Attributes.SetAttribute("class", $"{classes} form-control");

    //if (context.AllAttributes["class"] != null)
    //{
    //    var a = context.AllAttributes["class"].Value.ToString();
    //}
    #region demo
    //[HtmlTargetElement(Attributes = "myclass")]
    //public class BackgroundColorTH : TagHelper
    //{
    //    public string myclass { get; set; }

    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        output.Attributes.SetAttribute("class", $"btn btn-primary");
    //    }
    //}




    //[HtmlTargetElement("myInput")]
    //public class AspButton1TH : TagHelper
    //{


    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        output.TagName = "input";
    //        output.TagMode = TagMode.StartTagAndEndTag;
    //        output.Attributes.SetAttribute("type", "text");
    //        //output.Attributes.Add("ng-model", "ovais");
    //        //output.Content.SetHtmlContent("hello");

    //    }
    //}



    //[HtmlTargetElement("input", Attributes = "indicator")]
    //public class movais : TagHelper
    //{

    //    public string indicatorclass { get; set; }

    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        output.Attributes.SetAttribute("class", "btn btn-primary");
    //        output.Attributes.Add("value", "ok");
    //    }

    //}

    #endregion
}
