using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Installs
{
    public class InstallerContentList : VisualElement
    {
        private const string LIST_NAME = "GC-Install-Content-List";

        private const string ELEMENT_NAME_ICON = "GC-Install-Content-Element-Icon";
        private const string ELEMENT_NAME_TITLE = "GC-Install-Content-Element-Title";
        private const string ELEMENT_NAME_VERSION = "GC-Install-Content-Element-Version";
        private const string ELEMENT_NAME_STATUS = "GC-Install-Content-Element-Status";

        private static readonly IIcon ICON_INSTRUCTION = new IconInstructions(ColorTheme.Type.Blue);
        private static readonly IIcon ICON_CONDITION = new IconConditions(ColorTheme.Type.Green);
        private static readonly IIcon ICON_EVENT = new IconTriggers(ColorTheme.Type.Red);
        private static readonly IIcon ICON_NONE = new IconNone(ColorTheme.Type.White);

        private static readonly IIcon ICON_CHECK = new IconCheckmark(ColorTheme.Type.Green);
        private static readonly IIcon ICON_UPDATE = new IconDownload(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_UPLOAD = new IconUpload(ColorTheme.Type.Green);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly InstallerManagerWindow m_Window;

        private Label m_SearchingLabel;
        private ProgressBar m_SearchingProgress;
        
        private ScrollView m_ScrollView;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public InstallerContentList(InstallerManagerWindow window)
        {
            this.m_Window = window;
        }

        internal void OnEnable()
        {
            this.m_ScrollView = new ScrollView(ScrollViewMode.Vertical)
            {
                name = LIST_NAME
            };
            
            this.Add(this.m_ScrollView);
        }

        internal void OnDisable()
        {
            
        }
        
        internal void Refresh()
        {
            this.m_ScrollView.contentContainer.Clear();
            foreach (KeyValuePair<string,List<Installer>> entry in this.m_Window.InstallAssetsMap)
            {
                this.MakeModule(entry.Key, entry.Value);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void MakeModule(string name, List<Installer> assets)
        {
            InstallerElementModule module = new InstallerElementModule(
                this.m_Window,
                name, 
                assets
            );
            
            this.m_ScrollView.contentContainer.Add(module);
        }
    }
}