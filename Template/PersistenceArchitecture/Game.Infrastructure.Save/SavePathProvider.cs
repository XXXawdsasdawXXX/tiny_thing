using System.IO;
using UnityEngine;

namespace Game.Infrastructure.Save
{
    public sealed class SavePathProvider
    {
        public string RootDirectory { get; }

        public SavePathProvider()
        {
            RootDirectory = Path.Combine(Application.persistentDataPath, "save");
        }

        public string SettingsFile => "settings.json";

        public string MetaFile => "meta.json";

        public string SlotFile(string slotId) => Path.Combine("slots", $"{slotId}.json");

        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(RootDirectory, relativePath);
        }

        public void EnsureRootExists()
        {
            Directory.CreateDirectory(RootDirectory);
            Directory.CreateDirectory(Path.Combine(RootDirectory, "slots"));
        }
    }
}
