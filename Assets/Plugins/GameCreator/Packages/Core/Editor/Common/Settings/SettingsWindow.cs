using System;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    internal class SettingsWindow : EditorWindow
    {
        private const string MENU_ITEM = "Game Creator/Settings #%k";
        private const string MENU_TITLE = "Game Creator Settings";

        private const int MIN_WIDTH = 800;
        private const int MIN_HEIGHT = 600;
        
        private const int FIXED_PANEL_INDEX = 0;
        private const float FIXED_PANEL_WIDTH = 250f;

        private const string USS_PATH = EditorPaths.COMMON + "Settings/Stylesheets/SettingsWindow";
        
        private const string KEY_CACHE_INDEX = "gcset:cache-index";

        private static IIcon ICON_WINDOW;
        private static SettingsWindow WINDOW;

        // PROPERTIES: ----------------------------------------------------------------------------

        private static int CacheIndex
        {
            get => EditorPrefs.GetInt(KEY_CACHE_INDEX, 0);
            set => EditorPrefs.SetInt(KEY_CACHE_INDEX, value);
        }

        internal SettingsContentList ContentList { get; set; }
        internal SettingsContentDetails ContentDetails { get; set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int> EventChangeSelection;

        // INITIALIZERS: --------------------------------------------------------------------------
        
        [MenuItem(MENU_ITEM, priority = 10)]
        public static void OpenWindow()
        {
            if (WINDOW != null) WINDOW.Close();
            
            WINDOW = GetWindow<SettingsWindow>();
            WINDOW.minSize = new Vector2(MIN_WIDTH, MIN_HEIGHT);
        }

        public static void OpenWindow(string repositoryID)
        {
            OpenWindow();
            
            int index = WINDOW.ContentList.FindIndex(repositoryID);
            if (index < 0) return;
            
            WINDOW.ContentList.Index = index;
        }

        private void OnEnable()
        {
            ICON_WINDOW ??= new IconWindowSettings(ColorTheme.Type.TextLight);
            this.titleContent = new GUIContent(MENU_TITLE, ICON_WINDOW.Texture);
            
            StyleSheet[] styleSheetsCollection = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in styleSheetsCollection)
            {
                this.rootVisualElement.styleSheets.Add(styleSheet);
            }

            TwoPaneSplitView splitView = new TwoPaneSplitView(
                FIXED_PANEL_INDEX,
                FIXED_PANEL_WIDTH,
                TwoPaneSplitViewOrientation.Horizontal
            );

            this.rootVisualElement.Add(splitView);
            
            this.ContentList = new SettingsContentList(this);
            this.ContentDetails = new SettingsContentDetails(this);

            splitView.Add(this.ContentList);
            splitView.Add(this.ContentDetails);
            
            this.ContentList.OnEnable();
            this.ContentDetails.OnEnable();
        }

        private void OnDisable()
        {
            this.ContentList?.OnDisable();
            this.ContentDetails?.OnDisable();
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        public void OnChangeSelection(int index)
        {
            CacheIndex = index;
            this.EventChangeSelection?.Invoke(index);
        }
    }
}
