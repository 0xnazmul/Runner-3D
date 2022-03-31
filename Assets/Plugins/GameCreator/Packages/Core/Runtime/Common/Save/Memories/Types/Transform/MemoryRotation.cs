using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Image(typeof(IconRotation), ColorTheme.Type.Green)]
    
    [Title("Rotation")]
    [Category("Transform/Rotation")]
    
    [Description("Remembers the rotation of the object")]

    [Serializable]
    public class MemoryRotation : Memory
    {
        public override string Title => "Rotation";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            return new TokenRotation(target);
        }

        public override void OnRemember(GameObject target, Token token)
        {
            if (token is TokenRotation tokenRotation)
            {
                target.transform.rotation = tokenRotation.Rotation;
            }
        }
    }
}