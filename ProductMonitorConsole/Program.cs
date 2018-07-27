using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using CommandLine;
using WebParser;
using WebParser.Parsers;
using WebParser.WebManagers;
using Cintron.LoggerCin;

namespace ProductMonitorConsole
{
    class Options
    {
        [Option('k', "key", DefaultValue = "", HelpText = "IFTTT webhook key.")]
        public String IFTTTKey { get; set; }


        [Option('e', "event", DefaultValue = "", HelpText = "IFTTT webhook event name.")]
        public String IFTTTEvent { get; set; }

        [Option('i', "interval", DefaultValue = 5, HelpText = "Time interval to check products.")]
        public int Interval { get; set; }

        [Option('j', "jsonconfig", DefaultValue = "", HelpText = "JSON configuration file to use.")]
        public String JsonPath { get; set; }

    }


    class ConsoleScrapperData
    {
        public ConsoleScrapperData()
        {
            iFTTTEventName = "";
            iFTTTWebhookKey = "";
            iFTTTNotify = false;
            monitoringInterval = 300000;
            productsToCheck = new List<Product>();
        }
        private String iFTTTWebhookKey;
        public String IFTTTWebhookKey
        {
            get
            {
                return iFTTTWebhookKey;
            }

            set
            {
                iFTTTWebhookKey = value;
                if (iFTTTWebhookKey.Length == 22 && !String.IsNullOrEmpty(iFTTTEventName))
                    IFTTTNotify = true;
                else
                    IFTTTNotify = false;
            }
        }

        private String iFTTTEventName;
        public String IFTTTEventName
        {
            get
            {
                return iFTTTEventName;
            }

            set
            {
                iFTTTEventName = value;
                if (iFTTTWebhookKey.Length == 22 && !String.IsNullOrEmpty(iFTTTEventName))
                    iFTTTNotify = true;
                else
                    IFTTTNotify = false;
            }
        }

        private List<Product> productsToCheck;

        public List<Product> ProductsToCheck
        {
            get
            {
                return productsToCheck;
            }
            set
            {
                productsToCheck = value;
            }
        }

        private bool iFTTTNotify;
        public bool IFTTTNotify
        {
            get
            {
                return iFTTTNotify;
            }
            set
            {
                iFTTTNotify = value;
            }
        }

        private int monitoringInterval;
        public int MonitoringInterval
        {
            get
            {
                return monitoringInterval;
            }
            set
            {
                monitoringInterval = value;
            }
        }
    }
    class Program
    {
        private static String jsonFile;
        private static Timer aTimer;
        private static ConsoleScrapperData scrapperData;
        private static LoggerCin logger;
        private static bool inMainMenu = true;
        static void Main(string[] args)
        {
            logger = new LoggerCin(AppDomain.CurrentDomain.BaseDirectory + "logs.log");
            try
            {
                scrapperData = new ConsoleScrapperData();
                ParserResult<Options> result = CommandLine.Parser.Default.ParseArguments<Options>(args);
                if (!result.Errors.Any())
                {
                    var options = result.Value;
                    if (!String.IsNullOrEmpty(options.JsonPath))
                        jsonFile = options.JsonPath;
                    else
                        jsonFile = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";

                    if (!String.IsNullOrEmpty(options.IFTTTKey))
                        scrapperData.IFTTTWebhookKey = options.IFTTTKey;
                    if (!String.IsNullOrEmpty(options.IFTTTEvent))
                        scrapperData.IFTTTEventName = options.IFTTTEvent;
                    if (options.Interval >= 1)
                        scrapperData.MonitoringInterval = options.Interval * 60000;
                }

                if (File.Exists(jsonFile))
                {
                    scrapperData = JSONToConfiguration(jsonFile);
                }

                if(scrapperData.ProductsToCheck.Any())
                {
                    scrapperData.ProductsToCheck = scrapperData.ProductsToCheck.OrderByDescending(p => p.StoreName).ToList();
                }

                System.Console.WriteLine("Running scheduled product check every {0} minutes...", (int)scrapperData.MonitoringInterval / 60000);
                if (!String.IsNullOrEmpty(scrapperData.IFTTTEventName) && !String.IsNullOrEmpty(scrapperData.IFTTTWebhookKey))
                    System.Console.WriteLine("IFTTT Webhook key and event name added. Notifications enabled!");
                else
                    System.Console.WriteLine("No IFTTT webhook key and event name. Notifications disabled!");

                if (scrapperData.ProductsToCheck.Any())
                {
                    System.Console.WriteLine("Monitoring " + scrapperData.ProductsToCheck.Count() + " products from configuration.");
                }

                aTimer = new Timer
                {
                    Interval = scrapperData.MonitoringInterval
                };
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
                ConsoleLoop().Wait();
            }
            catch (Exception ex)
            {
                if(logger!=null)
                    logger.Fatal(ex.ToString());
                else
                {
                    LoggerCin logger = new LoggerCin(AppDomain.CurrentDomain.BaseDirectory + "Fatal.log");

                }

            }
        }

