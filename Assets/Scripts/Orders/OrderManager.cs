using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoffeeRush
{
    public sealed class OrderManager
    {
        private readonly List<RecipeData> recipes;
        private readonly OrderFactory factory;
        private readonly GameConfig config;
        private readonly EconomyService economy;
        private readonly UpgradeManager upgrades;
        private readonly List<Order> activeOrders = new List<Order>();
        private int errorShieldsRemaining;

        public IReadOnlyList<Order> ActiveOrders => activeOrders;
        public int MaxConcurrentOrders => config.BaseMaxConcurrentOrders + upgrades.GetExtraConcurrentOrders();

        public OrderManager(List<RecipeData> recipes, OrderFactory factory, GameConfig config,
            EconomyService economy, UpgradeManager upgrades)
        {
            this.recipes = recipes;
            this.factory = factory;
            this.config = config;
            this.economy = economy;
            this.upgrades = upgrades;
        }

        public void BeginDay() => errorShieldsRemaining = upgrades.GetErrorShields();

        public Order TryCreateOrder()
        {
            if (activeOrders.Count >= MaxConcurrentOrders) return null;
            float patience = config.OrderPatienceSeconds + upgrades.GetExtraPatienceSeconds();
            Order order = factory.Create(recipes, patience);
            activeOrders.Add(order);
            GameEvents.RaiseOrderAdded(order);
            return order;
        }

        public void Tick(float deltaTime)
        {
            for (int i = activeOrders.Count - 1; i >= 0; i--)
            {
                Order order = activeOrders[i];
                order.Tick(deltaTime);
                if (order.RemainingSeconds <= 0f)
                {
                    ResolveAsExpired(order);
                }
            }
        }

        public bool TryServe(string orderId, IReadOnlyList<IngredientType> selectedIngredients)
        {
            Order order = activeOrders.FirstOrDefault(x => x.Id == orderId);
            if (order == null || order.IsResolved) return false;

            if (!RecipeValidator.IsCorrect(selectedIngredients, order.Recipe))
            {
                RegisterFailure(config.WrongOrderReputationPenalty, "Неверный напиток! Репутация снижена.");
                Remove(order);
                return false;
            }

            if (!order.MarkResolved()) return false;
            int bonus = upgrades.GetBonusRewardPercent();
            economy.RewardOrder(order.Recipe.BaseReward, bonus);
            GameEvents.RaiseNotification($"+{Mathf.RoundToInt(order.Recipe.BaseReward * (1f + bonus / 100f))} монет: {order.Recipe.DisplayName}");
            Remove(order);
            return true;
        }

        private void ResolveAsExpired(Order order)
        {
            if (!order.MarkResolved()) return;
            RegisterFailure(config.ExpiredOrderReputationPenalty, "Посетитель не дождался заказа!");
            Remove(order);
        }

        private void RegisterFailure(int penalty, string message)
        {
            economy.RegisterFailure();
            if (errorShieldsRemaining > 0)
            {
                errorShieldsRemaining--;
                GameEvents.RaiseNotification("Улучшение защитило репутацию от штрафа.");
            }
            else
            {
                ReputationService.Instance?.ApplyPenalty(penalty);
                GameEvents.RaiseNotification(message);
            }
        }

        private void Remove(Order order)
        {
            activeOrders.Remove(order);
            GameEvents.RaiseOrderRemoved(order.Id);
        }

        public void ClearAll()
        {
            foreach (Order order in activeOrders.ToArray())
                GameEvents.RaiseOrderRemoved(order.Id);
            activeOrders.Clear();
        }
    }
}
