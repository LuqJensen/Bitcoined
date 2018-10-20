using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bitcoined.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bitcoined.Controllers
{
    public class HomeController : Controller
    {
        public const string IP = "localhost";
        public const int PORT = 18443;

        public IActionResult Index()
        {
            return View();
        }

        public JObject InvokeMethod(string method, params object[] parameters)
        {
            Uri uri = new Uri($"http://{IP}:{PORT}");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Credentials = new NetworkCredential()
            {
                Password = "guddav"
            };
            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";

            JObject joe = new JObject
            {
                ["jsonrpc"] = "1.0",
                ["id"] = "1",
                ["method"] = method
            };

            if (parameters?.Length > 0)
            {
                JArray props = new JArray();
                foreach (var p in parameters)
                {
                    props.Add(p);
                }
                joe.Add(new JProperty("params", props));
            }

            string s = JsonConvert.SerializeObject(joe);
            byte[] byteArray = Encoding.UTF8.GetBytes(s);
            webRequest.ContentLength = byteArray.Length;
            
            using (Stream dataStream = webRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            using (var webResponse = webRequest.GetResponse())
            {
                using (Stream str = webResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(str))
                    {
                        return JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
                    }
                }
            }
        }

        public IActionResult Balance()
        {
            var balance = InvokeMethod("getbalance");

            return View(balance);
        }

        public IActionResult Address()
        {
            var address = InvokeMethod("getnewaddress");
            return View(address);
        }

        [HttpPost]
        public IActionResult SendToAddress(string address, double amount)
        {
            var balance = InvokeMethod("getbalance");
            var max = double.Parse(balance["result"].ToString());
            if (!(amount > 0) || amount > max)
            {
                throw new Exception("Insufficient funds");
            }
            var transaction = InvokeMethod("sendtoaddress", address, amount);
            return View("Transaction", transaction);
        }

        public IActionResult Unspent()
        {
            var unspent = InvokeMethod("listunspent");
            return View(unspent);
        }

        [HttpPost]
        public IActionResult Generate(int blocks)
        {
            var generate = InvokeMethod("generate", blocks);
            return View(generate);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
