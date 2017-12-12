using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace WebApplication2.Controllers
{
    public class JSONController : Controller
    {
        public ActionResult GetCats(string url)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(url);

                var jArray = JsonConvert.DeserializeObject<JToken>(json);
                var children = jArray.Children();
                var male = children.Where(c => (string)c["gender"] == "Male").Select(m => { return GetNames(m); });
                var female = children.Where(c => (string)c["gender"] == "Female").Select(m => { return GetNames(m); });

                List<string> maleList = male.Where(s => !string.IsNullOrEmpty(s)).ToList();
                List<string> femaleList = female.Where(s => !string.IsNullOrEmpty(s)).ToList();

                maleList.Sort();
                femaleList.Sort();

                CatsModel cats = new CatsModel();

                cats.male = maleList;
                cats.female = femaleList;
                return View(cats);
            }
        }

        private string GetNames(JToken m)
        {
            string name = null;
            if (m["pets"].HasValues)
            {
                name = (string)m["pets"][0]["name"];
            }
            return name;
        }
    }
}
