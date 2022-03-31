using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Preset")]
    public interface IKernelPreset
    {
        IUnitPlayer MakePlayer { get; }
        IUnitMotion MakeMotion { get; }
        IUnitDriver MakeDriver { get; }
        IUnitFacing MakeFacing { get; }
        IUnitAnimim MakeAnimim { get; }
    }
}