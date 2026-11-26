using UnityEngine;

namespace CoffeeRush
{
    public sealed class PauseView : MonoBehaviour
    {
        public void Resume() => GameManager.Instance?.TogglePause();
        public void OpenSettings() => GameManager.Instance?.OpenSettings();
        public void MainMenu() => GameManager.Instance?.ReturnToMenu();
    }
}
