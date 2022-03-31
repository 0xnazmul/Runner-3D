using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Position & Rotation")]
    [Category("Position & Rotation")]
    
    [Image(typeof(IconLocation), ColorTheme.Type.Yellow)]
    [Description("A point in space and euler rotation representing a location in the scene")]

    [Serializable]
    public class GetLocationPositionRotation : PropertyTypeGetLocation
    {
        [SerializeField] private Vector3 m_Position = Vector3.zero;
        [SerializeField] private Vector3 m_Rotation = Vector3.zero;
        
        public override Location Get(Args args) => this.GetLocation();
        public override Location Get(GameObject gameObject) => this.GetLocation();

        private Location GetLocation()
        {
            return new Location(
                this.m_Position, 
                Quaternion.Euler(this.m_Rotation)
            );
        }
        
        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationPositionRotation()
        );

        public override string String => $"{this.m_Position} & {this.m_Rotation}";
    }
}