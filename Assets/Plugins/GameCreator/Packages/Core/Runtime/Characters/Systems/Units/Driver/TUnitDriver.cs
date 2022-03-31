using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public abstract class TUnitDriver : TUnit, IUnitDriver
    {
        protected const float COYOTE_TIME = 0.3f;
        protected const float GROUND_TIME = 0.1f;

        // INTERFACE PROPERTIES: ------------------------------------------------------------------

        public abstract Vector3 WorldMoveDirection { get; }
        public abstract Vector3 LocalMoveDirection { get; }

        public abstract float SkinWidth { get; }
        public abstract bool IsGrounded { get; }
        public abstract Vector3 FloorNormal { get; }

        // INITIALIZERS: --------------------------------------------------------------------------

        public virtual void OnStartup(Character character)
        {
            this.Character = character;
        }

        public virtual void AfterStartup(Character character)
        {
            this.Character = character;
        }

        public virtual void OnDispose(Character character)
        {
            this.Character = character;
        }

        public virtual void OnEnable()
        { }

        public virtual void OnDisable()
        { }

        // METHODS: -------------------------------------------------------------------------------

        public abstract void SetPosition(Vector3 position);
        public abstract void SetRotation(Quaternion rotation);

        public abstract void AddPosition(Vector3 amount);
        public abstract void AddRotation(Quaternion amount);

        public virtual void OnUpdate()
        { }
        
        public virtual void OnFixedUpdate()
        { }

        public virtual void OnDrawGizmos(Character character)
        { }
    }
}