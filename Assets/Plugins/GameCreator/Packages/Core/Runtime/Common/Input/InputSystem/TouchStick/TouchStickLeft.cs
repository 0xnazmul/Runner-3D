using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    [Icon(RuntimePaths.GIZMOS + "GizmoTouchstick.png")]
    
    public class TouchStickLeft : TTouchStick
    {
        public static ITouchStick Create()
        {
            GameObject touchStick = new GameObject("Left Stick");
            TouchStickUtils.CreateCanvas(touchStick);
            TouchStickUtils.CreateControlsLeft(touchStick);

            return touchStick.GetComponentInChildren<ITouchStick>();
        }
    }
}