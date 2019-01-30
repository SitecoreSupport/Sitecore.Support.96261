namespace Sitecore.Support.Pipelines.RenderField
{
    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Data.Proxies;
    using Sitecore.Install.Framework;
    using Sitecore.Install.Items;
    using Sitecore.Install.Utils;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Pages;
    using System.Web;
    using Sitecore.Xml;
    using Sitecore.Xml.Xsl;
    using Sitecore.Collections;
    using System.Text;
    using System.Collections.Generic;

    public class GetLinkFieldValue : Sitecore.Pipelines.RenderField.GetLinkFieldValue
    {

        protected override Sitecore.Xml.Xsl.LinkRenderer CreateRenderer(Item item)
        {
            #region Modified Code
            return new Sitecore.Support.Pipelines.RenderField.LinkRenderer(item); //need to return overridden method
            #endregion
        }
    }

    public class LinkRenderer : Sitecore.Xml.Xsl.LinkRenderer
    {
        private readonly char[] _delimiter = new char[] { '=', '&' };
        public LinkRenderer(Item item) : base(item)
        {
        }

        public override RenderFieldResult Render()
        {
            #region Original Code
            string str8;
            SafeDictionary<string> dictionary = new SafeDictionary<string>();
            dictionary.AddRange(base.Parameters);
            if (MainUtil.GetBool(dictionary["endlink"], false))
            {
                return RenderFieldResult.EndLink;
            }
            string[] values = new string[] { "field", "select", "text", "haschildren", "before", "after", "enclosingtag", "fieldname", "disable-web-editing" };
            Set<string> set = Set<string>.Create(values);
            Sitecore.Data.Fields.LinkField linkField = base.LinkField;
            if (linkField != null)
            {
                string[] strArray2 = new string[] { dictionary["title"], linkField.Title };
                dictionary["title"] = HttpUtility.HtmlAttributeEncode(StringUtil.GetString(strArray2));
                string[] strArray3 = new string[] { dictionary["target"], linkField.Target };
                dictionary["target"] = StringUtil.GetString(strArray3);
                string[] strArray4 = new string[] { dictionary["class"], linkField.Class };
                dictionary["class"] = StringUtil.GetString(strArray4);
            }
            string str = string.Empty;
            string rawParameters = base.RawParameters;
            if (!string.IsNullOrEmpty(rawParameters) && (rawParameters.IndexOfAny(this._delimiter) < 0))
            {
                str = rawParameters;
            }
            if (string.IsNullOrEmpty(str))
            {
                Sitecore.Data.Items.Item targetItem = base.TargetItem;
                string str3 = (targetItem != null) ? targetItem.DisplayName : string.Empty;
                string[] strArray5 = new string[] { str, dictionary["text"], (linkField != null) ? linkField.Text : string.Empty, str3 };
                str = StringUtil.GetString(strArray5);
            }
            string url = base.GetUrl(linkField);
            if (((str8 = base.LinkType) == null) || (str8 != "javascript"))
            {
                string[] strArray7 = new string[] { dictionary["href"], url };
                dictionary["href"] = HttpUtility.HtmlEncode(StringUtil.GetString(strArray7));
            }
            else
            {
                dictionary["href"] = "#";
                string[] strArray6 = new string[] { dictionary["onclick"], url };
                dictionary["onclick"] = StringUtil.GetString(strArray6);
            }
            #endregion

            #region Added Code

            //XML Document contains style attribute but needed to add it to dictionary in order to render.

            string style = XmlUtil.GetAttribute("style", base.LinkField.Xml); //gets style attribute key-value as string
            if (!string.IsNullOrEmpty(style))
            {
                string[] strArrayStyle = new string[] { dictionary["style"], style };
                dictionary["style"] = StringUtil.GetString(strArrayStyle); //add style key-value pair in dictionary
            }
            #endregion

            #region Original Code
            StringBuilder tag = new StringBuilder("<a", 0x2f);
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                string key = pair.Key;
                string str7 = pair.Value;
                if (!set.Contains(key.ToLowerInvariant()))
                {
                    AddAttribute(tag, key, str7);
                }
            }
            tag.Append('>');
            if (!MainUtil.GetBool(dictionary["haschildren"], false))
            {
                if (string.IsNullOrEmpty(str))
                {
                    return RenderFieldResult.Empty;
                }
                tag.Append(str);
            }
            return new RenderFieldResult
            {
                FirstPart = tag.ToString(),
                LastPart = "</a>"
            };
            #endregion
        }
    }

    
}