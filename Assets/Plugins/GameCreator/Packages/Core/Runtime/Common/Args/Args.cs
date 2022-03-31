using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class Args
    {
        public static readonly Args EMPTY = new Args();
        
        private readonly Dictionary<int, Component> selfComponents;
        private readonly Dictionary<int, Component> targetComponents;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Self   { get; private set; }
        public GameObject Target { get; private set; }
        
        public Args Clone => new Args(this.Self, this.Target);

        // CONSTRUCTORS: --------------------------------------------------------------------------

        private Args()
        {
            this.selfComponents = new Dictionary<int, Component>();
            this.targetComponents = new Dictionary<int, Component>();
        }

        public Args(Component target) : this(target, target)
        { }

        public Args(GameObject target) : this(target, target)
        { }

        public Args(Component self, Component target) : this()
        {
            this.Self = self == null ? null : self.gameObject;
            this.Target = target == null ? null : target.gameObject;
        }

        public Args(GameObject self, GameObject target) : this()
        {
            this.Self = self;
            this.Target = target;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public T ComponentFromSelf<T>(bool inChildren = false) where T : Component
        {
            return this.GetComponent<T>(this.selfComponents, this.Self, inChildren);
        }

        public T ComponentFromTarget<T>(bool inChildren = false) where T : Component
        {
            return this.GetComponent<T>(this.targetComponents, this.Target, inChildren);
        }

        public void ChangeSelf(GameObject self)
        {
            if (this.Self == self) return;

            this.Self = self;
            this.selfComponents.Clear();
        }
        
        public void ChangeSelf<T>(T self) where T : Component
        {
            this.ChangeSelf(self != null ? self.gameObject : null);
        }

        public void ChangeTarget(GameObject target)
        {
            if (this.Target == target) return;

            this.Target = target;
            this.targetComponents.Clear();
        }
        
        public void ChangeTarget<T>(T target) where T : Component
        {
            this.ChangeTarget(target != null ? target.gameObject : null);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private TComponent GetComponent<TComponent>(
            IDictionary<int, Component> dictionary, GameObject gameObject, bool inChildren)
            where TComponent : Component
        {
            int hash = typeof(TComponent).GetHashCode();
            if (!dictionary.TryGetValue(hash, out Component value) || value == null)
            {
                value = inChildren
                    ? gameObject.GetComponent<TComponent>()
                    : gameObject.GetComponentInChildren<TComponent>();

                if (value == null) return null;
                dictionary[hash] = value;
            }

            return value as TComponent;
        }
    }
}