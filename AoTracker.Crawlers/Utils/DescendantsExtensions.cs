using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Utils
{
    public static class DescendantsExtensions
    {
        public static HtmlNode FirstWithClass(this IEnumerable<HtmlNode> doc, string targetedClass)
        {
            return
                doc.First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass); //trim because mal like to leave stary spaces from time to time
        }

        public static HtmlNode FirstOfDescendantsWithClass(this HtmlDocument doc, string descendants,
            string targetedClass)
        {
            return
                doc.DocumentNode.Descendants(descendants)
                    .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass);
        }

        public static HtmlNode FirstOrDefaultOfDescendantsWithClass(this HtmlDocument doc, string descendants,
            string targetedClass)
        {
            return
                doc.DocumentNode.Descendants(descendants)
                    .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass);
        }

        public static HtmlNode FirstOfDescendantsWithClassContaining(this HtmlDocument doc, string descendants,
            string targetedClass)
        {
            return
                doc.DocumentNode.Descendants(descendants)
                    .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains(targetedClass));
        }

        public static HtmlNode FirstOfDescendantsWithId(this HtmlDocument doc, string descendants, string targetedId)
        {
            return
                doc.DocumentNode.Descendants(descendants)
                    .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value.Trim() == targetedId);
        }

        public static HtmlNode FirstOfDescendantsWithClass(this HtmlNode doc, string descendants, string targetedClass)
        {
            return
                doc.Descendants(descendants)
                    .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass);
        }

        public static HtmlNode FirstOfDescendantsWithId(this HtmlNode doc, string descendants, string targetId)
        {
            return
                doc.Descendants(descendants)
                    .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value.Trim() == targetId);
        }

        public static HtmlNode FirstOrDefaultOfDescendantsWithClass(this HtmlNode doc, string descendants, string targetedClass)
        {
            return
                doc.Descendants(descendants)
                    .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass);
        }

        public static IEnumerable<HtmlNode> WhereOfDescendantsWithClass(this HtmlDocument doc, string descendants,
            string targetedClass)
        {
            return
                doc.DocumentNode.Descendants(descendants)
                    .Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass);
        }

        public static IEnumerable<HtmlNode> WhereOfDescendantsWithPartialClass(this HtmlDocument doc, string descendants,
            string targetedClass)
        {
            return
                doc.DocumentNode.Descendants(descendants)
                    .Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains(targetedClass));
        }

        public static IEnumerable<HtmlNode> WhereOfDescendantsWithPartialId(this HtmlNode doc, string descendants,
            string targetedId)
        {
            return
                doc.Descendants(descendants)
                    .Where(node => node.Attributes.Contains("id") && node.Attributes["id"].Value.Contains(targetedId));
        }

        public static IEnumerable<HtmlNode> WhereOfDescendantsWithClass(this HtmlNode doc, string descendants,
            string targetedClass)
        {
            return
                doc.Descendants(descendants)
                    .Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Trim() == targetedClass);
        }
    }
}
