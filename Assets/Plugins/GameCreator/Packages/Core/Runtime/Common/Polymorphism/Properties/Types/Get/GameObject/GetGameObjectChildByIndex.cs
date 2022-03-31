using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Game Object Child Index")]
    [Category("Transforms/Game Object Child Index")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayDot))]
    [Description("The N-th child of a game object")]

    [Serializable]
    public class GetGameObjectChildByIndex : PropertyTypeGetGameObject
    {
        [SerializeField] 
        private PropertyGetGameObject m_Transform = GetGameObjectPlayer.Create();
        
        [SerializeField] 
        private PropertyGetInteger m_Index = GetDecimalInteger.Create(0);

        public override GameObject Get(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            if (gameObject == null) return null;

            int index = (int) Math.Clamp(
                this.m_Index.Get(args), 
                0, gameObject.transform.childCount - 1
            );
            
            Transform child = gameObject.transform.GetChild(index);
            return child != null ? child.gameObject : null;
        }
        
        public static PropertyGetGameObject Create()
        {
            GetGameObjectChildByIndex instance = new GetGameObjectChildByIndex();
            return new PropertyGetGameObject(instance);
        }

        public override string String => $"{this.m_Transform}/{this.m_Index}";
    }
}