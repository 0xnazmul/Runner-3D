using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Exertion")]
    [Category("Characters/Exertion")]
    
    [Image(typeof(IconExertion), ColorTheme.Type.Yellow)]
    [Description("The Character's Exertion value")]

    [Keywords("Float", "Decimal", "Double", "Tired", "Heart", "Rate")]
    [Serializable]
    public class GetDecimalCharacterExertion : PropertyTypeGetDecimal
    {
        [SerializeField]
        protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public override double Get(Args args) => this.GetValue(args);

        private float GetValue(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null ? character.Animim.Exertion : 0f;
        }

        public GetDecimalCharacterExertion() : base()
        { }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalCharacterExertion()
        );

        public override string String => $"{this.m_Character} Exertion";
    }
}