using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoffeeRush
{
    public sealed class UpgradeManager
    {
        private readonly IReadOnlyList<UpgradeData> catalog;
        private readonly EconomyService economy;
        private readonly Dictionary<string, int> levels = new Dictionary<string, int>();

        public UpgradeManager(IReadOnlyList<UpgradeData> catalog, EconomyService economy)
        {
            this.catalog = catalog;
            this.economy = economy;
        }

        public void Load(SaveData save)
        {
            levels.Clear();
            if (save?.upgrades == null) return;
            foreach (UpgradeSaveEntry entry in save.upgrades)
                if (entry != null && !string.IsNullOrEmpty(entry.upgradeId))
                    levels[entry.upgradeId] = Mathf.Max(0, entry.level);
        }

        public int GetLevel(UpgradeData data) =>
            data != null && levels.TryGetValue(data.UpgradeId, out int level) ? level : 0;

        public bool CanPurchase(UpgradeData data) =>
            data != null && GetLevel(data) < data.MaxLevel && economy.CanAfford(data.Cost);

        public bool TryPurchase(UpgradeData data)
        {
            if (!CanPurchase(data) || !economy.TrySpend(data.Cost)) return false;
            int level = GetLevel(data) + 1;
            levels[data.UpgradeId] = level;
            GameEvents.RaiseUpgradePurchased(data, level);
            GameEvents.RaiseNotification($"Улучшение куплено: {data.DisplayName}");
            return true;
        }

        public float GetPreparationMultiplier()
        {
            float reduction = catalog.Where(x => x != null && x.Effect == UpgradeEffect.FasterPreparation)
                .Sum(x => x.EffectValue * GetLevel(x));
            return Mathf.Clamp(1f - reduction, 0.25f, 1f);
        }

        public int GetBonusRewardPercent() =>
            catalog.Where(x => x != null && x.Effect == UpgradeEffect.BonusRewardPercent)
                .Sum(x => Mathf.RoundToInt(x.EffectValue * GetLevel(x)));

        public int GetExtraConcurrentOrders() =>
            catalog.Where(x => x != null && x.Effect == UpgradeEffect.ExtraConcurrentOrder)
                .Sum(x => Mathf.RoundToInt(x.EffectValue * GetLevel(x)));

        public int GetErrorShields() =>
            catalog.Where(x => x != null && x.Effect == UpgradeEffect.ErrorShield)
                .Sum(x => Mathf.RoundToInt(x.EffectValue * GetLevel(x)));

        public float GetExtraPatienceSeconds() =>
            catalog.Where(x => x != null && x.Effect == UpgradeEffect.LongerOrderPatience)
                .Sum(x => x.EffectValue * GetLevel(x));

        public System.Collections.Generic.List<UpgradeSaveEntry> ToSaveEntries()
        {
            return levels.Select(pair => new UpgradeSaveEntry(pair.Key, pair.Value)).ToList();
        }
    }
}
