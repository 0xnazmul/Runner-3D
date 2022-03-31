using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomEditor(typeof(Skeleton))]
    public class SkeletonEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.CHARACTERS + "StyleSheets/Skeleton";
        
        private static Texture2D TEX_PREVIEW_ACCEPT;
        private static Texture2D TEX_PREVIEW_REJECT;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private int m_DrawDragType;
        
        private GUIStyle m_StylePreviewText;

        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            StyleSheet[] styleSheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in styleSheets) this.m_Root.styleSheets.Add(styleSheet);

            SerializedProperty mass = this.serializedObject.FindProperty("m_Mass");
            SerializedProperty material = this.serializedObject.FindProperty("m_Material");
            SerializedProperty collision = this.serializedObject.FindProperty("m_CollisionDetection");
            SerializedProperty volumes = this.serializedObject.FindProperty("m_Volumes");
            
            PropertyField fieldMass = new PropertyField(mass);
            PropertyField fieldMaterial = new PropertyField(material);
            PropertyField fieldCollision = new PropertyField(collision);
            PropertyField fieldVolumes = new PropertyField(volumes);
            
            VisualElement space = new VisualElement();
            space.AddToClassList("gc-character-skeleton-space");

            this.m_Root.Add(fieldMass);
            this.m_Root.Add(fieldMaterial);
            this.m_Root.Add(fieldCollision);
            this.m_Root.Add(space);
            this.m_Root.Add(fieldVolumes);

            return this.m_Root;
        }
        
        // AUTO BUILD CHARACTER: ------------------------------------------------------------------

        private void BuildCharacter(Animator animator)
        {
            if (animator == null) return;
            
            Skeleton skeleton = this.serializedObject.targetObject as Skeleton;
            if (skeleton == null) return;

            if (skeleton.VolumesLength > 0)
            {
                bool replace = EditorUtility.DisplayDialog(
                    "Creating a new Skeleton layout will replace the current one.",
                    "Do you want to override the current Skeleton?",
                    "Yes", "Cancel"
                );
                
                if (!replace) return;
            }

            Volumes volumes = SkeletonBuilder.Make(animator);
            
            this.serializedObject.Update();
            this.serializedObject.FindProperty("m_Volumes").SetValue(volumes);
            
            this.serializedObject.ApplyModifiedProperties();
            this.serializedObject.Update();
            
            this.m_Root.Unbind();
            this.m_Root.Bind(this.serializedObject);
        }
        
        // PREVIEW WINDOW: ------------------------------------------------------------------------
        
        public override bool HasPreviewGUI() => true;
        
        public override GUIContent GetPreviewTitle() => new GUIContent("Build humanoid Skeleton");

        public override void DrawPreview(Rect previewArea)
        {
            if (!TEX_PREVIEW_ACCEPT) TEX_PREVIEW_ACCEPT = MakeTexture(ColorTheme.Type.Green, 0.25f);
            if (!TEX_PREVIEW_REJECT) TEX_PREVIEW_REJECT = MakeTexture(ColorTheme.Type.Red, 0.25f);

            if (this.m_DrawDragType == 2) GUI.DrawTexture(
                previewArea, TEX_PREVIEW_ACCEPT,
                ScaleMode.StretchToFill, true
            );
            
            if (this.m_DrawDragType == 1) GUI.DrawTexture(
                previewArea, TEX_PREVIEW_REJECT,
                ScaleMode.StretchToFill, true
            );
            
            this.m_StylePreviewText ??= new GUIStyle(EditorStyles.whiteLabel)
            {
                alignment = TextAnchor.MiddleCenter
            };
            
            EditorGUI.LabelField(
                previewArea,
                "Drop a scene Character or Animator to build Skeleton",
                this.m_StylePreviewText
            );
            
            EventType currentEvent = Event.current.type;
            switch (currentEvent)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    this.m_DrawDragType = 1;
                    if (!previewArea.Contains(Event.current.mousePosition))
                    {
                        this.m_DrawDragType = 0;
                        break;
                    }
            
                    if (DragAndDrop.objectReferences.Length == 1)
                    {
                        GameObject dropObject = DragAndDrop.objectReferences[0] as GameObject;
                        
                        // PrefabAssetType dropType = PrefabUtility.GetPrefabAssetType(dropObject);
                        // bool acceptPrefab = dropType != PrefabAssetType.Model &&
                        //                     dropType == ;
                        
                        if (dropObject != null && AcceptType(dropObject))
                        {
                            Animator animator = dropObject.GetComponentInChildren<Animator>();
                            if (animator != null)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                                this.m_DrawDragType = 2;
                                if (currentEvent == EventType.DragPerform)
                                {
                                    this.m_DrawDragType = 0;
                                    DragAndDrop.AcceptDrag();
                                    this.BuildCharacter(animator);
                                }
                            }
                        }
                    }
                    Event.current.Use();
                    break;
            
                case EventType.DragExited:
                    this.m_DrawDragType = 0;
                    break;
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static bool AcceptType(GameObject dropObject)
        {
            if (dropObject == null) return false;
            
            PrefabAssetType dropType = PrefabUtility.GetPrefabAssetType(dropObject);
            if (dropType == PrefabAssetType.NotAPrefab) return true;

            return PrefabUtility.IsPartOfNonAssetPrefabInstance(dropObject);
        }
        
        private static Texture2D MakeTexture(ColorTheme.Type colorType, float alpha = 1.0f)
        {
            Color color = ColorTheme.Get(colorType);
            
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(color.r, color.g, color.b, alpha));
            texture.Apply();
            return texture;
        }
    }
}
