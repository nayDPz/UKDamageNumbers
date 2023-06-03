using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace UKDamageNumbers
{
    public class DamageNumberManager : MonoBehaviour
    {
        private static DamageNumberManager Instance;
        private static float baseParticleSize = 0.02f;
        private static float damageStackDuration = 0.5f;

        private ParticleSystem ps;
        private ParticleSystemRenderer renderer;
        private Color color;

        private Material particleMaterial;

        private ParticleSystem.Particle[] m_Particles;

        private Dictionary<GameObject, DamageStack> damageStacks = new Dictionary<GameObject, DamageStack>();

        public struct DamageStack
        {
            public float timer;
            public float damage;
            public ParticleSystem.Particle particle;
        }
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
            this.m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];

            this.renderer = base.GetComponent<ParticleSystemRenderer>();
            this.renderer.maxParticleSize = baseParticleSize * Config.damageNumberSize.Value;
            this.renderer.minParticleSize = baseParticleSize * Config.damageNumberSize.Value;
        }

        //private void Update()
        //{
        //    if (this.damageStacks.Count > 0)
        //    {
        //        for (int i = damageStacks.Count - 1; i >= 0; i--)
        //        {
        //            var item = damageStacks.ElementAt(i);
        //            DamageStack itemValue = item.Value;

        //            itemValue.timer -= Time.deltaTime;
        //            if (itemValue.timer <= 0) damageStacks.Remove(item.Key);

        //        }
        //    }
        //}

        //private DamageStack AddStack(GameObject victim, float amount, ParticleSystem.Particle particle)
        //{
        //    DamageStack stack;
        //    if(!damageStacks.TryGetValue(victim, out stack))
        //    {
        //        Log.LogInfo("ADDING STACK TO " + victim.gameObject);
        //        damageStacks.Add(victim, stack = new DamageStack { particle = particle });
        //    }
        //    else stack.particle.remainingLifetime = 0f;

        //    stack.timer = damageStackDuration;
        //    stack.damage += amount;

        //    return stack;
        //}

        public void SetMaterial(Material m)
        {
            this.particleMaterial = m;
        }

        public void SpawnDamageNumber(EnemyIdentifier victim, float amount, Vector3 position)
        {           
            if(this.particleMaterial)
                this.renderer.material = this.particleMaterial;

            this.renderer.maxParticleSize = baseParticleSize * Config.damageNumberSize.Value;
            this.renderer.minParticleSize = baseParticleSize * Config.damageNumberSize.Value;

            this.ps.Emit(new ParticleSystem.EmitParams
            {             
                position = position,
                startColor = Config.damageNumberColor.Value,
                applyShapeToPosition = true,
            }, 1);

            float dmg = amount;

            //if (Config.damageNumberStack.Value)
            //{            
            //    int i = this.ps.GetParticles(m_Particles);
            //    ParticleSystem.Particle last = m_Particles[i - 1];

            //    dmg = AddStack(victim.gameObject, amount, last).damage;
            //}
            //this.ps.SetParticles(m_Particles);

            dmg *= Mathf.Pow(10, Config.damageNumberMultiplier.Value);

            

            this.ps.GetCustomParticleData(this.customData, ParticleSystemCustomData.Custom1);
            this.customData[this.customData.Count - 1] = new Vector4(1f, 1f, dmg, 1f);
            this.ps.SetCustomParticleData(this.customData, ParticleSystemCustomData.Custom1);

        }

        private List<Vector4> customData = new List<Vector4>();

    }
}
