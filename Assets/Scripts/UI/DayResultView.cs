using TMPro;
using UnityEngine;

namespace CoffeeRush
{
    public sealed class DayResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text summaryText;

        private void OnEnable() => GameEvents.DayFinished += OnDayFinished;
        private void OnDisable() => GameEvents.DayFinished -= OnDayFinished;

        private void OnDayFinished(DaySummary s)
        {
            if (titleText != null)
                titleText.text = s.Reputation <= 0 ? "Кофейня закрыта" :
                    s.GoalReached ? "День завершён!" : "Смена окончена";
            if (summaryText != null)
                summaryText.text = $"День: {s.DayNumber}\nВыручка: {s.EarnedToday}\nЗаказов выполнено: {s.OrdersCompleted}\nОшибок: {s.OrdersFailed}\nРепутация: {s.Reputation}";
            gameObject.SetActive(true);
        }

        public void ContinueToUpgrades() => GameManager.Instance?.OpenUpgrades();
        public void BackToMenu() => GameManager.Instance?.ReturnToMenu();
        public void Retry() => GameManager.Instance?.StartNewRun();
    }
}
