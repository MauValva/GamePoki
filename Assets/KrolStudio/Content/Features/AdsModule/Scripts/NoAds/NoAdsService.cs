using UnityEngine;

namespace KrolStudio
{
    public class NoAdsService : INoAdsService
    {
        private const string SaveKey = "no_ads";

        public bool IsPurchased => PlayerPrefs.GetInt(SaveKey, 0) == 1;

        public void SetPurchased()
        {
            PlayerPrefs.SetInt(SaveKey, 1);
            PlayerPrefs.Save();
            Debug.Log("[NoAds] Purchased.");
        }
    }
}