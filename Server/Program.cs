using System.Threading.Tasks;
using TinyLogger;

namespace TACS_Server
{
    class Program
    {
        public static Settings Config;
        private static Logger Log;

        static async Task Main(string[] args)
        {
            //Read config
            Config = Settings.Load();

            Log = Logger.GetInstance();
            Log.Initialize(Config.Log.Prefix, Config.Log.Suffix, Config.Log.Directory, LogIntervalType.IT_PER_DAY, LogLevel.D, true, true);

            ChatServer cs = new ChatServer();
            await cs.Start();
                
            await Task.Delay(-1);
        }
    }
}
