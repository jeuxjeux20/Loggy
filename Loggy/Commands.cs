﻿using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
/// <summary>
/// My nice bot Loggy !
/// </summary>
/// 

namespace Loggy
{
    partial class Program
    {
        private bool IsJeuxjeux(User u)
        {
            return u.Id == 134348241700388864;
        }
        private Cooldown RegisterCooldown(int seconds, Command cm)
        {
            if (!cool.Keys.Any((com => { return com.Equals(cm); })))
                cool.Add(cm, new Cooldown(seconds));
            return cool[cm];

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
        cool.Add(a, new Cooldown(110));
    return cool[a].isFinished;
})
.Do(async e =>
    {
        cool[e.Command].Restart();
        HashSet<Message> h = new HashSet<Message>();
        for (int i = 0; i < 10; i++)
        {
            h.Add(await e.Channel.SendMessage("hi"));
            await Task.Delay(175);
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
                SettingsList.Add(new ServerSettings(e.Server.Id,hello.Id));
            await e.Channel.SendMessage("ok alright");
            await FindLogServer(e.Server).SendMessage("Future log messages will be sent here");
            using (StreamWriter sw = new StreamWriter("settings.xml",false))
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


        }
    }
}