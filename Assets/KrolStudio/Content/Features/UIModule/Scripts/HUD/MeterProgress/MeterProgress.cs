using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class MeterProgress : MonoBehaviour, IDisplayable
    {
        [SerializeField] TextMeshProUGUI meterText;
        [SerializeField] Slider meterSlider;

        public void SetProgressView(float value) =>
             meterSlider.value = value;

        public void SetTraveledDistance(float meter) =>
            meterText.text = $"{Mathf.FloorToInt(meter)}m";

        public void Display(bool value)
        {
            if (this == null || gameObject == null) return;
            gameObject.SetActive(value);
        }
    }
}