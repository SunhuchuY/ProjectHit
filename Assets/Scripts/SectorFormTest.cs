using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SectorFormTest : MonoBehaviour
{
    public float maxAngle = 50f;
    public float maxRadius = 2f;

    void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        Vector3 origin = transform.position;
        DrawFov(origin, forward);
    }

    void DrawFov(Vector3 origin, Vector3 forward)
    {
        Gizmos.color = Color.gray; 

        float totalAngle = maxAngle * 2;
        float step = totalAngle / 1000;

        for (float currentAngle = -maxAngle; currentAngle < maxAngle; currentAngle += step)
        {
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 direction = rotation * forward;
            Gizmos.DrawLine(origin, origin + direction * maxRadius);
        }

        Gizmos.DrawLine(origin, origin + Quaternion.Euler(0, -maxAngle, 0) * forward * maxRadius);
        Gizmos.DrawLine(origin, origin + Quaternion.Euler(0, maxAngle, 0) * forward * maxRadius);
    }
}
