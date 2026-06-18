using System;
using System.Collections.Generic;

namespace KrolStudio
{
    [Serializable]
    public class BoardData : IDataModel
    {
        public List<BoardEntry> Entries = new();
    }

    [Serializable]
    public class BoardEntry
    {
        public PartType type;
        public int level;
        public int cellIndex;
    }
}