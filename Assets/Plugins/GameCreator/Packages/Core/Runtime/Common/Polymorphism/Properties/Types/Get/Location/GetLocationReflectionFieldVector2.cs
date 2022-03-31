using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Field Vector2")]
    [Category("Reflection/Field Vector2")]
    
    [Image(typeof(IconComponent), ColorTheme.Type.Yellow)]
    [Description("A 'Vector2' value of a public or private field of a component")]

    [Keywords("Component", "Script", "Property", "Member", "Variable", "Value")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetLocationReflectionFieldVector2 : PropertyTypeGetLocation
    {
        [SerializeField] private ReflectionFieldVector2 m_Field = new ReflectionFieldVector2();

        public override Location Get(Args args) => new Location(this.m_Field.Value);

        public override string String => this.m_Field.ToString();
    }
}