using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Animation")]
    
    public interface IUnitAnimim : IUnitCommon
    {
        /**
         * IMPORTANT NOTE: It is required that the class implementing this interface has a
         * serializable field called 'm_Animator' that is used to know and change the reference
         * of the model in class ModelTool::ChangeModelEditor() 
        **/
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        Animator Animator { get; set; }
        BoneRack BoneRack { get; set; }
        
        float SmoothTime  { get; set; }
        float ModelOffset { get; set; }
        
        Vector3 RootMotionDeltaPosition { get; }
        Quaternion RootMotionDeltaRotation { get; }

        float HeartRate { get; set; }
        float Exertion  { get; set; }
        float Twitching { get; set; }

        // EVENTS: --------------------------------------------------------------------------------
        
        event Action<int> EventOnAnimatorIK;
    }
}