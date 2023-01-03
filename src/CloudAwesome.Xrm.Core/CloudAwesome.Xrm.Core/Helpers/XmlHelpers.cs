using System.IO;
using System.Text;
using System.Xml;

namespace CloudAwesome.Xrm.Core.Helpers
{
    public static class XmlHelpers
    {
        public static string AddPagingCookie(this string fetchXmlString, string cookie, int page, int count)
        {
            var stringReader = new StringReader(fetchXmlString);
            var reader = new XmlTextReader(stringReader);

            var document = new XmlDocument();
            document.Load(reader);

            if (document.DocumentElement == null) return fetchXmlString;
            
            var attributes = document.DocumentElement.Attributes;

            if (cookie != null)
            {
                var pagingAttribute = document.CreateAttribute("paging-cookie");
                pagingAttribute.Value = cookie;
                attributes.Append(pagingAttribute);
            }

            var pageAttribute = document.CreateAttribute("page");
            pageAttribute.Value = page.ToString();
            attributes.Append(pageAttribute);

            var countAttribute = document.CreateAttribute("count");
            countAttribute.Value = count.ToString();
            attributes.Append(countAttribute);

            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            document.WriteTo(writer);
            writer.Close();

            return stringBuilder.ToString();
        }
        
    }
}