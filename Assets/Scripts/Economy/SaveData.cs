using System;
using System.Collections.Generic;

namespace CoffeeRush
{
    [Serializable]
    public sealed class SaveData
    {
        public int wallet;
        public int bestDayRevenue;
        public int bestOrdersCompleted;
        public int currentDay = 1;
        public float masterVolume = 1f;
        public List<UpgradeSaveEntry> upgrades = new List<UpgradeSaveEntry>();
    }

    [Serializable]
    public sealed class UpgradeSaveEntry
    {
        public string upgradeId;
        public int level;
        public UpgradeSaveEntry(string id, int value) { upgradeId = id; level = value; }
    }
}
