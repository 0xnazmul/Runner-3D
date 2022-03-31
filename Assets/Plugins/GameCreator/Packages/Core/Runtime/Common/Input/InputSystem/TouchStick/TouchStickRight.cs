using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    [Icon(RuntimePaths.GIZMOS + "GizmoTouchstick.png")]
    
    public class TouchStickRight : TTouchStick
    {
        public static ITouchStick Create()
        {
            GameObject touchStick = new GameObject("Right Stick");
            TouchStickUtils.CreateCanvas(touchStick);
            TouchStickUtils.CreateControlsRight(touchStick);

            return touchStick.GetComponentInChildren<ITouchStick>();
        }
    }
}