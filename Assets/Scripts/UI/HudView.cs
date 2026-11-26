using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeRush
{
    public sealed class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text reputationText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text ordersText;
        [SerializeField] private Slider timerSlider;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField, Min(0.5f)] private float notificationDuration = 2f;
        private float notificationRemaining;

        private void OnEnable()
        {
            GameEvents.MoneyChanged += OnMoney;
            GameEvents.ReputationChanged += OnReputation;
            GameEvents.DayTimerChanged += OnTimer;
            GameEvents.OrdersChanged += OnOrders;
            GameEvents.Notification += OnNotification;
        }

        private void OnDisable()
        {
            GameEvents.MoneyChanged -= OnMoney;
            GameEvents.ReputationChanged -= OnReputation;
            GameEvents.DayTimerChanged -= OnTimer;
            GameEvents.OrdersChanged -= OnOrders;
            GameEvents.Notification -= OnNotification;
        }

        private void Update()
        {
            if (notificationRemaining > 0f && notificationText != null)
            {
                notificationRemaining -= Time.unscaledDeltaTime;
                if (notificationRemaining <= 0f) notificationText.text = string.Empty;
            }
        }
        
        public void Pause()
        {
            GameManager.Instance?.TogglePause();
        }

        private void OnMoney(int value) { if (moneyText != null) moneyText.text = $"Монеты: {value}"; }
        private void OnReputation(int value, int max) { if (reputationText != null) reputationText.text = $"Репутация: {value}/{max}"; }
        private void OnTimer(float value, float max)
        {
            if (timerText != null) timerText.text = $"{Mathf.CeilToInt(value / 60f):00}:{Mathf.CeilToInt(value % 60f):00}";
            if (timerSlider != null) { timerSlider.maxValue = max; timerSlider.value = value; }
        }
        private void OnOrders(int completed, int failed) { if (ordersText != null) ordersText.text = $"Успешно: {completed}   Ошибки: {failed}"; }
        private void OnNotification(string message)
        {
            if (notificationText == null) return;
            notificationText.text = message;
            notificationRemaining = notificationDuration;
        }
    }
}
