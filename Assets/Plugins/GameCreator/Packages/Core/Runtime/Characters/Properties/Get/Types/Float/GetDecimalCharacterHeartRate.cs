using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Heart Rate")]
    [Category("Characters/Heart Rate")]
    
    [Image(typeof(IconHeartBeat), ColorTheme.Type.Yellow)]
    [Description("The Character's Heart Rate value")]

    [Keywords("Float", "Decimal", "Double", "Tired", "Breathing", "Exertion", "Heart", "Rate")]
    [Serializable]
    public class GetDecimalCharacterHeartRate : PropertyTypeGetDecimal
    {
        [SerializeField]
        protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public override double Get(Args args) => this.GetValue(args);

        private float GetValue(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null ? character.Animim.HeartRate : 0f;
        }

        public GetDecimalCharacterHeartRate() : base()
        { }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalCharacterHeartRate()
        );

        public override string String => $"{this.m_Character} Heart-Rate";
    }
}