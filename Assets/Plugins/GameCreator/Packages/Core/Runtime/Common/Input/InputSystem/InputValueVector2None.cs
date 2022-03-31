using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("None")]
    [Category("None")]
    
    [Description("No input is executed")]
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]
    
    [Serializable]
    public class InputValueVector2None : TInputValueVector2
    {
        public override Vector2 Read() => Vector2.zero;
    }
}