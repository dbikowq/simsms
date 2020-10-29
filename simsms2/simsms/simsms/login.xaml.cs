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
using System.Windows.Shapes;
using TLSharp.Core;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Windows.Threading;
using System.Threading;
using static simsms.MainWindow;

namespace simsms
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Window
    {

        public static string ClientSecret = "client_secrert.json";
        public static readonly string[] ScopeSheets = { SheetsService.Scope.Spreadsheets };
        public static readonly string AppName = "PR.asistent.app";
        //public static readonly string SpreedSheetId = "1oOg-DAgquZ4dCsCYZiB38cdaMhEly96iRRDVwaEXTFc";
        public static readonly string SpreedSheetId = "1KDhrt2S9JtwTOrdTNJY9zmSmg8Itj4znjg6aXhcffoA";
        public const string Range = "'Sheet1'!A1:A1''";

        public static ValueRange response;
        public static ValueRange responseApikey;

        public login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            //UserCredential credential;
            //using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            //{
            //    string credPath = System.Environment.GetFolderPath(
            //    System.Environment.SpecialFolder.Personal);


            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        ScopeSheets,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;
            //    //Console.WriteLine("Credential file saved to: " + credPath);
            //}

            //// Create Google Sheets API service.
            //var service = new SheetsService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = AppName,
            //});



            //String spreadsheetId2 = SpreedSheetId;
            //String range2 = "A1:A";  // update cell F5 
            //ValueRange valueRange = new ValueRange();
            //valueRange.MajorDimension = "COLUMNS";//"ROWS";//COLUMNS






            //SpreadsheetsResource.ValuesResource.GetRequest getRequest = service.Spreadsheets.Values.Get(spreadsheetId2, range2);
            //ValueRange response = getRequest.Execute();


            if (response==null)
            {
                this.DialogResult = false;
                return;
            }

            if (tbpass.Text==response.Values[0][0].ToString())
            {
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.DialogResult==true)
            {
                this.DialogResult = false;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            //ValueRange response = getRequest.Execute();
            response = getRequest.Execute();

            SpreadsheetsResource.ValuesResource.GetRequest getRequest2 = service.Spreadsheets.Values.Get(spreadsheetId2, range3);

            responseApikey = getRequest2.Execute();
            apikey = responseApikey.Values[0][0].ToString();
        }
    }
}
