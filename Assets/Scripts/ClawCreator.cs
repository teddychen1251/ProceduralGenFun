using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawCreator
{
    public ClawCreator()
    {
    }

    public GameObject CreateLeft(float scale, Color color)
    {
        GameObject leftArm = new GameObject("Left Arm");

        // first section
        GameObject firstSection = new GameObject("first section");
        firstSection.AddComponent<MeshFilter>();
        firstSection.AddComponent<MeshRenderer>();

        Mesh firstMesh = new Mesh();
        Vector3[] firstVerts = new Vector3[4];
        int[] firstTris = new int[4 * 3];
        firstVerts[0] = new Vector3(0, 0, 0);
        firstVerts[1] = new Vector3(Mathf.Sqrt(3) / 6f, 0, -1);
        firstVerts[2] = new Vector3(-Mathf.Sqrt(3) / 12f, 0.35f, -1);
        firstVerts[3] = new Vector3(-Mathf.Sqrt(3) / 12f, -0.35f, -1);

        CreateTri(0, firstTris, 0, 1, 2);
        CreateTri(3, firstTris, 0, 2, 3);
        CreateTri(6, firstTris, 0, 3, 1);
        CreateTri(9, firstTris, 1, 3, 2);

        firstMesh.vertices = firstVerts;
        firstMesh.triangles = firstTris;
        firstMesh.RecalculateNormals();
        firstSection.GetComponent<MeshFilter>().mesh = firstMesh;
        firstSection.GetComponent<Renderer>().material.color = color;
        firstSection.transform.SetParent(leftArm.transform);
        // second section
        GameObject secondSection = new GameObject("second section");
        secondSection.AddComponent<MeshFilter>();
        secondSection.AddComponent<MeshRenderer>();

        Mesh secondMesh = new Mesh();
        Vector3[] secondVerts = new Vector3[8];
        int[] secondTris = new int[10 * 3];
        secondVerts[0] = new Vector3(-Mathf.Sqrt(3) / 24f, .25f, -1f);
        secondVerts[1] = new Vector3(Mathf.Sqrt(3) / 24f, .15f, -1f);
        secondVerts[2] = new Vector3(Mathf.Sqrt(3) / 24f, -.15f, -1f);
        secondVerts[3] = new Vector3(-Mathf.Sqrt(3) / 24f, -.25f, -1f);

        secondVerts[4] = new Vector3(-Mathf.Sqrt(3) / 12f, .35f, -2f);
        secondVerts[5] = new Vector3(Mathf.Sqrt(3) / 9f, .25f, -2f);
        secondVerts[6] = new Vector3(Mathf.Sqrt(3) / 9f, -.25f, -2f);
        secondVerts[7] = new Vector3(-Mathf.Sqrt(3) / 12f, -.35f, -2f);

        CreateQuad(0, secondTris, 0, 1, 5, 4);
        CreateQuad(6, secondTris, 1, 2, 6, 5);
        CreateQuad(12, secondTris, 2, 3, 7, 6);
        CreateQuad(18, secondTris, 3, 0, 4, 7);
        CreateQuad(24, secondTris, 4, 5, 6, 7);

        secondMesh.vertices = secondVerts;
        secondMesh.triangles = secondTris;
        secondMesh.RecalculateNormals();
        secondSection.GetComponent<MeshFilter>().mesh = secondMesh;
        secondSection.GetComponent<Renderer>().material.color = color;
        secondSection.transform.SetParent(leftArm.transform);
        // claw
        GameObject claw = new GameObject("claw");

        GameObject bottomPincer = new GameObject("bottom pincer");
        bottomPincer.AddComponent<MeshFilter>();
        bottomPincer.AddComponent<MeshRenderer>();
        Mesh bottomPincerMesh = new Mesh();
        Vector3[] botPincerVerts = new Vector3[17];
        int[] botPincerTris = new int[24 * 3];
        botPincerVerts[0] = new Vector3(-Mathf.Sqrt(3) / 21, .18f, -2f);
        botPincerVerts[1] = new Vector3(-Mathf.Sqrt(3) / 27, .23f, -2f);
        botPincerVerts[2] = new Vector3(Mathf.Sqrt(3) / 27, .23f, -2f);
        botPincerVerts[3] = new Vector3(Mathf.Sqrt(3) / 21, .18f, -2f);
        botPincerVerts[4] = new Vector3(Mathf.Sqrt(3) / 21, -.18f, -2f);
        botPincerVerts[5] = new Vector3(Mathf.Sqrt(3) / 27, -.23f, -2f);
        botPincerVerts[6] = new Vector3(-Mathf.Sqrt(3) / 27, -.23f, -2f);
        botPincerVerts[7] = new Vector3(-Mathf.Sqrt(3) / 21, -.18f, -2f);

        botPincerVerts[8] = new Vector3(-Mathf.Sqrt(3) / 12, .3f, -2.7f);
        botPincerVerts[9] = new Vector3(-Mathf.Sqrt(3) / 15, .35f, -2.7f);
        botPincerVerts[10] = new Vector3(Mathf.Sqrt(3) / 15, .35f, -2.7f);
        botPincerVerts[11] = new Vector3(Mathf.Sqrt(3) / 12, .3f, -2.7f);
        botPincerVerts[12] = new Vector3(Mathf.Sqrt(3) / 12, -.05f, -2.7f);
        botPincerVerts[13] = new Vector3(Mathf.Sqrt(3) / 15, -.35f, -2.7f);
        botPincerVerts[14] = new Vector3(-Mathf.Sqrt(3) / 15, -.35f, -2.7f);
        botPincerVerts[15] = new Vector3(-Mathf.Sqrt(3) / 12, -.05f, -2.7f);

        botPincerVerts[16] = new Vector3(0, 0f, -3.3f);

        CreateQuad(0, botPincerTris, 0, 1, 9, 8);
        CreateQuad(6, botPincerTris, 1, 2, 10, 9);
        CreateQuad(12, botPincerTris, 2, 3, 11, 10);
        CreateQuad(18, botPincerTris, 3, 4, 12, 11);
        CreateQuad(24, botPincerTris, 4, 5, 13, 12);
        CreateQuad(30, botPincerTris, 5, 6, 14, 13);
        CreateQuad(36, botPincerTris, 6, 7, 15, 14);
        CreateQuad(42, botPincerTris, 7, 0, 8, 15);

        CreateQuad(48, botPincerTris, 8, 9, 10, 11);
        CreateQuad(54, botPincerTris, 8, 11, 12, 15);
        
        CreateTri(60, botPincerTris, 12, 13, 16);
        CreateTri(63, botPincerTris, 13, 14, 16);
        CreateTri(66, botPincerTris, 14, 15, 16);
        CreateTri(69, botPincerTris, 15, 12, 16);
        
        bottomPincerMesh.vertices = botPincerVerts;
        bottomPincerMesh.triangles = botPincerTris;
        bottomPincerMesh.RecalculateNormals();
        bottomPincer.GetComponent<MeshFilter>().mesh = bottomPincerMesh;
        bottomPincer.GetComponent<Renderer>().material.color = color;
        bottomPincer.transform.SetParent(claw.transform);

        GameObject topPincer = new GameObject("top pincer");
        topPincer.AddComponent<MeshFilter>();
        topPincer.AddComponent<MeshRenderer>();
        Mesh topPincerMesh = new Mesh();
        Vector3[] topPincerVerts = new Vector3[7];
        int[] topPincerTris = new int[6 * 3];
        topPincerVerts[0] = new Vector3(-Mathf.Sqrt(3) / 12, .05f, -2.7f);
        topPincerVerts[1] = new Vector3(-Mathf.Sqrt(3) / 12, .3f, -2.7f);
        topPincerVerts[2] = new Vector3(-Mathf.Sqrt(3) / 15, .35f, -2.7f);
        topPincerVerts[3] = new Vector3(Mathf.Sqrt(3) / 15, .35f, -2.7f);
        topPincerVerts[4] = new Vector3(Mathf.Sqrt(3) / 12, .3f, -2.7f);
        topPincerVerts[5] = new Vector3(Mathf.Sqrt(3) / 12, .05f, -2.7f);

        topPincerVerts[6] = new Vector3(0, 0f, -3.2f);

        CreateTri(0, topPincerTris, 0, 1, 6);
        CreateTri(3, topPincerTris, 1, 2, 6);
        CreateTri(6, topPincerTris, 2, 3, 6);
        CreateTri(9, topPincerTris, 3, 4, 6);
        CreateTri(12, topPincerTris, 4, 5, 6);
        CreateTri(15, topPincerTris, 5, 1, 6);

        topPincerMesh.vertices = topPincerVerts;
        topPincerMesh.triangles = topPincerTris;
        topPincerMesh.RecalculateNormals();
        topPincer.GetComponent<MeshFilter>().mesh = topPincerMesh;
        topPincer.GetComponent<Renderer>().material.color = color;
        topPincer.transform.SetParent(claw.transform);

        claw.transform.SetParent(leftArm.transform);

        leftArm.transform.localScale = new Vector3(scale, scale, scale);

        return leftArm;
    }

    public GameObject CreateRight(float scale, Color color)
    {
        GameObject rightArm = new GameObject("Right Arm");

        // first section
        GameObject firstSection = new GameObject("first section");
        firstSection.AddComponent<MeshFilter>();
        firstSection.AddComponent<MeshRenderer>();

        Mesh firstMesh = new Mesh();
        Vector3[] firstVerts = new Vector3[4];
        int[] firstTris = new int[4 * 3];
        firstVerts[0] = new Vector3(0, 0, 0);
        firstVerts[1] = new Vector3(-Mathf.Sqrt(3) / 6f, 0, -1);
        firstVerts[2] = new Vector3(Mathf.Sqrt(3) / 12f, 0.35f, -1);
        firstVerts[3] = new Vector3(Mathf.Sqrt(3) / 12f, -0.35f, -1);

        CreateTri(0, firstTris, 0, 2, 1);
        CreateTri(3, firstTris, 0, 3, 2);
        CreateTri(6, firstTris, 0, 1, 3);
        CreateTri(9, firstTris, 1, 2, 3);

        firstMesh.vertices = firstVerts;
        firstMesh.triangles = firstTris;
        firstMesh.RecalculateNormals();
        firstSection.GetComponent<MeshFilter>().mesh = firstMesh;
        firstSection.GetComponent<Renderer>().material.color = color;
        firstSection.transform.SetParent(rightArm.transform);
        // second section
        GameObject secondSection = new GameObject("second section");
        secondSection.AddComponent<MeshFilter>();
        secondSection.AddComponent<MeshRenderer>();

        Mesh secondMesh = new Mesh();
        Vector3[] secondVerts = new Vector3[8];
        int[] secondTris = new int[10 * 3];
        secondVerts[0] = new Vector3(-Mathf.Sqrt(3) / 24f, .15f, -1f);
        secondVerts[1] = new Vector3(Mathf.Sqrt(3) / 24f, .25f, -1f);
        secondVerts[2] = new Vector3(Mathf.Sqrt(3) / 24f, -.25f, -1f);
        secondVerts[3] = new Vector3(-Mathf.Sqrt(3) / 24f, -.15f, -1f);

        secondVerts[4] = new Vector3(-Mathf.Sqrt(3) / 9f, .25f, -2f);
        secondVerts[5] = new Vector3(Mathf.Sqrt(3) / 12f, .35f, -2f);
        secondVerts[6] = new Vector3(Mathf.Sqrt(3) / 12f, -.35f, -2f);
        secondVerts[7] = new Vector3(-Mathf.Sqrt(3) / 9f, -.25f, -2f);

        CreateQuad(0, secondTris, 0, 1, 5, 4);
        CreateQuad(6, secondTris, 1, 2, 6, 5);
        CreateQuad(12, secondTris, 2, 3, 7, 6);
        CreateQuad(18, secondTris, 3, 0, 4, 7);
        CreateQuad(24, secondTris, 4, 5, 6, 7);

        secondMesh.vertices = secondVerts;
        secondMesh.triangles = secondTris;
        secondMesh.RecalculateNormals();
        secondSection.GetComponent<MeshFilter>().mesh = secondMesh;
        secondSection.GetComponent<Renderer>().material.color = color;
        secondSection.transform.SetParent(rightArm.transform);
        // claw
        GameObject claw = new GameObject("claw");

        GameObject bottomPincer = new GameObject("bottom pincer");
        bottomPincer.AddComponent<MeshFilter>();
        bottomPincer.AddComponent<MeshRenderer>();
        Mesh bottomPincerMesh = new Mesh();
        Vector3[] botPincerVerts = new Vector3[17];
        int[] botPincerTris = new int[24 * 3];
        botPincerVerts[0] = new Vector3(-Mathf.Sqrt(3) / 21, .18f, -2f);
        botPincerVerts[1] = new Vector3(-Mathf.Sqrt(3) / 27, .23f, -2f);
        botPincerVerts[2] = new Vector3(Mathf.Sqrt(3) / 27, .23f, -2f);
        botPincerVerts[3] = new Vector3(Mathf.Sqrt(3) / 21, .18f, -2f);
        botPincerVerts[4] = new Vector3(Mathf.Sqrt(3) / 21, -.18f, -2f);
        botPincerVerts[5] = new Vector3(Mathf.Sqrt(3) / 27, -.23f, -2f);
        botPincerVerts[6] = new Vector3(-Mathf.Sqrt(3) / 27, -.23f, -2f);
        botPincerVerts[7] = new Vector3(-Mathf.Sqrt(3) / 21, -.18f, -2f);

        botPincerVerts[8] = new Vector3(-Mathf.Sqrt(3) / 12, .3f, -2.7f);
        botPincerVerts[9] = new Vector3(-Mathf.Sqrt(3) / 15, .35f, -2.7f);
        botPincerVerts[10] = new Vector3(Mathf.Sqrt(3) / 15, .35f, -2.7f);
        botPincerVerts[11] = new Vector3(Mathf.Sqrt(3) / 12, .3f, -2.7f);
        botPincerVerts[12] = new Vector3(Mathf.Sqrt(3) / 12, -.05f, -2.7f);
        botPincerVerts[13] = new Vector3(Mathf.Sqrt(3) / 15, -.35f, -2.7f);
        botPincerVerts[14] = new Vector3(-Mathf.Sqrt(3) / 15, -.35f, -2.7f);
        botPincerVerts[15] = new Vector3(-Mathf.Sqrt(3) / 12, -.05f, -2.7f);

        botPincerVerts[16] = new Vector3(0, 0f, -3.3f);

        CreateQuad(0, botPincerTris, 0, 1, 9, 8);
        CreateQuad(6, botPincerTris, 1, 2, 10, 9);
        CreateQuad(12, botPincerTris, 2, 3, 11, 10);
        CreateQuad(18, botPincerTris, 3, 4, 12, 11);
        CreateQuad(24, botPincerTris, 4, 5, 13, 12);
        CreateQuad(30, botPincerTris, 5, 6, 14, 13);
        CreateQuad(36, botPincerTris, 6, 7, 15, 14);
        CreateQuad(42, botPincerTris, 7, 0, 8, 15);

        CreateQuad(48, botPincerTris, 8, 9, 10, 11);
        CreateQuad(54, botPincerTris, 8, 11, 12, 15);

        CreateTri(60, botPincerTris, 12, 13, 16);
        CreateTri(63, botPincerTris, 13, 14, 16);
        CreateTri(66, botPincerTris, 14, 15, 16);
        CreateTri(69, botPincerTris, 15, 12, 16);

        bottomPincerMesh.vertices = botPincerVerts;
        bottomPincerMesh.triangles = botPincerTris;
        bottomPincerMesh.RecalculateNormals();
        bottomPincer.GetComponent<MeshFilter>().mesh = bottomPincerMesh;
        bottomPincer.GetComponent<Renderer>().material.color = color;
        bottomPincer.transform.SetParent(claw.transform);

        GameObject topPincer = new GameObject("top pincer");
        topPincer.AddComponent<MeshFilter>();
        topPincer.AddComponent<MeshRenderer>();
        Mesh topPincerMesh = new Mesh();
        Vector3[] topPincerVerts = new Vector3[7];
        int[] topPincerTris = new int[6 * 3];
        topPincerVerts[0] = new Vector3(-Mathf.Sqrt(3) / 12, .05f, -2.7f);
        topPincerVerts[1] = new Vector3(-Mathf.Sqrt(3) / 12, .3f, -2.7f);
        topPincerVerts[2] = new Vector3(-Mathf.Sqrt(3) / 15, .35f, -2.7f);
        topPincerVerts[3] = new Vector3(Mathf.Sqrt(3) / 15, .35f, -2.7f);
        topPincerVerts[4] = new Vector3(Mathf.Sqrt(3) / 12, .3f, -2.7f);
        topPincerVerts[5] = new Vector3(Mathf.Sqrt(3) / 12, .05f, -2.7f);

        topPincerVerts[6] = new Vector3(0, 0f, -3.2f);

        CreateTri(0, topPincerTris, 0, 1, 6);
        CreateTri(3, topPincerTris, 1, 2, 6);
        CreateTri(6, topPincerTris, 2, 3, 6);
        CreateTri(9, topPincerTris, 3, 4, 6);
        CreateTri(12, topPincerTris, 4, 5, 6);
        CreateTri(15, topPincerTris, 5, 1, 6);

        topPincerMesh.vertices = topPincerVerts;
        topPincerMesh.triangles = topPincerTris;
        topPincerMesh.RecalculateNormals();
        topPincer.GetComponent<MeshFilter>().mesh = topPincerMesh;
        topPincer.GetComponent<Renderer>().material.color = color;
        topPincer.transform.SetParent(claw.transform);

        claw.transform.SetParent(rightArm.transform);

        rightArm.transform.localScale = new Vector3(scale, scale, scale);

        return rightArm;
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
