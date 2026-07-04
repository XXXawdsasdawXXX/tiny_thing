using Game.Infrastructure.Save;
using Game.Persistence.Meta;
using Game.Persistence.Profile;
using Game.Persistence.Settings;
using Game.Runtime.Session;
using Game.Runtime.Settings;
using VContainer;
using VContainer.Unity;

namespace Game.Composition
{
    public static class SaveInstaller
    {
        public static void RegisterSaveInfrastructure(IContainerBuilder builder)
        {
            builder.Register<SavePathProvider>(Lifetime.Singleton);
            builder.Register<FileSaveStorage>(Lifetime.Singleton).As<ISaveStorage>();
            builder.Register<JsonSaveSerializer>(Lifetime.Singleton).As<ISaveSerializer>();

            builder.Register<SettingsRepository>(Lifetime.Singleton);
            builder.Register<SaveMetaRepository>(Lifetime.Singleton);
            builder.Register<ProfileRepository>(Lifetime.Singleton);

            builder.Register<SettingsService>(Lifetime.Singleton);
            builder.Register<GameSessionPersistence>(Lifetime.Singleton);
        }

        public static void RegisterSessionStarter(
            IContainerBuilder builder,
            GameSessionLifetimeScope sessionScopePrefab)
        {
            builder.RegisterInstance(sessionScopePrefab);
            builder.Register<GameSessionStarter>(Lifetime.Singleton);
        }
    }
}
