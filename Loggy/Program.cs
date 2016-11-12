using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;
namespace Loggy
{
    [Serializable]
    public partial class Program
    {
        List<Pair<Message, ulong>> rolesChanges = new List<Pair<Message, ulong>>();
        string[] drugs = new string[] { "cake", "Windows 10", "Windows 9", "silent rd", "joolya", "rip", "rip vm", "Windows 7", "silent rd", "gruel", "memz", "bye" };
        static string isRequired(string s) => s.EndsWith("}") ? "" : "}";
        static void Main(string[] args) => new Program().Start(args);


        Dictionary<Channel, Channel> toRecord = new Dictionary<Channel, Channel>();

        #region Accept
        private bool isAcceptable(CommandEventArgs e)
        {
            bool acc = false;
            foreach (var role in e.User.Roles)
            {
                if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin") || IsJeuxjeux(e.User))
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
                if (role.Name.ToLower().Contains("logger") || role.Name.ToLower().Contains("Admin") || IsJeuxjeux(e))
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

        public Channel FindLogServer(Server s)
        {
            var found = SettingsList.Where(se => { return se.Id == s.Id; });
            if (found.Any())
            {
                return s.AllChannels.Where(c => found.First().ChannelIdToLog == c.Id).FirstOrDefault();
            }
            else { return s.DefaultChannel; }
        }




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
                if (err.Command.Text == "broadcast" || err.Command.Text == "disconnect" || err.Command.Text == "listserv" || err.Command.Text == "evaltrust")
                {
                    await err.Channel.SendMessage("**__No__**, only jeuxjeux20 can use this command");
                }
                else if (err.Command.Text == "eval")
                {
                    await err.Channel.SendMessage("You aren't allowed to eval.");
                }
                else if (cool.Keys.Any((c => { return c == err.Command; })))
                {
                    await err.Channel.SendMessage($"Please wait {cool[err.Command].secondsLeft} seconds. ty");
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
                    await Console.Out.WriteLineAsync(err.Exception.Message + " and omg " + err.Exception.InnerException);
                    try
                    {
                        await err.Channel.SendMessage($"wait wtf : ```{err.Exception.Message} {err.Exception.InnerException} ```");
                    }
                    catch (Exception)
                    {
                        await err.Channel.SendMessage($"wtf {err.Exception.Message}");
                    }
                }

            }
            else if (err.ErrorType == CommandErrorType.UnknownCommand)
            {
                var x = await err.Channel.SendMessage($"Unknown command. `{err.Message.Text}`");
                new Task(async () => { await Task.Delay(1666); await x.Delete(); }).Start();
            }
        }
        protected DiscordClient _client;

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
                        await Console.Out.WriteLineAsync($"Server: {e.Server.Name} --> received command : {e.Message.Text} from {e.User.Name}#{e.User.Discriminator}");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            await Console.Out.WriteLineAsync($"Server: {e.Server.Name} --> received command : {e.Message.Text} from {e.User.Name}#{e.User.Discriminator}");
                        }
                        catch
                        {

                        }
                    }
                };

            });
            _client.Ready += (s, e) =>
            {
                _client.SetGame("Type \"=help\" to get started !" );
                try
                {
                    using (StreamReader sr = new StreamReader("settings.xml"))
                    {
                        SerializableSettingsList = (ServerSettings[])new XmlSerializer(typeof(ServerSettings[])).Deserialize(sr);
                    }
                }
                catch { }
                try
                {
                    using (StreamReader sr = new StreamReader("trusted.xml"))
                    {
                        SerializableTrustedEvalList = (ulong[])new XmlSerializer(typeof(ulong[])).Deserialize(sr.BaseStream);
                    }
                }
                catch (Exception)
                {
                }
            };

            DoCommands();

            #region UserAndRoleEvents
            _client.UserBanned += async (s, e) =>
                {
                    await FindLogServer(e.Server).SendMessage($"**{e.User.Name}#{ e.User.Discriminator}** has been banned :boom:");
                };
            _client.UserLeft += async (s, e) =>
            {
                try
                {

                    if (!(await e.Server.GetBans()).Contains(e.User))
                    {
                        await FindLogServer(e.Server).SendMessage($"{e.User.Name}#{e.User.Discriminator} has left {e.Server.Name}.");
                    }
                }
                catch (Exception)
                {
                    await FindLogServer(e.Server).SendMessage($"{e.User.Name}#{e.User.Discriminator} has left {e.Server.Name}.");
                }
            };
            _client.UserUnbanned += async (s, e) =>
            {
                await FindLogServer(e.Server).SendMessage($"{e.User.Name} has been unbanned from {e.Server.Name}.");
            };
            _client.RoleCreated += async (s, e) =>
            {
                var x = await FindLogServer(e.Server).SendMessage($@"A role has been created :
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
                var x = await FindLogServer(e.Server).SendMessage($@":boom: a role has been ~~destroyed~~ deleted. Name : {e.Role.Name} Members : {members}
This message will be deleted in 10 seconds.");
                new Task(async () => { await Task.Delay(10000); await x.Delete(); }).Start();
            };
            _client.RoleUpdated += async (s, e) =>
            {
                Type ok = typeof(ServerPermissions);
                Type secondk = typeof(Role);
                Type boul = typeof(bool);
                string kek = "";
                string kek2 = string.Empty;
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
                
              
                Message x = null;
                string before = string.Empty;
                string after = string.Empty;
                if (rolesChanges.Any(something => { return something.Second.Equals(e.After.Id) && something != null; }))
                {
                    x = rolesChanges.Find(p => p.Second.Equals(e.After.Id)).First;
                    await x.Edit($@"This message will be deleted in 10 seconds.
A role has been changed. :
Name: {e.Before.Name} -> { e.After.Name}
Perms that changed:
{x.Text.Substring(x.Text.IndexOf("Perms that changed:") + x.Text.Length) + kek}
");
                }
                else
                {
                    x = await FindLogServer(e.Server).SendMessage($@"This message will be deleted in 10 seconds.
A role has been changed. :
Name : {e.Before.Name} -> {e.After.Name}
{kek2}
Perms that changed :
{kek}
"
);
                }

                rolesChanges.Add(new Pair<Message, ulong>(x, e.After.Id));
                new Task(async () => { await Task.Delay(17500); await x.Delete(); }).Start();
                rolesChanges = rolesChanges.Where(me => me.First != null).ToList();
            };
            _client.ChannelCreated += async (s, e) =>
            {
                await FindLogServer(e.Server).SendMessage($":white_check_mark: => A channel ({ e.Channel.Name}) has been created !");

            };
            _client.ChannelDestroyed += async (s, e) =>
            {
                await FindLogServer(e.Server).SendMessage($":boom: The channel {e.Channel.Name} has been **destroyed**");
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
            #region sym
            Dictionary<char, string> sym = new Dictionary<char, string>()
           {
                {'+', ":heavy_plus_sign: "},
                {'-', ":heavy_minus_sign:"},
                {'÷', ":heavy_division_sign:"},
                {'#',":hash:"},
                {'.', ":black_small_square:"},
                {'$', ":heavy_dollar_sign: " },
                {'!',":exclamation:" },
                {'?',":question:" },
                {'*',":asterisk:" }
           };
            #endregion
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
    [Serializable()]
    public class Pair<T1, T2>
    {
        /// <summary>
        /// topkek !
        /// </summary>
        [XmlElement]
        public T1 First { get; set; }
        [XmlElement]
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
    public class ServerSettings
    {
        [XmlAttribute("ServerId")]
        public ulong Id { get; set; }
        [XmlElement("ChannelId")]
        public ulong ChannelIdToLog { get; set; }
        
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

    }
}