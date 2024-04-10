﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Helpers
{
    /// <summary>    
    /// This is an HTML cleanup utility combining the benefits of the    
    /// HtmlAgilityPack to parse raw HTML and the AntiXss library    
    /// to remove potentially dangerous user input.    
    ///    
    /// Additionally it uses a list to limit    
    /// the number of allowed tags and attributes to a sensible level    
    /// </summary>    
    /// 
    public sealed class HtmlUtility
    {
        private static volatile HtmlUtility instance;
        private static object root = new object();
        private HtmlUtility() { }
        public static HtmlUtility Instance
        {
            get
            {
                if (instance == null)
                    lock (root)
                        if (instance == null)
                            instance = new HtmlUtility();
                return instance;
            }
        }


        private static readonly Dictionary<string, string[]> ValidHtmlTags =
            new Dictionary<string, string[]>
        {

        {"p", new string[]
        {"style", "class", "align"}},
        {"div", new string[]
        {"style", "class", "align"}},
        {"span", new string[]       {"style", "class"}},
        {"br", new string[]         {"style", "class"}},
        {"hr", new string[]         {"style", "class"}},
        {"label", new string[]      {"style", "class"}},
        {"h1", new string[]         {"style", "class"}},
        {"h2", new string[]         {"style", "class"}},
        {"h3", new string[]         {"style", "class"}},
        {"h4", new string[]         {"style", "class"}},
        {"h5", new string[]         {"style", "class"}},
        {"h6", new string[]         {"style", "class"}},
        {"font", new string[]       {"style", "class", "color", "face", "size"}},
        {"strong", new string[]     {"style", "class"}},
        {"b", new string[]          {"style", "class"}},
        {"em", new string[]         {"style", "class"}},
        {"i", new string[]          {"style", "class"}},
        {"u", new string[]          {"style", "class"}},
        {"strike", new string[]     {"style", "class"}},
        {"ol", new string[]         {"style", "class"}},
        {"ul", new string[]         {"style", "class"}},
        {"li", new string[]         {"style", "class"}},
        {"blockquote", new string[] {"style", "class"}},
        {"code", new string[]       {"style", "class"}},
        {"pre", new string[]       {"style", "class"}},
        {"a", new string[]          {"style", "class", "href", "title"}},
        {"img", new string[]        {"style", "class", "src", "height", "width", "alt", "title", "hspace", "vspace", "border"}},
        {"table", new string[]      {"style", "class"}},
        {"thead", new string[]      {"style", "class"}},
        {"tbody", new string[]      {"style", "class"}},
        {"tfoot", new string[]      {"style", "class"}},
        {"th", new string[]         {"style", "class", "scope"}},
        {"tr", new string[]         {"style", "class"}},
        {"td", new string[]         {"style", "class", "colspan"}},
        {"q", new string[]          {"style", "class", "cite"}},
        {"cite", new string[]       {"style", "class"}},
        {"abbr", new string[]       {"style", "class"}},
        {"acronym", new string[]    {"style", "class"}},
        {"del", new string[]        {"style", "class"}},
        {"ins", new string[]        {"style", "class"}}        };

        /// <summary>        
        /// Takes raw HTML input and cleans against a whitelist        
        /// </summary>        
        /// <param name="source">Html source</param>        
        /// <returns>Clean output</returns>       
        public string SanitizeHtml(string source)
        {
            HtmlDocument html = GetHtml(source);
            if (html == null) return String.Empty;
            // All the nodes            
            HtmlNode allNodes = html.DocumentNode;
            // Select whitelist tag names            
            string[] whitelist = (from kv in ValidHtmlTags
                                  select kv.Key).ToArray();
            // Scrub tags not in whitelist           
            CleanNodes(allNodes, whitelist);
            // Filter the attributes of the remaining           
            foreach (KeyValuePair<string, string[]> tag in ValidHtmlTags)
            {
                IEnumerable<HtmlNode> nodes = (from n in allNodes.DescendantsAndSelf()
                                               where n.Name == tag.Key
                                               select n);
                // No nodes? Skip.               
                if (nodes == null) continue;
                foreach (var n in nodes)
                {
                    // No attributes? Skip.                   
                    if (!n.HasAttributes) continue;
                    // Get all the allowed attributes for this tag                   
                    HtmlAttribute[] attr = n.Attributes.ToArray();
                    foreach (HtmlAttribute a in attr)
                    {
                        if (!tag.Value.Contains(a.Name))
                        {
                            a.Remove();  // Attribute wasn't in the whitelist                       
                        }
                        else
                        {
                            // *** New workaround. This wasn't necessary with the old library                           
                            if (a.Name == "href" || a.Name == "src")
                            {
                                a.Value = (!string.IsNullOrEmpty(a.Value)) ? a.Value.Replace(oldValue: "\r", newValue: "").Replace(oldValue: "\n", newValue: "") : "";
                                a.Value =
                                    (!string.IsNullOrEmpty(a.Value) &&
                                    (a.Value.IndexOf(value: "javascript") < 10 || a.Value.IndexOf(value: "eval") < 10)) ?
                                    a.Value.Replace(oldValue: "javascript", newValue: "").Replace(oldValue: "eval", newValue: "") : a.Value;
                            }
                            else if (a.Name == "class" || a.Name == "style")
                            {
                                a.Value =
                                    Microsoft.Security.Application.Encoder.CssEncode(a.Value);
                            }
                            else
                            {
                                a.Value =
                                    Microsoft.Security.Application.Encoder.HtmlAttributeEncode(a.Value);
                            }
                        }
                    }
                }
            }

            return allNodes.InnerHtml;

        }

        /// <summary>        
        /// Takes a raw source and removes all HTML tags        
        /// </summary>       
        /// 
        /// <param name="source"></param>        
        /// <returns></returns>       
        public string StripHtml(string source)
        {
            source = SanitizeHtml(source);
            // No need to continue if we have no clean Html            
            if (String.IsNullOrEmpty(source))
                return String.Empty;
            HtmlDocument html = GetHtml(source);
            StringBuilder result;
            result = new StringBuilder();
            // For each node, extract only the innerText           
            foreach (HtmlNode node in html.DocumentNode.ChildNodes)
                result.Append(node.InnerText);
            return result.ToString();
        }

        /// <summary>        
        /// Recursively delete nodes not in the whitelist       
        /// /// </summary>        
        private static void CleanNodes(HtmlNode node, string[] whitelist)
        {
            if (node.NodeType == HtmlNodeType.Element)
            {
                if (!whitelist.Contains(node.Name))
                {
                    node.ParentNode.RemoveChild(node);
                    return;
                }
            }
            if (node.HasChildNodes)
                CleanChildren(node, whitelist);
        }

        /// <summary>       
        /// Apply CleanNodes to each of the child nodes       
        /// </summary>       
        private static void CleanChildren(HtmlNode parent, string[] whitelist)
        {
            for (int i = parent.ChildNodes.Count - 1; i >= 0; i--)
                CleanNodes(parent.ChildNodes[i], whitelist);
        }
        /// <summary>      
        /// Helper function that returns an HTML document from text       
        /// </summary>        
        private static HtmlDocument GetHtml(string source)
        {
            HtmlDocument html;
            html = new HtmlDocument();
            html.OptionFixNestedTags = true;
            html.OptionAutoCloseOnEnd = true;
            html.OptionDefaultStreamEncoding = Encoding.UTF8;
            html.LoadHtml(source);
            // Encode any code blocks independently so they won't         
            // be stripped out completely when we do a final cleanup         
            foreach (var n in html.DocumentNode.DescendantsAndSelf())
            {
                if (n.Name == "code")
                {
                    //** Code tag attribute vulnerability fix                 
                    HtmlAttribute[] attr = n.Attributes.ToArray();
                    foreach (HtmlAttribute a in attr)
                    {
                        if (a.Name != "style" && a.Name != "class")
                        {
                            a.Remove();
                        }
                    } //** End fix                  
                    n.InnerHtml = Microsoft.Security.Application.Encoder.HtmlEncode(n.InnerHtml);

                }
            }
            return html;
        }

        public string HighlightKeyWords(string text, string keywords, string cssClass, bool fullMatch)
        {
            if (text == String.Empty || keywords == String.Empty || cssClass == String.Empty)
                return text;
            var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (!fullMatch)
                return words.Select(word => word.Trim()).Aggregate(text,
                             (current, pattern) =>
                             Regex.Replace(current,
                                             pattern,
                                               string.Format(format: "<span style=\"background-color:{0};font-weight:bold;font-size:11px;\">{1}</span>",
                                               arg0: cssClass,
                                               arg1: "$0"),
                                               RegexOptions.IgnoreCase));
            return words.Select(word => "\\b" + word.Trim() + "\\b")
                        .Aggregate(text, (current, pattern) =>
                                  Regex.Replace(current,
                                  pattern,
                                    string.Format(format: "<span style=\"background-color:{0};font-weight:bold;font-size:11px;\">{1}</span>",
                                    arg0: cssClass,
                                    arg1: "$0"),
                                    RegexOptions.IgnoreCase));

        }
    }
}