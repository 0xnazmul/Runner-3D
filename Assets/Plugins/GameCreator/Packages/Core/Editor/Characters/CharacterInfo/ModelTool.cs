using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    public class ModelTool : VisualElement
    {
        private const string USS_PATH = EditorPaths.CHARACTERS + "StyleSheets/Model";

        private const string PATH_DEFAULT_RTC = RuntimePaths.CHARACTERS + 
                                                "Assets/Controllers/CompleteLocomotion.controller";

        private const string NAME_ELEMENT = "GC-Character-Model";
        private const string NAME_DROPZONE = "GC-Character-Model-Dropzone";

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly SerializedProperty m_Property;
        
        // FIELDS: --------------------------------------------------------------------------------

        public readonly Character character;
        public readonly VisualElement dropzone;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ModelTool(SerializedProperty property)
        {
            this.m_Property = property;
            this.character = this.m_Property.serializedObject.targetObject as Character;

            this.name = NAME_ELEMENT;
            
            this.dropzone = new VisualElement
            {
                name = NAME_DROPZONE
            };
            
            DropModelManipulator manipulator = new DropModelManipulator(this);
            
            this.dropzone.AddManipulator(manipulator);
            
            Label dropZone = new Label("Drop a 3D model");
            
            this.dropzone.Add(dropZone);
            this.Add(this.dropzone);

            StyleSheet[] styleSheets = StyleSheetUtils.Load(USS_PATH);
            foreach (var styleSheet in styleSheets) this.styleSheets.Add(styleSheet);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void ChangeModelEditor(GameObject prefab, Vector3 offset)
        {
            this.m_Property.serializedObject.Update();

            if (this.character.Animim.Animator != null)
            {
                Object.DestroyImmediate(this.character.Animim.Animator.gameObject);
            }

            GameObject model = Object.Instantiate(prefab, this.character.transform);
            Vector3 position = Vector3.down * this.character.Motion.Height * 0.5f;

            model.name = prefab.name;
            model.transform.localPosition = position + offset;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;

            this.m_Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            this.m_Property.serializedObject.Update();

            Animator animator = model.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController == null)
            {
                var rtc = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(PATH_DEFAULT_RTC);
                animator.runtimeAnimatorController = rtc;
            }

            var propertyAnimator = this.m_Property.FindPropertyRelative("m_Animator");
            propertyAnimator.objectReferenceValue = animator;

            SerializationUtils.ApplyUnregisteredSerialization(this.m_Property.serializedObject);
        }
    }
}