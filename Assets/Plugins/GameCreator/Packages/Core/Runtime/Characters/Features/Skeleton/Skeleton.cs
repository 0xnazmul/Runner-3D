using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [CreateAssetMenu(
        fileName = "My Skeleton",
        menuName = "Game Creator/Characters/Skeleton",
        order = 50
    )]
    [Icon(RuntimePaths.GIZMOS + "GizmoSkeleton.png")]
    
    [Serializable]
    public class Skeleton : ScriptableObject
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] 
        private PhysicMaterial m_Material = null;

        [SerializeField]
        private CollisionDetectionMode m_CollisionDetection = CollisionDetectionMode.Discrete;

        [SerializeReference] 
        private Volumes m_Volumes = new Volumes();

        // PROPERTIES: ----------------------------------------------------------------------------

        public PhysicMaterial Material => m_Material;

        public CollisionDetectionMode CollisionDetection => m_CollisionDetection;

        public int VolumesLength => this.m_Volumes.Length;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public GameObject[] Refresh(Character character)
        {
            if (character == null) return new GameObject[0];

            Animator animator = character.Animim.Animator;
            return animator != null 
                ? this.Refresh(animator, character.Motion.Mass)
                : new GameObject[0];
        }
        
        public GameObject[] Refresh(Animator animator, float mass)
        {
            return this.m_Volumes.Update(animator, mass, this);
        }
        
        // DRAW GIZMOS: ---------------------------------------------------------------------------

        public void DrawGizmos(Animator animator, Volumes.Display display)
        {
            this.m_Volumes.DrawGizmos(animator, display);
        }
    }
}
