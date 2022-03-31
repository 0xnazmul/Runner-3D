using UnityEngine;

namespace GameCreator.Runtime.Common
{
	public static partial class GizmosExtension
	{
		public static void Circle(Vector3 position, float radius)
		{
            Circle(position, radius, Vector3.up);
		}

        public static void Circle(Vector3 position, float radius, Vector3 normal)
        {
            Vector3 top = normal.normalized * radius;
            Vector3 fwd = Vector3.Slerp(top, -top, 0.5f);
            Vector3 rht = Vector3.Cross(top, fwd).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4
            {
                [0]  = rht.x,
                [1]  = rht.y,
                [2]  = rht.z,
                [4]  = top.x,
                [5]  = top.y,
                [6]  = top.z,
                [8]  = fwd.x,
                [9]  = fwd.y,
                [10] = fwd.z
            };

            Vector3 prevPoint = position + matrix.MultiplyPoint3x4(Vector3.right);
            Vector3 nextPoint = Vector3.zero;

            for (int i = 0; i < 91; i++)
            {
                nextPoint.x = Mathf.Cos(i * 4 * Mathf.Deg2Rad);
                nextPoint.z = Mathf.Sin(i * 4 * Mathf.Deg2Rad);
                nextPoint.y = 0;

                nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);

                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }
    }
}