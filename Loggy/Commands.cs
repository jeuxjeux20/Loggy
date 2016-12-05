using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Discord;
using Discord.Commands;
using Microsoft.CSharp;

namespace Loggy
{
    public partial class Program
    {
        private delegate Task<Message> SendM(string s);
        private bool IsJeuxjeux(User u)
        {
            return u.Id == 134348241700388864;
        }

        private Cooldown RegisterCooldown(int seconds, Command cm)
        {
            if (!_cool.Keys.Any(com => com.Equals(cm)))
                _cool.Add(cm, new Cooldown(seconds));
            return _cool[cm];

        }
        private readonly Cooldown _plsWaitCooldown = new Cooldown(2);
        [Serializable]
        private class CommandSentByBotException : Exception
        {
            public CommandSentByBotException() { }
/*
            public CommandSentByBotException(string message) : base(message) { }
*/
/*
            public CommandSentByBotException(string message, Exception inner) : base(message, inner) { }
*/
            protected CommandSentByBotException(
              SerializationInfo info,
              StreamingContext context) : base(info, context) { }
        }
        private void DoCommands()
        {
            #region listserv

            Client.GetService<CommandService>().CreateCommand("listServ")
.Description("Get the list of the servers in where is the bot. (:kek:)")
.AddCheck((a, b, c) => b.Id == 134348241700388864, "u not jeuxjeux20")
.Do(async e =>
{
    string m = "```";
    foreach (var item in Client.Servers)
    {
        m += $"{item.Name} - count : {item.UserCount} \n";
       
    } m += "```";
    await e.Channel.SendMessage(m);
});


            #endregion
            #region clean

            Client.GetService<CommandService>().CreateCommand("clean")
.Description("Clean [number] messages")
.AddCheck((a, b, c) => IsAcceptable(b))
.Parameter("number")
.Do(async e =>
{
    try
    {
        int c = Math.Max(1, Convert.ToInt32(e.GetArg("number")));
        var toDel = await e.Channel.DownloadMessages(c);
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
            Client.GetService<CommandService>().CreateCommand("wizondrug")
                .Description("get kek and pizzas")
                .Alias("drugs", "electronicdrugs", "wizdrug")
                .AddCheck((a, b, c) => RegisterCooldown(50, a).IsFinished)

                .Do(async e =>
                {
                    _cool[e.Command].Restart();
                    Random kek = new Random(DateTime.UtcNow.Millisecond);
                    List<Message> ls = new List<Message>();
                    for (int i = 0; i < new Random().Next(6, 10); i++)
                    {
                        ls.Add(await e.Channel.SendMessage(_drugs[kek.Next(0, _drugs.Length)]));
                        await Task.Delay(600);
                    }
                    foreach (var item in ls)
                    {
                        await item.Delete();
                    }
                });
            #endregion // cooldowned
            #region MDMCK10

            Client.GetService<CommandService>().CreateCommand("MDMCK10")
        .Description("MDMCK10 has something to confess")
        .Do(async e =>
        {
            var x = await e.Channel.SendMessage("A gud person that makes bad mistakes sometimes");
            await Task.Delay(500);
            await x.Edit("A gud person that makes very bad mistakes sometimes");
        });


            #endregion
            #region kek

            Client.GetService<CommandService>().CreateCommand("keks")
.Description("get kek and pizzas")
.AddCheck((a, b, c) =>
{
    if (_cool.Keys.All(com => com != a))
        _cool.Add(a, new Cooldown(15));
    return _cool[a].IsFinished;
}, "plz wait a little bit")
.Do(async e =>
{
    _cool[e.Command].Restart();
    List<Message> x = new List<Message>
    {
        await e.Channel.SendMessage("cake"),
        await e.Channel.SendMessage("cake"),
        await e.Channel.SendMessage("cake"),
        await e.Channel.SendMessage("cake"),
        await e.Channel.SendMessage("cake"),
        await e.Channel.SendMessage("cake")
    };
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
            Client.GetService<CommandService>().CreateCommand("joolya7")
.Description("find out xd")
.Parameter("hi", ParameterType.Unparsed)
.AddCheck((a, b, c) =>
{
    if (_cool.Keys.All(com => com != a))
        _cool.Add(a, new Cooldown(7));
    return _cool[a].IsFinished;
}, "plz wait a little bit")
.Do(async e =>
{
    _cool[e.Command].Restart();
    string mes = "Windows 7";
    foreach (var item in _drugs)
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
            Client.GetService<CommandService>().CreateCommand("tipmedoge")
                .Description("try it out")
                .Do(async e =>
                {
                    await e.Channel.SendMessage(":regional_indicator_n::regional_indicator_o:");
                });
            #endregion           
            #region textemoji
            Client.GetService<CommandService>().CreateCommand("textToEmoji")
   .Description("grab a text, get an emoji kekekekekekekek")
   .Alias("emojiText", "textEmoji", "et", "kekwords", "say")
   .Parameter("param1", ParameterType.Unparsed)
   .Do(async e =>
   {
       Channel chtosend;
       if ((e.Server.Id == 110373943822540800 && e.Channel.Name == "general"))
       {
           chtosend = e.Server.AllChannels.First(w => w.Name == @"testing-[\]");
           await Console.Out.WriteLineAsync("found k");
       } else
       {
           chtosend = e.Channel;
       }
       string original = e.GetArg("param1").ToLower();
       string message = TextToEmoji(original,TextEmojiOptions.Lowercase);

       await chtosend.SendMessage(message);
   });
            #endregion // this one everyone loves it idk why
            #region etiTopkek
            Client.GetService<CommandService>().CreateCommand("topkek")
.Description("kekeek")
.Do(async e =>
{
    await e.Channel.SendFile("top kek.png");
});
            #endregion 
            #region drinkBleach
            Client.GetService<CommandService>().CreateCommand("bleach")
.Description("drink it = +69 attack")
.Do(async e =>
{
    await e.Channel.SendFile("bleach.jpeg");
});
            #endregion
            #region wiz
            Client.GetService<CommandService>().CreateCommand("wiz")
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
    await Task.Delay(1250);
    await kek.Edit("electronicwizzzzzzz(one)");

});
            #endregion
            #region loglist
            Client.GetService<CommandService>().CreateCommand("logList")
                .Description("Make a list of all loggers")
                .AddCheck((a, b, c) => IsAcceptable(b), "You must get a role that contains Logger or Admin in its name.")
                .Alias("recordList", "listLog", "listRecord")
                .Do(async e =>
                {
                    bool nothing = true;
                    string message = $"List of records for the server: {e.Server.Name}";
                    foreach (var item in _toRecord)
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
            Client.GetService<CommandService>().CreateCommand("delete")
.Description("Delete a record")
.Parameter("a")
.Parameter("b")
.AddCheck((a, b, c) => IsAcceptable(b), "You must get a role that contains Logger or Admin in its name.")
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
        foreach (var item in _toRecord.ToList())
        {
            if (item.Key == toRecordTemp.First && item.Value == toRecordTemp.Second)
            {
                _toRecord.Remove(item.Key);
            }
        }
        await e.Channel.SendMessage($"Sucessfully deleted record ! ({toRecordTemp.First} ---> {toRecordTemp.Second})");
    }
});
            #endregion           
            #region disconnect
            Client.GetService<CommandService>().CreateCommand("disconnect")
                        .Description("Disconnects the bot, what else")
                        .AddCheck((a, b, c) =>
                {
                    if (b.Name == "jeuxjeux20" && b.Discriminator == 4664) { return true; }
                    return false;
                }, "Only jeuxjeux20 can disconnect it")
                        .Do(async e =>
                        {
                            await e.Channel.SendMessage("Bye :frowning:");
                            await Task.Delay(1000);
                            await Client.Disconnect();
                            Client.Dispose();
                        });
            #endregion           
            #region invite
            Client.GetService<CommandService>().CreateCommand("invite")
.Description("Get an invite link kek")
.Do(async e =>
{
    await e.Channel.SendMessage(@"Here is the link : https://discordapp.com/oauth2/authorize?client_id=239326847433703424&scope=bot&permissions=0");
});
            #endregion
            #region about

            Client.GetService<CommandService>().CreateCommand("about")
        .Description("About the dev c:")
        .Alias("topkekkle")
        .Do(async e =>
        {
            await e.Channel.SendMessage
            (
$@"```This bot has been made by jeuxjeux20,
it's a funny bot with tons of functions !
it is currently on {Client.Servers.Count()} server(s)
I hope that you like it c:```");
        });

            #endregion
            #region record

            Client.GetService<CommandService>().CreateCommand("record")
            .Description("record a channel")
            .Parameter("a")
            .Parameter("b")
            .AddCheck((a, b, c) => IsAcceptable(b), "You must get a role that contains Logger or Admin in its name.")
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
                if (foundie.First == false || foundie.Second == false && IsAcceptable(e))
                {
                    await e.Channel.SendMessage($"Something didn't got right - Listening : {foundie.First} ; Target : {foundie.Second}");
                }
                else if (SafeTo(_toRecord, toRecordTemp))
                {
                    await e.Channel.SendMessage($"~~Succesfully installed Windows 10~~ Succesfully recording #{a} to output #{b}");
                    bool cool = true;
                    foreach (var item in _toRecord.ToList())
                    {
                        if (item.Key == toRecordTemp.First)
                        {
                            await e.Channel.SendMessage("A record is alerady targeting this channel, replacing...");
                            cool = false;
                            _toRecord[item.Key] = toRecordTemp.Second;
                            break;
                        }

                    }
                    if (cool)
                        _toRecord.Add(toRecordTemp.First, toRecordTemp.Second);
                }

                else if (!SafeTo(_toRecord, toRecordTemp))
                {
                    await e.Channel.SendMessage(":negative_squared_cross_mark: you can't make a record loop seriously m8");
                }
            });


            #endregion
            #region broadcast

            Client.GetService<CommandService>().CreateCommand("broadcast")
.Description("Only for jeuxjeux20, broadcast a message to all servers. this gonna be fun")
.Parameter("say", ParameterType.Unparsed)
.AddCheck((a, b, c) => IsJeuxjeux(b), "u not jeuxjeux20")

.Do(async e =>
{
    foreach (var item in Client.Servers)
    {
        await item.DefaultChannel.SendMessage(e.GetArg("say"));
    }
});


            #endregion
            #region SPAM HI

            Client.GetService<CommandService>().CreateCommand("hiSpam")
.Description("hi hi hi hi hi hi hi hi hi")
.AddCheck((a, b, c) =>
{
    if (_cool.Keys.All(com => com != a))
        _cool.Add(a, new Cooldown(35));
    return _cool[a].IsFinished;

})
.Do(async e =>
    {
        _cool[e.Command].Restart();
        HashSet<Message> h = new HashSet<Message>();
        for (int i = 0; i < 20; i++)
        {
            h.Add(await e.Channel.SendMessage("hi"));
            await Task.Delay(185);
        }
        foreach (Message item in h)
        {
            await item.Delete();
        }
    });


            #endregion
            #region LogChannel

            Client.GetService<CommandService>().CreateCommand("defaultchannel")
.Description("Set the default channel.")
.Parameter("ch")
.AddCheck((a, b, c) => IsAcceptable(b))
.Do(async e =>
    {
        string chanName = e.GetArg("ch");
        bool isHere = e.Server.AllChannels.Select(ok => ok.Mention).Contains(chanName);
        if (isHere)
        {
            Channel hello = e.Server.AllChannels.First(c => c.Mention == chanName);
            if (SettingsList.Any(s => s.Id == e.Server.Id))
                SettingsList.First(s => s.Id == e.Server.Id).ChannelIdToLog = hello.Id;
            else
                SettingsList.Add(new ServerSettings(e.Server.Id, hello.Id));
            await e.Channel.SendMessage("ok alright");
            await FindLogServer(e.Server).SendMessage("Future log messages will be sent here");
            using (StreamWriter sw = new StreamWriter("settings.xml", false))
            {
                var ser = new XmlSerializer(typeof(ServerSettings[]));
                ser.Serialize(sw.BaseStream, SerializableSettingsList);
            }
        }
        else
        {
            await e.Channel.SendMessage("nu channel found");
        }
    });


            #endregion
            #region Eval
            Client.GetService<CommandService>().CreateCommand("eval")
.Description("Evals a c# code executed apart and compiled, protected from rds xd and use output = [the variable] to output ; for defining classes : put [CD] at the end of the code; then type your class ;)")
.Parameter("to", ParameterType.Unparsed)
.AddCheck((a, b, c) => true) /* RegisterCooldown(5, a).isFinished || TrustedEvalList.Contains(b.Id); */
.Do(async e =>
    {
        string codeToEval = e.GetArg("to");
        codeToEval = codeToEval.Replace("`", "");
        codeToEval = codeToEval.Replace("csharp", "");
        codeToEval = codeToEval.Replace("System.Diagnostics", "NOPE");
        codeToEval = codeToEval.Replace("System.IO", "ck");
        codeToEval = codeToEval.Replace("StreamWriter", "fuk_u");
        codeToEval = codeToEval.Replace("StreamReader", "no");
        codeToEval = codeToEval.Replace("File", "JUst no");
        codeToEval = codeToEval.Replace("Write", "no");
        codeToEval = codeToEval.Replace("Process", "fukfuk");
        codeToEval = codeToEval.Replace("rd", "ur mom");
        codeToEval = codeToEval.Replace("while", "disabled");
        codeToEval = codeToEval.Replace("for", "disabled");
        string cl = string.Empty;
        var regex = new Regex("\\[CD\\].*$", RegexOptions.Singleline);
        if (codeToEval.Contains("[CD]"))
        {
            await e.Channel.SendMessage("CLASS DEFINITION FOUND");
        }
        if (regex.IsMatch(codeToEval))
        {
            string val = regex.Match(codeToEval).Value;
            codeToEval = codeToEval.Replace(val, "");
            cl = val.Replace("[CD]", string.Empty);
        }
        string classedCode = @"
using System;
using System.Linq;
using System.Collections.Generic;
namespace Eval {
    public class Evalued {        
        public static object EvalIt() {
            object output = null;"
            + codeToEval +
@"          
return output; }"
+ cl +
    @"}
}";
        Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v4.0"}
                };
        CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

        CompilerParameters compilerParams = new CompilerParameters
        {
            GenerateInMemory = true,
            GenerateExecutable = false,
            CompilerOptions = "/optimize"
        };
        compilerParams.ReferencedAssemblies.Add("System.Core.dll");
        Client.SetStatus(UserStatus.DoNotDisturb);
        var myTask = Task.Factory.StartNew(() => provider.CompileAssemblyFromSource(compilerParams, classedCode));    
            var results = await myTask;
        //catch (Exception ex)
        //{
        //    await e.Channel.SendMessage($"Exception occured : {ex.Message}");
        //    goto oh;
        //}
        Client.SetStatus(UserStatus.Online);
        if (results.Errors.Count > 0)
        {
            string ono = string.Empty;
            foreach (CompilerError item in results.Errors)
            {
                ono += item.ErrorText + Environment.NewLine;
            }
            var x = await e.Channel.SendMessage(ono);
            new Task(async () => { await Task.Delay(6000); await x.Delete(); }).Start();
        }
        else
        {
            var o = results.CompiledAssembly.CreateInstance("Eval.Evalued");
            var mi = o?.GetType().GetMethod("EvalIt");
            var res = mi?.Invoke(o, null);
            await e.Channel.SendMessage($@"Success !
```Output : {res ?? "null"}```");
        }
       
        Client.SetStatus(UserStatus.Online);
        
    });
#pragma warning restore CS1998 // Cette méthode async n'a pas d'opérateur 'await' et elle s'exécutera de façon synchrone
            #endregion
            #region Trust

            Client.GetService<CommandService>().CreateCommand("evaltrust")
.Description("Only for jeuxjeux20")
.Parameter("us")
.AddCheck((a, b, c) => IsJeuxjeux(b))
.Do(async e =>
    {
        var user = e.GetArg("us");
        var trustedOne = e.Server.Users.Where(u => u.NicknameMention == user || u.Mention == user);
        var enumerable = trustedOne as IList<User> ?? trustedOne.ToList();
        User toTrust = enumerable.Any() ? enumerable.First() : null;
        if (toTrust != null)
        {
            TrustedEvalList.Add(toTrust.Id);
            await e.Channel.SendMessage("Done ! :)");
            using (StreamWriter sw = new StreamWriter("trusted.xml", false))
            {
                new XmlSerializer(typeof(ulong[])).Serialize(sw, SerializableTrustedEvalList);
            }
        }
        else
        {
            await e.Channel.SendMessage("not found, make sure that you mentionned him");
        }
    });


            #endregion
            #region UNTRUST
            Client.GetService<CommandService>().CreateCommand("untrust")
.Description("Only for jeuxjeux20")
.Parameter("us")
.AddCheck((a, b, c) => IsJeuxjeux(b))
.Do(async e =>
{
    string user = e.GetArg("us");
    var trustedOne = e.Server.Users.Where(u => u.NicknameMention == user || u.Mention == user);
    var enumerable = trustedOne as IList<User> ?? trustedOne.ToList();
    User toTrust = enumerable.Any() ? enumerable.First() : null;
    if (toTrust != null)
    {
        TrustedEvalList.Remove(toTrust.Id);
        await e.Channel.SendMessage("Done ! :)");
        using (StreamWriter sw = new StreamWriter("trusted.xml", false))
        {
            new XmlSerializer(typeof(ulong[])).Serialize(sw, SerializableTrustedEvalList);
        }
    }
    else
    {
        await e.Channel.SendMessage("not found, make sure that you mentionned him");
    }

});
            #endregion
            #region FireC

            Client.GetService<CommandService>().CreateCommand("FireC")
.Description("xd")
.Parameter("nvm", ParameterType.Unparsed)
.Do(async e =>
    {
        var x = await e.Channel.SendMessage("Finding FireC...");
        await Task.Delay(750);
        await x.Edit("FireC located... finding IP");
        await Task.Delay(1250);
        uint kek = 1;
        while (kek <= 100)
        {
            await x.Edit("Parsing and deserializing FireC, this may take a while" + Environment.NewLine + AsciiBar.DrawProgressBar(kek) + $" - {kek}%");
            kek += (uint)new Random(DateTime.Now.Millisecond).Next(1, 15);
            await Task.Delay(250);
        }
        await x.Edit("FireC found ! : Pair<Discord.User`1,List<Hentai>>");
    });

            #endregion
            #region cth
            Client.GetService<CommandService>().CreateCommand("cth")
.Description("allé marin le panné")
.Do(async e =>
    {

        List<string> prout = new List<string>
        {
            "Stop with lol",
            "firec.lif = null",
            @"https://images-1.discordapp.net/.eJwNyEEOhCAMAMC_8ABooJTobwgSNKuU0BoPZv--O8d5zT1Ps5pddcjq3HZI4blZUZ65VduY21nzOMQWvlxWzWW_aldxHjHiAikBeg8QE_0rLhEohJACEpFHd_dP56fb0Zv5_gAGHCLW.Ey5o0ra3PquKJiQ46tnocLSb2GA",
            ":')",
            ":\")",
            "marin le pen",
            "i h8 firec",
            "mé si cé posibl avec la cart kiwi",
            "o poutine",
            "ui",
            "je veu fer un sit en .net"
        };
        string m = prout.ElementAt(new Random(DateTime.UtcNow.Millisecond + DateTime.Today.Second).Next(0, prout.Count - 1));
        await e.Channel.SendMessage(m);


    });
            #endregion
            #region cartkiwi

            Client.GetService<CommandService>().CreateCommand("cartkiwi")
    .Description("Description")
    .AddCheck((a,b,c) => RegisterCooldown(240, a).IsFinished && c.Server.Id == 249545918821433354)
    .Do(async e =>
        {
            _cool[e.Command].Restart();
            const string salut = @"Kiwiii !
Avec la carte Kiwi, tu payes moitié prix.
Et ton papa aussi.
Et ta maman aussi.
Et ton tonton aussi.
Et ta marraine aussi.
Billets s’il vous plait.
Oui Ouiii!
Voici la carte Kiwi. Nos billets moitié prix.
Moitié prix? C’est pas possible!
Mais si, c’est possible, avec la carte Kiwi, l’enfant de moins de seize ans
et ceux qui l’accompagnent jusqu’à quatre personnes payent tous moitié prix.
Un enfant, une carte Kiwi et on voyage à moitié prix.";
            List<string> list = new List<string>(Regex.Split(salut, Environment.NewLine));
            var x = await e.Channel.SendMessage("Cart kiwi c parti :");
            string messag = x.RawText;
            foreach (var item in list)
            {
                messag += $"{Environment.NewLine} {item}";
                await x.Edit(messag);
                await Task.Delay(1200 + item.Length * 25);
            }
        });


            #endregion
            #region Darkphoenix

            Client.GetService<CommandService>().CreateCommand("darkpheonix")
.Description("Darkekphoenix")
.AddCheck((a, b, c) => RegisterCooldown(15, a).IsFinished)
.Do(async e =>
    {
        SendM send = e.Channel.SendMessage;
        Random r = new Random(DateTime.Now.Millisecond);
        string s = "Coincidence scan running...";
        var x = await send(s);
        int num = 0;
        List<string> wiruses = new List<string>
        {
            "Coincidence.Android.Nexus7",
            "Coincidence.Samsung.ace",
            "Coincidence.Minor.Windows10",
            "Coincidence.CPU.Intel",
            "Coincidence.Ram.Same",
            "Coincidence.Minor.Android.6",
            "Coincidence.Heuristic",
            "Coincidence.Command",
            "Coincidence.Minor.ServerHosting",
            "Coincidence.Human",
            "Coincidence.Minor.HasComputers",
            "Coincidence.WantMore"
        };
        string wirus = "";
        for (int i = 0; i < 10; i++)
        {
            var temp = r.Next(1, 4);
            num += temp;
            for (int io = 0; io < temp; io++)
            {
                wirus += $"`{wiruses.ElementAt(r.Next(0, wiruses.Count - 1))}`" + Environment.NewLine;
            }
            var scan = "Coincidences found : " + num;
            await x.Edit($"{s} {Environment.NewLine} {wirus} {scan}");
            await Task.Delay(750 + r.Next(100, 1000));
        }
        await send("So much coincidence wow");
    });


            #endregion
            #region GetInvite

            Client.GetService<CommandService>().CreateCommand("grabinvite")
.Description("GRAB GRAB GRAB THAT INVITE :3333")
.Parameter("grab", ParameterType.Unparsed)
.AddCheck((a,b,c) => RegisterCooldown(10, a).IsFinished)
.Do(async e =>
    {
        _cool[e.Command].Restart();
        SendM send = e.Channel.SendMessage;
        if (e.User.Id == 244509121838186497)
        {
            await send("Grabbing that pussy...");
        }
        else
            await send("Grabbing...");
        string grabgrabgrabthatshit = e.GetArg("grab").ToLower();
        var servs = Client.Servers.Where(s => s.Name.ToLower().Contains(grabgrabgrabthatshit));
        var enumerable = servs as IList<Server> ?? servs.ToList();
        if (enumerable.Any())
        {
            try
            {
                var invit = (await enumerable.First().GetInvites()).Where(inv => !inv.IsRevoked);
                var invites = invit as IList<Invite> ?? invit.ToList();
                var inviturl = invites.First().Url.Remove(18, 1);
                if (invites.Any())
                    await send($"Returning the first one : {Environment.NewLine} {inviturl}");
                else
                    await send("No Invites found");
            }
            catch
            {
                await send(":warning: the bot failed to get the invite.");
            }
        }
        else
        {
            await send("The server couldn't be found :(");
        }
    });


            #endregion
            #region spam spam
 Client.GetService<CommandService>().CreateCommand("spam")
.Description("spamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspam")
.Parameter("spammy", ParameterType.Unparsed)
.AddCheck((a,b,c) => RegisterCooldown(20, a).IsFinished)
.Do(async e =>
    {
        _cool[e.Command].Restart();
        string spummmy = e.GetArg("spammy");
        HashSet<Message> m = new HashSet<Message>();
        for (int i = 0; i < 6; i++)
        {
            m.Add(await e.Channel.SendMessage(spummmy));
            await Task.Delay(250);
        }
        
        await Task.Delay(2500);
        foreach (var item in m)
        {
            await item.Delete();
        }
    });
            #endregion
            #region Role

            Client.GetService<CommandService>().CreateCommand("roleUpdates")
.Description("Select if you wanna get updates bout roles m8")
.Parameter("r")
.AddCheck((a,b,c) => IsAcceptable(b))
.Do(async e =>
    {
        string bs = e.GetArg("r");
        bool? right = null;
        try
        {
            bool tryright;
            bool.TryParse(bs,out tryright);
            right = tryright;
        } catch
        {
           await e.Channel.SendMessage("Sorry, but an error occured, made SURRRE you put true or false as a parameter :/");
        }
        if (right != null)
        {
            FindServSettings(e.Server).RoleUpdatesMessage = right ?? false;
            
            await e.Channel.SendMessage("success !");
            await FindLogServer(e.Server).SendMessage(right ?? false ? "Role updates has been enabled" : "Role updates has been disabled");
            SerializeServerSettingsAndSave();
        }
    });


            #endregion
            #region Rain-bow

            Client.GetService<CommandService>().CreateCommand("rainbow")
.Description("xdd")
.Parameter("param1", ParameterType.Unparsed)
.Do(async e =>
    {
        string k = e.GetArg("param1");
        var rol = e.Server.Roles.Where(r => r.Name.ToLower() == k.ToLower());
        var roles = rol as IList<Role> ?? rol.ToList();
        if (roles.Any())
        {
            var kek = roles.First();
            var thingy = typeof(Color);
            List<Color> toColor = new List<Color>();
            List<Color> awfulColors = new List<Color>
            {
                Color.DarkBlue,
                Color.DarkerGrey,
                Color.DarkGold,
                Color.DarkGreen,
                Color.DarkGrey,
                Color.DarkMagenta,
                Color.DarkOrange,
                Color.DarkPurple,
                Color.DarkRed,
                Color.DarkTeal,
                Color.Default,
                Color.LighterGrey,
                Color.LightGrey,
                Color.Teal              
            };
            foreach (FieldInfo item in thingy.GetFields())
            {                
                    if (!awfulColors.Contains(item.GetValue(new object())))
                    try
                    {
                        toColor.Add(item.GetValue(new object()) as Color);
                    }
                    catch
                    {
                        // i ignore you m8
                    }
            }
            for (int i = 0; i < 10; i++)
            {
                foreach (var item in toColor)
                {
                    await kek.Edit(null, null, item);
                }
                await Task.Delay(125);
            }
        } else
        {
            await e.Channel.SendMessage("nut fund");
        }
        await Task.Delay(1);
    });


            #endregion

            #region MyRegion

            

            #endregion
        }
    }
}
