using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Motion Controller")]
    [Image(typeof(IconChip), ColorTheme.Type.Blue)]

    [Category("Motion Controller")]
    [Description(
        "Motion system that defines how the Character responds to external stimulus"
    )]

    [Serializable]
    public class UnitMotionController : TUnitMotion
    {
        // PUBLIC FIELDS: -------------------------------------------------------------------------
        
        [SerializeField] private float m_Speed = 4f;
        [SerializeField] private float m_Rotation = 1800f;

        [SerializeField] private float m_Mass = 80f;
        [SerializeField] private float m_Height = 2.0f;
        [SerializeField] private float m_Radius = 0.2f;

        [SerializeField] private bool m_UseAcceleration = true;
        [ConditionEnable(nameof(m_UseAcceleration)), SerializeField] private float m_Acceleration = 10f;
        [ConditionEnable(nameof(m_UseAcceleration)), SerializeField] private float m_Deceleration = 4f;

        [SerializeField] private bool m_CanJump = true;
        [SerializeField] private int m_AirJumps = 0;
        [SerializeField, Min(0.1f)] private float m_JumpForce = 5f;
        [SerializeField, Min(0.0f)] private float m_JumpCooldown = 0.5f;

        [SerializeField] private float m_Gravity = -9.81f;
        [SerializeField] private float m_TerminalVelocity = -53f;
        
        // INTERFACE PROPERTIES: ------------------------------------------------------------------
        
        public override float JumpForce
        {
            get => this.m_JumpForce;
            set => this.m_JumpForce = value;
        }

        public override float LinearSpeed
        {
            get => this.m_Speed;
            set => this.m_Speed = value;
        }

        public override float AngularSpeed
        {
            get => this.m_Rotation;
            set => this.m_Rotation = value;
        }

        public override float Mass
        {
            get => this.m_Mass;
            set => this.m_Mass = value;
        }

        public override float Height
        {
            get => this.m_Height;
            set => this.m_Height = value;
        }

        public override float Radius
        {
            get => this.m_Radius;
            set => this.m_Radius = value;
        }

        public override bool CanJump
        {
            get => this.m_CanJump && !this.Character.Busy.AreLegsBusy;
            set => this.m_CanJump = value;
        }

        public override int AirJumps
        {
            get => m_AirJumps;
            set => m_AirJumps = value;
        }

        public override float Gravity
        {
            get => this.m_Gravity;
            set => this.m_Gravity = value;
        }

        public override float TerminalVelocity
        {
            get => this.m_TerminalVelocity;
            set => this.m_TerminalVelocity = value;
        }

        public override float JumpCooldown
        {
            get => this.m_JumpCooldown;
            set => this.m_JumpCooldown = value;
        }

        public override bool UseAcceleration => this.m_UseAcceleration;

        public override float Acceleration
        {
            get => this.m_Acceleration;
            set => this.m_Acceleration = value;
        }

        public override float Deceleration
        {
            get => this.m_Deceleration;
            set => this.m_Deceleration = value;
        }
    }
}
