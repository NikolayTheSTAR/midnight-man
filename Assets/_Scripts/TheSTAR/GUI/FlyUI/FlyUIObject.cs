using UnityEngine;
using UnityEngine.UI;

namespace TheSTAR.GUI
{
    public class FlyUIObject : MonoBehaviour
    {
        [SerializeField] private Image iconImg;

        public void SetIcon(Sprite iconSprite)
        {
            iconImg.sprite = iconSprite;
        }
    }
}