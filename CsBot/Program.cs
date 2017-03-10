using Discord;
using Discord.Commands;
using System;
using System.Linq;

namespace CsBot
{
    class Program
    {
        private DiscordClient _client;
        private DateTime time;
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

            _client.MessageReceived += async (s, e) =>
            {
                if (time < DateTime.Now)
                {
                    var text = e.Message.Text.ToLowerInvariant();
                    if (!e.Message.IsAuthor && (text.Contains("Dax0n") || text.Contains("chad") || text.Contains("elijah") || text.Contains("anderson")) || text.Contains("falcons"))
                    {
                        time = DateTime.Now.AddSeconds(60);
                        await e.Channel.SendMessage("FUCK CHAD! http://s2.quickmeme.com/img/ff/ff86a553405dee228bf4ba9783fe58c88ad3442d083a413d79f421d126c7e273.jpg");
                    }
                }
            };

            _client.ExecuteAndWait(async () => {
                await _client.Connect("Mjg5ODA4MzAxMTMzMjY2OTQ0.C6Rwkg.2g3pqYfFxOqE-CnTmfYvu4rZA28", TokenType.Bot);
                                
                var server = _client.GetServer(287351840583188481);
                time = DateTime.Now.AddSeconds(60);
                Console.WriteLine("test");
            });

            

        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");
        }
    }
}
