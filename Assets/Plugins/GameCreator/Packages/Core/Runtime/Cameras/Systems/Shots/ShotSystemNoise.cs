using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemNoise : TShotSystem
    {
        private const float SEED_MIN = -99f;
        private const float SEED_MAX = 99f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_AngleX = GetDecimalDecimal.Create(1f);
        [SerializeField] private PropertyGetDecimal m_AngleY = GetDecimalDecimal.Create(1f);
        [SerializeField] private PropertyGetDecimal m_AngleZ = GetDecimalDecimal.Create(1f);

        [SerializeField] private PropertyGetDecimal m_Speed = GetDecimalDecimal.Create(0.25f);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private float m_SeedX;
        private float m_SeedY;
        private float m_SeedZ;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ShotSystemNoise() : base()
        { }

        // IMPLEMENTS: ----------------------------------------------------------------------------

        public override void OnAwake(TShotType shotType)
        {
            base.OnAwake(shotType);
            
            this.m_SeedX = UnityEngine.Random.Range(SEED_MIN, SEED_MAX);
            this.m_SeedY = UnityEngine.Random.Range(SEED_MIN, SEED_MAX);
            this.m_SeedZ = UnityEngine.Random.Range(SEED_MIN, SEED_MAX);
        }

        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);
            if (!shotType.IsActive || !this.Active) return;

            float speed = (float) this.m_Speed.Get(shotType.Args);
            float time = shotType.ShotCamera.TimeMode.Time;
            
            float x = this.GetNoise(this.m_SeedX, speed, time);
            float y = this.GetNoise(this.m_SeedY, speed, time);
            float z = this.GetNoise(this.m_SeedZ, speed, time);
            
            Quaternion rotation = Quaternion.Euler(
                x * (float) this.m_AngleX.Get(shotType.Args),
                y * (float) this.m_AngleY.Get(shotType.Args),
                z * (float) this.m_AngleZ.Get(shotType.Args)
            );

            shotType.Rotation *= rotation;
        }

        private float GetNoise(float seed, float speed, float time)
        {
            float perlinNoise = Mathf.PerlinNoise(seed, (seed + time) * speed); 
            return Mathf.Clamp01(perlinNoise) * 2f - 1f;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

    }
}