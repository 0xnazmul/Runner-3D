using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Remove Prop")]
    [Description("Removes a prefab Prop (if any) from a Character")]

    [Category("Characters/Visuals/Remove Prop")]

    [Parameter("Character", "The character target")]
    [Parameter("Prop", "The prefab object prop that is removed to the character")]

    [Keywords("Characters", "Detach", "Let", "Sheathe", "Put", "Holster", "Object")]
    [Image(typeof(IconTennis), ColorTheme.Type.TextLight, typeof(OverlayMinus))]

    [Serializable]
    public class InstructionCharacterRemoveProp : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        
        [Space]
        [SerializeField] private PropertyGetGameObject m_Prop = new PropertyGetGameObject();

        public override string Title => $"Remove {this.m_Prop} from {this.m_Character}";

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            GameObject prefab = this.m_Prop.Get(args);
            if (prefab == null) return DefaultResult;
            
            character.Props.Remove(prefab);
            return DefaultResult;
        }
    }
}