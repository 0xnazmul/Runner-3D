using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Start Following")]
    [Description("Instructs a Character to follow another game object")]

    [Category("Characters/Navigation/Start Following")]

    [Parameter("Target", "The target game object to follow")]
    
    [Parameter(
        "Min Distance", 
        "Distance from the Target the Character aims to move when approaching the Target"
    )]
    
    [Parameter(
        "Max Distance", 
        "Maximum distance to the Target the Character leaves before attempting to move closer"
    )]
    
    [Keywords("Lead", "Pursue", "Chase", "Walk", "Run", "Position", "Location", "Destination")]
    [Image(typeof(IconCharacterRun), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionCharacterNavigationFollowStart : TInstructionCharacterNavigation
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectInstance.Create();

        [SerializeField] private float m_MinDistance = 2f;
        [SerializeField] private float m_MaxDistance = 4f;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_Character} Follow {this.m_Target}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;
            if (character.Busy.AreLegsBusy) return DefaultResult;
            
            GameObject target = this.m_Target.Get(args);
            if (target == null) return DefaultResult;
            
            character.Busy.MakeLegsBusy();
            character.Motion.StartFollowingTarget(
                target.transform,
                this.m_MinDistance,
                this.m_MaxDistance
            );

            return DefaultResult;
        }
    }
}