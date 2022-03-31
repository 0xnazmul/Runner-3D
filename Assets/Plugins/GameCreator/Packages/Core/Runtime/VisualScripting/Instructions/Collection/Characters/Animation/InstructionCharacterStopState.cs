using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Stop State")]
    [Description("Stops an animation State from a Character")]

    [Category("Characters/Animation/Stop State")]

    [Parameter("Character", "The character that stops its animation State")]
    [Parameter("Layer", "Slot number from which the state is removed")]

    [Parameter("Delay", "Amount of seconds to wait before the animation stops playing")]
    [Parameter("Transition", "The amount of seconds the animation takes to blend out")]

    [Keywords("Characters", "Animation", "Animate", "State", "Exit", "Stop")]
    [Image(typeof(IconCharacterState), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionCharacterStopState : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [Space]
        [SerializeField] private PropertyGetInteger m_Layer = new PropertyGetInteger(1);

        [Space] 
        [SerializeField] private float m_Delay = 0f;
        [SerializeField] private float m_Transition = 0.1f;

        public override string Title => $"Stop state on {this.m_Character}";

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            int layer = (int) this.m_Layer.Get(args);
            character.States.Stop(layer, this.m_Delay, this.m_Transition);
            return DefaultResult;
        }
    }
}