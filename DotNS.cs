using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace dotNS
{
    public class DotNS
    {
        protected string _Nation = null;

        public string Nation { get { return _Nation; } }

        protected bool _IsAuthed = false;
        public bool IsAuthed { get { return _IsAuthed; } }

        public string Pin { get; protected set; } = "-1";

        protected string _UserAgent;
        public string UserAgent { get { return _UserAgent; } set { _UserAgent = value; } }

        protected List<long> _RL_Requests = new List<long>();

        protected bool _RateLimit = true;
        public bool RateLimit { get { return _RateLimit; } set { _RateLimit = value; _RL_Requests = new List<long>(); } }

        public static string DefaultUA = "DotNS Default UserAgent nk.ax";

        public DotNS()
        {
            UserAgent = DefaultUA;
        }

        public bool UpdatePin(string nation, string password)
        {
            var nvc = new NameValueCollection();
            nvc.Add("nation", nation);
            nvc.Add("q", "unread");
            var resp = Utilities.API(nvc, password);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Pin = resp.Headers.GetValues("X-Pin").First();
                _IsAuthed = true;
                _Nation = nation;
                return true;
            }
            return false;
        }

        public DotNS(string nation, string password, string useragent = null)
        {
            _Nation = nation;
            UserAgent = useragent is null ? DefaultUA : useragent;
            UpdatePin(nation, password);
        }

        public void ProcessRatelimit(bool addValue = false)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (_RateLimit)
            {
                _RL_Requests.RemoveAll((z) => { return z + 30000 <= now; });

                int currReq = _RL_Requests.Count;
                
                if (currReq >= 49)
                {
                    var el = _RL_Requests.First();

                    Thread.Sleep((int)(31000 - (now - el)));
                }
            }
            if (addValue)
            {
                _RL_Requests.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        }

        public HttpResponseMessage API(NameValueCollection nvc, string pass = null, string pin = "0")
        {
            ProcessRatelimit(true);
            return Utilities.API(nvc, pass, pin, UserAgent);
        }
    }
}
