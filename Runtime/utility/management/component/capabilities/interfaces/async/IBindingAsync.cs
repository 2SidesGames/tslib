using System.Threading;
using Cysharp.Threading.Tasks;

namespace SGLib.Utility.Management.Component.Capabilities.Async
{
    public interface IBindingAsync
    {
        public UniTask BindAsync(CancellationToken cancellationToken);
    }
}