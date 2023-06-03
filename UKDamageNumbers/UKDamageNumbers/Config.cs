using System;
using UnityEngine;
using BepInEx.Configuration;


namespace UKDamageNumbers
{
    public static class Config
    {
        public static ConfigEntry<Color> damageNumberColor;
        public static ConfigEntry<float> damageNumberSize;
        public static ConfigEntry<int> damageNumberFont;
        public static ConfigEntry<int> damageNumberMultiplier;
        public static ConfigEntry<bool> damageNumberStack;

        public static ConfigEntry<Color> bloodColor;
        public static void ReadConfig()
        {
            damageNumberColor = DamageNumbersPlugin.Instance.Config.Bind<Color>(new ConfigDefinition("Damage Numbers", "Color"), Color.yellow, new ConfigDescription("color of the damage numbers"));
            damageNumberSize = DamageNumbersPlugin.Instance.Config.Bind<float>(new ConfigDefinition("Damage Numbers", "Size"), 1f, new ConfigDescription("size of the damage numbers"));
            damageNumberFont = DamageNumbersPlugin.Instance.Config.Bind<int>(new ConfigDefinition("Damage Numbers", "Font"), 0, new ConfigDescription("index of the font of the damage numbers"));
            damageNumberMultiplier = DamageNumbersPlugin.Instance.Config.Bind<int>(new ConfigDefinition("Damage Numbers", "Multiplier"), 0, new ConfigDescription("multiplies damage numbers by 10^(x). max 2 (100)"));
            damageNumberStack = DamageNumbersPlugin.Instance.Config.Bind<bool>(new ConfigDefinition("Damage Numbers", "Stacking"), true, new ConfigDescription("(doesnt work) whether damage numbers stack"));

            bloodColor = DamageNumbersPlugin.Instance.Config.Bind<Color>(new ConfigDefinition("Blood", "Color"), Color.red, new ConfigDescription("color of blood"));
        }
    }
}
