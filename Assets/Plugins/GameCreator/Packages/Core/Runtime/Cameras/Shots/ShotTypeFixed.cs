using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Fixed Position")]
    [Category("Fixed Position")]
    [Image(typeof(IconShotFixed), ColorTheme.Type.Blue)]
    
    [Description("The Camera is locked in place but can pivot around itself")]
    
    [Serializable]
    public class ShotTypeFixed : TShotTypeLook
    { }
}