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
        public Slider mult;

        public Button next;
        public Button prev;

        public RawImage preview;

        public Material[] fonts;
        public int currentFontIndex;

        public bool stackDamage;
        public int damageMultiplier;

        private void Start()
        {
            Color config = Config.damageNumberColor.Value;
            float configSize = Config.damageNumberSize.Value;
            this.currentFontIndex = Config.damageNumberFont.Value;
            this.damageMultiplier = Config.damageNumberMultiplier.Value;

            // BrainDef : ScriptableObject
            this.red.value = config.r;
            this.green.value = config.g;
            this.blue.value = config.b;
            this.alpha.value = config.a;
            this.size.value = configSize;
            this.mult.value = this.damageMultiplier;
            this.color = config;

            next.onClick.AddListener(delegate
            {
                SetMaterial(currentFontIndex + 1);
            });
            prev.onClick.AddListener(delegate
            {
                SetMaterial(currentFontIndex - 1);
            });

            red.onValueChanged.AddListener(SetRed);
            green.onValueChanged.AddListener(SetGreen);
            blue.onValueChanged.AddListener(SetBlue);
            alpha.onValueChanged.AddListener(SetAlpha);
            size.onValueChanged.AddListener(SetSize);
            size.maxValue = maxSize;

            mult.onValueChanged.AddListener(SetMultiplier);
            mult.maxValue = 2;
            mult.wholeNumbers = true;

            // im fuuuuuuuuuuuuuuuuuuuuuucked in the ehead
            this.preview = base.transform.Find("Image").gameObject.AddComponent<RawImage>();
            this.preview.raycastTarget = false;

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
        public void SetMultiplier(float amount)
        {
            int i = Mathf.FloorToInt(amount);
            damageMultiplier = Mathf.Clamp(i, 0, 2);
            UpdateColor();
        }
        public void SetStack(bool b)
        {
            this.stackDamage = b;
            UpdateColor();
        }
        public void SetMaterial(int i)
        {
            if (i < 0) i = (this.fonts.Length - 1) + i;
            if (i >= this.fonts.Length) i = i - this.fonts.Length;

            currentFontIndex = i;
            UpdateColor();
        }


        private void UpdateColor()
        {
            Material newFont = this.fonts[currentFontIndex];
            preview.texture = newFont.GetTexture("_FontTex");
            preview.color = this.color;
            preview.transform.localScale = Vector3.one * sizeMultiplier;

            Config.damageNumberFont.Value = this.currentFontIndex;
            Config.damageNumberSize.Value = this.sizeMultiplier;
            Config.damageNumberColor.Value = this.color;
            Config.damageNumberMultiplier.Value = this.damageMultiplier;
            Config.damageNumberStack.Value = this.stackDamage;

            DamageNumberManager.GetInstance().SetMaterial(newFont);
        }
    }
}
