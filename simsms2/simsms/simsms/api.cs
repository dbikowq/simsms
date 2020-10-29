using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;
using static simsms.MainWindow;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Windows.Threading;

namespace simsms
{
    public class api
    {
        public static HttpWebRequest request;
        public static HttpWebResponse response;
        public static CookieContainer cook = new CookieContainer();
        public static Match pattern;



        public static DispatcherTimer timer = new DispatcherTimer();

        public class responseBalance
        {
            public string response { get; set; }
            public string balance { get; set; }
            public string karma { get; set; }
            public string name { get; set; }
        }

        public static void getBalance(String apiKey)
        {
            //request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=get_balance&service=opt19&apikey=" + apiKey);
            request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=get_userinfo&service=opt19&apikey=" + apiKey);
            request.Method = "GET";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.CookieContainer = cook;
            request.Headers.Add("Upgrade-Insecure-Requests", "1");

            response = (HttpWebResponse)request.GetResponse();
            String tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //var resp = JsonConvert.DeserializeObject<responseBalance>("");
            try
            {
                var resp = JsonConvert.DeserializeObject<responseBalance>(tmp);
                MessageBox.Show(resp.balance);
            }
            catch (Exception e)
            {
                MessageBox.Show("Сервер вернул ошибку","simsms.org",MessageBoxButton.OK,MessageBoxImage.Information);
            }


            //pattern = Regex.Match(tmp, @"balance.:.(.*?)\D}");
            
        }

        public class responseNumber
        {
            public string response { get; set; }
            public string number { get; set; }
            public int id { get; set; }
            public int again { get; set; }
            public object text { get; set; }
            public string extra { get; set; }
            public double karma { get; set; }
            public object pass { get; set; }
            public object sms { get; set; }
            public int balanceOnPhone { get; set; }
            public object service { get; set; }
            public object country { get; set; }
            public string CountryCode { get; set; }
            public int branchId { get; set; }
            public bool callForwarding { get; set; }
            public int goipSlotId { get; set; }
        }

        public static String getNumber(String apiKey)
        {
            //request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=get_number&country=RU&service=opt19&apikey=" + apiKey);
            request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=get_number&country=RU&service=opt104&apikey=" + apiKey);
            request.Method = "GET";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.CookieContainer = cook;
            request.Headers.Add("Upgrade-Insecure-Requests", "1");


            response = (HttpWebResponse)request.GetResponse();
            String tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //tmp = "{\"response\":\"1\",\"number\":\"9871234567\",\"id\":25623}";
            //{"response":"1","number":"9619257303","id":52051674,"again":0,"text":null,"extra":"","karma":49.010000000000005,"pass":null,"sms":null,"balanceOnPhone":0,"service":null,"country":null,"CountryCode":"+7","branchId":0,"callForwarding":false,"goipSlotId":-1}

            var resp = JsonConvert.DeserializeObject<responseNumber>(tmp);


            //pattern = Regex.Match(tmp, @"number.:.(\d+).,.id.:(\d+),");
            //MainWindow.numbers.Add(new numbers { number = pattern.Groups[1].Value, id = pattern.Groups[2].Value });
            MainWindow.numbers.Add(new numbers { number = resp.number, id = Convert.ToString(resp.id) });
            //return pattern.Groups[1].Value;
            return resp.number;
        }


        public class responseBan
        {
            public string response { get; set; }
            public string number { get; set; }
            public int id { get; set; }
            public int again { get; set; }
            public string text { get; set; }
            public string extra { get; set; }
            public double karma { get; set; }
            public string pass { get; set; }
            public string sms { get; set; }
            public int balanceOnPhone { get; set; }
            public string service { get; set; }
            public string country { get; set; }
            public string CountryCode { get; set; }
            public int branchId { get; set; }
            public bool callForwarding { get; set; }
            public int goipSlotId { get; set; }
            public int lifeSpan { get; set; }

        }


        public class responseCode
        {
            public string response { get; set; }
            public object number { get; set; }
            public int id { get; set; }
            public object text { get; set; }
            public string extra { get; set; }
            public double karma { get; set; }
            public string pass { get; set; }
            public object sms { get; set; }
            public int balanceOnPhone { get; set; }
        }


        public static String getCode(String apiKey)
        {
            var needNumber = (from numbers num in MainWindow.numbers where myNumberTb.Text == num.number select num).FirstOrDefault();
            //request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=get_sms&country=ru&service=opt19&id=" + needNumber.id + "&apikey=" + apiKey);
            request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=get_sms&country=ru&service=opt104&id=" + needNumber.id + "&apikey=" + apiKey);
            request.Method = "GET";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.CookieContainer = new CookieContainer();
            request.Timeout = 2000;
            //request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.AllowAutoRedirect = false;

            response = (HttpWebResponse)request.GetResponse();
            String tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var resp = JsonConvert.DeserializeObject<responseCode>(tmp);

            //pattern = Regex.Match(tmp,@"\]\s(\d+)\s");

            if (resp.text != null)
            {
                return resp.text.ToString();
            }
            else
            {
                return "";
            }
            
        }



        public static string ban(String apiKey)
        {
            var needId = (from numbers num in MainWindow.numbers where myNumberTb.Text == num.number select num).FirstOrDefault();
            request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=ban&country=ru&service=opt104&&apikey=" + apiKey + "&id=" + needId.id);
            //request = (HttpWebRequest)WebRequest.Create("http://simsms.org/priemnik.php?metod=ban&country=ru&service=opt19&&apikey=" + apiKey + "&id=" + needId.id);
            request.Method = "GET";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.CookieContainer = new CookieContainer();
            request.Timeout = 2000;
            //request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.AllowAutoRedirect = false;

            response = (HttpWebResponse)request.GetResponse();
            String tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var resp = JsonConvert.DeserializeObject<responseBan>(tmp);

            if (resp.response=="1")
            {
                MessageBox.Show("Номер заблокирован","Успешно",MessageBoxButton.OK,MessageBoxImage.Information);
                MainWindow.numbers.Remove((from numbers num in MainWindow.numbers where num.id==needId.id select num).FirstOrDefault());
                return "";
                //var needNumber = (from numbers num in MainWindow.numbers where myNumberTb.Text == num.number select num).FirstOrDefault();
            }
            else
            {
                return "Ошибка блокировки номера";
            }
            //pattern = Regex.Match(tmp,@"\]\s(\d+)\s");

            //if (resp.text != null)
            //{
            //    return resp.text.ToString();
            //}
            //else
            //{
            //    return "";
            //}

        }


        public static void getCookie()
        {
            request = (HttpWebRequest)WebRequest.Create("http://simsms.org/");
            request.Method = "GET";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.CookieContainer = cook;
            request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");

            response = (HttpWebResponse)request.GetResponse();
            String tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();
        
        }


    }
}
