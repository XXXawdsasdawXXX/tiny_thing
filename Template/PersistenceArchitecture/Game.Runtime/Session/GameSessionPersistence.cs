using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Persistence.Meta;
using Game.Persistence.Profile;
using Game.Runtime.Session;

namespace Game.Runtime.Session
{
    public sealed class GameSessionPersistence
    {
        private readonly ProfileRepository _profiles;
        private readonly SaveMetaRepository _meta;

        public GameSessionPersistence(ProfileRepository profiles, SaveMetaRepository meta)
        {
            _profiles = profiles;
            _meta = meta;
        }

        public async UniTask LoadIntoAsync(
            GameSessionState session,
            string slotId,
            CancellationToken cancellationToken = default)
        {
            ProfileSaveData? data = await _profiles.LoadAsync(slotId, cancellationToken);
            data ??= ProfileSaveData.CreateNew("Hero");

            session.LoadFrom(slotId, data);

            SaveMeta meta = await _meta.LoadOrDefaultAsync(cancellationToken);
            meta.LastSlotId = slotId;
            await _meta.SaveAsync(meta, cancellationToken);
        }

        public async UniTask<GameSessionState> LoadAsync(
            string slotId,
            CancellationToken cancellationToken = default)
        {
            GameSessionState session = new();
            await LoadIntoAsync(session, slotId, cancellationToken);
            return session;
        }

        public async UniTask SaveAsync(
            GameSessionState session,
            CancellationToken cancellationToken = default)
        {
            await _profiles.SaveAsync(session.SlotId, session.ToSaveData(), cancellationToken);

            SaveMeta meta = await _meta.LoadOrDefaultAsync(cancellationToken);
            meta.LastSlotId = session.SlotId;
            await _meta.SaveAsync(meta, cancellationToken);
        }

        public async UniTask<GameSessionState> LoadLastAsync(CancellationToken cancellationToken = default)
        {
            SaveMeta meta = await _meta.LoadOrDefaultAsync(cancellationToken);
            return await LoadAsync(meta.LastSlotId, cancellationToken);
        }

        public async UniTask<GameSessionState> CreateNewAsync(
            string slotId,
            string heroName,
            CancellationToken cancellationToken = default)
        {
            GameSessionState session = new();
            session.LoadFrom(slotId, ProfileSaveData.CreateNew(heroName));
            await SaveAsync(session, cancellationToken);
            return session;
        }
    }
}
