using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Collect Characters")]
    [Description("Collects all Characters that within a certain radius of a position")]
    
    [Image(typeof(IconBust), ColorTheme.Type.Teal, typeof(OverlayListVariable))]
    
    [Category("Variables/Collect Characters")]
    
    [Serializable]
    public class InstructionVariablesCollectCharacters : TInstructionVariablesCollect
    {
        protected override string TitleTarget => "Characters";
        
        protected override List<GameObject> Collect(Vector3 origin, float maxRadius, float minDistance)
        {
            List<GameObject> result = new List<GameObject>();
            List<ISpatialHash> elements = SpatialHashCharacters.Get.Query(origin, maxRadius);

            foreach (ISpatialHash element in elements)
            {
                if (Vector3.Distance(element.Position, origin) <= minDistance) continue;
                
                Character character = element as Character;
                if (character == null) continue;
                
                result.Add(character.gameObject);
            }

            return result;
        }
    }
}