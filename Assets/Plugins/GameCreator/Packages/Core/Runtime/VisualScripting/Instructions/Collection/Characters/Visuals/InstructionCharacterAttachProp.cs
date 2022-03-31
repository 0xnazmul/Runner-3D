using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Attach Prop")]
    [Description("Attaches a prefab Prop onto a Character's bone")]

    [Category("Characters/Visuals/Attach Prop")]

    [Parameter("Character", "The character target")]
    [Parameter("Prop", "The prefab object that is attached to the character")]
    [Parameter("Bone", "Which bone the prop is attached to")]
    [Parameter("Position", "Local offset from which the prop is distanced from the bone")]
    [Parameter("Rotation", "Local offset from which the prop is rotated from the bone")]

    [Keywords("Characters", "Add", "Grab", "Draw", "Pull", "Take", "Object")]
    [Image(typeof(IconTennis), ColorTheme.Type.Yellow)]

    [Serializable]
    public class InstructionCharacterAttachProp : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        
        [Space]
        [SerializeField] private PropertyGetGameObject m_Prop = new PropertyGetGameObject();

        [SerializeField] private Bone m_Bone = new Bone(HumanBodyBones.RightHand);
        
        [SerializeField] private Vector3 m_Position = Vector3.zero;
        [SerializeField] private Vector3 m_Rotation = Vector3.zero;

        public override string Title => $"Attach {this.m_Prop} on {this.m_Character} {this.m_Bone}";

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            GameObject prefab = this.m_Prop.Get(args);
            if (prefab == null) return DefaultResult;
            
            character.Props.Attach(
                this.m_Bone, 
                prefab, 
                this.m_Position,
                Quaternion.Euler(this.m_Rotation)
            );
            
            return DefaultResult;
        }
    }
}