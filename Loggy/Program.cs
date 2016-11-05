using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.Collections;
using System.Diagnostics;

namespace Loggy
{
    class Program
    {
        string[] drugs = new string[] { "cake", "Windows 10", "Windows 9", "silent rd", "joolya", "rip", "rip vm", "Windows 7", "silent rd", "gruel", "memz", "bye" };
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
        DateTime lastJoolya = DateTime.UtcNow;
        Dictionary<Command, Cooldown> cool = new Dictionary<Command, Cooldown>();
        private void Repeat(byte n, Action act)
        {
            for (byte i = 0; i > n; i--)
            {
                act();
            }
        }
        private async Task ok(object lol, CommandErrorEventArgs err)
        {
            if (err.ErrorType == CommandErrorType.BadPermissions)
            {
                if (err.Command.Text == "broadcast" || err.Command.Text == "disconnect" || err.Command.Text == "listserv")
                {
                    await err.Channel.SendMessage("**__No__**, only jeuxjeux20 can use this command");
                }
                else if (cool.Keys.Any((c => { return c == err.Command; })))
                {
                    await err.Channel.SendMessage($"Please wait {cool[err.Command].cooldownSeconds} seconds. ty");
                }
                else
                {
                    await err.Channel.SendMessage($":red_circle: => You didn't have got permissions to use this command (try attributting yourself a role named \"Logger\")`");
                }
            }
            else if (err.ErrorType == CommandErrorType.BadArgCount)
            {
                await err.Channel.SendMessage($"Argument error. Please check what arguments you typed in.");
            }
            else if (err.ErrorType == CommandErrorType.Exception)
            {
                if (err.Exception.Message.Contains("2000"))
                {
                    await err.Channel.SendMessage("Too much **chara**cters, m8");
                }
                else if (err.Exception.Message.Contains("blank"))
                {
                    await err.Channel.SendMessage("No parameters provided, m9");
                }
                else
                {
                    await err.Channel.SendMessage($"wait wtf : ```{err.Exception.Message} {err.Exception.StackTrace} {err.Exception.InnerException} ```");
                }

            }
            else if (err.ErrorType == CommandErrorType.UnknownCommand)
            {
                var x = await err.Channel.SendMessage($"Unknown command. `{err.Message.Text}`");
                new Task(async () => { await Task.Delay(1666); await x.Delete(); }).Start();
            }
        }
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
            #region declares
            _client = new DiscordClient();

