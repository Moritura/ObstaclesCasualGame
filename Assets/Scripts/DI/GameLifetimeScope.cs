using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{

    [SerializeField] private GameWorld _gameWorld;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Player>(Lifetime.Singleton)
            .AsSelf()
            .AsImplementedInterfaces()
            .WithParameter("view", _gameWorld.PlayerView);

        builder.Register<Gate>(Lifetime.Singleton)
            .AsSelf()
            .AsImplementedInterfaces()
            .WithParameter("view", _gameWorld.GateView);

        builder.Register<UserInput>(Lifetime.Singleton)
            .AsSelf()
            .AsImplementedInterfaces();

        builder.Register<Map>(Lifetime.Singleton)
            .AsSelf()
            .AsImplementedInterfaces();

        builder.RegisterEntryPoint<Path>(Lifetime.Scoped)
            .AsSelf()
            .AsImplementedInterfaces()
            .WithParameter("view", _gameWorld.PathView);

        builder.RegisterEntryPoint<Game>(Lifetime.Scoped)
            .AsSelf()
            .AsImplementedInterfaces()
            .WithParameter("view", _gameWorld.GameView);

        builder.RegisterEntryPointExceptionHandler(ex =>
        {
            Debug.LogError("Entry point exception: " + ex);
        });
    }
}
