using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public static class SkinMeshUtils
    {
        private const string NAME = "Armature-{0}";

        private class Armature : Dictionary<string, Transform>
        {
            public Armature(Transform transform)
            {
                GatherBones(transform);
            }

            private void GatherBones(Transform transform)
            {
                if (this.ContainsKey(transform.name))
                {
                    Remove(transform.name);
                }

                Add(transform.name, transform);
                int childCount = transform.childCount;
                
                for (int i = 0; i < childCount; ++i)
                {
                    this.GatherBones(transform.GetChild(i));
                }
            }

            public Transform Get(string name)
            {
                return this.ContainsKey(name) ? this[name] : null;
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public static GameObject PutOn(GameObject prefab, Character character)
        {
            if (prefab == null || character == null) return null;

            Animator animator = character.Animim.Animator;
            if (animator == null) return null;

            Transform root = animator.transform;
            Armature armature = new Armature(root);
            
            GameObject instance = Object.Instantiate(prefab, root.position, root.rotation);
            instance.name = string.Format(NAME, prefab.name);
            
            SkinnedMeshRenderer[] renderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();
            Transform target = SetupSkin(instance.transform, root);
            
            foreach (SkinnedMeshRenderer render in renderers)
            {
                SkinnedMeshRenderer renderer = AddSkinnedMeshRenderer(render, target);
                renderer.bones = GetTransforms(render.bones, armature);
            }

            return target.gameObject;
        }

        public static GameObject TakeOff(GameObject prefab, Character character)
        {
            if (prefab == null || character == null) return null;

            Animator animator = character.Animim.Animator;
            if (animator == null) return null;

            string wearName = string.Format(NAME, prefab.name);
            Transform wear = animator.transform.Find(wearName);
            
            if (wear != null) Object.Destroy(wear.gameObject);
            return null;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static Transform SetupSkin(Transform source, Transform parent)
        {
            Animator animator = source.GetComponent<Animator>();
            if (animator != null) Object.Destroy(animator);
            
            source.SetParent(parent);

            for (int i = source.childCount - 1; i >= 0; --i)
            {
                Object.Destroy(source.GetChild(i).gameObject);
            }

            return source;
        }

        private static SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, Transform parent)
        {
            GameObject instance = new GameObject(source.name);
            instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            SkinnedMeshRenderer instanceMesh = instance.AddComponent<SkinnedMeshRenderer>();
            instanceMesh.sharedMesh = source.sharedMesh;
            instanceMesh.materials = source.materials;
            return instanceMesh;
        }

        private static Transform[] GetTransforms(Transform[] sources, Armature armature)
        {
            Transform[] transforms = new Transform[sources.Length];
            for (int i = 0; i < sources.Length; ++i)
            {
                transforms[i] = armature.Get(sources[i].name);
            }

            return transforms;
        }
    }
}