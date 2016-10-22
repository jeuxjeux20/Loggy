using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
namespace Loggy
{
    class Program
    {
        static void Main(string[] args) => new Program().Start(args);

        private DiscordClient _client;
        private bool safeTo(Dictionary<Channel, Channel> dict, Pair<Channel, Channel> per)
        {
            bool k = true;
            foreach (var item in dict)
            {
                if (item.Key == per.Second && item.Value == per.First)
                {
                    k = false;
                    break;
                }
            }
            return k;
        }
        public void Start(string[] args = null)
        {
            _client = new DiscordClient();

            var toRecord = new Dictionary<Channel, Channel>();
            _client.UsingCommands(x =>
            {
                x.PrefixChar = '&';
                x.HelpMode = HelpMode.Public;
            });

            _client.GetService<CommandService>().CreateCommand("logList")
                .Description("Make a list of all loggers")
                .Do(async e =>
                    {
                        try
                        {
                            foreach (var item in toRecord)
                            {

                            }
                        }
                        catch (Exception)
                        {
                            await e.Channel.SendMessage("There isn't any loggers ! Create one using &record");
                        }
                    });


            _client.GetService<CommandService>().CreateCommand("record")
            .Description("record a channel")
            .Parameter("a", ParameterType.Required)
            .Parameter("b", ParameterType.Required)
            .Do(async e =>
             {
                 string a = e.GetArg("a");
                 string b = e.GetArg("b");
                 Pair<bool, bool> foundie = new Pair<bool, bool>(false, false); // topkek
                 var toRecordTemp = new Pair<Channel, Channel>(null, null);
                 bool isAcceptable = false;
                 foreach (var role in e.User.Roles)
                 {
                     if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin"))
                     {
                         isAcceptable = true;
                         break;
                     }
                 }
                 foreach (var item in e.Server.AllChannels)
                 {

                     if (a == b)
                     {
                         await e.Channel.SendMessage("no really, silly guy xd");
                         break;
                     }
                     if (item.Name == a)
                     {
                         foundie.First = true;
                         toRecordTemp.First = item;
                     }
                     if (item.Name == b)
                     {
                         foundie.Second = true;
                         toRecordTemp.Second = item;
                     }
                 }
                 if (foundie.First == false || foundie.Second == false && isAcceptable && safeTo(toRecord, toRecordTemp))
                 {
                     await e.Channel.SendMessage($"Something didn't got right - Listening : {foundie.First} ; Target : {foundie.Second}");
                 }
                 else if (isAcceptable && safeTo(toRecord, toRecordTemp))
                 {
                     await e.Channel.SendMessage($"~~Succesfully installed Windows 10~~ Succesfully recording #{a} to output #{b}");
                     toRecord.Add(toRecordTemp.First, toRecordTemp.Second);
                 }
                 else if (!isAcceptable)
                 {
                     await e.Channel.SendMessage($"Sorry, but you aren't allowed to use this command ; please get a role that contains \"logger\" or \"Admin\" in its name");
                 }
                 else if (!safeTo(toRecord, toRecordTemp))
                 {
                     await e.Channel.SendMessage(":negative_squared_cross_mark: you can't make a record loop seriously m8");
                 }
             });


            _client.MessageReceived += async (s, e) =>
            {
                foreach (KeyValuePair<Channel, Channel> item in toRecord)
                {
                    if (e.Channel == item.Key)
                    {
                        await item.Value.SendMessage($"#{e.Channel.Name} | **{e.Message.User}** said : {e.Message.RawText}");
                    }
                }
            };

            _client.ExecuteAndWait(async () =>
            {
                if (args == null || (args.Length > 0 ? args?[0] == "" : true))
                    args = new string[1] { "MjM5MzI2ODQ3NDMzNzAzNDI0.CuzKTg.c8nGLDNs-RLPVOVX9DWD4eyoH20" };
                await _client.Connect(args[0], TokenType.Bot);
                await Console.Out.WriteLineAsync("I guess i'm connected");
            });
        }
    }
    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}