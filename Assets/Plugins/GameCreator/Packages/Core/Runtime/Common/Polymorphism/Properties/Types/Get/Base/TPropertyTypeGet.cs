using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Image(typeof(IconCircleSolid), ColorTheme.Type.Green)]

    public abstract class TPropertyTypeGet<T>
    {
        public virtual T Get(Args args) => default;
        public virtual T Get(GameObject gameObject) => this.Get(new Args(gameObject));

        public abstract string String { get; }
    }
}