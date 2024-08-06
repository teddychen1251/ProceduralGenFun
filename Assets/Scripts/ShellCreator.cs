using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCreator
{
    static float POINTY_PROB = 0.2f;
    static float NUBBY_PROB = 0.4f;
    static float NUBBY_RIDGES_PROB = 0.8f;
    static float TURBAN_PROB = 0.4f;
    static float TURBAN_RIDGES_PROB = 0.6f;

    static float MAX_NUBBY_RIDGE_AMP = 0.01f;
    static int MAX_NUBBY_RIDGE_FREQ = 12;
    static float MAX_TURBAN_RIDGE_AMP = 0.05f;
    static int MAX_TURBAN_RIDGE_FREQ = 12;

    int tRes;
    int sRes;

    public ShellCreator(int tRes, int sRes)
    {
        this.tRes = tRes;
        this.sRes = sRes;
    }

    public GameObject CreateShell(float width, float height)
    {
        GameObject shell = new GameObject("shell");
        shell.AddComponent<MeshFilter>();
        shell.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[(tRes + 1) * sRes];
        int[] tris = new int[tRes * sRes * 6 * 2]; // *2 so we can look inside shell

        // generate surface of revolution
        float tIncr = 1.0f / tRes;
        float sIncr = 2 * Mathf.PI / sRes;
        int vIndex = 0;
        // figure out shell type
        bool ridges;
        float ridgeAmp;
        int ridgeFreq;
        float prob = Random.value;
        if (prob <= POINTY_PROB)
        {
            ridges = false;
            ridgeAmp = 0f;
            ridgeFreq = 0;
        }
        else if (prob <= POINTY_PROB + NUBBY_PROB)
        {
            ridges = Random.value <= NUBBY_RIDGES_PROB;
            ridgeAmp = Random.value * 2 * MAX_NUBBY_RIDGE_AMP - MAX_NUBBY_RIDGE_AMP;
            ridgeFreq = Random.Range(1, MAX_NUBBY_RIDGE_FREQ + 1);
        }
        else
        {
            ridges = Random.value <= TURBAN_RIDGES_PROB;
            ridgeAmp = Random.value * 2 * MAX_TURBAN_RIDGE_AMP - MAX_TURBAN_RIDGE_AMP;
            ridgeFreq = Random.Range(1, MAX_TURBAN_RIDGE_FREQ + 1);
        }
        for (int i = 0; i < tRes + 1; i++)
        {
            float t = i * tIncr;
            for (int j = 0; j < sRes; j++)
            {
                float s = j * sIncr;
                float rVal;
                if (prob <= POINTY_PROB)
                {
                    rVal = R_pointy(t, width);
                }
                else if (prob <= POINTY_PROB + NUBBY_PROB)
                {
                    rVal = R_nubby(t, width, ridges, ridgeAmp, ridgeFreq);
                }
                else
                {
                    rVal = R_turban(t, width, ridges, ridgeAmp, ridgeFreq);
                }
                verts[vIndex++] = new Vector3(rVal * Mathf.Cos(s), H(t, height), rVal * Mathf.Sin(s));
            }
        }
        int tIndex = 0;
        for (int i = 0; i < tRes; i++)
        {
            for (int j = 0; j < sRes; j++)
            {
                tris[tIndex++] = i * sRes + j;
                tris[tIndex++] = (i + 1) * sRes + j;
                tris[tIndex++] = (i + 1) * sRes + (j + 1) % sRes;
                tris[tIndex++] = i * sRes + j;
                tris[tIndex++] = (i + 1) * sRes + (j + 1) % sRes;
                tris[tIndex++] = i * sRes + (j + 1) % sRes;
            }
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        shell.GetComponent<MeshFilter>().mesh = mesh;
        shell.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);

        return shell;
    }

    float H(float t, float height)
    {
        return height * t;
    }
    float R_pointy(float t, float width)
    {
        // width = diameter
        float val = 0.5f * width * Mathf.Pow(1 - Mathf.Pow(t, 2), 3);
        return val;
    }
    float R_nubby(float t, float width, bool ridges, float ridgeAmp, int ridgeFreq)
    {
        float val = 0.5f * width * Mathf.Sqrt(-(t - 1));
        if (ridges)
        {
            val += ridgeAmp * Mathf.Sin(ridgeFreq * 2 * Mathf.PI * t);
        }
        return val;
    }
    float R_turban(float t, float width, bool ridges, float ridgeAmp, int ridgeFreq)
    {
        float val = 0.5f * width * (-Mathf.Pow(t, 4) - Mathf.Pow(t, 2) + t + 1);
        if (ridges)
        {
            val += ridgeAmp * Mathf.Sin(ridgeFreq * 2 * Mathf.PI * t);
        }
        return val;
    }
}
