using System;
using System.Net;
using System.Xml;
using System.Collections;

namespace project
{
    public class Runtime
    {
        static void Main(string[] args)
        {
            Weather app = new Weather();
            if (args.Length > 0) {
                switch (args[0])
                {
                    case "-f":
                        app.forecast(args[1], args[2]);
                        break;
                    case "-w":
                        app.now(args[1]);
                        break;
                    case "-h":
                        Console.WriteLine("csw [option] [argument 1] [argument 2]\n\nOptions:\n-f\tforecast. argument 1 is city name, argument 2 is number of days you want to get forcast for. Legitimate periods are from 1 to 16 days.\n-w\tweather. argument 1 is city name. Checkout current weather for given city.\n-h\tthis help message.");
                        break;
                    default:
                        app.now(args[0]);
                        break;
                }
            } else {
                Console.WriteLine("enter city name as an argument, or -h for help");
            }
        }
    }

    class Weather
    {
        private string AppId = "6ea5cf4906d41b9e4108e66624861864";
        private string Url = "http://api.openweathermap.org/data/2.5/";
        private string Units = "metric";
        private string metric = "°C";
        private string Mode = "xml";

        public string request(string method, string city, string param = "")
        {
            string req = this.Url;
            req += method;
            req += "?q="+city;
            req += "&mode="+this.Mode;
            req += "&units="+this.Units;
            req += param;
            req += "&appid="+this.AppId;
            return req;
        }

        public void now(string city = "")
        {
            XmlDocument doc = this.parseXml(this.fetch(this.request("weather", city)));
            string cityName = doc.SelectSingleNode("/current/city/@name").Value;
            string sunRise = doc.SelectSingleNode("/current/city/sun/@rise").Value;
            string sunSet = doc.SelectSingleNode("/current/city/sun/@set").Value;
            string humidity = doc.SelectSingleNode("/current/humidity/@value").Value;
            string humidityUnit = doc.SelectSingleNode("/current/humidity/@unit").Value;
            string min = Math.Round(decimal.Parse(doc.SelectSingleNode("/current/temperature/@min").Value), 0).ToString();
            //string max = Math.Round(decimal.Parse(doc.SelectSingleNode("/current/temperature/@max").Value), 0).ToString();
            string windName = doc.SelectSingleNode("/current/wind/speed/@name").Value;
            string cloudsName = doc.SelectSingleNode("/current/clouds/@name").Value;
            string weather = doc.SelectSingleNode("/current/weather/@value").Value;
            string t = "\t";
            Console.WriteLine(weather+t+min+this.metric+t+cloudsName+t+windName);
        }

        public void forecast(string city, string days)
        {
            XmlDocument doc = this.parseXml(this.fetch(this.request("forecast/daily", city, "&cnt="+days)));
            XmlNodeList el = doc.SelectNodes("/weatherdata/forecast/*");
            for (int i=0; i < el.Count; i++) {
                string date = el[i].SelectSingleNode("@day").Value;
                string symbol = el[i].SelectSingleNode("symbol/@name").Value;
                string min = Math.Round(decimal.Parse(el[i].SelectSingleNode("temperature/@min").Value), 0).ToString();
                string max = Math.Round(decimal.Parse(el[i].SelectSingleNode("temperature/@max").Value), 0).ToString();
                string windString = el[i].SelectSingleNode("windSpeed/@name").Value;
                string cloudString = el[i].SelectSingleNode("clouds/@value").Value;
                string t = "\t";
                Console.WriteLine(date+t+min+this.metric+" -- "+max+this.metric+t+symbol+t+cloudString+t+windString);
            }
        }

        public string fetch(string url = "")
        {
            var w = new WebClient();
            string data = "";
            try {
                data = w.DownloadString(url);
            } catch {
                data = "no internet connection";
                Console.WriteLine(data);
                Environment.Exit(-1);
            }
            return data;
        }

        public XmlDocument parseXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }
    }
}
