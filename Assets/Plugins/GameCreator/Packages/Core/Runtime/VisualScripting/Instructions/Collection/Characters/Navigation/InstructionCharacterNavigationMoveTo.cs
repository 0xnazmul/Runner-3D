using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Move To")]
    [Description("Instructs the Character to move to a new location")]

    [Category("Characters/Navigation/Move To")]
    
    [Parameter(
        "Wait to Finish", 
        "If true this Instruction waits until the Character reaches its destination or it is canceled"
    )]
    
    [Parameter(
        "Stop Distance", 
        "Distance to the destination that the Character considers it has reached the target"
    )]
    
    [Example(
        "The Stop Distance field is useful if you want [Character A] to approach another " +
        "[Character B]. With a Stop Distance of 0, [Character A] tries to occupy the same " +
        "space as the other one, bumping into it. Having a Stop Distance value of 2 allows " +
        "[Character A] to stop 2 units away from [Character B]'s position"
    )]

    [Keywords("Walk", "Run", "Position", "Location", "Destination")]
    [Image(typeof(IconCharacterWalk), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionCharacterNavigationMoveTo : TInstructionCharacterNavigation
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetLocation m_Location = GetLocationNavigationMarker.Create;

        [SerializeField] private bool m_WaitToArrive = true;
        [SerializeField] private float m_StopDistance = 0f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private bool m_MovementComplete;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Move {this.m_Character} to {this.m_Location}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            this.m_MovementComplete = false;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;
            if (character.Busy.AreLegsBusy) return;

            Location location = this.m_Location.Get(args);
            character.Motion.MoveToLocation(
                location, 
                this.m_StopDistance,
                this.OnFinish
            );

            if (this.m_WaitToArrive)
            {
                await this.Until(() => this.m_MovementComplete);
            }
        }

        private void OnFinish(Character character)
        {
            if (character != null) character.Busy.RemoveLegsBusy();
            this.m_MovementComplete = true;
        }
    }
}