using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _NueCore.SettingsSystem
{
    public class SettingsSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text percentTextField;
        [SerializeField] private Image activeImage;
        [SerializeField] private Image deActiveImage;
        
        
        public Slider Slider => slider;

        public void Build(float currentValue,Action<float> onValueChanged)
        {
            slider.onValueChanged.AddListener(value=> { UpdateSlider(onValueChanged, value); });
            slider.value = currentValue;
            onValueChanged?.Invoke(currentValue);
            UpdateSliderValue(currentValue);
        }

        private void UpdateSlider(Action<float> onValueChanged, float value)
        {
            onValueChanged?.Invoke(value);
            UpdateSliderValue(value);
        }

        public void UpdateSliderValue(float value)
        {
            if (percentTextField)
            {
                percentTextField.SetText($"{value}%");
            }

            if (value<=0)
            {
                deActiveImage.gameObject.SetActive(true);
                activeImage.gameObject.SetActive(false);
            }
            else
            {
                deActiveImage.gameObject.SetActive(false);
                activeImage.gameObject.SetActive(true);
            }
        }
    }
}