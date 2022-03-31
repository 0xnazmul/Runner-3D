using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Parameter("Button", "The button that triggers the event")]
    [Parameter(
        "Min Distance", 
        "If set to None, the input acts globally. If set to Game Object, the event " +
        "only fires if the target object is within the specified radius"
    )]

    [Serializable]
    public abstract class TEventInput : Event
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private InputPropertyButton m_Button = InputButtonJump.Create();

        [SerializeField]
        private CompareMinDistanceOrNone m_MinDistance = new CompareMinDistanceOrNone();

        // METHODS: -------------------------------------------------------------------------------

        protected internal override void OnAwake(Trigger trigger)
        {
            base.OnAwake(trigger);
            this.m_Button.OnStartup();
            
            this.m_Button.RegisterStart(this.OnStart);
            this.m_Button.RegisterCancel(this.OnCancel);
            this.m_Button.RegisterPerform(this.OnPerform);
        }

        protected internal override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            this.m_Button.Enable();
        }

        protected internal override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            this.m_Button.Disable();
        }

        protected internal override void OnDestroy(Trigger trigger)
        {
            base.OnDestroy(trigger);
            this.m_Button.OnDispose();
            
            this.m_Button.ForgetStart(this.OnStart);
            this.m_Button.ForgetCancel(this.OnCancel);
            this.m_Button.ForgetPerform(this.OnPerform);
        }

        protected internal override void OnUpdate(Trigger trigger)
        {
            base.OnUpdate(trigger);
            this.m_Button.OnUpdate();
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void Execute()
        {
            bool matchDistance = this.m_MinDistance.Match(
                this.m_Trigger.transform,
                new Args(this.Self)
            );
            
            if (!matchDistance) return;
            _ = this.m_Trigger.Execute(this.Self);
        }

        // VIRTUAL METHODS: -----------------------------------------------------------------------

        protected virtual void OnStart()
        { }

        protected virtual void OnCancel()
        { }

        protected virtual void OnPerform()
        { }
        
        // GIZMOS: --------------------------------------------------------------------------------

        protected internal override void OnDrawGizmosSelected(Trigger trigger)
        {
            base.OnDrawGizmosSelected(trigger);
            this.m_MinDistance.OnDrawGizmos(
                trigger.transform,
                new Args(trigger.gameObject)
            );
        }
    }
}