using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Discord;
using Discord.Commands;
using static System.Console;

namespace Loggy
{
    [Serializable]
    public partial class Program
    {
        public enum TextEmojiOptions
        {
            Lowercase,
            NoOptions
        }

        private readonly string[] _drugs =
        {
            "cake", "Windows 10", "Windows 9", "silent rd", "joolya", "rip", "rip vm",
            "Windows 7", "silent rd", "gruel", "memz", "bye"
        };


        private readonly Dictionary<Channel, Channel> _toRecord = new Dictionary<Channel, Channel>();
        protected DiscordClient Client;
        private List<Pair<Message, ulong>> _rolesChanges = new List<Pair<Message, ulong>>();
        private readonly Dictionary<Command, Cooldown> _cool = new Dictionary<Command, Cooldown>();

        private List<ulong> TrustedEvalList { get; set; } = new List<ulong>();

        private ulong[] SerializableTrustedEvalList
        {
            get { return TrustedEvalList.ToArray(); }
            set { TrustedEvalList = value.ToList(); }
        }

        private List<ServerSettings> SettingsList { get; set; } = new List<ServerSettings>();

        public ServerSettings[] SerializableSettingsList
        {
            get { return SettingsList.ToArray(); }
            set { SettingsList = value.ToList(); }
        }

        private static void Main(string[] args) => new Program().Start(args);

        public Channel FindLogServer(Server s)
        {
            try
            {
                var found = SettingsList.Where(se => se.Id == s.Id);
                var serverSettings = found as IList<ServerSettings> ?? found.ToList();
                if (serverSettings.Any())
                    return s.AllChannels.First(c => serverSettings.First().ChannelIdToLog == c.Id);
                return s.DefaultChannel;
            }
            catch
            {
                return s.DefaultChannel;
            }
        }

        private ServerSettings FindServSettings(Server ser)
        {
            if (SettingsList.Any(s => s.Id == ser.Id))
                return SettingsList.First(s => s.Id.Equals(ser.Id));
            var a = new ServerSettings(ser.Id);
            SettingsList.Add(a);
            return a;
        }

/*
        private void Repeat(byte n, Action act)
        {
            for (byte i = 0; i > n; i--)
                act();
        }
*/

        private async Task Ok(CommandErrorEventArgs err)
        {
            if (err.ErrorType == CommandErrorType.BadPermissions)
                if ((err.Command.Text == "broadcast") || (err.Command.Text == "disconnect") ||
                    (err.Command.Text == "listserv") || (err.Command.Text == "evaltrust"))
                {
                    await err.Channel.SendMessage("**__No__**, only jeuxjeux20 can use this command");
                }
                else if (err.Command.Text == "eval")
                {
                    await err.Channel.SendMessage("You aren't allowed to eval.");
                }
                else if (_cool.Keys.Any(c => c == err.Command))
                {
                    if (!_plsWaitCooldown.IsFinished)
                        await Task.Delay((_plsWaitCooldown.SecondsLeft ?? 1) + new Random().Next(1, 5));
                    await err.Channel.SendMessage($"Please wait {_cool[err.Command].SecondsLeft} seconds. ty");
                    _plsWaitCooldown.Restart();
                }
                else
                {
                    await
                        err.Channel.SendMessage(
                            ":red_circle: => You didn\'t have got permissions to use this command (try attributting yourself a role named \"Logger\")`");
                }
            else if (err.ErrorType == CommandErrorType.BadArgCount)
                await err.Channel.SendMessage("Argument error. Please check what arguments you typed in.");
            else if (err.ErrorType == CommandErrorType.Exception)
                if (err.Exception.Message.Contains("2000"))
                {
                    await err.Channel.SendMessage("Too much **chara**cters, m8");
                }
                else if (err.Exception.Message.Contains("blank"))
                {
                    await err.Channel.SendMessage("No parameters provided, m9");
                }
                else if (err.Exception is CommandSentByBotException)
                {
                    await Out.WriteLineAsync($"haha nice try {err.User.Mention}");
                }
                else
                {
                    await Out.WriteLineAsync(err.Exception.Message + " and omg " + err.Exception.InnerException);
                    try
                    {
                        await
                            err.Channel.SendMessage(
                                $"wait wtf : ```{err.Exception.Message} {err.Exception.InnerException} ```");
                        await
                            Out.WriteLineAsync(
                                $"{err.Exception.Message} {err.Exception.InnerException} {err.Exception.StackTrace}");
                    }
                    catch (Exception)
                    {
                        await err.Channel.SendMessage($"wtf {err.Exception.Message}");
                    }
                }
        }

