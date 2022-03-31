using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Title("First Person")]
    [Category("First Person")]
    [Image(typeof(IconShotFirstPerson), ColorTheme.Type.Blue)]

    [Description("Moves with the head of a Character")]

    [Serializable]
    public class ShotTypeFirstPerson : TShotType
    {
        [SerializeField] private ShotSystemFirstPerson m_FirstPerson;
        [SerializeField] private ShotSystemHeadBobbing m_HeadBobbing;
        [SerializeField] private ShotSystemHeadLeaning m_HeadLeaning;
        [SerializeField] private ShotSystemNoise m_Noise;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override Args Args
        {
            get
            {
                this.m_Args ??= new Args(this.m_ShotCamera, null);
                this.m_Args.ChangeTarget(this.m_FirstPerson.GetTarget(this));
        
                return this.m_Args;
            }
        }

        public override bool UseSmoothPosition => false;
        public override bool UseSmoothRotation => false;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotTypeFirstPerson()
        {
            this.m_FirstPerson = new ShotSystemFirstPerson();
            this.m_HeadBobbing = new ShotSystemHeadBobbing();
            this.m_HeadLeaning = new ShotSystemHeadLeaning();
            this.m_Noise = new ShotSystemNoise();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Character Character => this.m_FirstPerson.GetTarget(this);
        
        // MAIN METHODS: --------------------------------------------------------------------------

        public override void Awake(ShotCamera shotCamera)
        {
            base.Awake(shotCamera);
            this.OnAwake(shotCamera);
        }

        public override void Start(ShotCamera shotCamera)
        {
            base.Start(shotCamera);
            this.OnStart(shotCamera);
        }

        public override void Destroy(ShotCamera shotCamera)
        {
            base.Destroy(shotCamera);
            this.OnDestroy(shotCamera);
        }

        public override void Update()
        {
            base.Update();
            this.OnUpdate();
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnAwake(ShotCamera shotCamera)
        {
            base.OnAwake(shotCamera);
            this.m_FirstPerson?.OnAwake(this);
            this.m_HeadBobbing?.OnAwake(this);
            this.m_HeadLeaning?.OnAwake(this);
            this.m_Noise?.OnAwake(this);
        }

        public override void OnStart(ShotCamera shotCamera)
        {
            base.OnStart(shotCamera);
            this.m_FirstPerson?.OnStart(this);
            this.m_HeadBobbing?.OnStart(this);
            this.m_HeadLeaning?.OnStart(this);
            this.m_Noise?.OnStart(this);
        }

        public override void OnDestroy(ShotCamera shotCamera)
        {
            base.OnDestroy(shotCamera);
            this.m_FirstPerson?.OnDestroy(this);
            this.m_HeadBobbing?.OnDestroy(this);
            this.m_HeadLeaning?.OnDestroy(this);
            this.m_Noise?.OnDestroy(this);
        }

        public override void OnEnable(TCamera camera)
        {
            base.OnEnable(camera);
            this.m_FirstPerson?.OnEnable(this, camera);
            this.m_HeadBobbing?.OnEnable(this, camera);
            this.m_HeadLeaning?.OnEnable(this, camera);
            this.m_Noise?.OnEnable(this, camera);
        }

        public override void OnDisable(TCamera camera)
        {
            base.OnDisable(camera);
            this.m_FirstPerson?.OnDisable(this, camera);
            this.m_HeadBobbing?.OnDisable(this, camera);
            this.m_HeadLeaning?.OnDisable(this, camera);
            this.m_Noise?.OnDisable(this, camera);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            this.m_FirstPerson?.OnUpdate(this);
            this.m_HeadBobbing?.OnUpdate(this);
            this.m_HeadLeaning?.OnUpdate(this);
            this.m_Noise?.OnUpdate(this);
        }
    }
}