using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
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

        public enum World
        {
            //World Assembly API
            NumNations, NumDelegates, Delegates, Members, Happenings, Proposals, Resultion, Voters, VoteTrack, DelLog, DelVotes, LastResolution
        }
    }

    public class CensusNode
    {
        public long timestamp;
        public double value;
    }

    public enum Census
    {
        CivilRights = 0,
        Economy = 1,
        PoliticalFreedom = 2,
        Population = 3,
        Authoritarianism = 53,
        AverageDispIncome = 85,
        AverageInc = 72,
        AveraceIncPoor = 73,
        AverageIncRich = 74,
        Averageness = 67,
        BlackMarket = 79,
        BusinessSubsid = 31,
        Charmlessness = 64,
        Cheerfulness = 40,
        Compassion = 6,
        Compliance = 42,
        Corruption = 51,
        Crime = 77,
        Culture = 55,
        DeathRate = 5,
        DefenseForces = 46,
        EcoFriendliness = 7,
        EconomicFreedom = 48,
        EconomicOutput = 76,
        Employment = 56,
        EnvironmentalBeauty = 63,
        ForeignAid = 78,
        FreedomFromTax = 50,
        GovernmentSize = 27,
        Health = 39,
        HumanDevelopmentIndex = 68,
        IdeologicalRadicality = 45,
        Ignorance = 37,
        Inclusiveness = 71,
        IncomeEquality = 33,
        ArmsManufacturing = 16,
        CarManufacturing = 10,
        BasketWeaving = 12,
        BeverageSales = 18,
        BookPublishing = 24,
        CheeseExports = 11,
        FurnitureRestoration = 22,
        Gambling = 25,
        InformationTechnology = 13,
        Insurance = 21,
        Mining = 20,
        PizzaDelivery = 14,
        Retail = 23,
        TimberWoodchipping = 19,
        TroutFishing = 15,
        Influence = 65,
        Integrity = 52,
        Intelligence = 36,
        InternationalArtwork = 86,
        LawEnforcement = 30,
        Lifespan = 44,
        Niceness = 34,
        Nudity = 9,
        Obesity = 61,
        Pacifism = 47,
        PoliticalApathy = 38,
        Primitiveness = 69,
        PublicEducation = 75,
        PublicHealthcare = 29,
        PublicTransport = 57,
        RecreationalDrugs = 60,
        Religiousness = 32,
        Residency = 80,
        Rudeness = 35,
        Safety = 43,
        ScientificAdvancement = 70,
        Agriculture = 17,
        Manufacturing = 26,
        Secularism = 62,
        SocialConservatism = 8,
        Taxation = 49,
        Tourism = 58,
        WealthGaps = 4,
        Weaponization = 59,
        Weather = 41,
        Welfare = 28,
        Endorsements = 66,
        YouthRebelliousness = 54,
        Patriotism = 87,
        FoodQuality = 88
    }

    public enum WorldAssembly
    {
        GeneralAssembly = 1,
        SecurityCouncil = 2
    }

    public enum RequestType
    {
        Nation, Region
    }

    public enum CardSeason
    {
        Season1 = 1,
        Season2 = 2
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
            Xml = Utilities.Parse(Encoding.UTF8.GetString(cont).Replace("&amp;", "&abcamp;").Replace("&", "&amp;").Replace("&abcamp;", "&amp;"), "*");
            return Xml;
        }
    }
    
    public enum MarketType
    {
        bid, ask
    }

    public class Market
    {
        public string Nation;
        public double Price;
        public long Timestamp;
        public MarketType Type;
    }

    public class Trade
    {
        public string Buyer;
        public string Seller;
        public double Price;
        public long Timestamp;
    }

    public enum CardCategory
    {
        legendary,
        epic,
        ultrarare,
        rare,
        uncommon,
        common
    }

    public class IncompleteTradingCard
    {
        public long ID;
        public CardCategory Category;
        public CardSeason Season;
    }

    public class TradingCard
    {
        public long ID;
        public CardCategory Category;
        private string _FlagURL;
        private Bitmap BMPFlag;
        public string FlagURL
        {
            get {
                if (!_FlagURL.Contains("https://"))
                {
                    if (_FlagURL.Contains("uploads/"))
                    {
                        _FlagURL = $"https://www.nationstates.net/images/cards/s{(int)Season}/{_FlagURL}";
                    }
                    else
                    {
                        _FlagURL = $"https://www.nationstates.net/images/cards/s{(int)Season}/{_FlagURL}{(_FlagURL.EndsWith(".png") ? "" : ".jpg")}";
                    }
                }
                return _FlagURL;
            }
            set
            {
                if (_FlagURL is null)
                {
                    _FlagURL = value;
                }
                else
                {
                    throw new Exception("Value is read-only");
                }
            }
        }
        public Bitmap Flag { get { if (BMPFlag is null) { BMPFlag = Utilities.GetPicture(FlagURL); return BMPFlag; } return BMPFlag; } }
        public string Govt;
        public double MarketValue;
        public List<Market> Markets;
        public string Name;
        public List<string> Owners;
        public string Region;
        public CardSeason Season;
        public string Slogan;
        public List<Trade> Trades;
        public string Type;
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
        [Obsolete("Use .Flag property instead")]
        public Bitmap GetFlag()
        {
            return Utilities.GetPicture(FlagURL);
        }

        public Bitmap _Flag;
        public Bitmap Flag { get { if (_Flag is null) { _Flag = GetFlag(); } return _Flag; } }

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
        [Obsolete("Use .Flag property instead")]
        public Bitmap GetFlag()
        {
            return Utilities.GetPicture(FlagURL);
        }

        public Bitmap _Flag;
        public Bitmap Flag { get { if (_Flag is null) { _Flag = GetFlag(); } return _Flag; } }

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