            var toRecord = new Dictionary<Channel, Channel>();
            #endregion           
            _client.MessageReceived += async (s, e) =>
            {
                foreach (KeyValuePair<Channel, Channel> item in toRecord)
                {
                    if (e.Channel == item.Key)
                    {
                        await item.Value.SendMessage($"#{e.Channel.Name} | **{e.Message.User}** said : {e.Message.Text}");
                    }
                }
            };
            _client.MessageUpdated += async (s, e) =>
            {
                foreach (KeyValuePair<Channel, Channel> item in toRecord)
                {
                    if (e.Channel == item.Key)
                    {
                        await item.Value.SendMessage($@"#{e.Channel.Name} | **{e.User.Name}#{e.User.Discriminator}** edited this message :
{e.Before.Text}
:arrow_down:
{e.After.Text}");
                    }
                }
            };
            _client.UsingCommands(x =>
            {
                x.PrefixChar = '=';
                x.HelpMode = HelpMode.Public;
                x.ErrorHandler += async (a, b) => { await ok(a, b); };
                x.ExecuteHandler += async (a, e) =>
                {

                    try
                    {
                        await Console.Out.WriteLineAsync($"Server: {e.Server.Name} --> received command : {e.Message.Text} from {e.User.Name}#{e.User.Discriminator} ! invite link : {(await e.Server.GetInvites()).First()}");
                    }
                    catch (Exception)
                    {
                        await Console.Out.WriteLineAsync($"Server: {e.Server.Name} --> received command : {e.Message.Text} from {e.User.Name}#{e.User.Discriminator} ! invite link : 403");

                    }
                };
            });
            _client.Ready += (s, e) =>
            {
                _client.SetGame("Type \"=help\" to get started !");
            };
            #region listserv

            _client.GetService<CommandService>().CreateCommand("listServ")
.Description("Get the list of the servers in where is the bot. (:kek:)")
.AddCheck((a, b, c) => { return b.Id == 134348241700388864; }, "u not jeuxjeux20")
.Do(async e =>
    {
        string m = "```";
        foreach (var item in _client.Servers)
        {
            var linq = item.Users
            .Where
            ((u) =>
            {
                return u.Status == UserStatus.Online && !u.IsBot;
            })
            .Select((u) =>
            {
                return u.Name + "#" + u.Discriminator;
            })
            .ItinerateAndGet();

            m += $"{item.Name} => users count : {item.UserCount} - online users {linq} {Environment.NewLine}";
        }
        m += "```";
        await e.Channel.SendMessage(m);
    });


            #endregion
            #region clean

            _client.GetService<CommandService>().CreateCommand("clean")
.Description("Clean [number] messages")
.AddCheck((a, b, c) => { return isAcceptable(b); })
.Parameter("number", ParameterType.Required)
.Do(async e =>
    {
        try
        {
            Message[] toDel;
            int c = Math.Max(1, Convert.ToInt32(e.GetArg("number")));
            toDel = await e.Channel.DownloadMessages(c);
            await e.Channel.DeleteMessages(toDel);
            await e.Channel.SendMessage("Succesfully deleted " + c + " messages !");
        }
        catch (Exception)
        {
            await e.Channel.SendMessage("It seems that the bot hasn't got permissions to delete these.");
        }
    });
            #endregion
            #region wizOnDrugs
            _client.GetService<CommandService>().CreateCommand("wizondrug")
                .Description("get kek and pizzas")
                .Alias(new string[] { "drugs", "electronicdrugs", "wizdrug" })
                .AddCheck((a, b, c) =>
                {
                    if (!cool.Keys.Any((com => { return com == a; })))
                        cool.Add(a, new Cooldown(69));
                    return cool[a].isFinished;
                })

                .Do(async e =>
                {
                    cool[e.Command].Restart();
                    Random kek = new Random(DateTime.UtcNow.Millisecond);
                    List<Message> ls = new List<Message>();
                    for (int i = 0; i < new Random().Next(6, 10); i++)
                    {
                        ls.Add(await e.Channel.SendMessage(drugs[kek.Next(0, drugs.Length)]));
                        await Task.Delay(600);
                    }
                    foreach (var item in ls)
                    {
                        await item.Delete();
                    }
                });
            #endregion // cooldowned
            #region MDMCK10

            _client.GetService<CommandService>().CreateCommand("MDMCK10")
        .Description("MDMCK10 has something to confess")
        .Do(async e =>
            {
                var x = await e.Channel.SendMessage("A gud person that makes bad mistakes sometimes");
                await Task.Delay(500);
                await x.Edit("A gud person that makes very bad mistakes sometimes");
            });


            #endregion
            #region kek

            _client.GetService<CommandService>().CreateCommand("keks")
.Description("get kek and pizzas")
.AddCheck((a, b, c) =>
{
    if (!cool.Keys.Any((com => { return com == a; })))
        cool.Add(a, new Cooldown(15));
    return cool[a].isFinished;
}, "plz wait a little bit")
.Do(async e =>
    {
        cool[e.Command].Restart();
        List<Message> x = new List<Message>();
        x.Add(await e.Channel.SendMessage("cake"));
        x.Add(await e.Channel.SendMessage("cake"));
        x.Add(await e.Channel.SendMessage("cake"));
        x.Add(await e.Channel.SendMessage("cake"));
        x.Add(await e.Channel.SendMessage("cake"));
        x.Add(await e.Channel.SendMessage("cake"));
        await Task.Delay(3000);
        foreach (var item in x)
        {
            await item.Delete();
        }
        await Task.Delay(7500);
        x.Add(await e.Channel.SendMessage("rip"));
        x.Add(await e.Channel.SendMessage("vm"));
        x.Add(await e.Channel.SendMessage("ripvm"));
        x.Add(await e.Channel.SendMessage("bye"));
        x.Add(await e.Channel.SendMessage("Windows 9"));
        x.Add(await e.Channel.SendMessage("hi"));
        await e.Channel.SendMessage("You want kek ?");
        x.Add(await e.Channel.SendMessage("cake"));
        await Task.Delay(3000);
        foreach (var item in x)
        {
            await item.Delete();
        }
    });


            #endregion
            #region It's julia lol
            _client.GetService<CommandService>().CreateCommand("joolya7")
.Description("find out xd")
.Parameter("hi", ParameterType.Unparsed)
.AddCheck((a, b, c) => {
    if (!cool.Keys.Any(com => { return com == a; }))
        cool.Add(a, new Cooldown(7));
    return cool[a].isFinished;
}, "plz wait a little bit")
.Do(async e =>
    {
        cool[e.Command].Restart();
        string mes = "Windows 7";
        foreach (var item in drugs)
        {
            if (e.GetArg("hi").Equals(item))
            {
                mes = item;
                break;
            }
        }
        Message[] k =
        {
        await e.Channel.SendMessage(mes),
        await e.Channel.SendMessage(mes)
        };
        await Task.Delay(750);
        foreach (var item in k)
        {
            await item.Delete();
        }
    });
            #endregion
            #region dogetip
            _client.GetService<CommandService>().CreateCommand("tipmedoge")
                .Description("try it out")
                .Do(async e =>
                    {
                        await e.Channel.SendMessage(":regional_indicator_n::regional_indicator_o:");
                    });
            #endregion           
            #region textemoji
            _client.GetService<CommandService>().CreateCommand("textToEmoji")
   .Description("grab a text, get an emoji kekekekekekekek")
   .Alias(new string[] { "emojiText", "textEmoji", "et", "kekwords", "say" })
   .Parameter("param1", ParameterType.Unparsed)
   .Do(async e =>
       {
           string original = e.GetArg("param1").ToLower();
           string message = TextToEmoji(original);

           await e.Channel.SendMessage(message);
       });
            #endregion // this one everyone loves it idk why
            #region etiTopkek
            _client.GetService<CommandService>().CreateCommand("topkek")
.Description("kekeek")
.Do(async e =>
    {
        await e.Channel.SendFile("top kek.png");
    });
            #endregion 
            #region drinkBleach
            _client.GetService<CommandService>().CreateCommand("bleach")
.Description("drink it = +69 attack")
.Do(async e =>
    {
        await e.Channel.SendFile("bleach.jpeg");
    });
            #endregion
            #region wiz
            _client.GetService<CommandService>().CreateCommand("wiz")
.Description("wiz.z.z.z")
.Do(async e =>
    {
        var kek = await e.Channel.SendMessage("electronicwiz");
        await kek.Edit("electronicwizz");
        await Task.Delay(390);
        await kek.Edit("electronicwizzz");
        await Task.Delay(390);
        await kek.Edit("electronicwizzzz");
        await Task.Delay(390);
        await kek.Edit("electronicwizzzzz");
        await Task.Delay(390);
        await kek.Edit("electronicwizzzzzz");
        await Task.Delay(390);
        await kek.Edit("electronicwizzzzzzz");
        await Task.Delay(390);
    });
            #endregion
            #region loglist
            _client.GetService<CommandService>().CreateCommand("logList")
                .Description("Make a list of all loggers")
                .AddCheck((a, b, c) => { return isAcceptable(b); }, "You must get a role that contains Logger or Admin in its name.")
                .Alias(new string[] { "recordList", "listLog", "listRecord" })
                .Do(async e =>
                    {
                        bool nothing = true;
                        string message = $"List of records for the server: {e.Server.Name}";
                        foreach (var item in toRecord)
                        {
                            if (item.Key.Server == e.Server)
                            {
                                nothing = false;
                                message += $"{Environment.NewLine} {item.Key} ---> {item.Value}";
                            }

                        }

                        if (nothing)
                            await e.Channel.SendMessage("There isn't any loggers ! Create one using &record");
                        else
                        {
                            await e.Channel.SendMessage(message);
                        }
                    });
            #endregion           
            #region delete
            _client.GetService<CommandService>().CreateCommand("delete")
.Description("Delete a record")
.Parameter("a", ParameterType.Required)
.Parameter("b", ParameterType.Required)
.AddCheck((a, b, c) => { return isAcceptable(b); }, "You must get a role that contains Logger or Admin in its name.")
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
        if (foundie.First && foundie.Second)
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
            #endregion           
            #region disconnect
            _client.GetService<CommandService>().CreateCommand("disconnect")
                        .Description("Disconnects the bot, what else")
                        .AddCheck((a, b, c) => { if (b.Name == "jeuxjeux20" && b.Discriminator == 4664) { return true; } else { return false; } }, "Only jeuxjeux20 can disconnect it")
                        .Do(async e =>
                         {
                             await e.Channel.SendMessage("Bye :frowning:");
                             await Task.Delay(1000);
                             await _client.Disconnect();
                             _client.Dispose();
                         });
            #endregion           
            #region invite
            _client.GetService<CommandService>().CreateCommand("invite")
.Description("Get an invite link kek")
.Do(async e =>
    {
        await e.Channel.SendMessage(@"Here is the link : https://discordapp.com/oauth2/authorize?client_id=239326847433703424&scope=bot&permissions=0");
    });
            #endregion
            #region about

            _client.GetService<CommandService>().CreateCommand("about")
        .Description("About the dev c:")
        .Alias(new string[] { "topkekkle" })
        .Do(async e =>
            {
                await e.Channel.SendMessage
                (
$@"```This bot has been made by jeuxjeux20,
it's a funny bot with tons of functions !
it is currently on {_client.Servers.Count()} server(s)
I hope that you like it c:```");
            });

            #endregion
            #region record

            _client.GetService<CommandService>().CreateCommand("record")
            .Description("record a channel")
            .Parameter("a", ParameterType.Required)
            .Parameter("b", ParameterType.Required)
            .AddCheck((a, b, c) => { return isAcceptable(b); }, "You must get a role that contains Logger or Admin in its name.")
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
                 else if (safeTo(toRecord, toRecordTemp))
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

                 else if (!safeTo(toRecord, toRecordTemp))
                 {
                     await e.Channel.SendMessage(":negative_squared_cross_mark: you can't make a record loop seriously m8");
                 }
             });


            #endregion
            #region broadcast

            _client.GetService<CommandService>().CreateCommand("broadcast")
.Description("Only for jeuxjeux20, broadcast a message to all servers. this gonna be fun")
.Parameter("say", ParameterType.Unparsed)
.AddCheck((a, b, c) => { return b.Id == 134348241700388864; }, "u not jeuxjeux20")

.Do(async e =>
{
    foreach (var item in _client.Servers)
    {
        await item.DefaultChannel.SendMessage(e.GetArg("say"));
    }
});


            #endregion

            #region UserAndRoleEvents
            _client.UserBanned += async (s, e) =>
                {
                    await e.Server.DefaultChannel.SendMessage($"**{e.User.Name}#{ e.User.Discriminator}** has been banned :boom:");
                };
            _client.UserLeft += async (s, e) =>
            {
                try
                {

                    if (!(await e.Server.GetBans()).Contains(e.User))
                    {
                        await e.Server.DefaultChannel.SendMessage($"{e.User.Name}#{e.User.Discriminator} has left {e.Server.Name}.");
                    }
                }
                catch (Exception)
                {
                    await e.Server.DefaultChannel.SendMessage($"{e.User.Name}#{e.User.Discriminator} has left {e.Server.Name}.");
                }
            };
            _client.UserUnbanned += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage($"{e.User.Name} has been unbanned from {e.Server.Name}.");
            };
            _client.RoleCreated += async (s, e) =>
            {
                var x = await e.Server.DefaultChannel.SendMessage($@"A role has been created :
Name : {e.Role.Name}
IsMentionnable : {e.Role.IsMentionable}
Position : {e.Role.Position}
This message will be deleted in 10 seconds.");
                new Task(async () => { await Task.Delay(10000); await x.Delete(); }).Start();
            };
            _client.RoleDeleted += async (s, e) =>
            {
                string members = string.Empty;
                foreach (var item in e.Role.Members)
                {
                    members += $"{item} ; ";
                }
                var x = await e.Server.DefaultChannel.SendMessage($@":boom: a role has been ~~destroyed~~ deleted. Name : {e.Role.Name} Members : {members}
This message will be deleted in 10 seconds.");
                new Task(async () => { await Task.Delay(10000); await x.Delete(); }).Start();
            };
            _client.RoleUpdated += async (s, e) =>
            {
                Type ok = typeof(ServerPermissions);
                Type boul = typeof(bool);
                string kek = "";
                #region for
                foreach (PropertyInfo propertyInfo in ok.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(e.Before.Permissions, null);
                        object secondValue = propertyInfo.GetValue(e.After.Permissions, null);

                        if (!Equals(firstValue, secondValue) && firstValue is bool && secondValue is bool)
                        {
                            try
                            {
                                kek += $"{propertyInfo.Name} : {Convert.ToBoolean(firstValue)} -> {Convert.ToBoolean(secondValue)} {Environment.NewLine}";
                            }
                            catch (Exception)
                            {
                                ;
                            }

                        }
                    }
                }
                #endregion
                var x = await e.Server.DefaultChannel.SendMessage($@"This message will be deleted in 10 seconds.
A role has been changed. :
**BEFORE**
Name : {e.Before.Name},
**AFTER**
Name : {e.After.Name},
---------------------
Perms that changed :
{kek}
"
);

                new Task(async () => { await Task.Delay(10000); await x.Delete(); }).Start();

            };
            _client.ChannelCreated += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage($":white_check_mark: => A channel ({ e.Channel.Name} has been created !");
            };
            _client.ChannelDestroyed += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage($":boom: The channel {e.Channel.Name} has been **destroyed**");
            };
            #endregion
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

        private void _client_MessageDeleted(object sender, MessageEventArgs e)
        {
            throw new NotImplementedException();
        }
        public string TextToEmoji(string input)
        {
            string message = "";
            #region dict
            Dictionary<int, string> dict = new Dictionary<int, string>() {
            {0, ":zero:"},
            {1, ":one:"},
            {2, ":two:"},
            {3, ":three:"},
            {4, ":four:"},
            {5, ":five:"},
            {6, ":six:"},
            {7, ":seven:"},
            {8, ":eight:"},
            {9, ":nine:"},
            {10, ":keycap_ten:"}
           };
            #endregion
            Dictionary<char, string> sym = new Dictionary<char, string>()
           {
                {'+', ":heavy_plus_sign: "},
                {'-', ":heavy_minus_sign:"},
                {'÷', ":heavy_division_sign:"},
                {'#',":hash:"},
                {'.', ":black_small_square:"},
                {'$', ":heavy_dollar_sign: " },
                {'!',":exclamation:" },
                {'?',":question:" }
           };
            foreach (char c in input.ToCharArray())
            {
                if (char.IsLetter(c) && System.Text.RegularExpressions.Regex.IsMatch(c.ToString(), "^[a-zA-Z]*$"))
                {

                    message += $":regional_indicator_{c}:";
                }
                else if (char.IsDigit(c))
                {
                    message += dict[int.Parse(c.ToString())];
                }
                else if (c == '*')
                {
                    message += ":asterisk:";
                }
                else if (sym.ContainsKey(c))
                {
                    message += sym[c];
                }
                else if (c == ' ')
                {
                    message += "   ";
                }
                else // u wut m8 ? nothing m9 
                {
                    message += c;
                }
            }
            return message;
        }

    }
    /// <summary>
    /// you wot m9
    /// </summary>
    /// <typeparam name="T1">the first m9</typeparam>
    /// <typeparam name="T2">the second m7</typeparam>
    public class Pair<T1, T2> // ye it is public
    {
        /// <summary>
        /// topkek !
        /// </summary>
        public T1 First { get; set; }
        public T2 Second { get; set; }
        /// <summary>
        /// oh no let's construct this kek 
        /// </summary>
        /// <param name="first">first kek</param>
        /// <param name="second">second kekkeroni</param>
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
    public static class Extensions
    {
        /// <summary>
        /// A function that LET U CONTINUE EVEN IN A FEKIN EXCEPTION GOT THROWN
        /// </summary>
        /// <param name="a">ur action</param>
        /// <returns>An exception if one get thrown; else u r null m8</returns>
        public static Exception IgnoreException(Action a)
        {

            try
            {
                a();
            }
            catch (Exception uWotm8)
            {
                return uWotm8;
            }
            return null;
        }
        /// <summary>
        /// Let you continue if an exception got thrown
        /// </summary>
        /// <typeparam name="TResult">The result of the func</typeparam>
        /// <param name="FuncToTest">A func to test</param>
        /// <returns>The result, or the default value of it if an exception got thrown</returns>
        public static TResult IgnoreException<TResult>(Func<TResult> FuncToTest)
        {
            try
            {
                return FuncToTest();

            }
            catch (Exception)
            {
                return default(TResult);
            }
        }
        /// <summary>
        /// A function that let you continue even if an exception got thrown
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the func</typeparam>
        /// <param name="FuncToTest">the func to test.</param>
        /// <param name="onError">the value to return if an exception got thrown</param>
        /// <returns>the result of FuncToTest OR onError if an exception got thrown</returns>
        public static TResult IgnoreException<TResult>(Func<TResult> FuncToTest, TResult onError)
        {
            try
            {
                return FuncToTest();

            }
            catch (Exception)
            {
                return onError;
            }
        }
        public static TResult IgnoreException<P1, TResult>(P1 p1, Func<P1, TResult> FuncToTest, TResult onError)
        {
            try
            {
                return FuncToTest(p1);

            }
            catch (Exception)
            {
                return onError;
            }
        }
        public static string ItinerateAndGet(this IEnumerable r)
        {
            var toReturn = string.Empty;
            foreach (var item in r)
            {
                toReturn += $"{item.ToString()} - ";
            }
            return toReturn;

        }
        public static string ItinerateAndGet(this IEnumerable r, Func<object, object> act)
        {
            var toReturn = string.Empty;
            foreach (var item in r)
            {
                toReturn += $"{act(item)} - ";
            }
            return toReturn;
        }
    }

}