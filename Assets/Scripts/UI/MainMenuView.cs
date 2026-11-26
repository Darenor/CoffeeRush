using UnityEngine;

namespace CoffeeRush
{
    public sealed class MainMenuView : MonoBehaviour
    {
        public void Play() => GameManager.Instance?.StartNewRun();
        public void OpenUpgrades() => GameManager.Instance?.OpenUpgrades();
        public void OpenSettings() => GameManager.Instance?.OpenSettings();
        public void Quit() => GameManager.Instance?.QuitToDesktop();
    }
}
