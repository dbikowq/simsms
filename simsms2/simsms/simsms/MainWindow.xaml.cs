using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Windows.Threading;
using System.Threading;
using OfficeOpenXml;
using Microsoft.Win32;
using TLSharp.Core;
using TeleSharp.TL;
using TeleSharp.TL.Messages;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Media;

namespace simsms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string namePC;
        public static string ClientSecret = "client_secrert.json";
        public static readonly string[] ScopeSheets = { SheetsService.Scope.Spreadsheets };
        public static readonly string AppName = "PR.asistent.app";
        //public static readonly string SpreedSheetId = "1oOg-DAgquZ4dCsCYZiB38cdaMhEly96iRRDVwaEXTFc";
        public static readonly string SpreedSheetId = "17OyLUb0Y-Ts9DJat--4e_XkrUJjqnGB76Vw0G_9fEgQ";
        public const string Range = "'Sheet1'!A1:A1''";

        public static string[,] data = new string[,]
        {
            { "11","22"},
            {"22","33" }
        };




        //public static List<services> services = new List<services>();
        public static List<numbers> numbers = new List<numbers>();
        public static int indexListBox;
        public static HttpWebRequest request;
        public static HttpWebResponse response;
        public static CookieContainer cook = new CookieContainer();
        public static Match pattern;
        public static string apikey;
        public DispatcherTimer timer = new DispatcherTimer();

        public static string pathExcel;

        public static TextBox myNumberTb;



        public static TelegramClient telegramClient;
        public static string hash;

        public MainWindow()
        {
            InitializeComponent();
            myNumberTb = myNumber;
            api api = new api();
            login log = new login();
            if (log.ShowDialog().Value)
            {
                return;
            }
            else
            {
                this.Close();
            }
        }
        #region allProcedures
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //api.getCode("a",2);
            apikey = textBoxApi.Password;
            //loadServices();
            api.getCookie();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            api.getBalance(apikey);
        }


        


        //public void loadServices()
        //{
        //    //StreamReader sr = new StreamReader("");
        //    UTF8Encoding enc = new UTF8Encoding();
        //    byte[] myStr;
        //    string myStr2;
        //    HtmlDocument doc = new HtmlDocument();
        //    WebClient wc = new WebClient();
        //    String tmp = wc.DownloadString("http://simsms.org/new_theme_api.html");
        //    doc.LoadHtml(tmp);
        //    var node = doc.DocumentNode.SelectNodes(".//img[contains(@src,'/templates/SMSv1/images/ico/')]");

        //    for (int i=0;i<node.Count;i++)
        //    {
        //        myStr = Encoding.Default.GetBytes(node[i].Attributes[1].Value);
        //        myStr2 = Encoding.UTF8.GetString(myStr);
        //        services.Add(new simsms.services { name = myStr2, code = node[i].Attributes[2].Value });
        //        listBoxServices.Items.Add(myStr2);
        //    }


        //}

        private void getNumber_Click(object sender, RoutedEventArgs e)
        {
            //indexListBox = listBoxServices.SelectedIndex;
            myNumber.Text = api.getNumber(apikey);

            if (myNumber.Text!="")
            {
                Clipboard.SetText(myNumber.Text);
                getCod_Click(getCod, null);
            }

           


            //timer.Tick += new EventHandler(timer_tick);
            //timer.Interval = new TimeSpan(0,0,10);
            //timer.Start();

        }

        //private void timer_tick(object sender, EventArgs e)
        //{
        //    myCode.Text= api.getCode(apikey, indexListBox);
        //    if (myCode.Text!="")
        //    {
        //        timer.Stop();
        //    }
        //}

        private void getCod_Click(object sender, RoutedEventArgs e)
        {
            getCod.IsEnabled = false;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            
            //while (myCode.Text == "")
            //{
            //    Thread.Sleep(5000);
            //    myCode.Text = api.getCode(apikey);
            //    Thread tt = new Thread(threadGetCode);
            //    tt.Start();
            //}
            //await threadGetCode();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            myCode.Text = api.getCode(apikey);
            if (myCode.Text!="")
            {
                timer.Stop();
                getCod.IsEnabled = true;
                SystemSounds.Hand.Play();
                Clipboard.SetText(myCode.Text);
            }
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //if (pathExcel!="")
            //{
            //    ExcelPackage.LicenseContext = LicenseContext.Commercial;
            //    FileInfo f1 = new FileInfo(pathExcel);
            //    ExcelPackage exc = new ExcelPackage(f1);
            //    ExcelWorksheet ws = exc.Workbook.Worksheets[0];
            //    int lasrCells = ws.Dimension.Rows + 1;
            //    ws.Cells[lasrCells, 1].Value = tbNumber.Text;
            //    ws.Cells[lasrCells, 2].Value = tbPass.Text;
            //    exc.Save();
            //    tbNumber.Text = "";
            //    tbPass.Text = "";
            //}


            // SpreadsheetsService 

            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal);


                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    ScopeSheets,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName,
            });



            String spreadsheetId2 = SpreedSheetId;
            String range2 = "A1:A";  // update cell F5 
            String range3 = "B1:B";  // update cell F5 
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "COLUMNS";//"ROWS";//COLUMNS

            




            SpreadsheetsResource.ValuesResource.GetRequest getRequest = service.Spreadsheets.Values.Get(spreadsheetId2, range2);
            ValueRange response = getRequest.Execute();
            if (response.Values==null)
            {
                MessageBox.Show("Нет данных для получения", "Подождите...",MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }
            String result = null;
            List<String> results = new List<String>();
            try
            {
                foreach (var value in response.Values)
                {
                    result += value[0];
                    results.Add((String)value[0]);
                }

                var oblist = new List<object>() { "" };
                valueRange.Values = new List<IList<object>> { oblist };

                var lastRange = "A" + (response.Values.Count);

                SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, spreadsheetId2, lastRange);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                UpdateValuesResponse result2 = update.Execute();

                pattern = Regex.Match(results[results.Count - 1], @"(.*?):(.*?)$");
                tbNumber.Text = pattern.Groups[1].Value;
                tbPass.Text = pattern.Groups[2].Value;

                //добавить лог

                getRequest = service.Spreadsheets.Values.Get(spreadsheetId2, range3);
                response = getRequest.Execute();



                oblist = new List<object>() { "Пользователь " + namePC + " взял " + pattern.Groups[0].Value };
                valueRange.Values = new List<IList<object>> { oblist };
                
                if (response.Values==null)
                {
                    lastRange = "B1";
                }
                else
                {
                    lastRange = "B" + (response.Values.Count+1);
                }
                

                update = service.Spreadsheets.Values.Update(valueRange, spreadsheetId2, lastRange);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                result2 = update.Execute();



            }
            catch
            {

            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog()==true)
            {
                pathExcel = open.FileName;
            }
        }
        #endregion
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //var store = new FileSessionStore();
            var client = new TelegramClient(1683198, "651cf34b8b68dd9fe0b144ae5889e8eb");
            await client.ConnectAsync();
            var hash = await client.SendCodeRequestAsync(tbNumbetTelega.Text);
            var code = "40835";//tbCodeTelega.Text;
            var user = await client.MakeAuthAsync(tbNumbetTelega.Text,hash,code);


            var result = await client.GetContactsAsync();
            //var dialogs = (TLDialog)await client.GetUserDialogsAsync();
            TLDialogs dialog = (TLDialogs)await client.GetUserDialogsAsync();
            
            TLUser chat = (TLUser)dialog.Users[0];

            await client.SendMessageAsync(new TLInputPeerUser() { UserId=chat.Id,AccessHash=chat.AccessHash.Value},"*8088*79026885467*79087215309");

            //var chat = dialogs.Chats
            //.Where(c => c.GetType() == typeof(TLChannel))
            //.Cast<TLChannel>()
            //.FirstOrDefault(c => c.Title == "<channel_title>");
            //await client.SendMessageAsync(new TLInputPeerChannel() { ChannelId=});
            //var dd = (TLUser)dialogs.Users[0];
        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //var result = await client

        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            telegramClient = new TelegramClient(Convert.ToInt32(apiID.Text),apiIDHash.Text);
            await telegramClient.ConnectAsync();
            hash = await telegramClient.SendCodeRequestAsync(tbNumbetTelega.Text);
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var user = await telegramClient.MakeAuthAsync(tbNumbetTelega.Text,hash,tbCodeTelega.Text);
        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //пофиксииииить!!!!!
            telegramClient = new TelegramClient(Convert.ToInt32(apiID.Text), apiIDHash.Text);
            await telegramClient.ConnectAsync();
            TLDialogs dialog = (TLDialogs)await telegramClient.GetUserDialogsAsync();
            //TLUser user = (TLUser)dialog.Users.Where(x => x.GetType() == typeof(TLUser)).Cast<TLUser>().FirstOrDefault(x => x.FirstName == "SIP.tg" && x.Id=414318140);
            TLUser user = (TLUser)dialog.Users.Where(x => x.GetType() == typeof(TLUser)).Cast<TLUser>().FirstOrDefault(x => x.Id==414318140);
            await telegramClient.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id, AccessHash = user.AccessHash.Value }, "*8088*" + falseNumber.Text + "*" + trueNumber.Text);
        }

        private void btnBan_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            myNumber.Text = api.ban(apikey);
            myCode.Text = "";
            getCod.IsEnabled = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            textBoxApi.Password = apikey;
            namePC = Environment.MachineName;
            this.Title = "Ассистент - " + namePC;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (tbSetting2.Password=="show" && setting2.Visibility==Visibility.Collapsed)
            {
                setting2.Visibility = Visibility.Visible;
                btnSetting2.Content = "Скрыть";
                return;
            }
            if (setting2.Visibility==Visibility.Visible)
            {
                setting2.Visibility = Visibility.Collapsed;
                btnSetting2.Content = "Показать";
            }
        }

    }
}
