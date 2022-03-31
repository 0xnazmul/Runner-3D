using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class PropertyGetGameObject : TPropertyGet<PropertyTypeGetGameObject, GameObject>
    {
        public PropertyGetGameObject() : base(new GetGameObjectInstance())
        { }

        public PropertyGetGameObject(PropertyTypeGetGameObject defaultType) : base(defaultType)
        { }
        
        public T Get<T>(Args args) where T : Component
        {
            GameObject gameObject = this.Get(args);
            return gameObject != null ? gameObject.Get<T>() : null;
        }

        public T Get<T>(GameObject target) where T : Component
        {
            GameObject gameObject = this.Get(target);
            return gameObject != null ? gameObject.Get<T>() : null;
        }
        
        public T Get<T>(Component component) where T : Component
        {
            GameObject gameObject = this.Get(component);
            return gameObject != null ? gameObject.Get<T>() : null;
        }
    }
}