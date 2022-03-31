using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public abstract class PolymorphicItemManipulator : MouseManipulator
    {
        [Serializable]
        protected class DragData
        {
            public TPolymorphicItemTool source;
        }

        // STATIC PROPERTIES: ---------------------------------------------------------------------

        private static readonly string DragDataType = typeof(DragData).ToString();
        
        protected static bool IsActive = false;

        // MEMBERS: -------------------------------------------------------------------------------

        protected readonly TPolymorphicItemTool m_Item;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected PolymorphicItemManipulator(TPolymorphicItemTool item)
        {
            this.m_Item = item;
            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected void SetSourceState(bool transparent)
        {
            DragData data = GetDragData();
            if (data == null) return;

            data.source.style.opacity = transparent ? 0.25f : 1f;
        }

        protected bool SamePolymorphicList()
        {
            IPolymorphicListTool sourceParentTool = GetDragData()?.source?.ParentTool;
            if (sourceParentTool == null) return false;

            return this.m_Item.ParentTool == sourceParentTool;
        }

        // STATIC METHODS: ------------------------------------------------------------------------

        protected static DragData GetDragData()
        {
            return DragAndDrop.GetGenericData(DragDataType) as DragData;
        }

        protected static void SetDragData(TPolymorphicItemTool item)
        {
            DragAndDrop.SetGenericData(DragDataType, new DragData
            {
                source = item
            });
        }
    }
}