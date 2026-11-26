using UnityEngine;

namespace CoffeeRush
{
    public sealed class EconomyService
    {
        public int Wallet { get; private set; }
        public int EarnedToday { get; private set; }
        public int BestDayRevenue { get; private set; }
        public int BestOrdersCompleted { get; private set; }
        public int OrdersCompleted { get; private set; }
        public int OrdersFailed { get; private set; }

        public void Initialize(SaveData save)
        {
            Wallet = Mathf.Max(0, save.wallet);
            BestDayRevenue = Mathf.Max(0, save.bestDayRevenue);
            BestOrdersCompleted = Mathf.Max(0, save.bestOrdersCompleted);
            GameEvents.RaiseMoneyChanged(Wallet);
        }

        public bool CanAfford(int cost) => cost >= 0 && Wallet >= cost;

        public bool TrySpend(int amount)
        {
            if (!CanAfford(amount)) return false;
            Wallet -= amount;
            GameEvents.RaiseMoneyChanged(Wallet);
            return true;
        }

        public void BeginDay()
        {
            EarnedToday = 0;
            OrdersCompleted = 0;
            OrdersFailed = 0;
        }

        public void RewardOrder(int baseReward, int bonusPercent)
        {
            int reward = Mathf.Max(0, Mathf.RoundToInt(baseReward * (1f + bonusPercent / 100f)));
            Wallet += reward;
            EarnedToday += reward;
            OrdersCompleted++;
            BestDayRevenue = Mathf.Max(BestDayRevenue, EarnedToday);
            BestOrdersCompleted = Mathf.Max(BestOrdersCompleted, OrdersCompleted);
            GameEvents.RaiseMoneyChanged(Wallet);
            GameEvents.RaiseOrdersChanged(OrdersCompleted, OrdersFailed);
        }

        public void RegisterFailure()
        {
            OrdersFailed++;
            GameEvents.RaiseOrdersChanged(OrdersCompleted, OrdersFailed);
        }
    }
}
