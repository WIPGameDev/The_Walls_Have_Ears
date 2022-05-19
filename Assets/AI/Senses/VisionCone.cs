using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    protected MeshFilter meshFilter;
    protected Mesh mesh;

    protected byte segments = 4;

    protected float distance = 10f;

    protected float halfAngle = 10f;

    private void OnEnable()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            mesh = new Mesh();

            mesh.name = "Vision Cone";

            mesh = CreateMesh();

            meshFilter.mesh = mesh;
        }
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] Vertices = new Vector3[segments + 1];

        #region Vertices
        Vertices[0] = Vector3.zero;

        Vector3 endPoint = Vector3.forward * distance;

        int rotation = 360 / segments;

        for (int i = 1; i <= segments; i++)
        {
            Vertices[i] = Quaternion.Euler(0, 0, rotation * i) * Vector3.up * halfAngle + endPoint;
        }
        #endregion

        #region Triangles
        #region Code long 
        int[] Triangles = new int[(segments + segments - 2) * 3];
        int x = 0;
        for (int i = 1; i <= segments; i++)
        {
            Triangles[x++] = 0;

            if (i != segments)
                Triangles[x++] = i + 1;
            else
                Triangles[x++] = 1;

            Triangles[x++] = i;
        }
        #endregion

        #region Circle
        //int k = 1;
        //int count = 1;
        //for (int i = 1; i <= segments - 2; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        Triangles[x++] = k;

        //        if (j < 2)
        //        {
        //            if (k + count <= segments)
        //                k += count;
        //            else
        //            {
        //                if (k + count - segments == 1)
        //                    k = k + count++ - segments;
        //                else
        //                    k = k + ++count - segments;
        //            }
        //        }
        //    }
        //}
        #endregion
        #endregion

        mesh.vertices = Vertices;

        mesh.triangles = Triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateMesh();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    #region getters and setters
    public byte Segments
    {
        get
        {
            return segments;
        }
        set
        {
            segments = value;
        }
    }

    public float Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
        }
    }

    public float HalfAngle
    {
        get
        {
            return halfAngle;
        }
        set
        {
            halfAngle = value;
        }
    }
    #endregion
}
