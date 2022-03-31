using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Euler Angles")]
    [Category("Euler Angles")]
    
    [Image(typeof(IconRotation), ColorTheme.Type.Yellow)]
    [Description("Rotation from the euler angle of each individual axis in world space")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationEuler : PropertyTypeGetRotation
    {
        [SerializeField] private Vector3 m_Rotation = new Vector3(0f, 180f, 0f);
        
        public override Quaternion Get(Args args) => Quaternion.Euler(this.m_Rotation);
        public override Quaternion Get(GameObject gameObject) => Quaternion.Euler(this.m_Rotation);

        public GetRotationEuler()
        { }
        
        public GetRotationEuler(Vector3 angles)
        {
            this.m_Rotation = angles;
        }
        
        public GetRotationEuler(float x, float y, float z)
        {
            this.m_Rotation = new Vector3(x, y, z);
        }
        
        public GetRotationEuler(Transform transform)
        {
            this.m_Rotation = transform != null 
                ? transform.rotation.eulerAngles
                : Vector3.zero;
        }
        
        public static PropertyGetRotation Create => new PropertyGetRotation(
            new GetRotationEuler()
        );

        public override string String => $"Angles {this.m_Rotation}";
    }
}