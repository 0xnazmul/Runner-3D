using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("3D Character Controller")]
    [Image(typeof(IconCharacter), ColorTheme.Type.Green)]
    
    [Category("3D Character Controller")]
    [Description("Configures the default 3D character controller")]

    [Serializable]
    public class KernelPreset3DController : IKernelPreset
    {
        public IUnitPlayer MakePlayer => new UnitPlayerDirectional();
        public IUnitMotion MakeMotion => new UnitMotionController();
        public IUnitDriver MakeDriver => new UnitDriverController();
        public IUnitFacing MakeFacing => new UnitFacingPivot();
        public IUnitAnimim MakeAnimim => new UnitAnimimKinematic();
    }
}