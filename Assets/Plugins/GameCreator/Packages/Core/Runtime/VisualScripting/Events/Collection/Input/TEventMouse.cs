using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace GameCreator.Runtime.VisualScripting
{
    [Parameter("Button", "The mouse button to detect")]
    [Parameter(
        "Min Distance", 
        "If set to None, the mouse input acts globally. If set to Game Object, the event " +
        "only fires if the target object is within a certain radius"
    )]
    
    [Keywords("Left", "Middle", "Right")]
    
    [Serializable]
    public abstract class TEventMouse : Event
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] protected MouseButton m_Button = MouseButton.Left;
        
        [SerializeField]
        private CompareMinDistanceOrNone m_MinDistance = new CompareMinDistanceOrNone();
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected internal override void OnUpdate(Trigger trigger)
        {
            base.OnUpdate(trigger);

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            if (!this.InteractionSuccessful(trigger)) return;
            if (!this.m_MinDistance.Match(trigger.transform, new Args(this.Self))) return;
            
            _ = this.m_Trigger.Execute(this.Self);
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract bool InteractionSuccessful(Trigger trigger);
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected bool WasPressedThisFrame
        {
            get
            {
                Mouse mouse = Mouse.current;
                return mouse != null && this.GetButton().wasPressedThisFrame;
            }
        }
        
        protected bool WasReleasedThisFrame
        {
            get
            {
                Mouse mouse = Mouse.current;
                return mouse != null && this.GetButton().wasReleasedThisFrame;
            }
        }
        
        protected bool IsPressed
        {
            get
            {
                Mouse mouse = Mouse.current;
                return mouse != null && this.GetButton().IsPressed();
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private ButtonControl GetButton()
        {
            return this.m_Button switch
            {
                MouseButton.Left => Mouse.current.leftButton,
                MouseButton.Right => Mouse.current.rightButton,
                MouseButton.Middle => Mouse.current.middleButton,
                MouseButton.Forward => Mouse.current.forwardButton,
                MouseButton.Back => Mouse.current.backButton,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
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