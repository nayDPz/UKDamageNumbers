using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace UKDamageNumbers
{
    [ExecuteAlways]
    public class DamageNumberManager : MonoBehaviour
    {
        private static DamageNumberManager Instance;
        private static float baseParticleSize = 0.02f;

        private ParticleSystem ps;
        private ParticleSystemRenderer renderer;
        private Color color;

        public static DamageNumberManager GetInstance()
        {
            if (!Instance)
            {
                Instance = GameObject.Instantiate(DamageNumbersPlugin.prefab).GetComponent<DamageNumberManager>();
            }
            return Instance;
        }

        
        private void Awake()
        {
            if (Instance) Destroy(Instance);
            Instance = this;

            this.ps = base.GetComponent<ParticleSystem>();
            this.renderer = base.GetComponent<ParticleSystemRenderer>();
            this.renderer.maxParticleSize = baseParticleSize * Config.damageNumberSize.Value;
            this.renderer.minParticleSize = baseParticleSize * Config.damageNumberSize.Value;
        }


        public void SpawnDamageNumber(float amount, Vector3 position)
        {

            this.renderer.maxParticleSize = baseParticleSize * Config.damageNumberSize.Value;
            this.renderer.minParticleSize = baseParticleSize * Config.damageNumberSize.Value;

            this.ps.Emit(new ParticleSystem.EmitParams
            {             
                position = position,
                startColor = Config.damageNumberColor.Value,
                applyShapeToPosition = true,
            }, 1);


            this.ps.GetCustomParticleData(this.customData, ParticleSystemCustomData.Custom1);
            this.customData[this.customData.Count - 1] = new Vector4(1f, 1f, amount, 1f);
            this.ps.SetCustomParticleData(this.customData, ParticleSystemCustomData.Custom1);

        }

        private List<Vector4> customData = new List<Vector4>();

    }
}
