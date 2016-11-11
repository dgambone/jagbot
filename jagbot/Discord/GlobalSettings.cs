using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JagBot
{
    public class GlobalSettings
    {
        public enum BotRights
        {
            Admin = 0,
            None = 1,
            Moderate = 2
        }

        private const string path = "./config/global.json";
        private static GlobalSettings _instance = new GlobalSettings();

        public static void Load()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{path} is missing.");
            }
            else
            {
                _instance = JsonConvert.DeserializeObject<GlobalSettings>(File.ReadAllText(path));
            }

        }
        public static void Save()
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(_instance, Formatting.Indented));
        }

        //Discord
        public class DiscordSettings
        {
            [JsonProperty("username")]
            public string Email;
            [JsonProperty("password")]
            public string Password;
        }
        [JsonProperty("discord")]
        private DiscordSettings _discord = new DiscordSettings();
        public static DiscordSettings Discord => _instance._discord;

        //Users
        public class UserSettings
        {
            [JsonProperty("dev")]
            public ulong DevId;
        }
        [JsonProperty("users")]
        private UserSettings _users = new UserSettings();
        public static UserSettings Users => _instance._users;

        //Github
        public class GithubSettings
        {
            [JsonProperty("username")]
            public string Username;
            [JsonProperty("password")]
            public string Password;
            [JsonIgnore]
            public string Token => Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password));
        }
        [JsonProperty("github")]
        private GithubSettings _github = new GithubSettings();
        public static GithubSettings Github => _instance._github;


        #region "Our Custom Settings"


        [JsonProperty("dave")]
        private DaveBro _dave = new DaveBro();
        [JsonProperty("graham")]
        private GrahamBro _graham = new GrahamBro();
        [JsonProperty("joe")]
        private JoeBro _joe = new JoeBro();
        [JsonProperty("sergei")]
        private SergeiBro _sergei = new SergeiBro();
        [JsonProperty("james")]
        private JamesBro _james = new JamesBro();

        public static DaveBro Dave => _instance._dave;
        public static GrahamBro Graham => _instance._graham;
        public static JoeBro Joe => _instance._joe;
        public static SergeiBro Sergei => _instance._sergei;
        public static JamesBro James => _instance._james;


        public class DaveBro : Bro { }
        public class GrahamBro : Bro { }
        public class JoeBro : Bro { }
        public class SergeiBro : Bro { }
        public class JamesBro : Bro { }

        public class Bro
        {
            [JsonProperty("id")]
            public ulong ID { get; set; }

            [JsonProperty("botright")]
            public BotRights BotRight { get; set; }
        }

        #endregion

    }
}
