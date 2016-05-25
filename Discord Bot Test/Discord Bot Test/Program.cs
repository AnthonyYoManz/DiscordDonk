using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordSharp;
using DiscordSharp.Objects;

namespace Discord_Bot_Test
{
    class Program
    {
        public static bool isBot = true;
        public static string baseUrl = "https://discordapp.com/api";
        public static MemoList memoList;
        public static TuneList tuneList;

        static void Main(string[] args)
        {
            memoList = new MemoList();
            memoList.initialise("memos.txt");
            tuneList = new TuneList();
            tuneList.initialise("tunes.txt");

            DiscordClient client = new DiscordClient("ItIsAMystery", isBot);
            client.ClientPrivateInformation.Email = "email";
            client.ClientPrivateInformation.Password = "pass";

            // Then, we are going to set up our events before connecting to discord, to make sure nothing goes wrong.

            Console.WriteLine("Defining Events");
            // find that one you interested in 

            client.Connected += (sender, e) => // Client is connected to Discord
            {
                Console.WriteLine("Connected! User: " + e.User.Username);
                client.UpdateCurrentGame("Odorwatch");
            };


            client.PrivateMessageReceived += (sender, e) => // Private message has been received
            {
                if (e.Message == "!help")
                {
                    e.Author.SendMessage("No.");
                    // Because this is a private message, the bot should send a private message back
                    // A private message does NOT have a channel
                }
            };


            client.MessageReceived += (sender, e) => // Channel message has been received
            {
                try
                {
                    if (e.MessageText.ElementAt(0) == '!')
                    {
                        //user maybe goin for a function innit
                        string function = e.MessageText;
                        if (e.MessageText.Contains(' '))
                        {
                            function = e.MessageText.Split(' ')[0];
                            function = function.ToLower();
                            string param = e.MessageText.Substring(e.MessageText.IndexOf(' ')).Trim();
                            if (function == "!memo")
                            {
                                if (e.MessageText.Split(' ').Count() > 2)
                                {
                                    string param2 = param.Substring(param.IndexOf(' ')).Trim();
                                    param = param.Split(' ')[0].Trim();
                                    string result = memoList.handleMemoUpdate(param, param2);
                                    if (result != "")
                                    {
                                        e.Channel.SendMessage(result);
                                    }
                                }
                                else
                                {
                                    string result = memoList.handleMemoUpdate(param);
                                    if (result != "")
                                    {
                                        e.Channel.SendMessage(result);
                                    }
                                }
                            }
                            if (function == "!tune")
                            {
                                string result = "";
                                int depth = -1;
                                if (Int32.TryParse(param.Trim(), out depth))
                                {
                                    result = tuneList.getRandomTune(depth);
                                }
                                else
                                {
                                    result = tuneList.handleTuneUpdate(param);
                                }
                                if (result != "")
                                {
                                    e.Channel.SendMessage(result);
                                }
                            }
                        }
                        else
                        {
                            function = function.ToLower();
                            if (function == "!test")
                            {
                                e.Channel.SendMessage("It begins...");
                            }
                            else if (function == "!help")
                            {
                                e.Channel.SendMessage("No.");
                            }
                            else if (function == "!memolist")
                            {
                                e.Channel.SendMessage(memoList.getList());
                            }
                            else if (function == "!tune")
                            {
                                string tune = tuneList.getRandomTune();
                                if (tune != "")
                                {
                                    e.Channel.SendMessage(tune);
                                }
                            }
                            else if (function == "!tunecount")
                            {
                                e.Channel.SendMessage("I know of " + tuneList.getCount() + " tunes.");
                            }
                        }
                    }
                }
                catch (Exception d)
                {
                    Console.WriteLine("Something went wrong!\n" + d.Message + "\nPress any key to close this window.");
                }
            };

            // Now, try to connect to Discord.
            try
            {
                // Make sure that IF something goes wrong, the user will be notified.
                // The SendLoginRequest should be called after the events are defined, to prevent issues.
                Console.WriteLine("Sending login request");
                client.SendLoginRequest();
                Console.WriteLine("Connecting client in separate thread");
                // Cannot convert from 'method group' to 'ThreadStart', so i removed threading
                // Pass argument 'true' to use .Net sockets.
                client.Connect();
                client.UpdateBotStatus(true);
                // Login request, and then connect using the discordclient i just made.
                Console.WriteLine("Client connected!");

            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong!\n" + e.Message + "\nPress any key to close this window.");
            }

            // Done! your very own Discord bot is online!


            // Now to make sure the console doesnt close:
            Console.ReadKey();
            memoList.cleanup();
            tuneList.cleanup();
            Environment.Exit(0); // Make sure all threads are closed.
        }
    }
}
