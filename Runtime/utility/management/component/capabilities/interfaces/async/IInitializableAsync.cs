using System.Threading;
using Cysharp.Threading.Tasks;

namespace SGLib.Utility.Management.Component.Capabilities.Async
{
    public interface IInitializableAsync
    {
        /// <summary>
        /// Initializes the component.
        /// </summary>
        public UniTask InitializeAsync(CancellationToken cancellationToken);
    }
}