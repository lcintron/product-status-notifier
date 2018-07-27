using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Scripting.JavaScript.Dom;
using AngleSharp.Network.Default;

namespace WebParser
{
    public class WebParser
    {
        public WebParser()
        {

        }
        protected virtual async Task<IDocument> GetWeb(String Uri)
        {
            try
            {
                /*
                 * */
               
               
                                       
                var requester = new HttpRequester();
                string userAgentHeader = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.108 Safari/537.36";
                string acceptHeaer = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                string acceptEncodingHeader = "gzip, deflate, br";
                string acceptLanguageHeader = "en-US,en;q=0.9,es;q=0.8";
                string connectionHeader = "keep-alive";
                string dntHeader = "1";

                requester.Headers["User-Agent"] = userAgentHeader;
                requester.Headers["Accept"] = acceptHeaer;
                requester.Headers["DNT"] = dntHeader;
                requester.Headers["Accept-Language"] = acceptLanguageHeader;
                requester.Headers["Accept-Encoding"] = acceptEncodingHeader;
                requester.Headers["Connection"] = connectionHeader;
                requester.Headers["Upgrade-Insecure-Requests"] = "1";

                //if (Uri.Contains("newegg.com"))
                //{
                    //string hostHeader = "newegg.needle.com";
                    //string refererHeader = "https://www.newegg.com/Product/ProductList.aspx?Submit=ENE&DEPA=0&Order=BESTMATCH&Description=vega+64&N=-1&isNodeId=1";
                    //requester.Headers["Host"] = hostHeader;
                    //requester.Headers["Referer"] = refererHeader;
                    //requester.Headers["Cookie"] = "ADCOOKIE=74; NetworkingPV=1; ComponentsPV=117; StoragePV=3; TOTALPRD=307; _sckey=mb3-5a29dbb744f186.95597886; sr_pp2_exp=2; sr_mc_exp=2; __gads=ID=0df2d6cc0ae21af2:T=1512931137:S=ALNI_MYr3UR-xsmacFUcm5Wg_Arbl9lWsA; sr_ismember=true; sr_browser_id=64cf9e1e-3c60-4eef-9f81-f527f37aaedc; _ga=GA1.2.1165389491.1512692654; spid=C8688D1A-2BD3-4941-8B71-72CB5E9FDE06; s_fid=76FE065E2DE09A60-3CEAD29E99F57969; s_vi=[CS]v1|2D14EDD705032D0D-600011846000CBEC[CE]; cid_csid=1a9d885a-f7fb-43fb-9e08-990d7c75784f; mt.v=2.976334526.1512692651305; _gid=GA1.2.2073633060.1513375881; NV%5FCOUNTRY=#5%7B%22Sites%22%3A%7B%22USA%22%3A%7B%22Value%22%3A%22U%22%2C%22Exp%22%3A%221515967927%22%7D%7D%7D; _gac_UA-1147542-1=1.1513660765.CjwKCAiA693RBRAwEiwALCc3uxlwD6tBXO4Xf7ulY39XzI7QZMq_2EzmgF5yqbZvIKwsc8173CMDmhoCdo4QAvD_BwE; mp_dev_mixpanel=%7B%22distinct_id%22%3A%20%221606d3657018a8-04015d06cfd2ef-e323462-1aeaa0-1606d365702766%22%2C%22g_search_engine%22%3A%20%22google%22%7D; _sr_sp_id.373b=78e07289-92d7-4824-b676-6350e0d68103.1512705368.7.1513661760.1513638418.fab137e9-d667-4b8c-ba07-da5d928ae3ce; NV%5FNEWGOOGLE%5FANALYTICS=#5%7B%22Sites%22%3A%7B%22USA%22%3A%7B%22Values%22%3A%7B%22w14%22%3A%221%22%7D%2C%22Exp%22%3A%221516379202%22%7D%7D%7D; NV%5FCARTINFO=#5%7b%22Sites%22%3a%7b%22USA%22%3a%7b%22Value%22%3a%220%2bItem%2b%257c%2b%2525240.00%22%2c%22Exp%22%3a%221514432867%22%7d%7d%7d; CRTOABE=1; s_cc=true; _scsess=sess-6-5a3c1a2f8011f0.23221557; NVTC=248326808.0001.319501644.1512692651.1513888270.1513891804.35; AKA_A2=1; ADCOOKIE=27; sp_ssid=1513892201574; TT3bl=false; TURNTO_VISITOR_SESSION=1; TURNTO_VISITOR_COOKIE=6RcTyq7tz1gW1KD,1,0,0,null,,,0,0,0,0,0,0,0; NV%5FSPT=12-Stu-999x1-0-100; NV%5FDVINFO=#5%7b%22Sites%22%3a%7b%22USA%22%3a%7b%22Values%22%3a%7b%22w19%22%3a%22Y%22%2c%22w51%22%3a%22T%255FPRaqYlBzIgooSLbKGLqgG7KACRrexOLa48fNuo%252fxpD1wEG9fj6Qz0t1NzlxofnHO%22%2c%22s81%22%3a%2250618426%22%7d%2c%22Exp%22%3a%221513974663%22%7d%7d%7d; NV%5FRLN=#5%7b%22Sites%22%3a%7b%22USA%22%3a%7b%22Values%22%3a%7b%22sj%22%3a%221%22%2c%22s62%22%3a%22l.cintron%2540live.com%22%7d%2c%22Exp%22%3a%221516485484%22%7d%7d%7d; NV%5FCUSTOMERLOGIN=#5%7b%22Sites%22%3a%7b%22USA%22%3a%7b%22Values%22%3a%7b%22sj%22%3a%221%22%2c%22sb%22%3a%22BQcZe4w9Cqn6lTRFGz1xxcpv4COGmCAQtAd6erBANEcQ%252b5LyerFyMA%253d%253d%22%2c%22sd%22%3a%22l.cintron%2540live.com%22%2c%22sc%22%3a%2250618426%22%2c%22si%22%3a%22LUIS%2bCINTRON%22%2c%22s125%22%3a%22Knu7sPnAosH3azxbkfAGP0CiIvFkO197U7Ya0ovVGULqvc%252fOBQnMUyJYOAcTG1kt%22%2c%22so%22%3a%222%22%2c%22sl%22%3a%22376141692%22%2c%22s91%22%3a%220%22%7d%2c%22Exp%22%3a%222460578359%22%7d%7d%7d; NV%5FOTHERINFO=#5%7b%22Sites%22%3a%7b%22USA%22%3a%7b%22Values%22%3a%7b%22s123%22%3a%22IeZqyxSxLhw%253d%22%2c%22sb%22%3a%22BQcZe4w9Cqn6lTRFGz1xxcpv4COGmCAQtAd6erBANEcQ%252b5LyerFyMA%253d%253d%22%2c%22sd%22%3a%22BQcZe4w9Cqn6lTRFGz1xxcpv4COGmCAQtAd6erBANEcQ%252b5LyerFyMA%253d%253d%22%2c%22sc%22%3a%22xp7YQxjzXjF0nhLc%252fTURUXfp8%252fVU8YOu%22%2c%22si%22%3a%22LUIS%2bCINTRON%22%2c%22sg%22%3a%22LjaOq%252fY7s%252bY%253d%22%2c%22se%22%3a%22g9apxUNmrTnfcoFWMlOohg%253d%253d%22%2c%22s111%22%3a%220%22%2c%22s115%22%3a%220nNMDJV3VOjhhOc68MAw6Q%253d%253d%22%2c%22sn%22%3a%2294050651896016920171221135804%22%2c%22IsEggXpert%22%3a%220%22%7d%2c%22Exp%22%3a%222460578359%22%7d%7d%7d; NV%5FPRDLIST=#5%7B%22Sites%22%3A%7B%22USA%22%3A%7B%22Values%22%3A%7B%22w32%22%3A%22g%22%2C%22wg%22%3A%2236%22%2C%22wl%22%3A%22BESTMATCH%22%2C%22wn%22%3A%22Y%22%2C%22w47%22%3A%22N82E16814126223%252CN82E16800995282%252CN82E16814131728%252CN82E16800995285%22%7D%2C%22Exp%22%3A%221600293660%22%7D%7D%7D; s_sq=%5B%5BB%5D%5D; mt.mbsh=%7B%22fs%22:1513891809146,%22sf%22:1,%22lf%22:1513893664494%7D; s_sess=%20s_cpc%3D0%3B%20s_event53key%3Dvega%252064%3B%20s_event53%3D%3B%20s_stv%3Dvega%252064%3B%20s_evar17%3Dpage%2520viewed%253A1%252Csort%2520by%253Abestmatch%252Cview%2520count%253A36%3B%20s_ev3%3Dnon-internal%2520campaign%3B; NV_NVTCTIMESTAMP=1513893977; NV%5FCONFIGURATION=#5%7b%22Sites%22%3a%7b%22USA%22%3a%7b%22Values%22%3a%7b%22w58%22%3a%22USD%22%2c%22wd%22%3a%221%22%2c%22w57%22%3a%22USA%22%2c%22w61%22%3a%221545197960000%22%2c%22s93%22%3a%22en%22%2c%22w60%22%3a%221513888263734.19%22%7d%2c%22Exp%22%3a%221600290379%22%7d%7d%7d; NV%5FTHIRD%5FPARTY=#5%7B%22Sites%22%3A%7B%22USA%22%3A%7B%22Values%22%3A%7B%22wu%22%3A%22https%253A%252F%252Fwww.newegg.com%252FProduct%252FProductList.aspx%253FSubmit%253DENE%2526DEPA%253D0%2526Order%253DBESTMATCH%2526Description%253Dvega%252B64%2526N%253D-1%2526isNodeId%253D1%22%7D%2C%22Exp%22%3A%221516485978%22%7D%7D%7D; utag_main=_st:1513895778529$dc_visit:35$ses_id:1513892043629%3Bexp-session$dc_event:18%3Bexp-session$dc_region:us-east-1%3Bexp-session; _uetsid=_uetf0e133dc; mp_newegg_mixpanel=%7B%22distinct_id%22%3A%20%221603382294647c-0e8636f8711fc-7b113d-2a3000-1603382294772b%22%2C%22g_search_engine%22%3A%20%22google%22%7D; TURNTO_TEASER_SHOWN=1513893981452; mt.visits=%7B%22lastVisit%22:1513893989951,%22visits%22:%5B7,4,4,5,3,7,1,0,0,0,0,1,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0%5D%7D; s_pers=%20s_ev19%3D%255B%255B%2527knc%2527%252C%25271512692653111%2527%255D%252C%255B%2527afc%2527%252C%25271513477841930%2527%255D%252C%255B%2527knc%2527%252C%25271513660761509%2527%255D%252C%255B%2527afc%2527%252C%25271513787256680%2527%255D%255D%7C1671553656680%3B%20s_cp_persist%3DAFC-C8Junction-Ebates%2520Performance%2520Marketing%252C%2520Inc.-_-na-_-na-_-na%7C1516420035236%3B%20productnum%3D15%7C1516485660738%3B%20s_vs%3D1%7C1513895837053%3B%20gpv_pv%3Dhome%2520%253E%2520components%2520%253E%2520video%2520cards%2520%2526%2520video%2520devices%2520%253E%2520desktop%2520graphics%2520cards%2520%253E%2520powercolor%2520%253E%2520item%2523%253An82e16814131728%253Aproduct%7C1513895837061%3B%20s_nr%3D1513894037063-Repeat%7C1545430037063%3B%20gpvch%3Dbrowsing%7C1513895837065%3B; needlepin=N190d151269265631944d99002212d37dde9bb17df0e7de7df0f099001500000000117def4fa07def4fa032d90177defdd0f00000000000003%5B@%5D0177defdf463245";
                //}
                
                var config = Configuration.Default.WithDefaultLoader(requesters: new[] { requester }).WithCss().WithCookies().WithJavaScript();

                // We create a new context
                var context = BrowsingContext.New(config);
                IDocument response = await context.OpenAsync(Uri);
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<Product> RequestProduct(String url)
        {
            return new Product();
        }
    }
}