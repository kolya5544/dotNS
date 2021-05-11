using dotNS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dotNS
{
    public static class Actions
    {
        public static PublicNationInfo GetNationInfo(this DotNS api, string nation)
        {
            var nvc = new NameValueCollection();
            nvc.Add("nation", nation);
            var resp = Utilities.API(nvc, null, 0, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml);

            var nsinfo = new PublicNationInfo();
            foreach (XmlNode node in nodelist)
            {
                switch (node.Name.ToLower())
                {
                    case "name":
                        nsinfo.Name = node.InnerText; break;
                    case "type":
                        nsinfo.Type = node.InnerText; break;
                    case "fullname":
                        nsinfo.FullName = node.InnerText; break;
                    case "motto":
                        nsinfo.Motto = node.InnerText; break;
                    case "category":
                        nsinfo.Category = node.InnerText; break;
                    case "unstatus":
                        nsinfo.WAStatus = node.InnerText; break;
                    case "endorsements":
                        string[] endors = node.InnerText.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        if (endors.Length == 0 && node.InnerText.Length > 1)
                        {
                            endors = new string[1] { node.InnerText };
                        }
                        nsinfo.Endorsements = endors.ToList(); break;
                    case "issues_answered":
                        nsinfo.IssuesAnswered = long.Parse(node.InnerText); break;
                    case "freedom":
                        if (nsinfo.Freedom == null) nsinfo.Freedom = new PublicNationStats();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            switch (n.Name.ToLower())
                            {
                                case "civilrights":
                                    nsinfo.Freedom.CivilRights = n.InnerText; break;
                                case "economy":
                                    nsinfo.Freedom.Economy = n.InnerText; break;
                                case "politicalfreedom":
                                    nsinfo.Freedom.PoliticalFreedom = n.InnerText; break;
                            }
                        }
                        break;
                    case "region":
                        nsinfo.Region = node.InnerText; break;
                    case "population":
                        nsinfo.Population = long.Parse(node.InnerText) * 1000000; break;
                    case "tax":
                        nsinfo.Tax = double.Parse(node.InnerText, CultureInfo.InvariantCulture) / 100; break;
                    case "animal":
                        nsinfo.Animal = node.InnerText; break;
                    case "currency":
                        nsinfo.Currency = node.InnerText; break;
                    case "demonym":
                        nsinfo.Demonym1 = node.InnerText; break;
                    case "demonym2":
                        nsinfo.Demonym2 = node.InnerText; break;
                    case "demonym2plural":
                        nsinfo.Demonym2Plural = node.InnerText; break;
                    case "flag":
                        nsinfo.FlagURL = node.InnerText; break;
                    case "majorindustry":
                        nsinfo.MajorIndustry = node.InnerText; break;
                    case "govtpriority":
                        nsinfo.GovPriority = node.InnerText; break;
                    case "govt":
                        nsinfo.Gov = new Government();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            switch (n.Name.ToLower())
                            {
                                case "administration":
                                    nsinfo.Gov.Administration = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "defence":
                                    nsinfo.Gov.Defence = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "education":
                                    nsinfo.Gov.Education = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "environment":
                                    nsinfo.Gov.Environment = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "healthcare":
                                    nsinfo.Gov.Healthcare = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "commerce":
                                    nsinfo.Gov.Commerce = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "internationalaid":
                                    nsinfo.Gov.InternationalAid = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "lawandorder":
                                    nsinfo.Gov.LawAndOrder = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "publictransport":
                                    nsinfo.Gov.PublicTransport = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "socialequality":
                                    nsinfo.Gov.SocialEquality = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "spirituality":
                                    nsinfo.Gov.Spirituality = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                                case "welfare":
                                    nsinfo.Gov.Welfare = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100; break;
                            }
                        }
                        break;
                    case "founded":
                        nsinfo.Founded = node.InnerText; break;
                    case "firstlogin":
                        nsinfo.FirstLogin = long.Parse(node.InnerText, CultureInfo.InvariantCulture); break;
                    case "lastlogin":
                        nsinfo.LastLogin = long.Parse(node.InnerText, CultureInfo.InvariantCulture); break;
                    case "lastactivity":
                        nsinfo.LastActive = node.InnerText; break;
                    case "influence":
                        nsinfo.Influence = node.InnerText; break;
                    case "freedomscores":
                        if (nsinfo.Freedom == null) nsinfo.Freedom = new PublicNationStats();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            switch (n.Name.ToLower())
                            {
                                case "civilrights":
                                    nsinfo.Freedom.Score_CR = int.Parse(n.InnerText); break;
                                case "economy":
                                    nsinfo.Freedom.Score_Eco = int.Parse(n.InnerText); break;
                                case "politicalfreedom":
                                    nsinfo.Freedom.Score_PF = int.Parse(n.InnerText); break;
                            }
                        }
                        break;
                    case "publicsector":
                        nsinfo.PublicSector = double.Parse(node.InnerText, CultureInfo.InvariantCulture) / 100; break;
                    case "deaths":
                        nsinfo.Deaths = new List<DeathCause>();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            var cause = new DeathCause()
                            {
                                Name = n.Attributes["type"].Value,
                                Percentage = double.Parse(n.InnerText, CultureInfo.InvariantCulture) / 100
                            };
                            nsinfo.Deaths.Add(cause);
                        } 
                        break;
                    case "leader":
                        nsinfo.Leader = node.InnerText; break;
                    case "capital":
                        nsinfo.Capital = node.InnerText; break;
                    case "religion":
                        nsinfo.Religion = node.InnerText; break;
                    case "factbooks":
                        nsinfo.Factbooks = long.Parse(node.InnerText); break;
                    case "dispatches":
                        nsinfo.Dispatches = long.Parse(node.InnerText); break;
                    case "dbid":
                        nsinfo.ID = long.Parse(node.InnerText); break;
                }
            }
            return nsinfo;
        }

        public static PublicRegionInfo GetRegionInfo(this DotNS api, string region)
        {
            var nvc = new NameValueCollection();
            nvc.Add("region", region);
            var resp = Utilities.API(nvc, null, 0, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml, "/REGION/*");

            var nsinfo = new PublicRegionInfo();
            foreach (XmlNode node in nodelist)
            {
                switch (node.Name.ToLower())
                {
                    case "name":
                        nsinfo.Name = node.InnerText; break;
                    case "factbook":
                        nsinfo.Factbook = node.InnerText; break;
                    case "numnations":
                        nsinfo.NumNations = int.Parse(node.InnerText); break;
                    case "nations":
                        string[] nations = node.InnerText.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        if (nations.Length == 0) nations = new string[1] { node.InnerText };
                        nsinfo.Nations = nations;
                        break;
                    case "delegate":
                        if (node.InnerText == "0") { nsinfo.Delegate = null; break; }
                        nsinfo.Delegate = node.InnerText; break;
                    case "delegatevotes":
                        nsinfo.DelegateVotes = int.Parse(node.InnerText); break;
                    case "delegateauth":
                        nsinfo.DelegateAuthority = Utilities.ParseAuthority(node.InnerText); break;
                    case "founder":
                        if (node.InnerText == "0") { nsinfo.Founder = null; break; }
                        nsinfo.Founder = node.InnerText; break;
                    case "founderauth":
                        nsinfo.FounderAuthority = Utilities.ParseAuthority(node.InnerText); break;
                    case "officers":
                        List<PublicRegionOfficer> officers = new List<PublicRegionOfficer>();
                        foreach (XmlNode officerNode in node.ChildNodes)
                        {
                            var c = officerNode.ChildNodes;
                            var off = new PublicRegionOfficer();
                            off.Nation = c.FindProperty("nation");
                            off.OfficeName = c.FindProperty("office");
                            off.Authority = Utilities.ParseAuthority(c.FindProperty("authority"));
                            off.Time = long.Parse(c.FindProperty("time"));
                            off.AssignedBy = c.FindProperty("by");
                            off.Order = int.Parse(c.FindProperty("order"));
                            officers.Add(off);
                        }
                        nsinfo.Officers = officers.ToArray(); break;
                    case "power":
                        nsinfo.Power = node.InnerText; break;
                    case "flag":
                        nsinfo.FlagURL = node.InnerText; break;
                    case "embassies":
                        List<Embassy> embassies = new List<Embassy>();
                        foreach (XmlNode embassyNode in node.ChildNodes)
                        {
                            var emb = new Embassy();
                            if (embassyNode.Attributes != null && embassyNode.Attributes["type"] != null) emb.Denied = embassyNode.Attributes["type"].InnerText == "denied";
                            emb.Nation = embassyNode.InnerText;
                        }
                        nsinfo.Embassies = embassies.ToArray();
                        break;
                    case "lastupdate":
                        nsinfo.LastUpdate = long.Parse(node.InnerText); break;
                }
            }
            return nsinfo;
        }

        public static PrivateNationInfo GetPrivateNation(this DotNS api)
        {
            if (!api.IsAuthed) throw new Exception("Not authentificated.");
            var nvc = new NameValueCollection();
            nvc.Add("nation", api.Nation);
            nvc.Add("q", "dossier+issues+issuesummary+nextissue+nextissuetime+notices+packs+ping+rdossier+unread");
            var resp = Utilities.API(nvc, null, api.Pin, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml);

            var nsinfo = new PrivateNationInfo();
            foreach (XmlNode node in nodelist)
            {
                switch (node.Name.ToLower())
                {
                    case "unread":
                        nsinfo.UnreadIssues = int.Parse(node.ChildNodes.FindProperty("issues"));
                        nsinfo.UnreadTelegrams = int.Parse(node.ChildNodes.FindProperty("telegrams"));
                        nsinfo.UnreadNotices = int.Parse(node.ChildNodes.FindProperty("notices"));
                        nsinfo.UnreadRegion = int.Parse(node.ChildNodes.FindProperty("rmb"));
                        nsinfo.UnreadWA = int.Parse(node.ChildNodes.FindProperty("wa"));
                        nsinfo.UnreadNews = int.Parse(node.ChildNodes.FindProperty("news"));
                        break;
                    case "notices":
                        List<Notice> notices = new List<Notice>();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            var child = n.ChildNodes;
                            var notice = new Notice();
                            notice.Read = child.FindProperty("ok") == "1";
                            notice.Text = child.FindProperty("text");
                            notice.Timestamp = long.Parse(child.FindProperty("timestamp"));
                            notice.Title = child.FindProperty("title");
                            notice.Type = child.FindProperty("type");
                            notice.TypeIcon = child.FindProperty("type_icon");
                            notice.URL = child.FindProperty("url");
                            notice.Who = child.FindProperty("who");
                            notices.Add(notice);
                        }
                        nsinfo.Notices = notices.ToArray();
                        break;
                    case "dossier":
                        var dossier = new List<string>();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            dossier.Add(n.InnerText);
                        }
                        nsinfo.NationDossier = dossier.ToArray();
                        break;
                    case "rdossier":
                        var rdossier = new List<string>();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            rdossier.Add(n.InnerText);
                        }
                        nsinfo.RegionDossier = rdossier.ToArray();
                        break;
                    case "issues":
                        var issues = new List<Issue>();
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            var child = n.ChildNodes;
                            var issue = new Issue();
                            issue.Title = child.FindProperty("title");
                            issue.Text = child.FindProperty("text");
                            issue.Author = child.FindProperty("author");
                            issue.Editor = child.FindProperty("editor");
                            issue.Pic1 = child.FindProperty("pic1");
                            issue.Pic2 = child.FindProperty("pic2");
                            issue.ID = int.Parse(node.ChildNodes.FindProperty("id"));
                            var options = new List<IssueOption>();
                            foreach (XmlNode n2 in child)
                            {
                                if (n2.Name == "OPTION")
                                {
                                    var option = new IssueOption()
                                    {
                                        ID = int.Parse(n2.Attributes["id"].InnerText),
                                        Text = n2.InnerText
                                    };
                                    options.Add(option);
                                }
                            }
                            issue.Options = options.ToArray();
                            issues.Add(issue);
                        }
                        nsinfo.Issues = issues.ToArray();
                        break;
                    case "nextissue":
                        nsinfo.NextIssue = node.InnerText; break;
                    case "nextissuetime":
                        nsinfo.NextIssueTime = long.Parse(node.InnerText); break;
                    case "packs":
                        nsinfo.Packs = int.Parse(node.InnerText); break;
                }
            }
            return nsinfo;
        }

        public static bool Verify(this DotNS api, string nation, string code)
        {
            var nvc = new NameValueCollection();
            nvc.Add("a", "verify");
            nvc.Add("nation", nation);
            nvc.Add("checksum", code);
            var resp = Utilities.API(nvc, null, 0, api.UserAgent);
            string stringResp = Utilities.StrResp(resp).Trim();
            if (stringResp == "1") return true;
            return false;
        }

        public static XmlNodeList AddressIssue(this DotNS api, Issue issue, IssueOption option)
        {
            if (!api.IsAuthed) throw new Exception("Not authentificated.");
            var nvc = new NameValueCollection();
            nvc.Add("nation", api.Nation);
            nvc.Add("c", "issue");
            nvc.Add("issue", issue.ID.ToString());
            nvc.Add("option", option.ID.ToString());
            var resp = Utilities.API(nvc, null, api.Pin, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml, "*");
            return nodelist;
        }

        public static string[] PublicShard(this DotNS api, string name, Shards.PublicShard[] shards, RequestType type = RequestType.Nation)
        {
            var nvc = new NameValueCollection();
            if (type == RequestType.Nation) { nvc.Add("nation", name); } else { nvc.Add("region", name); }
            string requests = ""; shards.ToList().ForEach(z => { requests += z + "+"; }); requests = requests.TrimEnd('+');
            nvc.Add("q", requests);
            var resp = Utilities.API(nvc, null, 0, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml, "*");
            List<string> result = new List<string>();
            foreach (Shards.PublicShard ps in shards)
            {
                result.Add(Utilities.FindProperty(nodelist, ps.ToString()));
            }
            return result.ToArray();
        }

        public static string PublicShard(this DotNS api, string name, Shards.PublicShard shard, RequestType type = RequestType.Nation)
        {
            string[] shards = PublicShard(api, name, new Shards.PublicShard[] { shard }, type);
            if (shards.Length > 0) return shards[0];
            return null;
        }

        public static XmlNodeList RawPublicShard(this DotNS api, string name, Shards.PublicShard[] shards, RequestType type = RequestType.Nation)
        {
            var nvc = new NameValueCollection();
            if (type == RequestType.Nation) { nvc.Add("nation", name); } else { nvc.Add("region", name); }
            string requests = ""; shards.ToList().ForEach(z => { requests += z + "+"; }); requests = requests.TrimEnd('+');
            nvc.Add("q", requests);
            var resp = Utilities.API(nvc, null, 0, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml, "*");
            return nodelist;
        }

        public static XmlNodeList RawPublicShard(this DotNS api, string name, Shards.PublicShard shard, RequestType type = RequestType.Nation)
        {
            return RawPublicShard(api, name, new Shards.PublicShard[] { shard }, type);
        }

        public static string[] PrivateShard(this DotNS api, Shards.PrivateShard[] shards)
        {
            if (!api.IsAuthed) throw new Exception("Not authentificated.");
            var nvc = new NameValueCollection();
            nvc.Add("nation", api.Nation);
            string requests = ""; shards.ToList().ForEach(z => { requests += z + "+"; }); requests = requests.TrimEnd('+').ToLower();
            nvc.Add("q", requests);
            var resp = Utilities.API(nvc, null, api.Pin, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml, "*");
            List<string> result = new List<string>();
            foreach (Shards.PrivateShard ps in shards)
            {
                result.Add(Utilities.FindProperty(nodelist, ps.ToString()));
            }
            return result.ToArray();
        }

        public static string PrivateShard(this DotNS api, Shards.PrivateShard shard)
        {
            string[] shards = PrivateShard(api, new Shards.PrivateShard[] { shard });
            if (shards.Length > 0) return shards[0];
            return null;
        }

        public static XmlNodeList RawPrivateShard(this DotNS api, Shards.PrivateShard[] shards)
        {
            if (!api.IsAuthed) throw new Exception("Not authentificated.");
            var nvc = new NameValueCollection();
            nvc.Add("nation", api.Nation);
            string requests = ""; shards.ToList().ForEach(z => { requests += z + "+"; }); requests = requests.TrimEnd('+').ToLower();
            nvc.Add("q", requests);
            var resp = Utilities.API(nvc, null, api.Pin, api.UserAgent);
            string xml = Utilities.StrResp(resp);
            var nodelist = Utilities.Parse(xml, "*");
            return nodelist;
        }

        public static XmlNodeList RawPrivateShard(this DotNS api, Shards.PrivateShard shard)
        {
            return RawPrivateShard(api, new Shards.PrivateShard[] { shard });
        }

        public static DailyDataDump GetDump(this DotNS api, RequestType type = RequestType.Nation)
        {
            WebClient web = new WebClient();
            byte[] data = web.DownloadData(type == RequestType.Nation ? "https://www.nationstates.net/pages/nations.xml.gz" : "https://www.nationstates.net/pages/regions.xml.gz");
            var ddd = new DailyDataDump()
            {
                Content = data
            };
            return ddd;
        }
    }
}
