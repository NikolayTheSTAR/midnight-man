using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image fillImg;
    [SerializeField] private TextMeshProUGUI valueLabel;

    public void Set(int currentValue, int maxValue)
    {
        valueLabel.text = $"{currentValue}/{maxValue}";
        fillImg.fillAmount = (float)currentValue / (float)maxValue;
    }
}