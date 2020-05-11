using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
namespace Drag_and_drop.Controllers
{
    public class WebController : Controller
    {
        // GET: Web
        public ActionResult Org()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Org(string name)
        {
            List<string> a = new List<string>();
            if (TempData["list"] != null)
                a = (List<string>)TempData["list"];
            a.Add(name);
            TempData["list"] = a;
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public string List()
        {
            var json = JsonConvert.SerializeObject(TempData["list"]);
            return json;
        }
        public ActionResult ip()
        {
            ip publicip = new ip();
            var client = new HttpClient();
            var response = client.GetAsync("http://ip-api.com/json");
            response.Wait();
            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsStringAsync();
                readTask.Wait();
                ip ip3 = JsonConvert.DeserializeObject<ip>(readTask.Result);
                publicip = ip3;
            }
            string ip = HttpContext.Request.UserHostAddress;
            if(string.IsNullOrEmpty(ip) || ip=="::1")
            {
                string hostName = Dns.GetHostName();
                IPHostEntry myIP = Dns.GetHostEntry(hostName);
                IPAddress[] address = myIP.AddressList;
                ip = address[2].ToString();
            }
            ViewBag.ip = ip;
            ViewBag.ip2ip = publicip.query;
            ViewBag.ip2c = publicip.country;
            ViewBag.ip2s = publicip.regionName;
            ViewBag.ip2ci = publicip.city;
            ViewBag.ip2z = publicip.zip;
            return PartialView("ip");
        }
    }
}