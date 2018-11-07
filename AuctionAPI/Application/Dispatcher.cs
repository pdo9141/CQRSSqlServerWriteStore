using AuctionFramework;
using AuctionFramework.Commands;

namespace AuctionAPI.Application {
    public static class Dispatcher
    {
        internal static CommandDispatcher Instance { get; set; }

        static Dispatcher()
        {
            var repository = new AggregateRepository();
            var commandHandlerMap = new CommandHandlerMap(new Handlers(repository));
            Instance = new CommandDispatcher(commandHandlerMap);
        }
    }
}