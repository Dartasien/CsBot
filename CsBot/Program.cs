using Discord;
using Discord.Commands;
using System;

namespace CsBot
{
    class Program
    {
        private DiscordClient _client;
        private DateTime _canRespondToAnyoneTime;
        private int _respondToAnyoneTimer;
        private DateTime _canRespondToChadTime;
        private int _respondToChadTimer;
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args) => new Program().Start();

        public void Start()
        {
            _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.AppName = "CsBot";
                x.AppUrl = "www.google.com";
                x.LogHandler = Log;
            });

            _client.UsingCommands(x => {
                x.PrefixChar = '~';
                x.HelpMode = HelpMode.Public;
            });

            _client.MessageReceived += async (s, e) =>
            {
                var text = e.Message.Text.ToLowerInvariant();

                if (e.Message.User.Id == 203379577899188224)
                {
                    if (_canRespondToChadTime < DateTime.Now)
                    {
                        HandleRespondToChadTImer();
                        await e.Channel.SendFile("img/fchad.jpg");
                    }
                }

                if (_canRespondToAnyoneTime < DateTime.Now)
                {
                    if (!e.Message.IsAuthor)
                    {
                        if (text.Contains("fuck chad"))
                        {
                            HandleRespondToAnyoneTimer();
                            await e.Channel.SendMessage("YEAH!");
                        }
                        else if (text.Contains("Dax0n") || text.Contains("chad") || text.Contains("elijah") ||
                             text.Contains("anderson"))
                        {
                            HandleRespondToAnyoneTimer();
                            await e.Channel.SendMessage("FUCK CHAD!");
                        }
                        else if (text.Contains("falcons"))
                        {
                            HandleRespondToAnyoneTimer();
                            await e.Channel.SendMessage("FUCK CHAD AND HIS FALCONS!");
                        }
                    }
                }
            };

            _client.GetService<CommandService>().CreateCommand("greet")
               .Alias("gr", "hi")
               .Description("Greets a person.")
               .Parameter("GreetedPerson")
               .Do(async e =>
               {
                   await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                });

            //_client.GetService<CommandService>().CreateCommand("trump") //create command greet
            //    .Description("Don't even.") //add description, it will be shown when ~help is used
            //    .Do(async e =>
            //    {
            //        await e.Channel.SendMessage(
            //            "An immoral asshat who only looks out for himself and will drive this country into war, cause us to lose our closest allies, and likely sell us out to the highest bidder if he lasts that long.");
            //        //sends a message to channel with the given text
            //    });

            _client.GetService<CommandService>().CreateCommand("setRespondTimer")
                .Alias("srt")
                .Description("Sets the timer for CsBot to respond to anyone with Chad dissing messages.")
                .Parameter("timer") 
                .Do(async e =>
                {
                    if (e.User.ServerPermissions.ManageServer)
                    {
                        int newTimer;
                        if (int.TryParse(e.GetArg("timer"), out newTimer))
                        {
                            _respondToAnyoneTimer = newTimer;
                            _canRespondToAnyoneTime = DateTime.Now;
                        }
                        else
                        {
                            await e.Channel.SendMessage(
                                $"{e.GetArg("timer")} must be an integer value between 0 and {int.MaxValue}");
                        }
                    }
                });

            _client.GetService<CommandService>().CreateCommand("setChadTimer")
               .Alias("sct")
               .Description("Sets the timer for CsBot to respond to Chad's messages.")
               .Parameter("timer")
               .Do(async e =>
                {
                    if (e.User.ServerPermissions.ManageServer)
                    {
                        int oldTimer;
                        if (int.TryParse(e.GetArg("timer"), out oldTimer))
                        {
                            _respondToChadTimer = oldTimer;
                            _canRespondToChadTime = DateTime.Now;
                        }
                        else
                        {
                            await e.Channel.SendMessage(
                                $"{e.GetArg("timer")} must be an integer value between 0 and {int.MaxValue}");
                        }
                    }
                });

            _client.ExecuteAndWait(async () => {
                await _client.Connect("Mjg5ODA4MzAxMTMzMjY2OTQ0.C6Rwkg.2g3pqYfFxOqE-CnTmfYvu4rZA28", TokenType.Bot);

                _respondToAnyoneTimer = 0;
                _canRespondToAnyoneTime = DateTime.Now;
                _respondToChadTimer = 0;
                _canRespondToChadTime = DateTime.Now;
                
            });

        }

        private void HandleRespondToChadTImer()
        {
            if (_respondToChadTimer == 0)
            {
                _respondToChadTimer = 600;
            }
            _canRespondToChadTime = DateTime.Now.AddSeconds(_respondToChadTimer);
        }

        private void HandleRespondToAnyoneTimer()
        {
            if (_respondToAnyoneTimer == 0)
            {
                _respondToAnyoneTimer = 5;
            }
            _canRespondToAnyoneTime = DateTime.Now.AddSeconds(_respondToAnyoneTimer);
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");
        }
    }
}
