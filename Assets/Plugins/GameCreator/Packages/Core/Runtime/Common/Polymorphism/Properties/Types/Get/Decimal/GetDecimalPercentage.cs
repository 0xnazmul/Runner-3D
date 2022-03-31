using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Percentage")]
    [Category("Percentage")]
    
    [Image(typeof(IconPercent), ColorTheme.Type.TextNormal)]
    [Description("A fixed percentage value")]

    [Keywords("Float", "Decimal", "Double")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalPercentage : PropertyTypeGetDecimal
    {
        [SerializeField] protected PercentageWithoutLabel m_Value = new PercentageWithoutLabel();

        public override double Get(Args args) => this.m_Value.UnitRatio;
        public override double Get(GameObject gameObject) => this.m_Value.UnitRatio;
        
        public GetDecimalPercentage() : base()
        { }

        public GetDecimalPercentage(float percent) : this()
        {
            this.m_Value = new PercentageWithoutLabel(percent);
        }

        public static PropertyGetDecimal Create(float unit = 1f) => new PropertyGetDecimal(
            new GetDecimalPercentage(unit)
        );

        public override string String => this.m_Value.ToString();
    }
}