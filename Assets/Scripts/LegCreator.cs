using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegCreator
{
    public LegCreator()
    {

    }

    public GameObject CreateLeg(float height, float width, float extrusion, Color color)
    {
        GameObject leg = new GameObject("Leg");
        leg.AddComponent<MeshFilter>();
        leg.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[13];
        int[] tris = new int[12 * 3];

        float topWidth = width / Mathf.Sin(Mathf.Atan(height / extrusion));

        float z = (-width / (2 * extrusion) - extrusion / width - 1) / (1 / extrusion + 1 / width);
        float y = height * z / extrusion + height * width / (2 * extrusion);
        Vector3 intersection = new Vector3(0, y, z);

        verts[0] = new Vector3(-width / 2, 0, topWidth / 2);
        verts[1] = new Vector3(width / 2, 0, topWidth / 2);
        verts[2] = new Vector3(width / 2, 0, -topWidth / 2);
        verts[3] = new Vector3(-width / 2, 0, -topWidth / 2);

        verts[4] = new Vector3(-width / 2, -height / 2, width / 2 - extrusion);
        verts[5] = new Vector3(width / 2, -height / 2, width / 2 - extrusion);
        verts[6] = new Vector3(width / 2, -height / 2, -width / 2 - extrusion);
        verts[7] = new Vector3(-width / 2, -height / 2, -width / 2 - extrusion);

        verts[8] = new Vector3(-width / 2, -height / 2, width / 2 - extrusion);
        verts[9] = new Vector3(width / 2, -height / 2, width / 2 - extrusion);
        verts[10] = new Vector3(width / 2, -height / 2, -width / 2 - extrusion);
        verts[11] = new Vector3(-width / 2, -height / 2, -width / 2 - extrusion);
        /*
        verts[4] = new Vector3(-width / 2, -height / 2, width / 2 - extrusion);
        verts[5] = new Vector3(width / 2, -height / 2, width / 2 - extrusion);
        verts[6] = intersection + new Vector3(width / 2, 0, 0);
        verts[7] = intersection - new Vector3(width / 2, 0, 0);

        verts[8] = new Vector3(-width / 2, -height / 2, width / 2 - extrusion);
        verts[9] = new Vector3(width / 2, -height / 2, width / 2 - extrusion);
        verts[10] = intersection + new Vector3(width / 2, 0, 0);
        verts[11] = intersection - new Vector3(width / 2, 0, 0);
        */
        verts[12] = new Vector3(0, -height, -extrusion);

        CreateQuad(0, tris, 1, 0, 4, 5);
        CreateQuad(6, tris, 2, 1, 5, 6);
        CreateQuad(12, tris, 3, 2, 6, 7);
        CreateQuad(18, tris, 0, 3, 7, 4);

        CreateTri(24, tris, 9, 8, 12);
        CreateTri(27, tris, 10, 9, 12);
        CreateTri(30, tris, 11, 10, 12);
        CreateTri(33, tris, 8, 11, 12);

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        leg.GetComponent<MeshFilter>().mesh = mesh;
        leg.GetComponent<Renderer>().material.color = color;

        return leg;
    }

    // quad creation helper, v1...4 indices in clockwise order
    void CreateQuad(int start, int[] tris, int v1, int v2, int v3, int v4)
    {
        CreateTri(start, tris, v1, v2, v3);
        CreateTri(start + 3, tris, v1, v3, v4);
    }
    // triangle creation helper, v1...3 indices in clockwise order
    void CreateTri(int start, int[] tris, int v1, int v2, int v3)
    {
        tris[start++] = v1;
        tris[start++] = v2;
        tris[start++] = v3;
    }
}
