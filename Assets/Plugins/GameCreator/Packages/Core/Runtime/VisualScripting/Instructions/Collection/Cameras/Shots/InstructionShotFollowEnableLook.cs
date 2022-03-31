using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    [Category("Cameras/Shots/Follow Target/Enable Look")]
    [Image(typeof(IconShotFollow), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionShotFollowEnableLook : TInstructionShotEnableLook
    { }
}