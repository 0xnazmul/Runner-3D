using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public static class SkeletonBuilder
    {
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static Volumes Make(Animator animator)
        {
            if (animator == null) return null;
            if (!animator.isHuman) return null;
            
            return new Volumes(
                MakeHips(animator),
                MakeChest(animator),
                MakeHead(animator),
                
                MakeUpperLegL(animator),
                MakeLowerLegL(animator),
                
                MakeUpperLegR(animator),
                MakeLowerLegR(animator),
                
                MakeUpperArmL(animator),
                MakeLowerArmL(animator),
                
                MakeUpperArmR(animator),
                MakeLowerArmR(animator)
            );
        }
        
        // PRIVATE STATIC METHODS: ----------------------------------------------------------------

        private static IVolume MakeHips(Animator animator)
        {
            Transform hips = animator.GetBoneTransform(HumanBodyBones.Hips);
            Transform spine = animator.GetBoneTransform(HumanBodyBones.Spine);

            Bounds boundsTorso = BoundsFromTorso(animator, hips);
            Bounds boundsHip = boundsTorso;
            
            if (spine != null)
            {
                boundsHip = BoundsClip(
                    boundsTorso, 
                    hips, spine, false
                );
            }

            return new VolumeBox(
                HumanBodyBones.Hips, new JointNone(),
                boundsHip.center, boundsHip.size
            );
        }
        
        private static IVolume MakeChest(Animator animator)
        {
            Transform spine = animator.GetBoneTransform(HumanBodyBones.Spine);
            if (spine == null) return null;
            
            Bounds boundsTorso = BoundsFromTorso(animator, spine);
            Bounds boundsSpine = BoundsClip(
                boundsTorso, 
                spine, spine, true
            );

            JointCharacter spineJoint = new JointCharacter(
                HumanBodyBones.Hips,
                Vector3.right,
                Vector3.forward,
                new Vector2(-20f, 20f), 
                new Vector2(10f, 0f)
            );
            
            return new VolumeBox(
                HumanBodyBones.Spine, 
                spineJoint,
                boundsSpine.center,
                boundsSpine.size
            );
        }
        
        private static IVolume MakeHead(Animator animator)
        {
            Transform boneHips = animator.GetBoneTransform(HumanBodyBones.Hips);
            Transform boneSpine = animator.GetBoneTransform(HumanBodyBones.Spine);
            
            Transform boneHead = animator.GetBoneTransform(HumanBodyBones.Head);

            Transform boneArmL = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            Transform boneArmR = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            
            
            float radius = (
                Vector3.Distance(boneArmL.position, boneArmR.position) * 
                0.25f * (1f / GetBoneLossyScale(boneHead))
            );
            
            Vector3 center = Vector3.zero;
            
            CalculateDirection(
                boneHead.InverseTransformPoint(boneHips.position),
                out int direction, 
                out float distance
            );

            center[direction] = distance > 0f ? -radius : radius;

            HumanBodyBones jointBone = boneSpine != null 
                ? HumanBodyBones.Spine
                : HumanBodyBones.Hips;

            JointCharacter headJoint = new JointCharacter(
                jointBone,
                Vector3.right,
                Vector3.forward,
                new Vector2(-40f, 25f), 
                new Vector2(25f, 0f)
            );

            return new VolumeSphere(
                HumanBodyBones.Head, 
                headJoint,
                center,
                radius
            );
        }

        private static IVolume MakeUpperLegL(Animator animator)
        {
            return MakeUpperLimb(
                animator,
                HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg, HumanBodyBones.Hips,
                0.3f, new Vector2(-20f, 70f), new Vector2(30f, 0f) 
            );
        }
        
        private static IVolume MakeUpperLegR(Animator animator)
        {
            return MakeUpperLimb(
                animator,
                HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg, HumanBodyBones.Hips,
                0.3f, new Vector2(-20f, 70f), new Vector2(30f, 0f)
            );
        }

        private static IVolume MakeLowerLegL(Animator animator)
        {
            return MakeLowerLimb(
                animator,
                HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftUpperLeg,
                0.25f, new Vector2(-80f, 0f), new Vector2(0f, 0f)
            );
        }
        
        private static IVolume MakeLowerLegR(Animator animator)
        {
            return MakeLowerLimb(
                animator,
                HumanBodyBones.RightLowerLeg, HumanBodyBones.RightUpperLeg,
                0.25f, new Vector2(-80f, 0f), new Vector2(0f, 0f)
            );
        }
        
        private static IVolume MakeUpperArmL(Animator animator)
        {
            Transform boneSpine = animator.GetBoneTransform(HumanBodyBones.Spine);

            return MakeUpperLimb(
                animator,
                HumanBodyBones.LeftUpperArm,
                HumanBodyBones.LeftLowerArm, 
                boneSpine != null ? HumanBodyBones.Spine : HumanBodyBones.Hips,
                0.25f, new Vector2(-70f, 10f), new Vector2(50f, 0f)
            );
        }
        
        private static IVolume MakeUpperArmR(Animator animator)
        {
            Transform boneSpine = animator.GetBoneTransform(HumanBodyBones.Spine);
            
            return MakeUpperLimb(
                animator,
                HumanBodyBones.RightUpperArm,
                HumanBodyBones.RightLowerArm, 
                boneSpine != null ? HumanBodyBones.Spine : HumanBodyBones.Hips,
                0.25f, new Vector2(-70f, 10f), new Vector2(50f, 0f)
            );
        }
        
        private static IVolume MakeLowerArmL(Animator animator)
        {
            return MakeLowerLimb(
                animator,
                HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftUpperArm,
                0.2f, new Vector2(-90f, 0f), new Vector2(0f, 0f)
            );
        }
        
        private static IVolume MakeLowerArmR(Animator animator)
        {
            return MakeLowerLimb(
                animator,
                HumanBodyBones.RightLowerArm, HumanBodyBones.RightUpperArm,
                0.2f, new Vector2(-90f, 0f), new Vector2(0f, 0f)
            );
        }

        private static IVolume MakeUpperLimb(Animator animator, HumanBodyBones upperBone, 
            HumanBodyBones lowerBone, HumanBodyBones parentBone, float radiusScale,
            Vector2 twistLimits, Vector2 swingLimits)
        {
            Transform upperBoneTransform = animator.GetBoneTransform(upperBone);
            Transform lowerBoneTransform = animator.GetBoneTransform(lowerBone);
            
            Vector3 endPoint = lowerBoneTransform.position;
            CalculateDirection(
                upperBoneTransform.InverseTransformPoint(endPoint), 
                out int direction, 
                out float distance
            );
            
            Vector3 limbPosition = Vector3.zero;
            limbPosition[direction] = distance * 0.5f;
            
            float limbHeight = Mathf.Abs(distance);
            float limbRadius = Mathf.Abs(distance * radiusScale);
            
            JointCharacter joint = new JointCharacter(
                parentBone,
                Vector3.right,
                Vector3.forward,
                twistLimits, 
                swingLimits
            );
            
            return new VolumeCapsule(
                upperBone, 
                joint,
                limbPosition,
                limbHeight,
                limbRadius,
                (VolumeCapsule.Direction) direction
            );
        }
        
        private static IVolume MakeLowerLimb(Animator animator, HumanBodyBones lowerBone, 
            HumanBodyBones parentBone, float radiusScale,
            Vector2 twistLimits, Vector2 swingLimits)
        {
            Transform lowerBoneTransform = animator.GetBoneTransform(lowerBone);
            Transform parentBoneTransform = animator.GetBoneTransform(parentBone);
            
            Vector3 endPoint = (
                lowerBoneTransform.position - parentBoneTransform.position + 
                lowerBoneTransform.position
            );
            
            CalculateDirection(
                lowerBoneTransform.InverseTransformPoint(endPoint), 
                out int direction, 
                out float distance
            );

            Transform[] children = lowerBoneTransform.GetComponentsInChildren<Transform>();
            if (children.Length > 1)
            {
                Bounds bounds = new Bounds();
                foreach (Transform child in children)
                {
                    bounds.Encapsulate(lowerBoneTransform.InverseTransformPoint(child.position));
                }

                distance = distance > 0 
                    ? bounds.max[direction]
                    : bounds.min[direction];
            }
            
            Vector3 limbPosition = Vector3.zero;
            limbPosition[direction] = distance * 0.5f;
            
            float limbHeight = Mathf.Abs(distance);
            float limbRadius = Mathf.Abs(distance * radiusScale);
            
            JointCharacter joint = new JointCharacter(
                parentBone,
                Vector3.right,
                Vector3.forward,
                twistLimits, 
                swingLimits
            );
            
            return new VolumeCapsule(
                lowerBone, 
                joint,
                limbPosition,
                limbHeight,
                limbRadius,
                (VolumeCapsule.Direction) direction
            );
        }
        
        // PRIVATE STATIC UTILITY METHODS: --------------------------------------------------------

        private static float GetBoneLossyScale(Transform bone)
        {
            Vector3 scale = bone.lossyScale;
            if (scale.x > scale.y && scale.x > scale.z) return scale.x;
            if (scale.y > scale.x && scale.y > scale.z) return scale.y;
            
            return scale.z;
        }
        
        private static void CalculateDirection(Vector3 point, out int direction, out float distance)
        {
            direction = 0;
            if (Mathf.Abs(point[1]) > Mathf.Abs(point[0])) direction = 1;
            if (Mathf.Abs(point[2]) > Mathf.Abs(point[direction])) direction = 2;

            distance = point[direction];
        }
        
        private static Bounds BoundsClip(Bounds bounds, Transform origin, Transform clip, bool below)
        {
            int axis = LargestComponent(bounds.size);

            float dotMax = Vector3.Dot(Vector3.up, origin.TransformPoint(bounds.max));
            float dotMin = Vector3.Dot(Vector3.up, origin.TransformPoint(bounds.min));

            if (dotMax > dotMin == below)
            {
                Vector3 min = bounds.min;
                min[axis] = origin.InverseTransformPoint(clip.position)[axis];
                bounds.min = min;
            }
            else
            {
                Vector3 max = bounds.max;
                max[axis] = origin.InverseTransformPoint(clip.position)[axis];
                bounds.max = max;
            }
            
            return bounds;
        }
        
        private static Bounds BoundsFromTorso(Animator animator, Transform origin)
        {
            Transform legL = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            Transform legR = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            Transform armL = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            Transform armR = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            
            Bounds bounds = new Bounds();
            bounds.Encapsulate(origin.InverseTransformPoint(legL.position));
            bounds.Encapsulate(origin.InverseTransformPoint(legR.position));
            bounds.Encapsulate(origin.InverseTransformPoint(armL.position));
            bounds.Encapsulate(origin.InverseTransformPoint(armR.position));
            
            Vector3 size = bounds.size;
            size[SmallestComponent(bounds.size)] = size[LargestComponent(bounds.size)] * 0.5f;
            bounds.size = size;
            return bounds;
        }

        private static int SmallestComponent(Vector3 point)
        {
            int direction = 0;
            if (Mathf.Abs(point[1]) < Mathf.Abs(point[0])) direction = 1;
            if (Mathf.Abs(point[2]) < Mathf.Abs(point[direction])) direction = 2;
            return direction;
        }

        private static int LargestComponent(Vector3 point)
        {
            int direction = 0;
            if (Mathf.Abs(point[1]) > Mathf.Abs(point[0])) direction = 1;
            if (Mathf.Abs(point[2]) > Mathf.Abs(point[direction])) direction = 2;
            return direction;
        }

        private static int SecondLargestComponent(Vector3 point)
        {
            int smallest = SmallestComponent(point);
            int largest = LargestComponent(point);
            
            if (smallest < largest)
            {
                int temp = largest;
                largest = smallest;
                smallest = temp;
            }

            if (smallest == 0 && largest == 1) return 2;
            if (smallest == 0 && largest == 2) return 1;
            
            return 0;
        }
    }
}
