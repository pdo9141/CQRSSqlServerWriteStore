using System.Threading.Tasks;

namespace AuctionFramework.Commands
{
    public class CommandDispatcher
    {
        private readonly CommandHandlerMap _map;

        public CommandDispatcher(CommandHandlerMap map)
        {
            _map = map;
        }

        public Task Dispatch(object command)
        {
            var handler = _map.Get(command);

            return handler(command);
        }
    }
}
