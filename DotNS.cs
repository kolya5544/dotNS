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

        protected long _Pin = -1;
        public long Pin { get { return _Pin; } }

        protected string _UserAgent;
        public string UserAgent { get { return _UserAgent; } set { _UserAgent = value; } }

        protected List<long> _RL_Requests = new List<long>();

        protected bool _RateLimit = true;
        public bool RateLimit { get { return _RateLimit; } set { _RateLimit = value; _RL_Requests = new List<long>(); } }

        public DotNS()
        {

        }

        public bool UpdatePin(string nation, string password)
        {
            var nvc = new NameValueCollection();
            nvc.Add("nation", nation);
            nvc.Add("q", "unread");
            var resp = Utilities.API(nvc, password);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string[] pin = resp.Headers.GetValues("X-Pin").ToArray();
                _Pin = long.Parse(pin[0]);
                _IsAuthed = true;
                _Nation = nation;
                return true;
            }
            return false;
        }

        public DotNS(string nation, string password, string useragent = "DotNS Default UserAgent nk.ax")
        {
            _Nation = nation;
            UserAgent = useragent;
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

        public HttpResponseMessage API(NameValueCollection nvc, string pass = null, long pin = 0)
        {
            ProcessRatelimit(true);
            return Utilities.API(nvc, pass, pin, UserAgent);
        }
    }
}
