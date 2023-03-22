using System;
using BepInEx;
using UnityEngine;
using ULTRAKILL;
using System.Reflection;
using System.IO;
using UnityEngine.UI;

namespace UKDamageNumbers
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class DamageNumbersPlugin : BaseUnityPlugin
    {
        public const string GUID = "com.nayDPz.DamageNumbers";
        public const string MODNAME = "DamageNumbers";
        public const string VERSION = "1.0.0";

        public static DamageNumbersPlugin Instance { get; private set; }
        internal static AssetBundle Assets;

        internal static GameObject prefab;
        internal static GameObject colorPanel;
        internal static GameObject button;
        internal static Font gameFont;
        private void Awake()
        {
            Instance = this;
            Log.Init(Logger);

            UKDamageNumbers.Config.ReadConfig();

            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UKDamageNumbers.assets"))
            {
                Assets = AssetBundle.LoadFromStream(assetStream);
            }

            AssetBundle gameAssetBundle0 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Magenta/Bundles/bundle-0"));
            AssetBundle gameAssetBundle1 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Magenta/Bundles/bundle-1"));

            gameFont = gameAssetBundle0.LoadAsset<Font>("VCR_OSD_MONO_1.001");
           
            GameObject shop = gameAssetBundle1.LoadAsset<GameObject>("Shop");


            GameObject color = shop.transform.Find("Canvas/Weapons/RevolverWindow/Color Screen/Standard/Custom/Unlocked/Color 1").gameObject;


            // just to avoid a harmless error
            color.GetComponent<GunColorSetter>().enabled = false;

            colorPanel = GameObject.Instantiate(color);
            Destroy(colorPanel.GetComponent<GunColorSetter>());

            color.GetComponent<GunColorSetter>().enabled = true;

            colorPanel.AddComponent<ColorSetter>();

            colorPanel.name = "UKDamageNumbersColorPanel";

            Image im = colorPanel.GetComponent<Image>();
            im.color = new Color(0, 0, 0, 0.8f);
            im.raycastTarget = false;

            Destroy(colorPanel.transform.Find("MetalPreview").gameObject);
            

            colorPanel.SetActive(false);


            GameObject butt = shop.transform.Find("Canvas/Main Menu/WeaponsButton").gameObject;

            button = GameObject.Instantiate(butt);
            button.transform.localPosition = new Vector3(-180f, 158f, -45f);
            button.transform.localScale = Vector3.one;
            button.transform.rotation = Quaternion.identity;
            button.name = "UKDamageNumbersButton";
            button.GetComponent<RectTransform>().offsetMax = new Vector2(-160f, 183f);

            Destroy(button.GetComponent<ShopButton>());
            //ShopButton sb = button.GetComponent<ShopButton>();
            //sb.toActivate = new GameObject[] { colorPanel };

            colorPanel.transform.SetParent(button.transform);
            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = "DamageNumbers";
            text.font = gameFont;
            text.fontSize = 12;

            colorPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 100f);

            HudOpenEffect h = button.GetComponent<HudOpenEffect>();
            h.originalHeight = 1f;
            h.originalWidth = 1f;

            GameObject mslider = colorPanel.transform.Find("Metal").gameObject;
            mslider.name = "Alpha";
            mslider.transform.Find("Text (1)").GetComponent<Text>().text = "A";

            GameObject sizeSlider = GameObject.Instantiate(mslider);
            sizeSlider.transform.SetParent(mslider.transform.parent);
            sizeSlider.transform.localScale = Vector3.one;
            sizeSlider.transform.rotation = Quaternion.identity;
            sizeSlider.transform.localPosition = new Vector3(30f, -80f, 0);
            sizeSlider.name = "Size";

            sizeSlider.transform.Find("Text (1)").GetComponent<Text>().text = "S";

            colorPanel.transform.SetParent(button.transform);
            colorPanel.transform.position = Vector3.zero;
            colorPanel.transform.rotation = Quaternion.identity;


            Sprite numbers = Assets.LoadAsset<Sprite>("texDamageBorder");
            colorPanel.transform.Find("Image").GetComponent<Image>().sprite = numbers;

            DontDestroyOnLoad(button);
            button.SetActive(false);

            gameAssetBundle0.Unload(false);
            gameAssetBundle1.Unload(false);

            prefab = Assets.LoadAsset<GameObject>("DamageNumberController");
            prefab.AddComponent<DamageNumberManager>();

            On.EnemyIdentifier.DeliverDamage += EnemyIdentifier_DeliverDamage;
            On.ShopZone.Start += SetupButtons;

        }

        
        private void SetupButtons(On.ShopZone.orig_Start orig, ShopZone self)
        {
            orig(self);

            GameObject menu = Instantiate(button, self.transform.Find("Canvas/Main Menu"));

            menu.SetActive(true);
            menu.SetActive(false);
            menu.SetActive(true);

            // font disappears when instantiating idek
            Text text = menu.transform.Find("Text").GetComponent<Text>();
            text.font = gameFont;

            // im braindead and tirewd dont a@ me
            ColorSetter setter = menu.transform.Find("UKDamageNumbersColorPanel").GetComponent<ColorSetter>();
            setter.transform.localPosition = new Vector3(46f, -28f, 0);
            setter.red = setter.transform.Find("Red/Slider").GetComponent<Slider>();
            setter.green = setter.transform.Find("Green/Slider").GetComponent<Slider>();
            setter.blue = setter.transform.Find("Blue/Slider").GetComponent<Slider>();
            setter.alpha = setter.transform.Find("Alpha/Slider").GetComponent<Slider>();
            setter.size = setter.transform.Find("Size/Slider").GetComponent<Slider>();

            menu.GetComponent<Button>().onClick.AddListener(delegate
            {
                setter.gameObject.SetActive(!setter.gameObject.activeSelf);
            });

        }

        private void EnemyIdentifier_DeliverDamage(On.EnemyIdentifier.orig_DeliverDamage orig, EnemyIdentifier self, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon)
        {

            if(self.dead)
            {
                orig(self, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon);
                return;
            }

            // FUCK YOU HAKITA OMG
            float health = self.health;
            orig(self, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon);

            float f = (!self.dead ? self.health : 0);
            float healthLost = health - f;

            DamageNumberManager.GetInstance().SpawnDamageNumber(healthLost, hitPoint);
        }



        
    }
}
