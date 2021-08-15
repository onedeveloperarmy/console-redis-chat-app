// See https://aka.ms/new-console-template for more information

using StackExchange.Redis;

namespace consoleApp
{
    class Program
    {
        private const string _connectionString = "localhost";
        private static ConnectionMultiplexer _connectionMultiplexer = ConnectionMultiplexer.Connect(_connectionString);
        private static string _username, _channelName;
        static void Main(string[] args)
        {
            Console.Write("Enter your username: ");
            _username = Console.ReadLine();
            Console.Write("Enter channel name: ");
            _channelName = Console.ReadLine();

            Console.Title = $"Hello {_username}, You are in chat room {_channelName}";

            var pubSub = _connectionMultiplexer.GetSubscriber();
            pubSub.Subscribe(_channelName,(channel,message) => DisplayMessage(message));
            pubSub.Publish(_channelName, $"{_username} has joined room {_channelName}");

            while (true)
            {
                var message = Console.ReadLine();
                pubSub.Publish(_channelName, $"{DateTime.Now.Hour}:{DateTime.Now.Second} {_username} says: {message}");
            }
        }

        private static void DisplayMessage(RedisValue message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}