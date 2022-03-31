using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace GameCreator.Runtime.Characters.IK
{
    public abstract class TRigAnimationRigging : TRig
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly AnimFloat m_Weight = new AnimFloat(0f, 0f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected RigLayer RigLayer { get; private set; }
        
        // ABSTRACT PROPERTIES: -------------------------------------------------------------------
        
        protected abstract float WeightTarget { get; }
        protected abstract float WeightSmoothTime { get; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public sealed override bool OnUpdate(Character character)
        {
            bool updateGraph = this.DoUpdate(character);

            this.m_Weight.UpdateWithDelta(
                this.IsActive ? this.WeightTarget : 0f,
                this.WeightSmoothTime, 
                character.Time.DeltaTime
            );
            
            this.RigLayer.rig.weight = this.m_Weight.Current;
            return updateGraph;
        }
        
        // VIRTUAL METHODS: -----------------------------------------------------------------------

        protected override bool DoStartup(Character character)
        {
            this.RigLayer = null;
            
            return this.BuildRigLayer(character);
        }
        
        protected override bool DoEnable(Character character)
        {
            return this.BuildRigLayer(character);
        }
        
        protected override bool DoDisable(Character character)
        {
            return this.BuildRigLayer(character);
        }
        
        protected override bool DoUpdate(Character character)
        {
            return this.BuildRigLayer(character);
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        private bool BuildRigLayer(Character character)
        {
            if (this.RigLayer != null) return false;
            
            GameObject rigLayer = new GameObject(Name);
            rigLayer.transform.SetParent(character.Animim.Animator.transform);
            rigLayer.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            this.RigLayer = new RigLayer(rigLayer.AddComponent<Rig>(), true);
            character.IK.RigBuilder.layers.Add(this.RigLayer);
            
            this.OnBuildRigLayer(character);
            return true;
        }
        
        protected abstract void OnBuildRigLayer(Character character);
    }
}