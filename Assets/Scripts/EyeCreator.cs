using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCreator : MonoBehaviour
{
    GameObject googly;
    GameObject pill;
    GameObject square;

    float stalkWidth = 0.01f;

    public EyeCreator(GameObject googly, GameObject pill, GameObject square)
    {
        this.googly = googly;
        this.pill = pill;
        this.square = square;
    }

    public GameObject CreateEye(float height, float styleProb, float eyeScale, float stalkRotation, Color color)
    {
        GameObject eye = new GameObject("eye");

        GameObject eyeStalk = new GameObject("eye stalk");
        eyeStalk.AddComponent<MeshFilter>();
        eyeStalk.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[] {
            new Vector3(-stalkWidth / 2, 0, stalkWidth / 2),
            new Vector3(stalkWidth / 2, 0, stalkWidth / 2),
            new Vector3(stalkWidth / 2, 0, -stalkWidth / 2),
            new Vector3(-stalkWidth / 2, 0, -stalkWidth / 2),

            new Vector3(-stalkWidth / 2, -height, stalkWidth / 2),
            new Vector3(stalkWidth / 2, -height, stalkWidth / 2),
            new Vector3(stalkWidth / 2, -height, -stalkWidth / 2),
            new Vector3(-stalkWidth / 2, -height, -stalkWidth / 2)
        };
        int[] tris = new int[8 * 3];
        CreateQuad(0, tris, 1, 0, 4, 5);
        CreateQuad(6, tris, 2, 1, 5, 6);
        CreateQuad(12, tris, 3, 2, 6, 7);
        CreateQuad(18, tris, 0, 3, 7, 4);

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        eyeStalk.GetComponent<MeshFilter>().mesh = mesh;
        eyeStalk.GetComponent<MeshRenderer>().material.color = color;
        eyeStalk.transform.RotateAround(Vector3.zero, Vector3.right, -stalkRotation);
        eyeStalk.transform.SetParent(eye.transform);

        GameObject eyeball;
        if (styleProb < .33)
        {
            eyeball = Instantiate(googly);
        }
        else if (styleProb < .67)
        {
            eyeball = Instantiate(pill);
        }
        else
        {
            eyeball = Instantiate(square);
        }
        eyeball.SetActive(true);
        eyeball.transform.localScale = new Vector3(eyeScale, eyeScale, eyeScale);
        eyeball.transform.SetParent(eye.transform);

        return eye;
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
