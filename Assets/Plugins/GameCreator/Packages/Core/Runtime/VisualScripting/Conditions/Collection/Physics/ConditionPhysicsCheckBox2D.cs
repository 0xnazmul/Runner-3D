using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Check Box 2D")]
    [Description("Returns true if casting a 2D box at a position doesn't collide with anything")]

    [Category("Physics/Check Box 2D")]
    
    [Parameter("Position", "The scene position where the box's center is cast. Z axis is ignored")]
    [Parameter("Size", "Size of each side's extension along its local axis")]
    [Parameter("Angle", "Cloc-wise rotation measured in degrees")]
    [Parameter("Layer Mask", "A bitmask that skips any objects that don't belong to the list")]
    
    [Example(
        "Note that this Instruction uses Unity's 2D physics engine. " +
        "It won't collide with any 3D objects"
    )]

    [Keywords("Check", "Collide", "Touch", "Suit", "Square", "Cube", "2D")]
    
    [Image(typeof(IconSquareOutline), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionPhysicsCheckBox2D : Condition
    {
        private static readonly Collider2D[] Colliders = new Collider2D[1];
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetPosition m_Position = GetPositionCharactersPlayer.Create;
        
        [SerializeField] private Vector2 m_Size = Vector2.one;
        [SerializeField] private float m_Angle = 0f;
        
        [SerializeField] private LayerMask m_LayerMask = -1;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Check Box 2D at {this.m_Position}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            int collisionCount = Physics2D.OverlapBoxNonAlloc(
                Vector2Utils.XY(this.m_Position.Get(args)),
                this.m_Size,
                this.m_Angle,
                Colliders,
                this.m_LayerMask
            );
            
            return collisionCount == 0;
        }
    }
}
