using UnityEngine;

namespace CoffeeRush
{
    /// <summary>Optional tiny helper to apply persisted master volume early.</summary>
    public sealed class AudioSettingsBootstrap : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float fallbackVolume = 1f;
        private void Awake()
        {
            float saved = PlayerPrefs.GetFloat("CoffeeRush_MasterVolume", fallbackVolume);
            AudioListener.volume = Mathf.Clamp01(saved);
        }
    }
}