        private static async Task ConsoleLoop()
        {
            while (true)
            {
                System.Console.WriteLine("\nCommands: 'A' to add a new product, 'I' to change the check interval, 'V' to view products being monitored, 'T' to test IFTTT notification,  'X' to close:");
                string cmd = System.Console.ReadLine().ToLower();
                if (cmd.Equals("a"))
                {
                    inMainMenu = false;
                    await AddProductLoop();
                    inMainMenu = true;
                }
                else if (cmd.Equals("i"))
                {
                    inMainMenu = false;
                    System.Console.WriteLine("How often do you want to check the products? (Currently set to {0} minutes, default: 5 minutes):  ", (int)scrapperData.MonitoringInterval/60000);
                    string sinterval = System.Console.ReadLine().Trim();
                    int interval = 0;
                    bool success = Int32.TryParse(sinterval, out interval);
                    if (!success)
                        scrapperData.MonitoringInterval = 300000;
                    else
                    {
                        scrapperData.MonitoringInterval = interval * 60000;
                        System.Console.WriteLine("Interval changed to " + sinterval + " minutes");
                        aTimer.Interval = scrapperData.MonitoringInterval;
                        aTimer.Stop();
                        aTimer.AutoReset = true;
                        aTimer.Start();
                        ConfigurationToJSON(scrapperData, jsonFile);
                    }

                    inMainMenu = true;
                }
                else if(cmd.Equals("t"))
                {
                    inMainMenu = false;
                    SendIFTTNotification(scrapperData.IFTTTEventName, "This is a test.", "http://www.ifttt.com");
                    System.Console.WriteLine("Notification request sent to event '"+ scrapperData.IFTTTEventName+"'. If you don't receive a notification on the IFTTT app, check your IFTTT key and webhook event name.\n");
                    System.Console.WriteLine();
                    inMainMenu = true;
                }
                else if (cmd.Equals("v"))
                {
                    inMainMenu = false;
                    foreach (Product origproduct in scrapperData.ProductsToCheck)
                    {
                        OutputProduct(origproduct);
                    }
                    inMainMenu = true;
                }
                else if (cmd.Equals("x"))
                {
                    inMainMenu = true;
                    Environment.Exit(0);
                    ConfigurationToJSON(scrapperData, jsonFile);
                    break;
                }
                else if(cmd.Equals("f"))
                {
                    await RunParsers();
                }
                else
                {
                    System.Console.WriteLine("Invalid command. Try again!\n");
                }
            }
        }

        private static void OutputProduct(Product origproduct)
        {
            System.Console.WriteLine("[" + origproduct.StoreName + "]"); 
            System.Console.WriteLine("Product: " + origproduct.Name);
            System.Console.WriteLine("Price: " + origproduct.Price);
            System.Console.WriteLine("Status: " + origproduct.Availability);
            System.Console.WriteLine();
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            RunParsers().Wait();
        }

