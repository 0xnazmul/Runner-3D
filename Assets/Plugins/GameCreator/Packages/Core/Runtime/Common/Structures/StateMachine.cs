using System;

namespace GameCreator.Runtime.Common
{
    public abstract class StateMachine
    {
        public abstract class State
        {
            public event Action<StateMachine, State> EventOnEnter;
            public event Action<StateMachine, State> EventOnExit;
            public event Action<StateMachine, State> EventOnBeforeUpdate;

            // PROPERTIES: ------------------------------------------------------------------------
            
            public bool IsActive { get; private set; }
            
            // PUBLIC METHODS: --------------------------------------------------------------------
            
            public void OnEnter(StateMachine stateMachine)
            {
                this.IsActive = true;
                this.WhenEnter(stateMachine);
                this.EventOnEnter?.Invoke(stateMachine, this);   
            }

            public void OnExit(StateMachine stateMachine)
            {
                this.IsActive = false;
                this.WhenExit(stateMachine);
                this.EventOnExit?.Invoke(stateMachine, this);   
            }

            public void OnUpdate(StateMachine stateMachine)
            {
                this.WhenUpdate(stateMachine);
                this.EventOnBeforeUpdate?.Invoke(stateMachine, this);   
            }
            
            // VIRTUAL METHODS: -------------------------------------------------------------------

            protected virtual void WhenEnter(StateMachine stateMachine)
            { }
            
            protected virtual void WhenExit(StateMachine stateMachine)
            { }
            
            protected virtual void WhenUpdate(StateMachine stateMachine)
            { }
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        protected State m_State;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<State> EventStateEnter;
        public event Action<State> EventStateExit;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected StateMachine(State state)
        {
            this.Change(state);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update()
        {
            this.m_State.OnUpdate(this);
        }

        public void Change(State state)
        {
            if (this.m_State != null)
            {
                this.m_State.OnExit(this);
                this.m_State.EventOnEnter -= this.OnEnterCallback;
                this.m_State.EventOnExit -= this.OnExitCallback;
            }

            this.m_State = state;
            if (this.m_State != null)
            {
                this.m_State.EventOnEnter += this.OnEnterCallback;
                this.m_State.EventOnExit += this.OnExitCallback;
                this.m_State.OnEnter(this);   
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnEnterCallback(StateMachine stateMachine, State state)
        {
            this.EventStateEnter?.Invoke(state);
        }
        
        private void OnExitCallback(StateMachine stateMachine, State state)
        {
            this.EventStateExit?.Invoke(state);
        }
    }
}
