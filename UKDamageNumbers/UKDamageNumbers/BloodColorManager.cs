using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UKDamageNumbers
{
    public class BloodColorManager : MonoBehaviour
    {
        public static BloodColorManager Instance;

        public Texture2D bloodSprite;
        private List<Coords> alphaPixelIndices = new List<Coords>();

        private Color _currentColor;
        public Color CurrentColor
        {
            get => _currentColor;
            set
            {
                _currentColor = value;
                this.SetSpriteColor(value);
            }
        }

        private struct Coords
        {
            public int x;
            public int y;
        }

        private void Awake()
        {
            if (Instance) Destroy(this);

            Instance = this;
        }

        private void Start()
        {
            this.CurrentColor = Config.bloodColor.Value;

            for(int y = 0; y < bloodSprite.height; y++)
            {
                for(int x = 0; x < bloodSprite.width; x++)
                {

                    if(bloodSprite.GetPixel(x, y).a > 0)
                    {
                        alphaPixelIndices.Add(new Coords { x = x, y = y });
                    }
                }
            }
        }

        // vanilla blood sprites are flat color. thank god
        public void SetSpriteColor(Color color)
        {

            Log.LogDebug("Setting color to " + color);
            for (int y = 0; y < bloodSprite.height; y++)
            {
                for (int x = 0; x < bloodSprite.width; x++)
                {
                    bloodSprite.SetPixel(x, y, color);
                }
            }
            bloodSprite.Apply();

            SetSpriteAlpha(color.a);
        }
        public void SetSpriteAlpha(float a)
        {
            for(int i = 0; i < alphaPixelIndices.Count; i++)
            {
                Color color = bloodSprite.GetPixel(alphaPixelIndices[i].x, alphaPixelIndices[i].y);
                color.a = a;
                bloodSprite.SetPixel(alphaPixelIndices[i].x, alphaPixelIndices[i].y, color);
            }

            bloodSprite.Apply();
        }

    }
}
