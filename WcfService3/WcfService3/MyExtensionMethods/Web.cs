using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace MyExtensionMethods
{
    //Faz uso do Package HtmlAgilityPack (instalar via Project - NuGetPackage)
    public static class Web
    {
        /// <summary>
        /// Função que recebe um URL e obtém o respetivo texto (com tags HTML, ou seja, ainda por processar).
        /// </summary>
        /// <param name="URL">Link</param>
        /// <returns></returns>
        public static string GetHTMLContentFromURL(string URL)
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8; //Para a codificação
            string content = client.DownloadString(URL);
            return content;
        }

        /// <summary>
        /// Função que recebe um URL e obtém o respetivo texto (sem conteúdo HTML).
        /// </summary>
        /// <param name="URL">Link</param>
        /// <param name="tagsToRemove">Parâmetro Opcional. Se pretendermos que o método remova em especifico um texto dentro das tags passadas como parâmetro (os params podem ser passados da seguinte forma (e.g., new List<string> { "br", "ol", "ul", "li" }) </param>
        /// <returns>Devolve texto de uma página web</returns>
        public static string GetTextContentFromURL(string URL, List<string> tagsToRemove = null)
        {
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8; //Para a codificação
            HtmlAgilityPack.HtmlDocument doc = web.Load(URL); //Usa o método Load do próprio package para obter o HTML da página. Poderia ter usado o método GetHTMLContentFromURL para obter uma string com o HTML, mas nesse caso ao invés de usar o web.Load(URL); usaria o doc.LoadHtml(html);

            StringBuilder sb = new StringBuilder();
            string final = "";

            #region Get text within title
            foreach (HtmlNode node in doc.DocumentNode.Descendants("title")) //texto vem habitualmente dentro dos paragrafos (identificados por p)
                sb.AppendLine(node.InnerText);

            final = sb.ToString();
            #endregion

            #region Get text within doc
            foreach (HtmlNode node in doc.DocumentNode.Descendants("p")) //texto vem habitualmente dentro dos paragrafos (identificados por p)
                sb.AppendLine(node.InnerText);

            final = sb.ToString();
            #endregion

            return final;
        }
    }
}