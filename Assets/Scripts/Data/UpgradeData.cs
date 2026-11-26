using UnityEngine;

namespace CoffeeRush
{
    public enum UpgradeEffect
    {
        FasterPreparation,
        BonusRewardPercent,
        ExtraConcurrentOrder,
        ErrorShield,
        LongerOrderPatience
    }

    [CreateAssetMenu(fileName = "Upgrade_New", menuName = "Coffee Rush/Upgrade")]
    public sealed class UpgradeData : ScriptableObject
    {
        [SerializeField] private string upgradeId;
        [SerializeField] private string displayName;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField, Min(0)] private int cost = 100;
        [SerializeField] private UpgradeEffect effect;
        [SerializeField] private float effectValue = 1f;
        [SerializeField, Min(1)] private int maxLevel = 1;

        public string UpgradeId => upgradeId;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public int Cost => cost;
        public UpgradeEffect Effect => effect;
        public float EffectValue => effectValue;
        public int MaxLevel => maxLevel;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(upgradeId))
                upgradeId = name.Trim().ToLowerInvariant().Replace(" ", "_");
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = name;
        }
#endif
    }
}