        private void SerializeServerSettingsAndSave()
        {
            using (var sw = new StreamWriter("settings.xml", false))
            {
                var ser = new XmlSerializer(typeof(ServerSettings[]));
                ser.Serialize(sw.BaseStream, SerializableSettingsList);
            }
            Out.WriteLineAsync("serialization done");
        }

        private bool SafeTo(Dictionary<Channel, Channel> dict, Pair<Channel, Channel> per)
        {
            var k = true;
            foreach (var item in dict)
                if ((item.Key == per.Second) && (item.Value == per.First))
                {
                    k = false;
                    break;
                }
            return k;
        }

        public void Start(string[] args = null)
        {
            #region declares

            Client = new DiscordClient();

            #endregion

            Client.MessageReceived += async (s, e) =>
            {
                foreach (var item in _toRecord)
                    if (e.Channel == item.Key)
                    {
                        await Task.Delay(new Random(DateTime.Now.Millisecond).Next(1000, 3000));
                        await
                            item.Value.SendMessage(
                                $"#{e.Channel.Name} | **{e.Message.User}** said : {e.Message.Text.Replace("@", "*@*")}");
                    }
            };

            Client.MessageUpdated += async (s, e) =>
            {
                foreach (var item in _toRecord)
                    if (e.Channel == item.Key)
                        await
                            item.Value.SendMessage(
                                $@"#{e.Channel.Name} | **{e.User.Name}#{e.User.Discriminator}** edited this message :
{e.Before.Text}
:arrow_down:
{e.After.Text}");
            };
            Client.UsingCommands(x =>
            {
                x.PrefixChar = '=';
                x.HelpMode = HelpMode.Public;
                x.ErrorHandler += async (a, b) => { await Ok(b); };
                x.ExecuteHandler += async (a, e) =>
                {
                    if (e.User.IsBot)
                        throw new CommandSentByBotException();
                    try
                    {
                        await
                            Out.WriteLineAsync(
                                $"Server: {e.Server.Name} --> received command : {e.Message.Text} from {e.User.Name}#{e.User.Discriminator}");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            await
                                Out.WriteLineAsync(
                                    $"Server: {e.Server.Name} --> received command : {e.Message.Text} from {e.User.Name}#{e.User.Discriminator}");
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                };
            });
            Client.Ready += (s, e) =>
            {
                Client.SetGame("Type \"=help\" to get started !");
                try
                {
                    using (var sr = new StreamReader("settings.xml"))
                    {
                        SerializableSettingsList =
                            (ServerSettings[]) new XmlSerializer(typeof(ServerSettings[])).Deserialize(sr);
                    }
                }
                catch
                {
                    // ignored
                }
                try
                {
                    using (var sr = new StreamReader("trusted.xml"))
                    {
                        SerializableTrustedEvalList =
                            (ulong[]) new XmlSerializer(typeof(ulong[])).Deserialize(sr.BaseStream);
                    }
                }
                catch
                {
                    // ignored
                }
            };

            DoCommands();

            #region UserAndRoleEvents

            Client.UserBanned +=
                async (s, e) =>
                {
                    await
                        FindLogServer(e.Server)
                            .SendMessage($"**{e.User.Name}#{e.User.Discriminator}** has been banned :boom:");
                };
            Client.UserLeft += async (s, e) =>
            {
                try
                {
                    if (!(await e.Server.GetBans()).Contains(e.User))
                        await
                            FindLogServer(e.Server)
                                .SendMessage($"{e.User.Name}#{e.User.Discriminator} has left {e.Server.Name}.");
                }
                catch (Exception)
                {
                    await
                        FindLogServer(e.Server)
                            .SendMessage($"{e.User.Name}#{e.User.Discriminator} has left {e.Server.Name}.");
                }
            };
            Client.UserUnbanned +=
                async (s, e) =>
                {
                    await FindLogServer(e.Server).SendMessage($"{e.User.Name} has been unbanned from {e.Server.Name}.");
                };
            Client.RoleCreated += async (s, e) =>
            {
                var x = await FindLogServer(e.Server).SendMessage($@"A role has been created :
Name : {e.Role.Name}
IsMentionnable : {e.Role.IsMentionable}
Position : {e.Role.Position}
This message will be deleted in 10 seconds.");
                new Task(async () =>
                {
                    await Task.Delay(10000);
                    await x.Delete();
                }).Start();
            };
            Client.RoleDeleted += async (s, e) =>
            {
                var members = string.Empty;
                foreach (var item in e.Role.Members)
                    members += $"{item} ; ";
                var x =
                    await
                        FindLogServer(e.Server)
                            .SendMessage(
                                $@":boom: a role has been ~~destroyed~~ deleted. Name : {e.Role.Name} Members : {members}
This message will be deleted in 10 seconds.");
                new Task(async () =>
                {
                    await Task.Delay(10000);
                    await x.Delete();
                }).Start();
            };
            Client.RoleUpdated += async (s, e) =>
            {
                if (!FindServSettings(e.Server).RoleUpdatesMessage)
                    goto nope;
                var ok = typeof(ServerPermissions);
                var kek = "";
                var kek2 = string.Empty;

                #region for

                foreach (var propertyInfo in ok.GetProperties())
                    if (propertyInfo.CanRead)
                    {
                        var firstValue = propertyInfo.GetValue(e.Before.Permissions, null);
                        var secondValue = propertyInfo.GetValue(e.After.Permissions, null);

                        if (Equals(firstValue, secondValue) || !(firstValue is bool) || !(secondValue is bool))
                            continue;
                        try
                        {
                            kek +=
                                $"{propertyInfo.Name} : {Convert.ToBoolean(firstValue)} -> {Convert.ToBoolean(secondValue)} {Environment.NewLine}";
                        }
                        catch
                        {
                            // ignored m8
                        }
                    }

                #endregion

                Message x;
                if (
                    _rolesChanges.Any(
                        something => something.Second.Equals(e.After.Id)))
                {
                    x = _rolesChanges.Find(p => p.Second.Equals(e.After.Id)).First;
                    try
                    {
                        await x.Edit($@"This message will be deleted in 10 seconds.
A role has been changed. :
Name: {e.Before.Name.Replace("@", "*@*")} -> {e.After.Name.Replace("@", "*@*")}
Perms that changed:
{Regex.Match(x.Text, "changed:(.*)", RegexOptions.Singleline).Groups[1] + kek}
");
                    }
                    catch (Exception)
                    {
                        await FindLogServer(e.Server).SendMessage("mhm something weird happened");
                    }
                }
                else
                {
                    x = await FindLogServer(e.Server).SendMessage($@"This message will be deleted in 10 seconds.
A role has been changed. :
Name : {e.Before.Name} -> {e.After.Name}
{kek2}
Perms that changed:
{kek}
"
                    );
                }

                _rolesChanges.Add(new Pair<Message, ulong>(x, e.After.Id));
                new Task(async () =>
                {
                    await Task.Delay(17500);
                    await x.Delete();
                }).Start();
                _rolesChanges = _rolesChanges.Where(me => me.First != null).ToList();
                nope:
                ;
            };
            Client.ChannelCreated +=
                async (s, e) =>
                {
                    await
                        FindLogServer(e.Server)
                            .SendMessage($":white_check_mark: => A channel ({e.Channel.Name}) has been created !");
                };
            Client.ChannelDestroyed +=
                async (s, e) =>
                {
                    await
                        FindLogServer(e.Server)
                            .SendMessage($":boom: The channel {e.Channel.Name} has been **destroyed**");
                };

            #endregion

            Client.ExecuteAndWait(async () =>
            {
                string tok;

                if ((args == null) || args.Length <= 0 || args[0] == "")
                {
                    using (
                        var file = File.Open(AppDomain.CurrentDomain.BaseDirectory + "toke.t", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite, FileShare.ReadWrite))
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
                    await Client.Connect(tok, TokenType.Bot);
                }
                catch (Exception)
                {
                    await Out.WriteLineAsync("Insert token plesssss");
                    await Out.FlushAsync();
                    var kek = await In.ReadLineAsync();
                    tok = kek;
                    goto Retry;
                }
                await Out.WriteLineAsync("I guess i'm connected");

                using (
                    var file = File.Open(AppDomain.CurrentDomain.BaseDirectory + "toke.t", FileMode.OpenOrCreate,
                        FileAccess.ReadWrite, FileShare.ReadWrite))
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

        public string TextToEmoji(string input, TextEmojiOptions top = TextEmojiOptions.NoOptions)
        {
            var message = top == TextEmojiOptions.Lowercase ? "​" : "";

            #region dict

            var dict = new Dictionary<int, string>
            {
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

            #region sym

            var sym = new Dictionary<char, string>
            {
                {'+', ":heavy_plus_sign: "},
                {'-', ":heavy_minus_sign:"},
                {'÷', ":heavy_division_sign:"},
                {'#', ":hash:"},
                {'.', ":black_small_square:"},
                {'$', ":heavy_dollar_sign: "},
                {'!', ":exclamation:"},
                {'?', ":question:"},
                {'*', ":asterisk:"}
            };

            #endregion

            foreach (var c in input)
                if (char.IsLetter(c) && Regex.IsMatch(c.ToString(), "^[a-zA-Z]*$"))
                    message += $":regional_indicator_{c}:";
                else if (char.IsDigit(c))
                    message += dict[int.Parse(c.ToString())];
                else if (sym.ContainsKey(c))
                    message += sym[c];
                else if (c == ' ')
                    message += "   ";
                else // u wut m8 ? nothing m9 
                    message += c;
            return message;
        }

        #region Accept

        private bool IsAcceptable(CommandEventArgs e)
        {
            var acc = false;
            foreach (var role in e.User.Roles)
                if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin") ||
                    IsJeuxjeux(e.User))
                {
                    acc = true;
                    break;
                }
            return acc;
        }

        private bool IsAcceptable(User e)
        {
            var acc = false;
            foreach (var role in e.Roles)
                if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin") || IsJeuxjeux(e))
                {
                    acc = true;
                    break;
                }
            return acc;
        }

        #endregion
    }

    /// <summary>
    ///     you wot m9
    /// </summary>
    /// <typeparam name="T1">the first m9</typeparam>
    /// <typeparam name="T2">the second m7</typeparam>
    [Serializable]
    public class Pair<T1, T2>
    {
        /// <summary>
        ///     oh no let's construct this kek
        /// </summary>
        /// <param name="first">first kek</param>
        /// <param name="second">second kekkeroni</param>
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        ///     topkek !
        /// </summary>
        [XmlElement]
        public T1 First { get; set; }

        [XmlElement]
        public T2 Second { get; set; }
    }

    public static class Extensions
    {
/*
        /// <summary>
        ///     Let you continue if an exception got thrown
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
*/

/*
        /// <summary>
        ///     A function that let you continue even if an exception got thrown
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
*/

/*
        public static TResult IgnoreException<TP1, TResult>(TP1 p1, Func<TP1, TResult> funcToTest, TResult onError)
        {
            try
            {
                return funcToTest(p1);
            }
            catch (Exception)
            {
                return onError;
            }
        }
*/

        //public static string ItinerateAndGet(this IEnumerable r)
        //{
        //    return r.Cast<object>().Aggregate(string.Empty, (current, item) => current + $"{item} - ");
        //}

/*
        public static string ItinerateAndGet(this IEnumerable r, Func<object, object> act)
        {
            return r.Cast<object>().Aggregate(string.Empty, (current, item) => current + $"{act(item)} - ");
        }
*/
    }

    public class ServerSettings
    {
        public ServerSettings()
        {
            Id = 0;
        }

        public ServerSettings(ulong u)
        {
            Id = u;
        }

        public ServerSettings(ulong u, ulong s)
        {
            Id = u;
            ChannelIdToLog = s;
        }

        [XmlAttribute("ServerId")]
        public ulong Id { get; set; }

        [XmlElement("ChannelId")]
        public ulong ChannelIdToLog { get; set; }

        [XmlElement("RoleUpdates")]
        public bool RoleUpdatesMessage { get; set; } = true;
    }
}