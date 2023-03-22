using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace UKDamageNumbers
{
    public class ColorSetter : MonoBehaviour
    {

        private static float maxSize = 3f;

        public Color color;
        public float sizeMultiplier = 1f;

        public Slider red;
        public Slider green;
        public Slider blue;      
        public Slider alpha;
        public Slider size;

        public Image preview;


        private void Start()
        {
            Color config = Config.damageNumberColor.Value;
            float configSize = Config.damageNumberSize.Value;

            // BrainDef : ScriptableObject
            this.red.value = config.r;
            this.green.value = config.g;
            this.blue.value = config.b;
            this.alpha.value = config.a;
            this.size.value = configSize;

            this.color = config;

            red.onValueChanged.AddListener(SetRed);
            green.onValueChanged.AddListener(SetGreen);
            blue.onValueChanged.AddListener(SetBlue);
            alpha.onValueChanged.AddListener(SetAlpha);
            size.onValueChanged.AddListener(SetSize);
            size.maxValue = maxSize;

            preview = base.transform.Find("Image").GetComponent<Image>();

            this.UpdateColor();
        }

        public void SetRed(float amount)
        {
            color.r = amount;
            UpdateColor();
        }
        public void SetGreen(float amount)
        {
            color.g = amount;
            UpdateColor();
        }
        public void SetBlue(float amount)
        {
            color.b = amount;
            UpdateColor();
        }
        public void SetAlpha(float amount)
        {
            color.a = amount;
            UpdateColor();
        }
        public void SetSize(float amount)
        {
            sizeMultiplier = amount;
            UpdateColor();
        }


        private void UpdateColor()
        {
            preview.color = this.color;
            preview.transform.localScale = Vector3.one * sizeMultiplier;
            Config.damageNumberSize.Value = this.sizeMultiplier;
            Config.damageNumberColor.Value = this.color;
        }
    }
}
