using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public class PolymorphicItemDrop : PolymorphicItemManipulator
    {
        private const string TXT_UNDO_MOVE = "Move List Element";

        // INITIALIZERS: --------------------------------------------------------------------------

        public PolymorphicItemDrop(TPolymorphicItemTool item) : base(item)
        { }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<DragEnterEvent>(OnDragEnter);
            target.RegisterCallback<DragLeaveEvent>(OnDragLeave);

            target.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
            target.RegisterCallback<DragPerformEvent>(OnDragPerform);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
            target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);

            target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);
            target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
        }

        // EVENT METHODS: -------------------------------------------------------------------------

        private void OnDragEnter(DragEnterEvent dragEvent)
        {
            if (!IsActive) return;
            if (!this.SamePolymorphicList()) return;

            this.SetSourceState(true);
            this.UpdateDropZone(dragEvent.mousePosition);
        }

        private void OnDragLeave(DragLeaveEvent dragEvent)
        {
            if (!IsActive) return;
            this.SetSourceState(false);

            this.m_Item.DropAbove.style.display = DisplayStyle.None;
            this.m_Item.DropBelow.style.display = DisplayStyle.None;
        }

        private void OnDragUpdate(DragUpdatedEvent dragEvent)
        {
            if (!IsActive) return;
            if (!this.SamePolymorphicList()) return;

            this.UpdateDropZone(dragEvent.mousePosition);            
        }

        private void OnDragPerform(DragPerformEvent dragEvent)
        {
            if (!IsActive) return;
            DragAndDrop.AcceptDrag();

            this.MakeMovement(dragEvent);
            this.FinalizeDrag();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void MakeMovement(IMouseEvent dragEvent)
        {
            float position = this.GetMousePercentagePosition(dragEvent.mousePosition);

            DragData source = GetDragData();

            if (source?.source == null) return;
            if (!this.SamePolymorphicList()) return;

            int sourceIndex = source.source.Index;
            int destinationIndex = this.m_Item.Index;

            if (position > 0.5f) destinationIndex += 1;
            if (sourceIndex < destinationIndex) destinationIndex -= 1;
            
            this.m_Item.ParentTool.MoveItems(sourceIndex, destinationIndex);
        }

        private void UpdateDropZone(Vector2 mousePosition)
        {
            if (GetDragData().source == this.m_Item) return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            float position = this.GetMousePercentagePosition(mousePosition);

            if (position <= 0.5f)
            {
                this.m_Item.DropAbove.style.display = DisplayStyle.Flex;
                this.m_Item.DropBelow.style.display = DisplayStyle.None;
            }
            else
            {
                this.m_Item.DropAbove.style.display = DisplayStyle.None;
                this.m_Item.DropBelow.style.display = DisplayStyle.Flex;
            }
        }

        private float GetMousePercentagePosition(Vector2 mousePosition)
        {
            Rect bounds = this.m_Item.worldBound;
            float position = (mousePosition.y - bounds.y) / bounds.height;

            position = Math.Max(0f, position);
            position = Math.Min(1f, position);

            return position;
        }

        private void FinalizeDrag()
        {
            this.SetSourceState(false);

            this.m_Item.DropAbove.style.display = DisplayStyle.None;
            this.m_Item.DropBelow.style.display = DisplayStyle.None;

            IsActive = false;
        }
    }
}