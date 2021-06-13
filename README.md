# dotNS - NationStates API access in C#
A C# library for NationStates API access. Developed by [The Empire of IKTeam (Our Glorious Nation)](https://www.nationstates.net/nation=ikteam) 

![Flag of Our Glorious Nation](https://img.nk.ax/sYDVqGRcFCK.png)

## Features
* High level API access to nation and region information, some public and private shards.
* Low level API access for public and private shards
* Issues interaction
* PIN-code authentication
* May act like both a low-level wrapper and high-level OOP library

## Usage
### Initialization
For public shards only, this should be sufficient:
```cs
using dotNS;
<...>
DotNS api = new DotNS();
```
If you later wish to authenticate, you can use this method call:
```cs
// Updates PIN value of the class. Can be used to re-auth.
api.UpdatePin("nation name", "password");
```
or just use this constructor:
```cs
// Initializes NS API wrapper and automatically acquires a PIN for private API
DotNS api = new DotNS("nation name", "password");
```
You can (and actually should) also define your own UserAgent. You can do it either using a constructor:
```cs
DotNS api = new DotNS("nation name", "password", "UserAgent");
```
or after initialization
```cs
api.UserAgent = "UserAgent";
```
### High level API example
#### Get basic nation information
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Use it to acquire nation information
PublicNationInfo nation = api.GetNationInfo("nation name");
// Access information you need
Console.WriteLine($"Population: {nation.Population}, motto: {nation.Motto}");
```
#### Get basic public shard information
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Use it to acquire basic public shard
string shard = api.PublicShard("nation name", Shards.PublicShard.Capital);
// Access information you've requested
Console.WriteLine($"The capital is {shard}");
```
#### Get basic region information
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Use it to acquire basic region info
PublicRegionInfo info = api.GetRegionInfo("region name");
// Access information you need
Console.WriteLine($"The founder is {info.Founder} and there are {info.NumNations} in this region.");
```
#### Verification API
```cs
using dotNS;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Make a call to the API
bool result = api.Verify("nation name", "code");
// Output "Correct!" if the code is correct, or "Incorrect!" if it isn't
Console.WriteLine(result ? "Correct!" : "Incorrect!");
```
#### Respond to an issue
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an authenticated API wrapper
DotNS api = new DotNS("nation name", "password");
// Use it to acquire private nation info
PrivateNationInfo info = api.GetPrivateNation();
// Access the first issue
Issue issue = info.Issues[0];
Console.WriteLine($"The title of the first issue is {issue.Title}, and there are {issue.Options.Length} options.");
// Resolve the issue using the first option.
api.AddressIssue(issue, issue.Options[0]);
// OR, to dismiss the issue
api.AddressIssue(issue, IssueOption.DISMISS);
```
#### Access multiple private shards
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an authenticated API wrapper
DotNS api = new DotNS("nation name", "password");
// Get several shards
string[] info = api.PrivateShard(new Shards.PrivateShard[] { Shards.PrivateShard.NextIssueTime, Shards.PrivateShard.NextIssue });
// Response to the first shard will be first element of the result array
Console.WriteLine($"Next issue time: {info[0]}"); // "Next issue time: 123456789"
Console.WriteLine($"Next issue {info[1]}"); // "Next issue in 3 hours"
```
#### World daily dumps
```cs
using dotNS;
using dotNS.Classes;
using System.Xml;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Get region dump
DailyDataDump dump = api.GetDump(RequestType.Region);
// Save as region.xml.gz
File.WriteAllBytes("region.xml.gz", dump.Content);
// Save as region.xml
File.WriteAllBytes("region.xml", dump.Decompress());
// Open as XML
XmlNodeList xml = dump.GetXml();
XmlNodeList regionsXml = xml.TakeNodes("regions");
Console.WriteLine($"There seem to be {regionsXml.Count} regions!");
```
#### Nation census statistics
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Get Black Market data for "IKTeam" nation between Feb 4 2021 and May 31 2021 (UNIX timestamp)
List<CensusNode> censusData = api.GetCensus("IKTeam", Census.BlackMarket, 1612456412, 1622456426);
// Output required information
CensusNode node = censusData[10];
Console.WriteLine($"On {DateTimeOffset.FromUnixTimeSeconds(node.timestamp):dd/MM/yyyy HH:mm:ss} the value of Black Market in IKTeam was {node.value}");
```
#### Get flag of a nation or region
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Get basic information of nation/region
// -> PublicRegionInfo info = api.GetRegionInfo("region");
PublicNationInfo info = api.GetNationInfo("nation");
// Get flag.
System.Drawing.Bitmap bmp = info.GetFlag();
bmp.Save("flag.png");
```
### Low level API example
#### Get advanced public shard information
```cs
using dotNS;
using dotNS.Classes;
using System.Xml;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Get policies of a nation
XmlNodeList policies = api.RawPublicShard("nation name", Shards.PublicShard.Policies);
// Takes all nodes in <POLICIES> tag, since `policies` looks like <nation><policies>...</policies></nation>
XmlNodeList nodes = policies.TakeNodes("policies");
foreach (XmlNode node in nodes)
{
    // Outputs name of a policy (for example, "Compulsory Organ Harvesting")
    Console.WriteLine(node.ChildNodes.FindProperty("name"));
}
```
#### Get advanced public region shard information
```cs
using dotNS;
using dotNS.Classes;
<...>
// Create an API wrapper
DotNS api = new DotNS();
// Get nations of a region
XmlNodeList nations = api.RawPublicShard("region name", Shards.PublicShard.Nations, RequestType.Region);
// Since the output contains <nations>...</nations> tag, we can use .FindProperty to access its contents
string[] RegionNations = nations.FindProperty("nations").Split(':');
foreach (string n in RegionNations)
{
    // Outputs all nations in a region.
    Console.WriteLine(n);
}
```
#### Using API directly
```cs
using dotNS;
using System.Collections.Specialized;
using System.Xml;
<...>
// This will be used for GET request parameters
NameValueCollection nvc = new NameValueCollection();
nvc.Add("nation", "ikteam");
nvc.Add("q", "leader");
var resp = Utilities.API(nvc); // You can also define pass (null by default) and pin (null by default) like Utilities.API(nvc, pass, PIN). You can also define UserAgent as the fourth argument.
// Use StrResp to convert the API response to XML string
string xml = Utilities.StrResp(resp);
 // Use Parse to convert the XML string to NodeList
XmlNodeList nodelist = Utilities.Parse(xml); // By default, it takes nodes by path `/NATION/*`. Use second argument to define the path, like Utilities.Parse(xml, "*");
// Use FindProperty to find a property or attribute by name. Returns first occurance.
string leaderName = nodelist.FindProperty("leader");
Console.WriteLine($"Leader is {leaderName}"); // "Leader is Overlord"
```

## Any issues?

Ask on GitHub issues! We are ready to help. We are open to new feature suggestions and pull requests.