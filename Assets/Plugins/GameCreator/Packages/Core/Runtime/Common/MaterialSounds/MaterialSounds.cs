using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class MaterialSounds
    {
        private static readonly Vector2 PITCH_VARIATION = new Vector2(-0.1f, 0.1f);
        private static readonly Vector2 PITCH_LERP_WEIGHT = new Vector2(0.5f, 1f);

        private const string MAIN_TEXTURE = "_MainTex";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private MaterialSoundsAsset m_SoundsAsset;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Dictionary<Texture, MaterialSoundTexture> m_LookupTable;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public LayerMask LayerMask => this.m_SoundsAsset != null
            ? this.m_SoundsAsset.MaterialSounds.LayerMask
            : 0;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal void OnStartup()
        {
            this.SetupSoundsTable();
        }

        internal void ChangeSoundsAsset(MaterialSoundsAsset materialSoundsAsset)
        {
            this.m_SoundsAsset = materialSoundsAsset;
            if (!Application.isPlaying) return;
            
            this.SetupSoundsTable();
        }
        
        public void Play(Transform transform, RaycastHit hit, float speed, Args args)
        {
            if (this.m_SoundsAsset == null) return;

            switch (hit.collider is TerrainCollider)
            {
                case true: this.PlayTerrain(transform, hit, speed, args); break;
                case false: this.PlayMesh(transform, hit, speed, args); break;
            }
        }

        // MATERIAL GROUND TYPE: ------------------------------------------------------------------

        private void PlayTerrain(Transform transform, RaycastHit hit, float speed, Args args)
        {
            Terrain terrain = hit.collider.Get<Terrain>();
            TerrainData terrainData = terrain.terrainData;

            float[] mixture = this.GetTerrainWeights(
                hit.point, 
                terrainData, 
                terrain.GetPosition()
            );

            Texture maxTexture = null;
            float maxWeight = 0f;
            
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                float weight = i < mixture.Length ? mixture[i] : 0f;
                Texture texture = terrainData.terrainLayers[i].diffuseTexture;
                
                if (weight > maxWeight)
                {
                    maxTexture = texture;
                    maxWeight = weight;
                }
                
                this.PlaySound(texture, weight, speed, transform, args);
            }

            if (maxTexture != null)
            {
                this.PlayImpact(maxTexture, transform, hit);
            }
        }

        private float[] GetTerrainWeights(Vector3 position, TerrainData terrainData, Vector3 terrainPosition)
        {
            float positionX = position.x - terrainPosition.x;
            float positionZ = position.z - terrainPosition.z;
            
            int mapX = (int)(positionX / terrainData.size.x * terrainData.alphamapWidth);
            int mapZ = (int)(positionZ / terrainData.size.z * terrainData.alphamapHeight);
            
            float[,,] alphamaps = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

            int textureCount = alphamaps.GetUpperBound(2);
            float[] mixture = new float[textureCount + 1];

            for(int i = 0; i < mixture.Length; ++i) 
            {
                mixture[i] = alphamaps[0, 0, i];
            }

            return mixture;
        }
        
        private void PlayMesh(Transform transform, RaycastHit hit, float speed, Args args)
        {
            Renderer renderer = hit.collider.Get<Renderer>();
            if (renderer == null) return;
            if (renderer.material == null) return;
            if (!renderer.material.HasTexture(MAIN_TEXTURE)) return;
            
            Texture texture = renderer.material.mainTexture;
            this.PlaySound(texture, 1f, speed, transform, args);
            this.PlayImpact(texture, transform, hit);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void SetupSoundsTable()
        {
            this.m_LookupTable = new Dictionary<Texture, MaterialSoundTexture>();
            if (this.m_SoundsAsset == null) return;
            
            foreach (MaterialSoundTexture materialSound in this.m_SoundsAsset.MaterialSounds.MaterialSounds)
            {
                Texture texture = materialSound.Texture;
                
                if (texture == null) continue;
                this.m_LookupTable[texture] = materialSound;
            }
        }
        
        private void PlaySound(Texture texture, float weight, float speed, Transform target, Args args)
        {
            if (texture == null) return;

            IMaterialSound materialSound;
            AudioConfigSoundEffect config;

            float pitch = Mathf.Lerp(PITCH_LERP_WEIGHT.x, PITCH_LERP_WEIGHT.y, weight);

            if (this.m_LookupTable.TryGetValue(texture, out MaterialSoundTexture material))
            {
                AudioClip audioClip = material.Audio;
                if (audioClip == null) return;

                materialSound = material;

                config = AudioConfigSoundEffect.Create(
                    material.Volume * weight * speed,
                    new Vector2(pitch + PITCH_VARIATION.x, pitch + PITCH_VARIATION.y), 
                    0f, SpatialBlending.Spatial,
                    target.gameObject
                );
            }
            else
            {
                materialSound = this.m_SoundsAsset.MaterialSounds.DefaultSounds;

                config = AudioConfigSoundEffect.Create(
                    materialSound.Volume * weight * speed,
                    new Vector2(pitch + PITCH_VARIATION.x, pitch + PITCH_VARIATION.y),
                    0f, SpatialBlending.Spatial,
                    target.gameObject
                );
            }

            if (config.Volume < float.Epsilon) return;
            _ = AudioManager.Instance.SoundEffect.Play(materialSound.Audio, config, args);
        }
        
        private void PlayImpact(Texture texture, Transform transform, RaycastHit hit)
        {
            if (texture == null) return;
            IMaterialSound materialSound;
            
            if (this.m_LookupTable.TryGetValue(texture, out MaterialSoundTexture material))
            {
                AudioClip audioClip = material.Audio;
                if (audioClip == null) return;
                
                materialSound = material;
            }
            else
            {
                materialSound = this.m_SoundsAsset.MaterialSounds.DefaultSounds;
            }

            materialSound?.Impact.Create(
                hit.point,
                Quaternion.FromToRotation(Vector3.up, hit.normal), 
                null
            );
        }
    }
}