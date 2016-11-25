using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// My nice bot Loggy !
/// </summary>
/// 

namespace Loggy
{
    partial class Program
    {
        private delegate Task<Message> SendM(string s);
        private bool IsJeuxjeux(User u)
        {
            return u.Id == 134348241700388864;
        }
        internal Cooldown RegisterCooldown(int seconds, Command cm)
        {
            if (!cool.Keys.Any((com => { return com.Equals(cm); })))
                cool.Add(cm, new Cooldown(seconds));
            return cool[cm];

        }

        [Serializable]
        public class CommandSentByBotException : Exception
        {
            public CommandSentByBotException() { }
            public CommandSentByBotException(string message) : base(message) { }
            public CommandSentByBotException(string message, Exception inner) : base(message, inner) { }
            protected CommandSentByBotException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
        private void DoCommands()
        {
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
                    return RegisterCooldown(50, a).isFinished;
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
.AddCheck((a, b, c) =>
{
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
       Channel chtosend;
       if ((e.Server.Id == 110373943822540800 && e.Channel.Name == "general"))
       {
           chtosend = e.Server.AllChannels.Where(w => w.Name == @"testing-[\]").First();
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
    await Task.Delay(1250);
    await kek.Edit("electronicwizzzzzzz(one)");

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
.AddCheck((a, b, c) => { return IsJeuxjeux(b); }, "u not jeuxjeux20")

.Do(async e =>
{
    foreach (var item in _client.Servers)
    {
        await item.DefaultChannel.SendMessage(e.GetArg("say"));
    }
});


            #endregion
            #region SPAM HI

            _client.GetService<CommandService>().CreateCommand("hiSpam")
.Description("hi hi hi hi hi hi hi hi hi")
.AddCheck((a, b, c) =>
{
    if (!cool.Keys.Any((com => { return com == a; })))
        cool.Add(a, new Cooldown(35));
    return cool[a].isFinished;

})
.Do(async e =>
    {
        cool[e.Command].Restart();
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

            _client.GetService<CommandService>().CreateCommand("defaultchannel")
.Description("Set the default channel.")
.Parameter("ch", ParameterType.Required)
.AddCheck((a, b, c) => { return isAcceptable(b); })
.Do(async e =>
    {
        string chanName = e.GetArg("ch");
        bool isHere = e.Server.AllChannels.Select(ok => { return ok.Mention; }).Contains(chanName);
        if (isHere)
        {
            Channel hello = e.Server.AllChannels.Where(c => c.Mention == chanName).First();
            if (SettingsList.Any(s => s.Id == e.Server.Id))
                SettingsList.Where(s => s.Id == e.Server.Id).First().ChannelIdToLog = hello.Id;
            else
                SettingsList.Add(new ServerSettings(e.Server.Id, hello.Id));
            await e.Channel.SendMessage("ok alright");
            await FindLogServer(e.Server).SendMessage("Future log messages will be sent here");
            using (StreamWriter sw = new StreamWriter("settings.xml", false))
            {
                var ser = new System.Xml.Serialization.XmlSerializer(typeof(ServerSettings[]));
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
            _client.GetService<CommandService>().CreateCommand("eval")
.Description("Evals a c# code executed apart and compiled, protected from rds xd and use output = [the variable] to output ; for defining classes : put [CD] at the end of the code; then type your class ;)")
.Parameter("to", ParameterType.Unparsed)
.AddCheck((a, b, c) =>
{

    return /* RegisterCooldown(5, a).isFinished || TrustedEvalList.Contains(b.Id); */ true;
})
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
        var Regex = new System.Text.RegularExpressions.Regex("\\[CD\\].*$", System.Text.RegularExpressions.RegexOptions.Singleline);
        if (codeToEval.Contains("[CD]"))
        {
            await e.Channel.SendMessage("CLASS DEFINITION FOUND");
        }
        if (Regex.IsMatch(codeToEval))
        {
            string val = Regex.Match(codeToEval).Value;
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
        _client.SetStatus(UserStatus.DoNotDisturb);
        CompilerResults results = null;
        var myTask = Task.Factory.StartNew(() =>
        {
            return provider.CompileAssemblyFromSource(compilerParams, classedCode);
        });    
            results = await myTask;
        //catch (Exception ex)
        //{
        //    await e.Channel.SendMessage($"Exception occured : {ex.Message}");
        //    goto oh;
        //}
        _client.SetStatus(UserStatus.Online);
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
            object o = results.CompiledAssembly.CreateInstance("Eval.Evalued");
            MethodInfo mi = o.GetType().GetMethod("EvalIt");
            object res = "no u wut";
            res = mi.Invoke(o, null);
            await e.Channel.SendMessage($@"Success !
```Output : {res ?? "null"}```");
        }
       
        _client.SetStatus(UserStatus.Online);
        ;
    });
#pragma warning restore CS1998 // Cette méthode async n'a pas d'opérateur 'await' et elle s'exécutera de façon synchrone
            #endregion
            #region Trust

            _client.GetService<CommandService>().CreateCommand("evaltrust")
.Description("Only for jeuxjeux20")
.Parameter("us", ParameterType.Required)
.AddCheck((a, b, c) => { return IsJeuxjeux(b); })
.Do(async e =>
    {
        string user = e.GetArg("us");
        var trustedOne = e.Server.Users.Where(u => u.NicknameMention == user || u.Mention == user);
        User toTrust = trustedOne.Any() ? trustedOne.First() : null;
        if (toTrust != null)
        {
            TrustedEvalList.Add(toTrust.Id);
            await e.Channel.SendMessage("Done ! :)");
            using (StreamWriter sw = new StreamWriter("trusted.xml", false))
            {
                new System.Xml.Serialization.XmlSerializer(typeof(ulong[])).Serialize(sw, SerializableTrustedEvalList);
            }
        }
        else
        {
            await e.Channel.SendMessage("not found, make sure that you mentionned him");
        }
    });


            #endregion
            #region UNTRUST
            _client.GetService<CommandService>().CreateCommand("untrust")
.Description("Only for jeuxjeux20")
.Parameter("us", ParameterType.Required)
.AddCheck((a, b, c) => { return IsJeuxjeux(b); })
.Do(async e =>
{
    string user = e.GetArg("us");
    var trustedOne = e.Server.Users.Where(u => u.NicknameMention == user || u.Mention == user);
    User toTrust = trustedOne.Any() ? trustedOne.First() : null;
    if (toTrust != null)
    {
        TrustedEvalList.Remove(toTrust.Id);
        await e.Channel.SendMessage("Done ! :)");
        using (StreamWriter sw = new StreamWriter("trusted.xml", false))
        {
            new System.Xml.Serialization.XmlSerializer(typeof(ulong[])).Serialize(sw, SerializableTrustedEvalList);
        }
    }
    else
    {
        await e.Channel.SendMessage("not found, make sure that you mentionned him");
    }

});
            #endregion
            #region FireC

            _client.GetService<CommandService>().CreateCommand("FireC")
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
            await x.Edit("Parsing and deserializing FireC, this may take a while" + Environment.NewLine + ASCIIBar.DrawProgressBar(kek) + $" - {kek}%");
            kek += (uint)new Random(DateTime.Now.Millisecond).Next(1, 15);
            await Task.Delay(250);
        }
        await x.Edit("FireC found ! : Pair<Discord.User`1,List<Hentai>>");
    });

            #endregion
            #region cth
            _client.GetService<CommandService>().CreateCommand("cth")
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

            _client.GetService<CommandService>().CreateCommand("cartkiwi")
    .Description("Description")
    .AddCheck((a,b,c) => { return RegisterCooldown(240, a).isFinished && c.Server.Id == 249545918821433354; })
    .Do(async e =>
        {
            cool[e.Command].Restart();
            string salut = @"Kiwiii !
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

            _client.GetService<CommandService>().CreateCommand("darkpheonix")
.Description("Darkekphoenix")
.AddCheck((a, b, c) => { return RegisterCooldown(15, a).isFinished; })
.Do(async e =>
    {
        SendM send = e.Channel.SendMessage;
        Random r = new Random(DateTime.Now.Millisecond);
        string s = "Coincidence scan running...";
        var x = await send(s);
        string scan = "Coincidences found : 1";
        int num = 0;
        List<string> Wiruses = new List<string>
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
                wirus += $"`{Wiruses.ElementAt(r.Next(0, Wiruses.Count - 1))}`" + Environment.NewLine;
            }
            scan = "Coincidences found : " + num;
            await x.Edit($"{s} {Environment.NewLine} {wirus} {scan}");
            await Task.Delay(750 + r.Next(100, 1000));
        }
        await send("So much coincidence wow");
    });


            #endregion
            #region GetInvite

            _client.GetService<CommandService>().CreateCommand("grabinvite")
.Description("GRAB GRAB GRAB THAT INVITE :3333")
.Parameter("grab", ParameterType.Unparsed)
.Do(async e =>
    {
        SendM Send = e.Channel.SendMessage;
        if (e.User.Id == 244509121838186497)
        {
            await Send("Grabbing that pussy...");
        }
        else
            await Send("Grabbing...");
        string grabgrabgrabthatshit = e.GetArg("grab").ToLower();
        var servs = _client.Servers.Where(s => s.Name.ToLower().Contains(grabgrabgrabthatshit));
        if (servs.Any())
        {
            try
            {
                var invit = (await servs.First().GetInvites()).Where(inv => !inv.IsRevoked);
                var inviturl = invit.First().Url.Remove(18, 1);
                if (invit.Any())
                    await Send($"Returning the first one : {Environment.NewLine} {inviturl}");
                else
                    await Send("No Invites found");
            }
            catch
            {
                await Send(":warning: the bot failed to get the invite.");
            }
        }
        else
        {
            await Send("The server couldn't be found :(");
        }
    });


            #endregion
            #region spam spam
 _client.GetService<CommandService>().CreateCommand("spam")
.Description("spamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspam")
.Parameter("spammy", ParameterType.Unparsed)
.AddCheck((a,b,c) => { return RegisterCooldown(20, a).isFinished; })
.Do(async e =>
    {
        cool[e.Command].Restart();
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

            _client.GetService<CommandService>().CreateCommand("roleUpdates")
.Description("Select if you wanna get updates bout roles m8")
.Parameter("r", ParameterType.Required)
.AddCheck((a,b,c) => { return isAcceptable(b); })
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

            _client.GetService<CommandService>().CreateCommand("rainbow")
.Description("xdd")
.Parameter("param1", ParameterType.Required)
.Do(async e =>
    {
        string k = e.GetArg("param1");
        var rol = e.Server.Roles.Where(r => r.Name.ToLower() == k.ToLower());
        if (rol.Any())
        {
            var kek = rol.First();
           
        }
        await Task.Delay(1);
    });


            #endregion
        }
    }
}
