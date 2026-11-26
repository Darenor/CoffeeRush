using UnityEngine;

namespace CoffeeRush
{
    public sealed class DayController
    {
        private readonly GameConfig config;
        private readonly OrderManager orders;
        private readonly EconomyService economy;
        private readonly ReputationService reputation;
        private float remaining;
        private float spawnTimer;
        private bool running;
        private int dayNumber;

        public float Remaining => remaining;
        public float Duration => config.DayDurationSeconds;
        public int DayNumber => dayNumber;
        public bool IsRunning => running;

        public DayController(GameConfig config, OrderManager orders, EconomyService economy,
            ReputationService reputation)
        {
            this.config = config;
            this.orders = orders;
            this.economy = economy;
            this.reputation = reputation;
        }

        public void StartDay(int number)
        {
            dayNumber = Mathf.Max(1, number);
            remaining = config.DayDurationSeconds;
            spawnTimer = 1f;
            running = true;
            economy.BeginDay();
            orders.BeginDay();
            GameEvents.RaiseDayTimerChanged(remaining, Duration);
            GameEvents.RaiseOrdersChanged(0, 0);
        }

        public void Tick(float deltaTime)
        {
            if (!running) return;
            remaining = Mathf.Max(0f, remaining - deltaTime);
            spawnTimer -= deltaTime;
            if (spawnTimer <= 0f)
            {
                orders.TryCreateOrder();
                spawnTimer = config.SpawnIntervalSeconds;
            }

            orders.Tick(deltaTime);
            GameEvents.RaiseDayTimerChanged(remaining, Duration);

            if (reputation.Current <= 0)
            {
                Finish(false, false);
                return;
            }
            if (economy.EarnedToday >= config.TargetMoneyPerDay)
            {
                Finish(true, false);
                return;
            }
            if (remaining <= 0f)
                Finish(false, true);
        }

        private void Finish(bool goalReached, bool outOfTime)
        {
            if (!running) return;
            running = false;
            orders.ClearAll();
            var summary = new DaySummary(dayNumber, economy.EarnedToday, economy.OrdersCompleted,
                economy.OrdersFailed, reputation.Current, goalReached, outOfTime);
            GameEvents.RaiseDayFinished(summary);
        }
    }
}
