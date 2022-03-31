using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCreator.Editor.Characters
{
    [CustomEditor(typeof(Character), true)]
    public class CharacterEditor : UnityEditor.Editor
    {
        private const string NAME_GROUP_GENERAL   = "GC-Character-GroupGeneral";
        private const string NAME_GROUP_GENERAL_L = "GC-Character-GroupGeneral-L";
        private const string NAME_GROUP_GENERAL_R = "GC-Character-GroupGeneral-R";

        private const string PATH_USS = EditorPaths.CHARACTERS + "StyleSheets/Character";
        
        private const string MODEL_PATH = RuntimePaths.CHARACTERS + "Assets/3D/Mannequin.fbx";
        private const string SKELETON_PATH = RuntimePaths.CHARACTERS + "Assets/3D/Skeleton.asset";
        private const string FOOTSTEPS_PATH = RuntimePaths.CHARACTERS + "Assets/3D/Footsteps.asset";
        private const string RTC_PATH = RuntimePaths.CHARACTERS + "Assets/Controllers/CompleteLocomotion.controller";
        
        // INSPECTOR: -----------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty propertyIsPlayer = this.serializedObject.FindProperty("m_IsPlayer");
            SerializedProperty propertyTime = this.serializedObject.FindProperty("m_Time");
            SerializedProperty propertyBusy = this.serializedObject.FindProperty("m_Busy");

            VisualElement groupGeneral  = new VisualElement { name = NAME_GROUP_GENERAL   };
            VisualElement groupGeneralL = new VisualElement { name = NAME_GROUP_GENERAL_L };
            VisualElement groupGeneralR = new VisualElement { name = NAME_GROUP_GENERAL_R };
            
            root.Add(groupGeneral);
            groupGeneral.Add(groupGeneralL);
            groupGeneral.Add(groupGeneralR);
            
            groupGeneralL.Add(new PropertyField(propertyBusy));
            groupGeneralR.Add(new PropertyField(propertyIsPlayer));
            groupGeneralR.Add(new PropertyField(propertyTime));
            
            SerializedProperty propertyKernel = this.serializedObject.FindProperty("m_Kernel");
            SerializedProperty propertyIK = this.serializedObject.FindProperty("m_InverseKinematics");
            SerializedProperty propertyFootsteps = this.serializedObject.FindProperty("m_Footsteps");
            SerializedProperty propertyRagdoll = this.serializedObject.FindProperty("m_Ragdoll");
            SerializedProperty propertyUniqueID = this.serializedObject.FindProperty("m_UniqueID");
            
            PropertyField fieldKernel = new PropertyField(propertyKernel);
            PropertyField fieldIK = new PropertyField(propertyIK);
            PropertyField fieldFootsteps = new PropertyField(propertyFootsteps);
            PropertyField fieldRagdoll = new PropertyField(propertyRagdoll);
            PropertyField fieldUniqueID = new PropertyField(propertyUniqueID);
            
            root.Add(fieldKernel);
            root.Add(fieldIK);
            root.Add(fieldFootsteps);
            root.Add(fieldRagdoll);
            root.Add(fieldUniqueID);
            
            StyleSheet[] styleSheets = StyleSheetUtils.Load(PATH_USS);
            foreach (StyleSheet styleSheet in styleSheets) root.styleSheets.Add(styleSheet);

            return root;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------

        private static void MakeCharacter(MenuCommand menuCommand, bool isPlayer)
        {
            GameObject instance = new GameObject(isPlayer ? "Player" : "Character");
            Character character = instance.AddComponent<Character>();

            float height = character.Motion.Height;
            character.transform.position += Vector3.up * (height * 0.5f);
            
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(MODEL_PATH);
            var skeleton = AssetDatabase.LoadAssetAtPath<Skeleton>(SKELETON_PATH);
            var footsteps = AssetDatabase.LoadAssetAtPath<MaterialSoundsAsset>(FOOTSTEPS_PATH);
            var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(RTC_PATH);

            character.ChangeModel(prefab, new Character.ChangeOptions 
            {
                controller = controller,
                materials = footsteps,
                skeleton = skeleton,
                offset = Vector3.zero
            });
            
            character.IsPlayer = isPlayer;

            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
        
        [MenuItem("GameObject/Game Creator/Characters/Character", false, 0)]
        public static void CreateCharacter(MenuCommand menuCommand)
        {
            MakeCharacter(menuCommand, false);
        }
        
        [MenuItem("GameObject/Game Creator/Characters/Player", false, 0)]
        public static void CreatePlayer(MenuCommand menuCommand)
        {
            MakeCharacter(menuCommand, true);
        }
    }
}