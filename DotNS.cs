using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

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

        public DotNS(string nation, string password)
        {
            _Nation = nation;
            UpdatePin(nation, password);
        }
    }
}
