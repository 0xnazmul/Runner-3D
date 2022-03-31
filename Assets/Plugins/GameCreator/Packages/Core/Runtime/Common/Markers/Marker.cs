using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [HelpURL("https://docs.gamecreator.io/gamecreator/characters/markers")]
    [AddComponentMenu("Game Creator/Characters/Marker", 200)]
    
    [Icon(RuntimePaths.GIZMOS + "GizmoMarker.png")]
	public class Marker : MonoBehaviour, ISpatialHash
	{
        #if UNITY_EDITOR
        public const string KEY_MARKER_CAPSULE_HEIGHT = "gc:marker-capsule-height";
        public const string KEY_MARKER_CAPSULE_RADIUS = "gc:marker-capsule-radius";
        #endif
        
        // STATIC: --------------------------------------------------------------------------------
        
        private static Dictionary<IdString, Marker> Markers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemsInit()
        {
            Markers = new Dictionary<IdString, Marker>();
        }
        
        private static readonly Color COLOR_GIZMO_CAPSULE = new Color(
            Color.yellow.r,
            Color.yellow.g, 
            Color.yellow.b,
            0.5f
        ); 
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private float m_StopDistance = 0.01f;
        
        [SerializeField] private UniqueID m_UniqueID = new UniqueID();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float StopDistance => Mathf.Max(0f, this.m_StopDistance);

        public Vector3 Position => this.transform.position;
        public Vector3 Direction => this.transform.TransformDirection(Vector3.forward);
        
        public Quaternion Rotation => this.transform.rotation;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            Markers[this.m_UniqueID.Get] = this;
        }

        private void OnDestroy()
        {
            Markers.Remove(this.m_UniqueID.Get);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool IsWithinRange(Vector3 target, float stopThreshold = 0f)
        {
            float threshold = Mathf.Max(this.m_StopDistance, stopThreshold);
            return Vector3.Distance(this.Position, target) <= threshold;
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static Marker GetMarkerByID(string characterID)
        {
            IdString id = new IdString(characterID);
            return GetMarkerByID(id);
        }
        
        public static Marker GetMarkerByID(IdString characterID)
        {
            return Markers.TryGetValue(characterID, out Marker character)
                ? character
                : null;
        }
        
        // INTERFACE SPATIAL HASH: ----------------------------------------------------------------

        Vector3 ISpatialHash.Position => this.transform.position;
        int ISpatialHash.UniqueCode => this.gameObject.GetInstanceID();

        // GIZMOS: --------------------------------------------------------------------------------

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position + Vector3.up * 0.01f;
            Quaternion rotation = transform.rotation;

            Gizmos.color = Color.yellow;
            GizmosExtension.Arrow(position, rotation, 0.2f);
            GizmosExtension.Cross(position, GizmosExtension.CrossDirection.Upwards, 0.05f);
            GizmosExtension.Circle(position, 0.05f);

            if (this.m_StopDistance >= 0.2f)
            {
                GizmosExtension.Circle(position, this.m_StopDistance);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = COLOR_GIZMO_CAPSULE;
            float radius = 0.2f;
            float height = 2f;
            
            #if UNITY_EDITOR
            radius = UnityEditor.EditorPrefs.GetFloat(KEY_MARKER_CAPSULE_RADIUS, 0.2f);
            height = UnityEditor.EditorPrefs.GetFloat(KEY_MARKER_CAPSULE_HEIGHT, 2f);
            #endif
            
            GizmosExtension.Cylinder(this.transform.position, height, radius);
        }
    }
}