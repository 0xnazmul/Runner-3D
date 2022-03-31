using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Trigger")]
    [Category("Interaction/Trigger")]
    
    [Image(typeof(IconTriggers), ColorTheme.Type.Yellow)]
    [Description("A Trigger component reference")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectTrigger : PropertyTypeGetGameObject
    {
        [SerializeField] protected Trigger m_Trigger;

        public override GameObject Get(Args args) => this.m_Trigger != null 
            ? this.m_Trigger.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => this.m_Trigger != null 
            ? this.m_Trigger.gameObject 
            : null;

        public GetGameObjectTrigger() : base()
        { }

        public GetGameObjectTrigger(GameObject gameObject) : this()
        {
            this.m_Trigger = gameObject.Get<Trigger>();
        }
        
        public GetGameObjectTrigger(Trigger trigger) : this()
        {
            this.m_Trigger = trigger;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectTrigger instance = new GetGameObjectTrigger();
            return new PropertyGetGameObject(instance);
        }
        
        public static PropertyGetGameObject Create(GameObject gameObject)
        {
            GetGameObjectTrigger instance = new GetGameObjectTrigger
            {
                m_Trigger = gameObject != null ? gameObject.Get<Trigger>() : null
            };
            
            return new PropertyGetGameObject(instance);
        }
        
        public static PropertyGetGameObject Create(Trigger trigger)
        {
            GetGameObjectTrigger instance = new GetGameObjectTrigger
            {
                m_Trigger = trigger
            };
            
            return new PropertyGetGameObject(instance);
        }

        public override string String => this.m_Trigger != null
            ? this.m_Trigger.gameObject.name
            : "(none)";
    }
}