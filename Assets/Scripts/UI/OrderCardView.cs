using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeRush
{
    public sealed class OrderCardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text customerNameText;
        [SerializeField] private TMP_Text recipeText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Image selectionHighlight;
        private OrderListPresenter presenter;

        private Order boundOrder;
        private float patienceMax;

        public string OrderId => boundOrder?.Id;
        public string CustomerName => "Посетитель";
        public string RecipeName => boundOrder?.Recipe?.DisplayName;
        
        
        public void SetPresenter(OrderListPresenter value)
        {
            presenter = value;
        }

        public void Select()
        {
            if (presenter == null)
            {
                Debug.LogError("OrderListPresenter не назначен!");
                return;
            }

            presenter.SelectOrder(this);
        }
        
        public void SetSelected(bool value)
        {
            if (selectionHighlight != null)
                selectionHighlight.enabled = value;
        }
        
        public void Bind(Order order, float maxPatience)
        {
            boundOrder = order;
            patienceMax = Mathf.Max(1f, maxPatience);
            if (customerNameText != null) customerNameText.text = "Посетитель";
            if (recipeText != null) recipeText.text = order.Recipe.DisplayName;
            if (timerSlider != null) timerSlider.maxValue = patienceMax;
            UpdateTimer();
            SetSelected(false);
        }
        
        private void Update()
        {
            if (boundOrder == null) return;
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (timerText != null) timerText.text = $"{Mathf.CeilToInt(boundOrder.RemainingSeconds)} сек.";
            if (timerSlider != null) timerSlider.value = boundOrder.RemainingSeconds;
        }
    }
}
