using System.Threading;
using Cysharp.Threading.Tasks;

namespace TSLib.Utility.Management.Component.Capabilities.Async
{
    public interface IBindingAsync
    {
        public UniTask BindAsync(CancellationToken cancellationToken);
    }
}