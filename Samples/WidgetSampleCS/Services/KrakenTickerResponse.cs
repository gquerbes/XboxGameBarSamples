using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetSampleCS.Services
{
        public class ADAUSD
        {
            public List<string> a { get; set; }
            public List<string> b { get; set; }
            public List<string> c { get; set; }
            public List<string> v { get; set; }
            public List<string> p { get; set; }
            public List<int> t { get; set; }
            public List<string> l { get; set; }
            public List<string> h { get; set; }
            public string o { get; set; }
        }

        public class XXBTZUSD
        {
            public List<string> a { get; set; }
            public List<string> b { get; set; }
            public List<string> c { get; set; }
            public List<string> v { get; set; }
            public List<string> p { get; set; }
            public List<int> t { get; set; }
            public List<string> l { get; set; }
            public List<string> h { get; set; }
            public string o { get; set; }
        }

        public class Result
        {
            public ADAUSD ADAUSD { get; set; }
            public XXBTZUSD XXBTZUSD { get; set; }
        }

        public class KrakenTickerResponse
        {
            public List<object> error { get; set; }
            public Result result { get; set; }
        }

}
