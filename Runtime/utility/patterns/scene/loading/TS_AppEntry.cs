using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Debug.Logging;
using TSLib.Utility.Patterns.Scene.Contexts;

namespace TSLib.Utility.Patterns.Scene.Loading
{
    public abstract class TS_AppEntry : TS_SceneContainer
    {
        protected AppCtx AppCtx;

        private async void Start()
        {
            var ct = this.GetCancellationTokenOnDestroy();

            CreateSceneContainer();

            try
            {
                ConfigureApp();
                CreateAppContext();

                AppCtx.GlobalCtx.SetActive(false);
                AppCtx.UtilityCtx.SetActive(false);

                await DeactivateAsync(ct);
                await InstantiateAsync(ct);
                await InitializeAsync(ct);
                await InjectAsync(ct);
                await RegisterAsync(ct);

                AppCtx.GlobalCtx.SetActive(true);
                AppCtx.UtilityCtx.SetActive(true);

                await ConfigureAsync(ct);

                await PreActivationAsync(ct); // optional

                await ActivateAsync(ct);

                await PostActivationAsync(ct); // optional

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
                OnTokenCanceled(); // could be ignored // optional
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

        protected abstract UniTask DeactivateAsync(CancellationToken ct);
        protected abstract UniTask InstantiateAsync(CancellationToken ct);
        protected abstract UniTask InitializeAsync(CancellationToken ct);
        protected abstract UniTask InjectAsync(CancellationToken ct);
        protected abstract UniTask RegisterAsync(CancellationToken ct);
        protected abstract UniTask ConfigureAsync(CancellationToken ct);

        protected abstract UniTask ActivateAsync(CancellationToken ct);

        protected abstract UniTask LoadSceneAdditiveAsync(CancellationToken ct);

        // optional
        protected virtual UniTask PreActivationAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask PostActivationAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual void OnTokenCanceled() { }
    }
}
