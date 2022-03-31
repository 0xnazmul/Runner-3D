using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Random Range")]
    [Category("Random/Random Range")]
    
    [Image(typeof(IconDice), ColorTheme.Type.TextNormal)]
    [Description("A random decimal number between two values (range is inclusive)")]
    
    [Parameter("Min Value", "The smallest value the random operation returns")]
    [Parameter("Max Value", "The largest value the random operation returns")]

    [Keywords("Float", "Decimal", "Double")]
    [Serializable]
    public class GetDecimalRandomRange : PropertyTypeGetDecimal
    {
        [SerializeField] private double m_MinValue = 5f;
        [SerializeField] private double m_MaxValue = 10f;
        
        public override double Get(Args args)
        {
            System.Random random = new System.Random();
            return random.NextDouble() * (this.m_MaxValue - this.m_MinValue) + this.m_MinValue;
        }

        public GetDecimalRandomRange() : base()
        { }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(new GetDecimalRandomRange());

        public override string String => $"Random({this.m_MinValue}, {this.m_MaxValue})";
    }
}