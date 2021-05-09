using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dotNS.Classes
{
    public class Shards
    {
        public enum PublicShard
        {
            //nation shards
            Admirable, Animal, AnimalTrait, Answered, Banner, Banners, Capital, Category,
            Census, Crime, Currency, CustomLeader, CustomCapital, CustomReligion, DBID,
            Deaths, Demonym, Demonym2, Demonym2plural, Dispatches, DispatchList, Endorsements,
            Factbooks, FactbookList, FirstLogin, Flag, Founded, FoundedTime, Freedom, Fullname,
            GAVote, GDP, Govt, GovtDesc, GovtPriority, Happenings, Income, IndustryDesc, Influence,
            LastActivity, LastLogin, Leader, Legislation, MajorIndustry, Motto, Name, Notable,
            Policies, Poorest, Population, PublicSector, RCensus, Region, Religion, Richest,
            SCVote, Sectors, Sensibilities, Tax, TGCanRecruit, TGCanCampaign, Type, WA,
            WABadges, WCensus, Zombie,
            //region shards
            CensusRanks, Delegate, DelegateAuth, DelegateVotes,
            Embassies, Embassyrmb, Factbook, Founder,
            FounderAuth, History, LastUpdate, Messages,
            Nations, NumNations, Officers, Poll, Power, Tags
        }

        public enum PrivateShard
        {
            Dossier, Issues, IssueSummary, NextIssue, NextIssueTime, Notices, Packs, Ping, RDossier, Unread
        }
    }

    public enum RequestType
    {
        Nation, Region
    }

    public enum Authority
    {
        Executive = 1, 
        WorldAssembly = 2, 
        Appearance = 4, 
        BorderControl = 8, 
        Communications = 16, 
        Embassies = 32, 
        Polls = 64
    }

    public class DailyDataDump
    {
        public byte[] Content;
        public byte[] Decompressed;
        public XmlNodeList Xml;

        public byte[] Decompress()
        {
            using (var from = new MemoryStream(Content))
            using (var to = new MemoryStream())
            using (var gZipStream = new GZipStream(from, CompressionMode.Decompress))
            {
                gZipStream.CopyTo(to);
                byte[] ba = to.ToArray();
                Decompressed = ba;
                return ba;
            }
        }

        public XmlNodeList GetXml()
        {
            byte[] cont = Decompressed;
            if (Decompressed is null)
            {
                cont = Decompress();
            }
            Xml = Utilities.Parse(Encoding.UTF8.GetString(cont), "*");
            return Xml;
        }
    }

    public class Notice
    {
        public bool Read;
        public string Text;
        public long Timestamp;
        public string Title;
        public string Type;
        public string TypeIcon;
        public string URL;
        public string Who;
    }

    public class IssueOption
    {
        public static readonly IssueOption DISMISS = new IssueOption() { ID = -1, Text = "Dismiss" };
        public int ID;
        public string Text;
    }

    public class Issue
    {
        public string Title;
        public string Text;
        public string Author;
        public string Editor;
        public string Pic1;
        public string Pic2;
        public int ID;
        public IssueOption[] Options;
    }

    public class PrivateNationInfo
    {
        public int UnreadIssues;
        public int UnreadTelegrams;
        public int UnreadNotices;
        public int UnreadRegion;
        public int UnreadWA;
        public int UnreadNews;
        public Notice[] Notices;
        public string[] NationDossier;
        public string[] RegionDossier;
        public Issue[] Issues;
        public string NextIssue;
        public long NextIssueTime;
        public int Packs;
    }

    public class PublicNationStats
    {
        public string CivilRights;
        public string Economy;
        public string PoliticalFreedom;
        public int Score_CR;
        public int Score_Eco;
        public int Score_PF;
    }

    public class DeathCause
    {
        public string Name;
        public double Percentage;
    }

    public class Government
    {
        public double Administration;
        public double Defence;
        public double Education;
        public double Environment;
        public double Healthcare;
        public double Commerce;
        public double InternationalAid;
        public double LawAndOrder;
        public double PublicTransport;
        public double SocialEquality;
        public double Spirituality;
        public double Welfare;
    }

    public class PublicRegionOfficer
    {
        public string Nation;
        public string OfficeName;
        public Authority[] Authority;
        public long Time;
        public string AssignedBy;
        public int Order;
    }

    public class Embassy
    {
        public string Nation;
        public bool Denied = false;
    }

    public class PublicRegionInfo
    {
        public Bitmap GetFlag()
        {
            return Utilities.GetPicture(FlagURL);
        }

        public string Name;
        public string Factbook;
        public int NumNations;
        public string[] Nations;
        public string Delegate;
        public int DelegateVotes;
        public Authority[] DelegateAuthority;
        public string Founder;
        public Authority[] FounderAuthority;
        public PublicRegionOfficer[] Officers;
        public string Power;
        public string FlagURL;
        public Embassy[] Embassies;
        public long LastUpdate;
    }

    public class PublicNationInfo
    {
        public Bitmap GetFlag()
        {
            return Utilities.GetPicture(FlagURL);
        }

        public string Name;
        public string Type;
        public string FullName;
        public string Motto;
        public string Category;
        public string WAStatus;
        public List<string> Endorsements;
        public long IssuesAnswered;
        public PublicNationStats Freedom;
        public string Region;
        public long Population;
        public double Tax;
        public string Animal;
        public string Currency;
        public string Demonym1;
        public string Demonym2;
        public string Demonym2Plural;
        public string FlagURL;
        public string MajorIndustry;
        public string GovPriority;
        public Government Gov;
        public string Founded;
        public long FirstLogin;
        public long LastLogin;
        public string LastActive;
        public string Influence;
        public double PublicSector;
        public List<DeathCause> Deaths;
        public string Leader;
        public string Capital;
        public string Religion;
        public long Factbooks;
        public long Dispatches;
        public long ID;
    }
}
