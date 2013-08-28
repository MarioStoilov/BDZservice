using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BdzWebsiteUtilities
{
    public static class BDZWebsiteUtilities
    {
        private const string REQUEST_TEMPLATE = "from_station={0}&to_station={1}&via_station=&date={2}&dep_arr=1&time_from={3}&time_to={4}&all_cats=checked&cardId=30&class=0&sort_by=0";

        public static List<List<RouteDTO>> GetRoutes(string fromStation, string toStation, string date, string startTime, string endTime)
        {
            string result = "";

            var httpClient = new HttpClient();
            string requestString = String.Format(REQUEST_TEMPLATE, fromStation, toStation, date, startTime, endTime);

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            var content = new StringContent(requestString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var message = httpClient.PostAsync("http://razpisanie.bdz.bg/SearchServlet?action=listOptions", content).Result;
            result = message.Content.ReadAsStringAsync().Result;

            return ParseHTML(result);
        }

        private static List<List<RouteDTO>> ParseHTML(string result)
        {
            List<List<RouteDTO>> routes = new List<List<RouteDTO>>();
            HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(result);
            int contNumber = 1;
            while (true)
            {

                HtmlNode divNode = htmlDocument.GetElementbyId("cont"+contNumber);
                if (divNode != null)
                {
                    HtmlNode table = divNode.SelectSingleNode("table");
                    if (table != null)
                    {
                        string fullRoute = "";
                        HtmlNodeCollection dataRows = table.SelectNodes("tr[@align]");
                        List<RouteDTO> currentRoutes = new List<RouteDTO>();
                        foreach (var dataRow in dataRows)
                        {
                            currentRoutes.Add(ParseDataRow(dataRow));
                            //fullRoute+=ParseDataRow(dataRow)+" then ";
                        }
                        routes.Add(currentRoutes);
                    }
                    contNumber++;
                }
                else
                {
                    break;
                }
            }
            return routes;
        }

        private static RouteDTO ParseDataRow(HtmlNode dataRow)
        {
            RouteDTO route = new RouteDTO();
            string data = "";
            HtmlNodeCollection dataColumns = dataRow.SelectNodes("td");

            //parse column 1
            HtmlNode firstColumn = dataColumns[0];
            HtmlNodeCollection links = firstColumn.SelectNodes("a");
            route.firstStation = links[0].InnerText;
            route.secondStation = links[1].InnerText;
            //data += firstStation + ">>" + secondStation + " ";

            //parse column 2
            HtmlNode secondColumn = dataColumns[1];
            HtmlNode link = secondColumn.SelectSingleNode("a");
            string href = link.Attributes["href"].Value;
            string hrefLeftCut = href.Substring(href.IndexOf("trainnum=")+"trainnum=".Length);

            string trainNumber = hrefLeftCut.Substring(0, hrefLeftCut.IndexOf('&'));
            route.trainNumber = trainNumber;

            //parse train name
            string[] trainDetails = secondColumn.SelectSingleNode("a").InnerText.Trim().Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder trainBuilder = new StringBuilder();
            trainBuilder.Append(trainDetails[0] + " ");
            for (int i = 1; i < trainDetails.Length; i++)
            {
                trainBuilder.Append(trainDetails[i] + " ");
            }
            route.train = trainBuilder.ToString();
            //data += train + " ";

            //parse column 3
            HtmlNode thirdColumn = dataColumns[2];
            route.options = new List<string>();
            foreach (var image in thirdColumn.SelectNodes("img"))
            {
                route.options.Add(image.Attributes["title"].Value);
            }

            route.departs = dataColumns[3].InnerText;
            route.arrives = dataColumns[4].InnerText;

            //data += departs + "-" + arrives;

            return route;
        }
    }
}
