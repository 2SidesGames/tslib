using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Debug.Logging;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Patterns.Scene.Loading
{
    public abstract class TS_AppEntry : MonoBehaviour
    {
        protected AppCtx AppCtx;

        private async void Start()
        {
            var ct = this.GetCancellationTokenOnDestroy();

            try
            {
                ConfigureApp();
                CreateAppContext();

                AppCtx.GlobalCtx.SetActive(false);
                AppCtx.UtilityCtx.SetActive(false);

                await InstantiateAsync(ct);
                await InitializeAsync(ct);
                await InjectAsync(AppCtx, ct);
                await RegisterAsync(ct);

                AppCtx.GlobalCtx.SetActive(true);
                AppCtx.UtilityCtx.SetActive(true);

                await ConfigureAsync(ct);

                // optional
                await ExecuteCustomOperationsAsync(ct);

                await LoadSceneAdditiveAsync(ct);

                if (ct.IsCancellationRequested) return;

                var scene = gameObject.scene;
                if (!scene.IsValid() || !scene.isLoaded)
                    throw new InvalidOperationException("(not loaded) scene was not loaded");

                // persistent, it acts as the container of app context.
                DontDestroyOnLoad(gameObject);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                OnTokenCanceled(); // could be ignored
            }
            catch (Exception ex)
            {
                TSLogger.LogException(ex, this);
            }
        }

        protected abstract void ConfigureApp();
        protected void CreateAppContext()
        {
            AppCtx = new AppCtx
            {
                UtilityCtx = new UtilityCtx(),
                GlobalCtx = new SharedCtx()
            };
        }
        protected abstract UniTask InstantiateAsync(CancellationToken ct);
        protected abstract UniTask InitializeAsync(CancellationToken ct);
        protected abstract UniTask InjectAsync(AppCtx appCtx, CancellationToken ct);
        protected abstract UniTask RegisterAsync(CancellationToken ct);
        protected abstract UniTask ConfigureAsync(CancellationToken ct);

        protected abstract UniTask LoadSceneAdditiveAsync(CancellationToken ct);

        // optional
        protected virtual UniTask ExecuteCustomOperationsAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual void OnTokenCanceled() { }
    }
}
