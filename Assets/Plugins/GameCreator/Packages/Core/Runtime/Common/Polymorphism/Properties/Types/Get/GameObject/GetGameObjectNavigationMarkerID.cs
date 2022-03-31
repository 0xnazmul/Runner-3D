using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Marker by ID")]
    [Category("Navigation/Marker by ID")]
    
    [Image(typeof(IconID), ColorTheme.Type.TextNormal)]
    [Description("Reference to a scene Marker game object by its ID")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectNavigationMarkerID : PropertyTypeGetGameObject
    {
        [SerializeField] 
        private PropertyGetString m_ID = new PropertyGetString();

        public override GameObject Get(Args args) => this.GetObject(args);

        private GameObject GetObject(Args args)
        {
            string id = this.m_ID.Get(args);
            Marker marker = Marker.GetMarkerByID(id);
            
            return marker != null ? marker.gameObject : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectNavigationMarkerID instance = new GetGameObjectNavigationMarkerID();
            return new PropertyGetGameObject(instance);
        }

        public override string String => $"Marker ID:{this.m_ID}";
    }
}