        private static async Task AddProductLoop()
        {
            System.Console.WriteLine("Add product url (Newegg.com, BHPhotoVideo.com Supported):");
            String url = System.Console.ReadLine().Trim();
            if (url.Contains("http") && (url.Contains("newegg.com") || url.Contains("bhphotovideo.com") || url.Contains("tigerdirect.com") || url.Contains("sabrepc.com")))
            {
                Product product;

                if (url.Contains("newegg.com"))
                {
                    NewEggParser parser = new NewEggParser();
                    product= await parser.RequestProduct(url);
                }
                else if(url.Contains("bhphotovideo.com"))
                {
                    BHParser parser = new BHParser();
                    product = await parser.RequestProduct(url);
                }
                else if(url.Contains("tigerdirect.com"))
                {
                    TigerDirectParser parser = new TigerDirectParser();
                    product = await parser.RequestProduct(url);
                }
                else
                {
                    SabreParser parser = new SabreParser();
                    product = await parser.RequestProduct(url);
                }
                
                OutputProduct(product);

                bool productExists = scrapperData.ProductsToCheck.Where(x => x.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase)).Any();

                if (productExists)
                {
                    Console.WriteLine("This product is already being monitored and can't be added to the list.");
                }
                else
                {
                    System.Console.WriteLine("Do you want to monitor this product? [Y/N]: ");
                    string answ = System.Console.ReadLine().ToLower();
                    if (answ.Equals("y"))
                    {
                        scrapperData.ProductsToCheck.Add(product);
                        SortProductsByStoreName();
                        System.Console.WriteLine("Now monitoring " + product.Name + "\n");
                        ConfigurationToJSON(scrapperData, jsonFile);
                        await RunParsers(true);
                    }
                }
            }
        }
        private static async Task RunParsers(bool ForceConsolePrint = false)
        {
            foreach(Product origproduct in scrapperData.ProductsToCheck)
            //Parallel.ForEach(scrapperData.ProductsToCheck, async (origproduct) =>
            {
                Product product;

                if (origproduct.Url.Contains("newegg.com"))
                {
                    NewEggParser parser = new NewEggParser();
                    product = await parser.RequestProduct(origproduct.Url);
                }
                else if(origproduct.Url.Contains("tigerdirect.com"))
                {
                    TigerDirectParser parser = new TigerDirectParser();
                    product = await parser.RequestProduct(origproduct.Url);
                }
                else if(origproduct.Url.Contains("bhphotovideo.com"))
                {
                    BHParser parser = new BHParser();
                    product = await parser.RequestProduct(origproduct.Url);
                }
                else
                {
                    SabreParser parser = new SabreParser();
                    product = await parser.RequestProduct(origproduct.Url);
                }

                if ((inMainMenu || ForceConsolePrint) && product.Valid)
                    OutputProduct(product);
                if (scrapperData.IFTTTNotify && product.Valid)
                {
                    bool productChanged = false;
                    if (!origproduct.Name.Equals(product.Name))
                    {
                        productChanged = true;
                        SendIFTTNotification("["+origproduct.StoreName+"] "+ origproduct.Name + "- Product Name changed!", "Orig: " + origproduct.Name + ", New: " + product.Name, product.Url);
                    }

                    if (!origproduct.Price.Equals(product.Price))
                    {
                        productChanged = true;
                        SendIFTTNotification("[" + origproduct.StoreName + "] " + origproduct.Name + "- Product Price changed!", "Orig: " + origproduct.Price+ ", New: " + product.Price, product.Url);
                    }

                    if (!origproduct.Availability.Equals(product.Availability))
                    {
                        productChanged = true;
                        SendIFTTNotification( "[" + origproduct.StoreName + "] " + origproduct.Name + "- Product Availability changed!", "Orig: " + origproduct.Availability+ ", New: " + product.Availability, product.Url);
                    }

                    if (productChanged)
                    {
                        origproduct.CopyFrom(product);
                        ConfigurationToJSON(scrapperData, jsonFile);
                    }
                }
                else if(!product.Valid)
                {
                    Console.WriteLine("Unable to Scrappe: {0}.", product.Url);
                }
            }//);

            logger.Info("Sites Scanned at: " + DateTime.Now.ToString("MMM dd, yyyy - HH:mm:ss.fff"));
        }

        private static void SendIFTTNotification(String value1 = "", String value2= "", String value3= "")
        {
            IFTTTManager man = new IFTTTManager();
            man.SendIFTTNotification(scrapperData.IFTTTEventName, scrapperData.IFTTTWebhookKey, value1, value2, value3);
        }

        private static void ConfigurationToJSON(ConsoleScrapperData scrapperData, String fileName="")
        {
            try
            {

                string rawJson = JsonConvert.SerializeObject(scrapperData, Formatting.Indented);
                if (!String.IsNullOrEmpty(fileName) && File.Exists(fileName) && fileName.ToLower().EndsWith(".json"))
                {

                    File.WriteAllText(fileName, rawJson);
                }
                else
                {
                    string tmpfileName = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";
                    File.WriteAllText(tmpfileName, rawJson);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                //should do something here.
                Console.WriteLine("\nUnable to write json configuration file, {0}!\n", fileName);
            }
        }

        private static ConsoleScrapperData JSONToConfiguration(String fileName)
        {
            ConsoleScrapperData scrapperData;
            try
            {

                if (File.Exists(fileName))
                {
                    StreamReader json = File.OpenText(fileName);
                    String rawJson = json.ReadToEnd();
                    scrapperData = JsonConvert.DeserializeObject<ConsoleScrapperData>(rawJson);
                }
                else
                {
                    scrapperData = new ConsoleScrapperData();
                }
            }catch(Exception ex)
            {
                logger.Error(ex.ToString());
                scrapperData = new ConsoleScrapperData();
            }
            scrapperData.ProductsToCheck = scrapperData.ProductsToCheck.Distinct(new DistinctProductComparer()).ToList();
            return scrapperData;
        }

        private static void SortProductsByStoreName()
        {
            scrapperData.ProductsToCheck = scrapperData.ProductsToCheck.OrderByDescending(p => p.StoreName).ToList();
        }
    }
}
