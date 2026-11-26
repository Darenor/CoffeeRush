using UnityEngine;

namespace CoffeeRush
{
    public sealed class StatePanel : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        [SerializeField] private GameState visibleState;
        [SerializeField] private bool includePausedOverlay;

        private void OnEnable()
        {
            GameEvents.StateChanged += OnStateChanged;

            if (GameManager.Instance != null)
                OnStateChanged(GameManager.Instance.State);
        }

        private void OnDisable()
        {
            GameEvents.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState state)
        {
            bool visible = state == visibleState ||
                (includePausedOverlay && state == GameState.Paused);

            if (content != null)
                content.SetActive(visible);
        }
    }
}