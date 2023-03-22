using System;
using UnityEngine;
using BepInEx.Configuration;


namespace UKDamageNumbers
{
    public static class Config
    {
        public static ConfigEntry<Color> damageNumberColor;
        public static ConfigEntry<float> damageNumberSize;

        public static void ReadConfig()
        {
            damageNumberColor = DamageNumbersPlugin.Instance.Config.Bind<Color>(new ConfigDefinition("Damage Numbers", "Color"), Color.yellow, new ConfigDescription("color of the damage numbers"));
            damageNumberSize = DamageNumbersPlugin.Instance.Config.Bind<float>(new ConfigDefinition("Damage Numbers", "Size"), 1f, new ConfigDescription("size of the damage numbers"));
        }
    }
}
