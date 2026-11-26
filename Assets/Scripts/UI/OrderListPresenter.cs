using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoffeeRush
{
    public sealed class OrderListPresenter : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private OrderCardView cardPrefab;
        [SerializeField] private BrewController brewController;
        private readonly Dictionary<string, OrderCardView> cards = new Dictionary<string, OrderCardView>();
        private OrderCardView selectedCard;

        private void OnEnable()
        {
            GameEvents.OrderAdded += OnOrderAdded;
            GameEvents.OrderRemoved += OnOrderRemoved;
        }

        private void OnDisable()
        {
            GameEvents.OrderAdded -= OnOrderAdded;
            GameEvents.OrderRemoved -= OnOrderRemoved;
        }
        
        private void OnOrderAdded(Order order)
        {
            if (content == null || cardPrefab == null || order == null) return;
            OrderCardView view = Instantiate(cardPrefab, content);
            float patience = GameManager.Instance.Config.OrderPatienceSeconds +
                GameManager.Instance.Upgrades.GetExtraPatienceSeconds();
            view.Bind(order, patience);
            view.SetPresenter(this);
            cards[order.Id] = view;
        }

        private void OnOrderRemoved(string id)
        {
            if (!cards.TryGetValue(id, out OrderCardView view))
                return;

            bool wasSelected = selectedCard == view;

            if (view != null)
                Destroy(view.gameObject);

            cards.Remove(id);

            if (wasSelected)
            {
                selectedCard = null;

                if (cards.Count > 0)
                {
                    foreach (var pair in cards)
                    {
                        SelectOrder(pair.Value);
                        break;
                    }
                }
                else if (brewController != null)
                {
                    brewController.SetSelectedOrder(null);
                }
            }
        }

        public void SelectOrder(OrderCardView card)
        {
            if (card == null) return;

            if (selectedCard != null)
                selectedCard.SetSelected(false);

            selectedCard = card;
            selectedCard.SetSelected(true);

            if (brewController != null)
                brewController.SetSelectedOrder(card);
        }
    }
}
