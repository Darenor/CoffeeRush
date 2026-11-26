using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeRush
{
    public sealed class UpgradeShopView : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private UpgradeRowView rowPrefab;
        [SerializeField] private List<UpgradeData> catalog = new List<UpgradeData>();

        private void OnEnable() => Refresh();

        public void Refresh()
        {
            if (content == null || rowPrefab == null || GameManager.Instance == null) return;
            foreach (Transform child in content) Destroy(child.gameObject);

            foreach (UpgradeData item in catalog)
            {
                if (item == null) continue;
                int level = GameManager.Instance.Upgrades.GetLevel(item);
                UpgradeRowView row = Instantiate(rowPrefab, content);
                row.Bind(item, level, GameManager.Instance.Upgrades.CanPurchase(item),
                    data => { GameManager.Instance.PurchaseUpgrade(data); Refresh(); });
            }
        }

        public void ContinueDay() => GameManager.Instance?.StartNewDay();
        public void MainMenu() => GameManager.Instance?.ReturnToMenu();
    }
}
