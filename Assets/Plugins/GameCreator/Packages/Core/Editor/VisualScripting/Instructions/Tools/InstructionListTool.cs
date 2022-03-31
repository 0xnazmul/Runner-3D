using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Editor.VisualScripting
{
    public class InstructionListTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Instruction-List-Foot-Add";
        
        private const string CLASS_INSTRUCTION_RUNNING = "gc-list-item-head-running";

        private static readonly IIcon ICON_PASTE = new IconPaste(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_PLAY = new IconPlay(ColorTheme.Type.TextNormal);

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly InstructionList m_Instructions;
        
        protected Button m_ButtonAdd;
        protected Button m_ButtonPaste;
        protected Button m_ButtonPlay;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-Instruction-List-Head";
        protected override string ElementNameBody => "GC-Instruction-List-Body";
        protected override string ElementNameFoot => "GC-Instruction-List-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.VISUAL_SCRIPTING + "Instructions/StyleSheets/Instructions-List"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => true;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => true;
        public override bool AllowCopyPaste => true;
        public override bool AllowInsertion => true;
        public override bool AllowBreakpoint => true;
        public override bool AllowDisable => true;
        public override bool AllowDocumentation => true;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public InstructionListTool(SerializedProperty property)
            : base(property, "m_Instructions")
        {
            this.SerializedObject.Update();
            
            this.m_Instructions = property.GetValue<InstructionList>();
            
            EditorApplication.playModeStateChanged += this.OnChangePlayMode;

            this.OnChangePlayMode(EditorApplication.isPlaying
                ? PlayModeStateChange.EnteredPlayMode
                : PlayModeStateChange.ExitingPlayMode
            );
        }
        
        ~InstructionListTool()
        {
            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new InstructionItemTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();
            
            this.m_ButtonAdd = new TypeSelectorElementInstruction(this.PropertyList, this)
            {
                name = NAME_BUTTON_ADD
            };
            
            this.m_ButtonPaste = new Button(() =>
            {
                if (!CopyPasteUtils.CanSoftPaste(typeof(Instruction))) return;
                
                int pasteIndex = this.PropertyList.arraySize;
                this.InsertItem(pasteIndex, CopyPasteUtils.SourceObject);
            })
            {
                name = "GC-Instruction-List-Foot-Button"
            };
            
            this.m_ButtonPaste.Add(new Image
            {
                image = ICON_PASTE.Texture
            });
            
            this.m_ButtonPlay = new Button(() => this.m_Instructions?.Run(null))
            {
                name = "GC-Instruction-List-Foot-Button"
            };
            
            this.m_ButtonPlay.Add(new Image
            {
                image = ICON_PLAY.Texture
            });
            
            this.m_Foot.Add(this.m_ButtonAdd);
            this.m_Foot.Add(this.m_ButtonPaste);
            this.m_Foot.Add(this.m_ButtonPlay);
        }

        private void OnChangePlayMode(PlayModeStateChange state)
        {
            this.m_ButtonPlay?.SetEnabled(state == PlayModeStateChange.EnteredPlayMode);
            if (this.m_Instructions == null) return;

            this.m_Instructions.EventRunInstruction -= this.OnRunInstruction;
            this.m_Instructions.EventEndRunning -= this.OnEndRunning;

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (this.m_Instructions.IsRunning)
                {
                    this.OnRunInstruction(this.m_Instructions.RunningIndex);
                }
                
                this.m_Instructions.EventRunInstruction += this.OnRunInstruction;
                this.m_Instructions.EventEndRunning += this.OnEndRunning;
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnRunInstruction(int index)
        {
            foreach (VisualElement child in this.m_Body.Children())
            {
                child.RemoveFromClassList(CLASS_INSTRUCTION_RUNNING);
            }
            
            if (this.m_Body.childCount <= index) return;
            this.m_Body[index].AddToClassList(CLASS_INSTRUCTION_RUNNING);
        }

        private void OnEndRunning()
        {
            foreach (VisualElement child in this.m_Body.Children())
            {
                child.RemoveFromClassList(CLASS_INSTRUCTION_RUNNING);
            }
        }
    }
}