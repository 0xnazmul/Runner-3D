using System;
using UnityEngine.InputSystem;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public abstract class TInputButtonInputAction : TInputButton
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public abstract InputAction InputAction { get; }

        public override bool Active
        {
            get => this.InputAction?.enabled ?? false;
            set
            {
                switch (value)
                {
                    case true: this.Enable(); break;
                    case false: this.Disable(); break;
                }
            }
        }

        // INITIALIZERS: --------------------------------------------------------------------------
        
        public override void OnStartup()
        {
            this.Enable();
        }

        public override void OnDispose()
        {
            this.Disable();
            this.InputAction?.Dispose();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Enable()
        {
            if (this.InputAction == null) return;
            if (this.InputAction.enabled) return;
            
            this.InputAction.Enable();
            this.InputAction.started += this.ExecuteEventStart;
            this.InputAction.canceled += this.ExecuteEventCancel;
            this.InputAction.performed += this.ExecuteEventPerform;
        }

        private void Disable()
        {
            if (this.InputAction == null) return;
            if (!this.InputAction.enabled) return;
            
            this.InputAction.Disable();
            this.InputAction.started -= this.ExecuteEventStart;
            this.InputAction.canceled -= this.ExecuteEventCancel;
            this.InputAction.performed -= this.ExecuteEventPerform;
        }
        
        private void ExecuteEventStart(InputAction.CallbackContext context)
        {
            this.ExecuteEventStart();
        }
        
        private void ExecuteEventCancel(InputAction.CallbackContext context)
        {
            this.ExecuteEventCancel();
        }
        
        private void ExecuteEventPerform(InputAction.CallbackContext context)
        {
            this.ExecuteEventPerform();
        }
    }
}