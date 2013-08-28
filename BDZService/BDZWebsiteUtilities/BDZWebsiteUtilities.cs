using BdzWebsiteUtilities;
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
        private const string REQUEST_ROUTES_TEMPLATE = "from_station={0}&to_station={1}&via_station=&date={2}&dep_arr=1&time_from={3}&time_to={4}&all_cats=checked&cardId=30&class=0&sort_by=0";

        private const string REQUEST_TRAIN_TEMPLATE = "http://razpisanie.bdz.bg/SearchServlet?action=listTrainStops&trainnum={0}&date={1}";

        private const string REQUEST_STATION_TEMPLATE = "http://razpisanie.bdz.bg/SearchServlet?action=listStation&station={0}&date={1}";

        public static List<List<RouteDTO>> GetRoutes(string fromStation, string toStation, string date, string startTime, string endTime)
        {
            string result = "";

            var httpClient = new HttpClient();
            string requestString = String.Format(REQUEST_ROUTES_TEMPLATE, fromStation, toStation, date, startTime, endTime);

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            var content = new StringContent(requestString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var message = httpClient.PostAsync("http://razpisanie.bdz.bg/SearchServlet?action=listOptions", content).Result;
            result = message.Content.ReadAsStringAsync().Result;

            return ParseRoutesHTML(result);
        }

        private static List<List<RouteDTO>> ParseRoutesHTML(string result)
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
                            currentRoutes.Add(ParseRouteDataRow(dataRow));
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

        private static RouteDTO ParseRouteDataRow(HtmlNode dataRow)
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

        public static TrainDTO GetTrain(string trainNumber, string date)
        {
            string result = "";

            var httpClient = new HttpClient();
            string requestString = String.Format(REQUEST_TRAIN_TEMPLATE, trainNumber, date);

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            var message = httpClient.GetByteArrayAsync(requestString).Result;
            var responseString = Encoding.UTF8.GetString(message, 0, message.Length - 1);
            return ParseTrainHTML(responseString); ;
        }

        private static TrainDTO ParseTrainHTML(string html)
        {
            HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode infoTable = htmlDocument.DocumentNode.SelectSingleNode("html/body/table/tr[not (@align)]/td/table");

            HtmlNodeCollection infoRows = infoTable.SelectNodes("tr");
            infoRows.RemoveAt(0);
            infoRows.RemoveAt(0);
            infoRows.RemoveAt(infoRows.Count - 2);

            HtmlNode TrainOptions = infoRows[infoRows.Count - 1];
            infoRows.RemoveAt(infoRows.Count - 1);
            TrainDTO train = new TrainDTO();
            train.stops = new List<TrainSimpleStopDTO>();
            train.options = new List<string>();

            foreach (var stopRow in infoRows)
            {
                HtmlNodeCollection columns = stopRow.SelectNodes("td");
                train.stops.Add(new TrainSimpleStopDTO() { station = columns[0].InnerText.Trim(), arrives = columns[1].InnerText.Trim(), departs = columns[2].InnerText.Trim() });
            }


            foreach (var image in TrainOptions.SelectSingleNode("td").SelectSingleNode("span").SelectNodes("img"))
            {
                train.options.Add(image.Attributes["title"].Value);
            }


            return train;
        }


        public static StationDTO GetStation(string station, string date)
        {
            string result = "";

            var httpClient = new HttpClient();
            string requestString = String.Format(REQUEST_STATION_TEMPLATE, station, date);

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            var message = httpClient.GetByteArrayAsync(requestString).Result;
            var responseString = Encoding.UTF8.GetString(message, 0, message.Length - 1);

            return ParseStationHTML(responseString);
        }

        private static StationDTO ParseStationHTML(string html)
        {
            StationDTO station = new StationDTO();
            station.arrives = new List<StationEntryDTO>();
            station.departs = new List<StationEntryDTO>();

            HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode infoTable = htmlDocument.DocumentNode.SelectSingleNode("html/body/table/tr[2]/td/table");

            HtmlNodeCollection rows = infoTable.SelectNodes("tr");
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            bool departing = true;

            foreach (var row in rows)
            {
                if (row.Attributes["bgcolor"].Value.ToLower()!="#f5f8fa")
                {
                    departing = false;
                    continue;
                }

                StationEntryDTO entry = new StationEntryDTO();
                HtmlNodeCollection columns = row.SelectNodes("td");
                entry.station = columns[0].InnerText.Trim();
                entry.train = columns[1].InnerText.Trim(); ;
                entry.time = columns[2].InnerText.Trim(); ;

                if (departing)
                {
                    station.departs.Add(entry);
                }
                else
                {
                    station.arrives.Add(entry);
                }
            }

            return station;
        }

    }
}
