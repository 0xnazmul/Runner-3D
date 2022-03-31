using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Driver")]
    
    public interface IUnitDriver : IUnitCommon
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        Vector3 WorldMoveDirection { get; }
        Vector3 LocalMoveDirection { get; }
        
        float SkinWidth { get; }
        bool IsGrounded { get; }
        Vector3 FloorNormal { get; }

        // POSITION MODIFIERS: --------------------------------------------------------------------

        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);

        void AddPosition(Vector3 amount);
        void AddRotation(Quaternion amount);
    }
}