using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Characters.Animim
{
    public class GesturePlayableBehaviour : TAnimimPlayableBehaviour
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override Playable AnimationPlayable { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public GesturePlayableBehaviour(AnimationClip animationClip, AvatarMask avatarMask,
            BlendMode blendMode, AnimimGraph animimGraph, ConfigGesture config) 
            : base(avatarMask, blendMode, animimGraph, config)
        {
            this.AnimationPlayable = AnimationClipPlayable.Create(
                animimGraph.Graph, 
                animationClip
            );
        }
        
        public GesturePlayableBehaviour() : base(null, BlendMode.Blend, null, default)
        { }
    }
}