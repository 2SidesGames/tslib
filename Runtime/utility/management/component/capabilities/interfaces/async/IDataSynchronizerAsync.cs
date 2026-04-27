using System.Threading;
using Cysharp.Threading.Tasks;

public interface IDataSynchronizerAsync
{
    public UniTask SaveDataAsync(CancellationToken cancellationToken);

    public UniTask LoadDataAsync(CancellationToken cancellationToken);
}