using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public struct Location
    {
        public enum Type
        {
            Position,
            Transform,
            Marker   
        }
    
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Vector3 m_Position;
        [SerializeField] private Quaternion m_Rotation;
        
        [SerializeField] private bool m_HasPosition;
        [SerializeField] private bool m_HasRotation;

        // PUBLIC PROPERTIES: ---------------------------------------------------------------------

        public Vector3 Position
        {
            get
            {
                switch (this.LocationType)
                {
                    case Type.Transform:
                        if (this.Transform == null) return Vector3.zero;
                        return this.Transform.position + 
                               this.Transform.TransformDirection(this.LocalOffset) +
                               this.WorldOffset;
                    case Type.Marker:
                        if (this.Marker == null) return Vector3.zero;
                        return this.Marker.Position + 
                               this.Marker.transform.TransformDirection(this.LocalOffset) +
                               this.WorldOffset;
                    case Type.Position:
                        return this.m_Position;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Quaternion Rotation => this.LocationType switch
        {
            Type.Transform when this.Transform != null => this.Transform.rotation,
            Type.Marker when this.Marker != null => this.Marker.Rotation,
            _ => this.m_Rotation
        };
        
        public bool HasPosition => this.LocationType switch
        {
            Type.Transform when this.Transform != null => true,
            Type.Marker when this.Marker != null => true,
            _ => this.m_HasPosition
        };
        
        public bool HasRotation => this.LocationType switch
        {
            Type.Transform when this.Transform != null => true,
            Type.Marker when this.Marker != null => true,
            _ => this.m_HasRotation
        };

        // PRIVATE PROPERTIES: --------------------------------------------------------------------
        
        private Type LocationType { get; }
        
        private Transform Transform { get; }
        private Marker Marker { get; }
        
        private Vector3 LocalOffset { get; }
        private Vector3 WorldOffset { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Location(bool hasPosition, bool hasRotation, 
            Vector3 position, Quaternion rotation) : this()
        {
            this.m_Position = position;
            this.m_Rotation = rotation;
            
            this.m_HasPosition = hasPosition;
            this.m_HasRotation = hasRotation;

            this.LocationType = Type.Position;
            this.Transform = null;
            this.Marker = null;
            
            LocalOffset = default;
            WorldOffset = default;
        }
        
        public Location(Vector3 position, Quaternion rotation) : this()
        {
            this.m_Position = position;
            this.m_Rotation = rotation;
            
            this.m_HasPosition = true;
            this.m_HasRotation = true;

            this.LocationType = Type.Position;
            this.Transform = null;
            this.Marker = null;
            
            LocalOffset = default;
            WorldOffset = default;
        }
        
        public Location(Vector3 position)
        {
            this.m_Position = position;
            this.m_Rotation = Quaternion.identity;
            
            this.m_HasPosition = true;
            this.m_HasRotation = false;
            
            this.LocationType = Type.Position;
            this.Transform = null;
            this.Marker = null;
            
            LocalOffset = default;
            WorldOffset = default;
        }
        
        public Location(Quaternion rotation)
        {
            this.m_Position = Vector3.zero;
            this.m_Rotation = rotation;
            
            this.m_HasPosition = false;
            this.m_HasRotation = true;
            
            this.LocationType = Type.Position;
            this.Transform = null;
            this.Marker = null;
            
            LocalOffset = default;
            WorldOffset = default;
        }

        public Location(Marker marker, Vector3 offset) : this()
        {
            if (marker == null) return;

            this.m_Position = marker.Position;
            this.m_Rotation = marker.Rotation;
            
            this.m_HasPosition = true;
            this.m_HasRotation = true;
            
            this.LocationType = Type.Marker;
            this.Transform = null;
            this.Marker = marker;
            
            LocalOffset = offset;
            WorldOffset = default;
        }

        public Location(GameObject gameObject, Vector3 offset) : this()
        {
            if (gameObject == null) return;

            this.m_Position = gameObject.transform.position;
            this.m_Rotation = gameObject.transform.rotation;
            
            this.m_HasPosition = true;
            this.m_HasRotation = true;
            
            this.LocationType = Type.Transform;
            this.Transform = gameObject.transform;
            this.Marker = null;
            
            LocalOffset = offset;
            WorldOffset = default;
        }
        
        public Location(Transform transform, Vector3 offset) : this()
        {
            if (transform == null) return;

            this.m_Position = transform.position;
            this.m_Rotation = transform.rotation;
            
            this.m_HasPosition = true;
            this.m_HasRotation = true;
            
            this.LocationType = Type.Transform;
            this.Transform = transform;
            this.Marker = null;
            
            LocalOffset = offset;
            WorldOffset = default;
        }
    }
}