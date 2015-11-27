using UnityEngine;
using System;
using System.Collections;

public class CubicBezier3D 
{

    [Serializable]
    public struct OrientedPoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public OrientedPoint(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector3 LocalToWorld(Vector3 point)
        {
            return position + rotation * point;
        }

        public Vector3 WorldToLocal(Vector3 point)
        {
            return Quaternion.Inverse(rotation) * (point - position);
        }

        public Vector3 LocalToWorldDirection(Vector3 dir)
        {
            return rotation * dir;
        }
    }

    // Optimized GetPoint
    Vector3 GetPoint(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return pts[0] * (omt2 * omt) +
                pts[1] * (3f * omt2 * t) +
                pts[2] * (3f * omt * t2) +
                pts[3] * (t2 * t);
    }

    // Get Tangent
    Vector3 GetTangent(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
                    pts[0] * (-omt2) +
                    pts[1] * (3 * omt2 - 2 * omt) +
                    pts[2] * (-3 * t2 + 2 * t) +
                    pts[3] * (t2);
        return tangent.normalized;
    }

    // Get Normal
    Vector3 GetNormal2D(Vector3[] pts, float t)
    {
        Vector3 tng = GetTangent(pts, t);
        return new Vector3(-tng.y, tng.x, 0f);
    }

    Vector3 GetNormal3D(Vector3[] pts, float t, Vector3 up)
    {
        Vector3 tng = GetTangent(pts, t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);
    }

    // Get Orientation
    Quaternion GetOrientation2D(Vector3[] pts, float t)
    {
        Vector3 tng = GetTangent(pts, t);
        Vector3 nrm = GetNormal2D(pts, t);
        return Quaternion.LookRotation(tng, nrm);
    }

    Quaternion GetOrientation3D(Vector3[] pts, float t, Vector3 up)
    {
        Vector3 tng = GetTangent(pts, t);
        Vector3 nrm = GetNormal3D(pts, t, up);
        return Quaternion.LookRotation(tng, nrm);
    }

    OrientedPoint[] createOP(int size)
    {
        OrientedPoint[] ops = new OrientedPoint[size];

        return ops;
    }
}
