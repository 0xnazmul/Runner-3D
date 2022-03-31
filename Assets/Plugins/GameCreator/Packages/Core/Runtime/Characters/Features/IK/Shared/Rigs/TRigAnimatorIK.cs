namespace GameCreator.Runtime.Characters.IK
{
    public abstract class TRigAnimatorIK : TRig
    {
        public sealed override bool OnUpdate(Character character)
        {
            return this.DoUpdate(character);
        }
    }
}