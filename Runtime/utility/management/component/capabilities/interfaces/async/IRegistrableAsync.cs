using System.Threading;
using Cysharp.Threading.Tasks;

namespace TSLib.Utility.Management.Component.Capabilities.Async
{
    public interface IRegistrableAsync
    {
        /// <summary>
        /// Registers the component with the
        /// relevant services.
        /// </summary>
        public UniTask RegisterAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Removes the component from the
        /// services where it was registered.
        /// </summary>
        public UniTask UnregisterAsync(CancellationToken cancellationToken);
    }
}

