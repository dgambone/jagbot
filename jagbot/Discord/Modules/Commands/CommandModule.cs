using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Modules;
using JagBot.Modules.Status;
using JagBot.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JagBot.Modules.Commands
{
    public class CommandModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private bool _isRunning;
        private HttpService _http;
        private SettingsManager<Settings> _settings;
        private CommandService _commandSvc;

        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;
            _http = _client.GetService<HttpService>();
            _settings = _client.GetService<SettingsService>()
                .AddModule<CommandModule, Settings>(manager);

            _commandSvc = _client.GetService<CommandService>();

            _commandSvc.CreateCommand("oye") //create command greet
                .Alias(new string[] { "oye", "oye!" }) //add 2 aliases, so it can be run with ~gr and ~hi
                .Description("Greets a person.") //add description, it will be shown when ~help is used
                                                 //.Parameter("GreetedPerson", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    
                    if (e.User.Id == 119186604735725568)
                    {
                        await e.Channel.SendMessage($"Piss off! {e.User.Name}, you fucking cunt!");
                    }
                    else
                    {
                        await e.Channel.SendMessage($"Oye oye! {e.User.Name}, you fucking cunt!");
                    }
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    //sends a message to channel with the given text
                });

            _commandSvc.CreateCommand("who is god") //create command greet
                .Description("Tells you who god is.") //add description, it will be shown when ~help is used
                .Alias(new string[] { "who is god?" }) //add 2 aliases, so it can be run with ~gr and ~hi//.Parameter("GreetedPerson", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    await e.Channel.SendMessage("Claude Giroux, http://claudegiroux28.com");
                    //sends a message to channel with the given text
                });

            _commandSvc.CreateCommand("members") //create command greet
                .Description("members") //add description, it will be shown when ~help is used
                .Do(async e =>
                {
                    if (e.User.Id == 119186604735725568)
                    {
                        await e.Channel.SendMessage($"Piss off {e.User.Name}");
                    }
                    StringBuilder sb = new StringBuilder();
                    foreach (var user in _client.Servers?.FirstOrDefault()?.Users)
                    {
                        sb.AppendLine(string.Format("ID: {0}, Name: {1}, Nickname: {2}, Status: {3}", user.Id, user.Name, user.Nickname, user.Status));
                    }
                    await e.Channel.SendMessage(sb.ToString());
                    //sends a message to channel with the given text
                });

            _commandSvc.CreateCommand("change james") //create command greet
                .Description("Changes James nickname") //add description, it will be shown when ~help is used
                .Parameter("NewName", ParameterType.Required) //as an argument, we have a person we want to greet
                .Parameter("PlusMinus", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    if (e.User.Id == 119186604735725568)
                    {
                        await e.Channel.SendMessage($"Piss off {e.User.Name}");
                    }
                    else
                    {
                        var james = e.Server.Users.Where(x => x.Id == 119186604735725568).FirstOrDefault();
                        string newName = e.GetArg("NewName") + ": " + e.GetArg("PlusMinus"); // James" + DateTime.Now.Ticks.ToString();
                        if (james != null)
                        {
                            await james.Edit(null, null, null, null, newName);
                            await e.Channel.SendMessage($"nickname is now " + newName);
                        }
                    }
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    //sends a message to channel with the given text
                });

            _commandSvc.CreateCommand("fart") //create command greet
                .Description("Plays a sound") //add description, it will be shown when ~help is used
                                              //.Parameter("NewName", ParameterType.Required) //as an argument, we have a person we want to greet
                                              //.Parameter("PlusMinus", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    var voiceChannel = _client.Servers.FirstOrDefault().VoiceChannels.FirstOrDefault(); // Finds the first VoiceChannel on the server 'Music Bot Server'

                    AudioService x = _client.GetService<AudioService>(); // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                    var _vClient = await x.Join(voiceChannel); // Join the Voice Channel, and return the IAudioClient.
                    string path = @"C:\Users\admin\OneDrive\Documents\Visual Studio 2015\Projects\Discord\Discord\mp3\fart-01.mp3";
                    SendAudio(path, _vClient);
                    _vClient.Wait();
                    _vClient.Clear();
                    await voiceChannel.LeaveAudio();
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    //sends a message to channel with the given text
                });
        }

        public void SendAudio(string filePath, IAudioClient vClient)
        {
            var channelCount = _client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
            var OutFormat = new WaveFormat(48000, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
            using (var MP3Reader = new Mp3FileReader(filePath)) // Create a new Disposable MP3FileReader, to read audio from the filePath parameter
            using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
            {
                resampler.ResamplerQuality = 60; // Set the quality of the resampler to 60, the highest quality
                int blockSize = OutFormat.AverageBytesPerSecond / 50; // Establish the size of our AudioBuffer
                byte[] buffer = new byte[blockSize];
                int byteCount;

                while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0) // Read audio into our buffer, and keep a loop open while data is present
                {
                    if (byteCount < blockSize)
                    {
                        // Incomplete Frame
                        for (int i = byteCount; i < blockSize; i++)
                            buffer[i] = 0;
                    }
                    vClient.Send(buffer, 0, blockSize); // Send the buffer to Discord
                }
            }

        }
    }

    /*
     * _client.GetService<CommandService>().CreateCommand("oye") //create command greet
                .Alias(new string[] { "oye", "oye!" }) //add 2 aliases, so it can be run with ~gr and ~hi
                .Description("Greets a person.") //add description, it will be shown when ~help is used
                //.Parameter("GreetedPerson", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    if(e.User.Id == 119186604735725568)
                    {
                        await e.Channel.SendMessage($"Piss off! {e.User.Name}, you fucking cunt!");
                    }else
                    {
                        await e.Channel.SendMessage($"Oye oye! {e.User.Name}, you fucking cunt!");
                    }
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    //sends a message to channel with the given text
                });

            _client.GetService<CommandService>().CreateCommand("who is god") //create command greet
                .Description("Tells you who god is.") //add description, it will be shown when ~help is used
                .Alias(new string[] { "who is god?" }) //add 2 aliases, so it can be run with ~gr and ~hi//.Parameter("GreetedPerson", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    await e.Channel.SendMessage("Claude Giroux, http://claudegiroux28.com");
                    //sends a message to channel with the given text
                });

            _client.GetService<CommandService>().CreateCommand("members") //create command greet
                .Description("members") //add description, it will be shown when ~help is used
                .Do(async e =>
                {
                    if (e.User.Id == 119186604735725568)
                    {
                        await e.Channel.SendMessage($"Piss off {e.User.Name}");
                    }
                    StringBuilder sb = new StringBuilder();
                    foreach(var user in _client.Servers?.FirstOrDefault()?.Users)
                    {
                        sb.AppendLine(string.Format("ID: {0}, Name: {1}, Nickname: {2}, Status: {3}", user.Id, user.Name, user.Nickname, user.Status));
                    }
                    await e.Channel.SendMessage(sb.ToString());
                    //sends a message to channel with the given text
                });

            _client.GetService<CommandService>().CreateCommand("change james") //create command greet
                .Description("Changes James nickname") //add description, it will be shown when ~help is used
                .Parameter("NewName", ParameterType.Required) //as an argument, we have a person we want to greet
                .Parameter("PlusMinus", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    if (e.User.Id == 119186604735725568)
                    {
                        await e.Channel.SendMessage($"Piss off {e.User.Name}");
                    }
                    else
                    {
                        var james = e.Server.Users.Where(x => x.Id == 119186604735725568).FirstOrDefault();
                        string newName = e.GetArg("NewName") + ": " + e.GetArg("PlusMinus"); // James" + DateTime.Now.Ticks.ToString();
                        if (james != null)
                        {
                            await james.Edit(null, null, null, null,newName );
                            await e.Channel.SendMessage($"nickname is now " + newName);
                        }
                    }
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    //sends a message to channel with the given text
                });

            _client.GetService<CommandService>().CreateCommand("fart") //create command greet
                .Description("Plays a sound") //add description, it will be shown when ~help is used
                //.Parameter("NewName", ParameterType.Required) //as an argument, we have a person we want to greet
                //.Parameter("PlusMinus", ParameterType.Required) //as an argument, we have a person we want to greet
                .Do(async e =>
                {
                    var voiceChannel = _client.Servers.FirstOrDefault().VoiceChannels.FirstOrDefault(); // Finds the first VoiceChannel on the server 'Music Bot Server'

                    AudioService x =  _client.GetService<AudioService>(); // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                    var _vClient = await x.Join(voiceChannel); // Join the Voice Channel, and return the IAudioClient.
                    string path = @"C:\Users\admin\OneDrive\Documents\Visual Studio 2015\Projects\Discord\Discord\mp3\fart-01.mp3";
                    SendAudio(path, _vClient);
                    _vClient.Wait();
                    _vClient.Clear();
                    await voiceChannel.LeaveAudio();
                    //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
                    //sends a message to channel with the given text
                });
    */
}
