using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public class PolymorphicItemDrag : PolymorphicItemManipulator
    {
        public PolymorphicItemDrag(TPolymorphicItemTool item) : base(item)
        { }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(this.OnMouseDown);
            target.RegisterCallback<DragExitedEvent>(this.OnDragExit);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<DragExitedEvent>(this.OnDragExit);
        }

        // EVENT METHODS: -------------------------------------------------------------------------

        private void OnMouseDown(MouseDownEvent mouseEvent)
        {
            if (CanStartManipulation(mouseEvent))
            {
                DragAndDrop.PrepareStartDrag();

                SetDragData(this.m_Item);

                DragAndDrop.StartDrag(this.m_Item.Title);
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;

                SetSourceState(true);
                IsActive = true;
            }
        }
        
        private void OnDragExit(DragExitedEvent evt)
        {
            SetSourceState(false);
        }
    }
}