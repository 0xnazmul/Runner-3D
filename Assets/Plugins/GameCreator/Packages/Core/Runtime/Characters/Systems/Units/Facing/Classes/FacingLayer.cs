using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    internal class FacingLayer
    {
        private enum System
        {
            Direction,
            Target
        }

        private const float MAX_ANGLE_ERROR = 1f;

        // MEMBERS: -------------------------------------------------------------------------------

        private System m_System;
        private Vector3 m_Direction;
        private Transform m_Target;

        private readonly bool m_AutoDestroyOnReach;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector3 Direction => this.m_Direction;

        // INITIALIZERS: --------------------------------------------------------------------------

        public FacingLayer(Character character, bool autoDestroyOnReach)
        {
            this.m_System = System.Direction;
            this.m_Direction = character.transform.TransformDirection(Vector3.forward);

            this.m_AutoDestroyOnReach = autoDestroyOnReach;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void SetDirection(Vector3 direction)
        {
            this.m_System = System.Direction;
            this.m_Direction = direction.normalized;
        }

        public void SetTarget(Transform target)
        {
            this.m_System = System.Target;
            this.m_Target = target;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        public bool Update(Character character)
        {
            if (this.m_System == System.Target && this.m_Target != null)
            {
                Vector3 direction = this.m_Target.position - character.transform.position;
                if (direction.sqrMagnitude >= float.Epsilon)
                {
                    this.m_Direction = direction.normalized;
                }
            }

            float angle = Vector3.Angle(this.m_Direction, character.Facing.WorldFaceDirection);
            return this.m_AutoDestroyOnReach && angle <= MAX_ANGLE_ERROR;
        }
    }
}