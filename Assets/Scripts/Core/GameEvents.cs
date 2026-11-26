using System;

namespace CoffeeRush
{
    /// <summary>Observer/event hub. UI observes these events; it does not own game rules.</summary>
    public static class GameEvents
    {
        public static event Action<int> MoneyChanged;
        public static event Action<int, int> ReputationChanged;
        public static event Action<float, float> DayTimerChanged;
        public static event Action<int, int> OrdersChanged;
        public static event Action<Order> OrderAdded;
        public static event Action<string> OrderRemoved;
        public static event Action<string> Notification;
        public static event Action<GameState> StateChanged;
        public static event Action<UpgradeData, int> UpgradePurchased;
        public static event Action<DaySummary> DayFinished;

        public static void RaiseMoneyChanged(int money) => MoneyChanged?.Invoke(money);
        public static void RaiseReputationChanged(int current, int max) => ReputationChanged?.Invoke(current, max);
        public static void RaiseDayTimerChanged(float current, float max) => DayTimerChanged?.Invoke(current, max);
        public static void RaiseOrdersChanged(int completed, int total) => OrdersChanged?.Invoke(completed, total);
        public static void RaiseOrderAdded(Order order) => OrderAdded?.Invoke(order);
        public static void RaiseOrderRemoved(string id) => OrderRemoved?.Invoke(id);
        public static void RaiseNotification(string message) => Notification?.Invoke(message);
        public static void RaiseStateChanged(GameState state) => StateChanged?.Invoke(state);
        public static void RaiseUpgradePurchased(UpgradeData data, int level) => UpgradePurchased?.Invoke(data, level);
        public static void RaiseDayFinished(DaySummary summary) => DayFinished?.Invoke(summary);

        // Useful for play-mode tests / domain reloads.
        public static void ClearAll()
        {
            MoneyChanged = null; ReputationChanged = null; DayTimerChanged = null;
            OrdersChanged = null; OrderAdded = null; OrderRemoved = null;
            Notification = null; StateChanged = null; UpgradePurchased = null;
            DayFinished = null;
        }
    }

    public struct DaySummary
    {
        public int DayNumber;
        public int EarnedToday;
        public int OrdersCompleted;
        public int OrdersFailed;
        public int Reputation;
        public bool GoalReached;
        public bool OutOfTime;

        public DaySummary(int day, int earned, int completed, int failed,
            int reputation, bool goalReached, bool outOfTime)
        {
            DayNumber = day; EarnedToday = earned; OrdersCompleted = completed;
            OrdersFailed = failed; Reputation = reputation;
            GoalReached = goalReached; OutOfTime = outOfTime;
        }
    }
}
