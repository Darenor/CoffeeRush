using System;
using System.IO;
using UnityEngine;

namespace CoffeeRush
{
    public sealed class SaveService
    {
        private const string BackupKey = "CoffeeRush_Save_Backup";
        private string FilePath => Path.Combine(Application.persistentDataPath, "coffee_rush_save.json");

        public SaveData Load(int fallbackWallet)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    SaveData fromFile = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
                    if (fromFile != null) return Normalize(fromFile, fallbackWallet);
                }
            }
            catch (Exception e) { Debug.LogWarning($"Save file load failed: {e.Message}"); }

            try
            {
                string backup = PlayerPrefs.GetString(BackupKey, string.Empty);
                if (!string.IsNullOrEmpty(backup))
                {
                    SaveData fromPrefs = JsonUtility.FromJson<SaveData>(backup);
                    if (fromPrefs != null) return Normalize(fromPrefs, fallbackWallet);
                }
            }
            catch (Exception e) { Debug.LogWarning($"PlayerPrefs backup load failed: {e.Message}"); }

            return new SaveData { wallet = fallbackWallet };
        }

        public void Save(SaveData data)
        {
            if (data == null) return;
            string json = JsonUtility.ToJson(data, true);
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                string temp = FilePath + ".tmp";
                File.WriteAllText(temp, json);
                if (File.Exists(FilePath)) File.Delete(FilePath);
                File.Move(temp, FilePath);
            }
            catch (Exception e) { Debug.LogWarning($"JSON save failed: {e.Message}"); }

            // Backup is useful for browser builds and platforms with restricted file access.
            PlayerPrefs.SetString(BackupKey, json);
            PlayerPrefs.Save();
        }

        public void DeleteSave()
        {
            try { if (File.Exists(FilePath)) File.Delete(FilePath); }
            catch (Exception e) { Debug.LogWarning($"Save deletion failed: {e.Message}"); }
            PlayerPrefs.DeleteKey(BackupKey);
            PlayerPrefs.Save();
        }

        private static SaveData Normalize(SaveData data, int fallbackWallet)
        {
            if (data.wallet < 0) data.wallet = fallbackWallet;
            if (data.upgrades == null) data.upgrades = new System.Collections.Generic.List<UpgradeSaveEntry>();
            data.currentDay = Math.Max(1, data.currentDay);
            data.masterVolume = UnityEngine.Mathf.Clamp01(data.masterVolume);
            return data;
        }
    }
}
