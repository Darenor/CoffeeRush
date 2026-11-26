using CoffeeRush;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class UpgradeRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text level;
    [SerializeField] private Button buyButton;
    private UpgradeData data;

    public void Bind(UpgradeData upgrade, int currentLevel, bool canBuy, System.Action<UpgradeData> onBuy)
    {
        data = upgrade;
        if (title != null) title.text = upgrade.DisplayName;
        if (description != null) description.text = upgrade.Description;
        if (cost != null) cost.text = $"Цена: {upgrade.Cost}";
        if (level != null) level.text = $"Уровень: {currentLevel}/{upgrade.MaxLevel}";
        if (buyButton != null)
        {
            buyButton.interactable = canBuy;
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => onBuy?.Invoke(data));
        }
    }
}
