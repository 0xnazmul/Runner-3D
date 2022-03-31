using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("On Step")]
    [Image(typeof(IconFootprint), ColorTheme.Type.Yellow)]
    
    [Category("Characters/On Step")]
    [Description("Executed every time the character takes a step")]

    [Serializable]
    public class EventOnCharacterStep : TEventCharacter
    {
        protected override void WhenEnabled(Trigger trigger, Character character)
        {
            character.Footsteps.EventStep += this.OnStep;
        }

        protected override void WhenDisabled(Trigger trigger, Character character)
        {
            character.Footsteps.EventStep -= this.OnStep;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnStep(Transform foot)
        {
            _ = this.m_Trigger.Execute(foot.gameObject);
        }
    }
}