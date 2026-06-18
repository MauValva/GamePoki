using System;
using System.Collections.Generic;

namespace KrolStudio
{
    [Serializable]
    public class PlayerData : IDataModel
    {
        public List<PartTypeSettings> Entries = new();

        public PlayerData(List<PartTypeSettings> entries)
        {
            Entries = entries;
        }
    }

    [Serializable]
    public class PlayerEntry
    {
        public PartType type;
        public int level;
    }
}