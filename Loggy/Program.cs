using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.IO;
namespace Loggy
{
    class Program
    {
        static void Main(string[] args) => new Program().Start(args);
        #region Accept
        private bool isAcceptable(CommandEventArgs e)
        {
            bool acc = false;
            foreach (var role in e.User.Roles)
            {
                if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin"))
                {
                    acc = true;
                    break;
                }
            }
            return acc;
        }
        private bool isAcceptable(User e)
        {
            bool acc = false;
            foreach (var role in e.Roles)
            {
                if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin"))
                {
                    acc = true;
                    break;
                }
            }
            return acc;
        }
        #endregion
        
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
                        bool nothing = true;
                        string message = "List of records :";
                        foreach (var item in toRecord)
                        {
                            nothing = false;
                            message += $"{Environment.NewLine} {item.Key} ---> {item.Value}";
                        }

                        if (nothing)
                            await e.Channel.SendMessage("There isn't any loggers ! Create one using &record");
                        else
                        {
                            await e.Channel.SendMessage(message);
                        }
                    });
            

            _client.GetService<CommandService>().CreateCommand("delete")
.Description("Delete a record")
.Parameter("a", ParameterType.Required)
.Parameter("b", ParameterType.Required)
.Do(async e =>
    {
        string a = e.GetArg("a");
        string b = e.GetArg("b");
        Pair<bool, bool> foundie = new Pair<bool, bool>(false, false); // topkek
        var toRecordTemp = new Pair<Channel, Channel>(null, null);

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
        if (isAcceptable(e) && foundie.First && foundie.Second)
        {
            foreach (var item in toRecord.ToList())
            {
                if (item.Key == toRecordTemp.First && item.Value == toRecordTemp.Second)
                {
                    toRecord.Remove(item.Key);
                }
            }
            await e.Channel.SendMessage($"Sucessfully deleted record ! ({toRecordTemp.First} ---> {toRecordTemp.Second})");
        }
    });


            _client.GetService<CommandService>().CreateCommand("disconnect")
            .Description("Disconnects the bot, what else")
            .Do(async e =>
             {
                 if (e.User.Name == "jeuxjeux20#4664")
                 {
                     await e.Channel.SendMessage("Bye :frowning:");
                     await Task.Delay(1000);
                     await _client.Disconnect();
                     _client.Dispose();
                 }
                 else
                 {
                     await e.Channel.SendMessage("Only jeuxjeux20 can disconnect the bot :p");
                 }
             });

            _client.GetService<CommandService>().CreateCommand("invite")
.Description("Get an invite link kek")
.Do(async e =>
    {
        await e.Channel.SendMessage(@"Here is the link : https://discordapp.com/oauth2/authorize?client_id=239326847433703424&scope=bot&permissions=0");
    });


            _client.GetService<CommandService>().CreateCommand("about")
        .Description("About the dev c:")
        .Do(async e =>
            {
                await e.Channel.SendMessage
                ($@"```This bot is made be jeuxjeux20,
                    it is currently on {_client.Servers.Count()} server(s)
                    I hope that you like it c:```");
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
                 if (foundie.First == false || foundie.Second == false && isAcceptable(e))
                 {
                     await e.Channel.SendMessage($"Something didn't got right - Listening : {foundie.First} ; Target : {foundie.Second}");
                 }
                 else if (isAcceptable(e) && safeTo(toRecord, toRecordTemp))
                 {
                     await e.Channel.SendMessage($"~~Succesfully installed Windows 10~~ Succesfully recording #{a} to output #{b}");
                     bool cool = true;
                     foreach (var item in toRecord.ToList())
                     {
                         if (item.Key == toRecordTemp.First)
                         {
                             await e.Channel.SendMessage("A record is alerady targeting this channel, replacing...");
                             cool = false;
                             toRecord[item.Key] = toRecordTemp.Second;
                             break;
                         }

                     }
                     if (cool)
                         toRecord.Add(toRecordTemp.First, toRecordTemp.Second);
                 }
                 else if (!isAcceptable(e))
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
                string tok;
                string path = AppDomain.CurrentDomain.BaseDirectory + "toke.t";

                string kek;
                if (args == null || (args.Length > 0 ? args?[0] == "" : true))
                {
                    tok = "kek";
                    using (FileStream file = File.Open(AppDomain.CurrentDomain.BaseDirectory + "toke.t", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (var str = new StreamReader(file))
                        {
                            tok = str.ReadToEnd();
                            str.Close();
                        }
                    }

                }
                else
                {
                    tok = args[0];
                }
                Retry:
                try
                {
                    await _client.Connect(tok, TokenType.Bot);
                }
                catch (Exception)
                {
                    await Console.Out.WriteLineAsync("Insert token plesssss");
                    await Console.Out.FlushAsync();
                    kek = await Console.In.ReadLineAsync();
                    tok = kek;
                    goto Retry;
                }
                await Console.Out.WriteLineAsync("I guess i'm connected");
                using (FileStream file = File.Open(AppDomain.CurrentDomain.BaseDirectory + "toke.t", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (var sw = new StreamWriter(file))
                    {
                        await sw.WriteLineAsync(tok);
                        await sw.FlushAsync();
                        sw.Close();
                    }
                }


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