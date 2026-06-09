using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Patterns.Scene.Loading
{
    public abstract class TS_SceneEntry : MonoBehaviour
    {
        protected AppCtx AppCtx;
        protected SceneCtx SceneCtx;

        public async UniTask LoadAsync(AppCtx appCtx, CancellationToken ct)
        {
            AppCtx = appCtx ?? throw new ArgumentNullException(nameof(appCtx));
            SceneCtx = new SceneCtx();

            SceneCtx.SetActive(false);

            await DeactivateAsync(ct);

            await PreconfigureSceneAsync(ct); // optional

            await InstantiateAsync(ct);
            await InitializeAsync(ct);
            await InjectAsync(SceneCtx, AppCtx, ct);
            await RegisterAsync(ct);

            SceneCtx.SetActive(true);

            await ConfigureAsync(ct);

            await PreActivationAsync(ct); // optional

            await ActivateAsync(ct);

            await PostActivationAsync(ct); // optional

            await LoadSceneAdditiveAsync(ct); // optional
            await UnLoadSceneAdditiveAsync(ct); // optional
        }

        protected abstract UniTask DeactivateAsync(CancellationToken ct);

        protected virtual UniTask PreconfigureSceneAsync(CancellationToken ct) => UniTask.CompletedTask;

        protected abstract UniTask InstantiateAsync(CancellationToken ct);
        protected abstract UniTask InitializeAsync(CancellationToken ct);
        protected abstract UniTask InjectAsync(SceneCtx sceneCtx, AppCtx appCtx, CancellationToken ct);
        protected abstract UniTask RegisterAsync(CancellationToken ct);
        protected abstract UniTask ConfigureAsync(CancellationToken ct);

        protected abstract UniTask ActivateAsync(CancellationToken ct);

        // optional
        protected virtual UniTask PreActivationAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask PostActivationAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask LoadSceneAdditiveAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask UnLoadSceneAdditiveAsync(CancellationToken ct) => UniTask.CompletedTask;
    }
}

