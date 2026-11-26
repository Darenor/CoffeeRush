using UnityEngine;
using UnityEngine.UI;

namespace CoffeeRush
{
    public sealed class SettingsView : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;

        private void Start()
        {
            if (volumeSlider != null)
            {
                volumeSlider.SetValueWithoutNotify(AudioListener.volume);
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            }
        }

        private void OnDestroy()
        {
            if (volumeSlider != null)
                volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        }

        private void OnVolumeChanged(float value) =>
            GameManager.Instance?.SetVolume(value);

        public void Back()
        {
            GameManager.Instance?.CloseSettings();
        }
    }
}
