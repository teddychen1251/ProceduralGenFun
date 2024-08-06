using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int seed = 0;
    public int resX = 4;
    public int resZ = 3;
    public float cellWidth = 1f;
    public float cellLength = 1f;
    public float height = 7;
    public TextAsset stoneImage;
    public float treasureChestProb = .05f;
    public GameObject treasureChest;

    private DungeonTile[,] dungeon;
    private int textureScale = 16;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);

        dungeon = new DungeonTile[resZ + 1, resX + 1];
        for (int i = 0; i < resZ + 1; i++)
        {
            for (int j = 0; j < resX + 1; j++)
            {
                dungeon[i, j] = new DungeonTile(treasureChestProb);
            }
        }


        // calculate and create floor plan
        int[][] walls = new int[2 * resX * resZ - resX - resZ][];
        int index = 0;
        // 0 - north wall, 1 - east wall
        for (int i = 1; i < dungeon.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < dungeon.GetLength(1) - 1; j++)
            {
                walls[index++] = new int[] { i, j, (int) DungeonWall.NORTH };
                walls[index++] = new int[] { i, j, (int) DungeonWall.EAST };
            }
        }
        // north row
        for (int i = 1; i < dungeon.GetLength(1) - 1; i++)
            walls[index++] = new int[] { dungeon.GetLength(0) - 1, i, (int) DungeonWall.EAST };
        // east column
        for (int i = 1; i < dungeon.GetLength(0) - 1; i++)
            walls[index++] = new int[] { i, dungeon.GetLength(1) - 1, (int) DungeonWall.NORTH };
        // shuffle walls
        for (int i = walls.Length - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int[] temp = walls[i];
            walls[i] = walls[rand];
            walls[rand] = temp;
        }
        GenerateMaze(walls);
        //CreateFloorPlan();
        CreateDungeon();
    }

    void GenerateMaze(int[][] walls)
    {
        int uniqueRegions = resX * resZ;
        int index = 0;
        while (uniqueRegions > 1)
        {
            int i = walls[index][0];
            int j = walls[index][1];
            DungeonWall dir = (DungeonWall) walls[index][2];
            index++;
            DungeonTile tile = dungeon[i, j];
            if (dir == DungeonWall.NORTH)
            {
                if (tile.id != dungeon[i + 1, j].id)
                {
                    HomogenizeRegion(i + 1, j, tile.id);
                    tile.BreakNorthWall();
                    uniqueRegions--;
                }
            }
            else
            {
                if (tile.id != dungeon[i, j + 1].id)
                {
                    HomogenizeRegion(i, j + 1, tile.id);
                    tile.BreakEastWall();
                    uniqueRegions--;
                }
            }
        }
    }
    void HomogenizeRegion(int i, int j, int id)
    {
        DungeonTile tile = dungeon[i, j];
        tile.id = id;
        if (!tile.eastWall && dungeon[i, j + 1].id != id)
        {
            HomogenizeRegion(i, j + 1, id);
        }
        if (!tile.northWall && dungeon[i + 1, j].id != id)
        {
            HomogenizeRegion(i + 1, j, id);
        }
        if (j > 1 && !dungeon[i, j - 1].eastWall && dungeon[i, j - 1].id != id)
        {
            HomogenizeRegion(i, j - 1, id);
        }
        if (i > 1 && !dungeon[i - 1, j].northWall && dungeon[i - 1, j].id != id)
        {
            HomogenizeRegion(i - 1, j, id);
        }
    }

    void CreateDungeon()
    {
        GameObject dungeon = new GameObject("Dungeon");

        GameObject floor = new GameObject("Floor");
        floor.AddComponent<MeshFilter>();
        floor.AddComponent<MeshRenderer>();
        floor.GetComponent<MeshFilter>().mesh = CreateFloor();
        floor.GetComponent<Renderer>().material.color = Color.white;
        floor.GetComponent<Renderer>().material.mainTexture = CreateFloorTexture();
        floor.transform.SetParent(dungeon.transform);

        GameObject walls = new GameObject("Walls");
        CreateWalls(walls.transform);
        walls.transform.position = new Vector3(-resX * cellWidth / 2, 0, -resZ * cellLength / 2);
        walls.transform.SetParent(dungeon.transform);

        // add treasure chests
        AddTreasureChests();
    }
    Mesh CreateFloor()
    {
        Mesh floorMesh = new Mesh();
        float x = resX * cellWidth;
        x -= x / 2;
        float z = resZ * cellLength;
        z -= z / 2;
        Vector3[] verts =
        {
            new Vector3(x, 0, -z),
            new Vector3(-x, 0, -z),
            new Vector3(-x, 0, z),
            new Vector3(x, 0, z)
        };
        int[] tris = { 0, 1, 2, 2, 3, 0 };
        Vector2[] uv = new Vector2[verts.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(verts[i].x > 0 ? resX : 0, verts[i].z > 0 ? resZ : 0);
        }
        floorMesh.vertices = verts;
        floorMesh.triangles = tris;
        floorMesh.uv = uv;
        floorMesh.RecalculateNormals();

        return floorMesh;
    }
    Texture2D CreateFloorTexture()
    {
        int width = 64;
        int height = 64;
        float scale = 10;
        Color color1 = new Color(139 / 256f, 69 / 256f, 19 / 256f);
        Color color2 = new Color(238 / 256f, 227 / 256f, 157 / 256f);
        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width * height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                float x = scale * i / (float)width;
                float y = scale * j / (float)height;
                float t = Mathf.PerlinNoise(x, y);
                colors[j * width + i] = Color.Lerp(color1, color2, t * t * t);
            }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }
    Texture2D CreateWallTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(stoneImage.bytes);
        return texture;
    }
    void CreateWalls(Transform parent)
    {
        float thickness = 0.1f;
        int doorFactor = 5;
        Texture2D tex = CreateWallTexture();
        // eastmost wall
        GameObject leftWall = BuildWall(0, 0, thickness, resZ * cellLength, height);
        leftWall.GetComponent<Renderer>().material.mainTexture = tex;
        leftWall.transform.SetParent(parent);
        // southernmost wall
        GameObject southWall = BuildWall(thickness, 0, resX * cellWidth, thickness, height);
        southWall.GetComponent<Renderer>().material.mainTexture = tex;
        southWall.transform.SetParent(parent);
        // westmost wall
        GameObject westWall = BuildWall(resX * cellWidth - thickness, thickness, resX * cellWidth, resZ * cellLength, height);
        westWall.GetComponent<Renderer>().material.mainTexture = tex;
        westWall.transform.SetParent(parent);
        // northmost wall
        GameObject northWall = BuildWall(thickness, resZ * cellLength - thickness, resX * cellWidth - thickness, resZ * cellLength, height);
        northWall.GetComponent<Renderer>().material.mainTexture = tex;
        northWall.transform.SetParent(parent);

        for (int i = 1; i < dungeon.GetLength(0); i++)
        {
            for (int j = 1; j < dungeon.GetLength(1); j++)
            {
                if (i != dungeon.GetLength(0) - 1)
                {
                    if (dungeon[i, j].northWall)
                    {
                        if (j == 1)
                        {
                            GameObject wall = BuildWall((j - 1) * cellWidth + thickness, i * cellLength - thickness, j * cellWidth, i * cellLength, height);
                            wall.GetComponent<Renderer>().material.mainTexture = tex;
                            wall.transform.SetParent(parent);
                        }
                        else if (j == dungeon.GetLength(1) - 1)
                        {
                            GameObject wall = BuildWall((j - 1) * cellWidth, i * cellLength - thickness, j * cellWidth - thickness, i * cellLength, height);
                            wall.GetComponent<Renderer>().material.mainTexture = tex;
                            wall.transform.SetParent(parent);
                        }
                        else
                        {
                            GameObject wall = BuildWall((j - 1) * cellWidth, i * cellLength - thickness, j * cellWidth, i * cellLength, height);
                            wall.GetComponent<Renderer>().material.mainTexture = tex;
                            wall.transform.SetParent(parent);
                        }
                    } 
                    else
                    {
                        if (j == 1)
                        {
                            GameObject eastPost = BuildWall(j * cellWidth - cellWidth / doorFactor, i * cellLength - thickness, j * cellWidth, i * cellLength, height);
                            GameObject westPost = BuildWall((j - 1) * cellWidth + thickness, i * cellLength - thickness, (j - 1) * cellWidth + cellWidth / doorFactor, i * cellLength, height);
                            eastPost.GetComponent<Renderer>().material.mainTexture = tex;
                            eastPost.transform.SetParent(parent);
                            westPost.GetComponent<Renderer>().material.mainTexture = tex;
                            westPost.transform.SetParent(parent);
                        }
                        else if (j == dungeon.GetLength(1) - 1)
                        {
                            GameObject eastPost = BuildWall(j * cellWidth - cellWidth / doorFactor, i * cellLength - thickness, j * cellWidth - thickness, i * cellLength, height);
                            GameObject westPost = BuildWall((j - 1) * cellWidth, i * cellLength - thickness, (j - 1) * cellWidth + cellWidth / doorFactor, i * cellLength, height);
                            eastPost.GetComponent<Renderer>().material.mainTexture = tex;
                            eastPost.transform.SetParent(parent);
                            westPost.GetComponent<Renderer>().material.mainTexture = tex;
                            westPost.transform.SetParent(parent);
                        }
                        else
                        {
                            GameObject eastPost = BuildWall(j * cellWidth - cellWidth / doorFactor, i * cellLength - thickness, j * cellWidth, i * cellLength, height);
                            GameObject westPost = BuildWall((j - 1) * cellWidth, i * cellLength - thickness, (j - 1) * cellWidth + cellWidth / doorFactor, i * cellLength, height);
                            eastPost.GetComponent<Renderer>().material.mainTexture = tex;
                            eastPost.transform.SetParent(parent);
                            westPost.GetComponent<Renderer>().material.mainTexture = tex;
                            westPost.transform.SetParent(parent);
                        }
                    }
                }
                if (j != dungeon.GetLength(1) - 1)
                {
                    if (dungeon[i, j].eastWall)
                    {
                        if (i == 1)
                        {
                            GameObject wall = BuildWall(j * cellWidth - thickness, (i - 1) * cellLength + thickness, j * cellWidth, i * cellLength - thickness, height);
                            wall.GetComponent<Renderer>().material.mainTexture = tex;
                            wall.transform.SetParent(parent);
                        }
                        else
                        {
                            GameObject wall = BuildWall(j * cellWidth - thickness, (i - 1) * cellLength, j * cellWidth, i * cellLength - thickness, height);
                            wall.GetComponent<Renderer>().material.mainTexture = tex;
                            wall.transform.SetParent(parent);
                        }
                    } 
                    else
                    {
                        if (i == 1)
                        {
                            GameObject northPost = BuildWall(j * cellWidth - thickness, i * cellLength - cellLength / doorFactor, j * cellWidth, i * cellLength - thickness, height);
                            GameObject southPost = BuildWall(j * cellWidth - thickness, (i - 1) * cellLength + thickness, j * cellWidth, (i - 1) * cellLength + cellLength / doorFactor, height);
                            northPost.GetComponent<Renderer>().material.mainTexture = tex;
                            northPost.transform.SetParent(parent);
                            southPost.GetComponent<Renderer>().material.mainTexture = tex;
                            southPost.transform.SetParent(parent);
                        }
                        else
                        {
                            GameObject northPost = BuildWall(j * cellWidth - thickness, i * cellLength - cellLength / doorFactor, j * cellWidth, i * cellLength - thickness, height);
                            GameObject southPost = BuildWall(j * cellWidth - thickness, (i - 1) * cellLength, j * cellWidth, (i - 1) * cellLength + cellLength / doorFactor, height);
                            northPost.GetComponent<Renderer>().material.mainTexture = tex;
                            northPost.transform.SetParent(parent);
                            southPost.GetComponent<Renderer>().material.mainTexture = tex;
                            southPost.transform.SetParent(parent);
                        }
                    }
                }
            }
        }

    }
    GameObject BuildWall(float minX, float minZ, float maxX, float maxZ, float height)
    {
        Mesh mesh = new Mesh();
        Vector3[] verts =
        {
            new Vector3(minX, 0, minZ),
            new Vector3(minX, 0, maxZ),
            new Vector3(maxX, 0, maxZ),
            new Vector3(maxX, 0, minZ),
            new Vector3(minX, height, minZ),
            new Vector3(minX, height, maxZ),
            new Vector3(maxX, height, maxZ),
            new Vector3(maxX, height, minZ)
        };
        int[] tris = { 
            1, 0, 2, 3, 2, 0, // bottom face
            0, 4, 7, 0, 7, 3, // south face
            2, 6, 5, 2, 5, 1, // north face
            1, 5, 4, 1, 4, 0, // west face
            3, 7, 6, 3, 6, 2, // east face
            4, 5, 6, 6, 7, 4 // top face
        };
        float uvVal = maxX - minX > maxZ - minZ ? maxX - minX : maxZ - minZ;
        Vector2[] uv =
        {
            new Vector2(uvVal, 0),
            new Vector2(0, 0),
            new Vector2(uvVal, 0),
            new Vector2(0, 0),
            new Vector2(uvVal, 1),
            new Vector2(0, 1),
            new Vector2(uvVal, 1),
            new Vector2(0, 1)
        };
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        GameObject wall = new GameObject("wall");
        wall.AddComponent<MeshFilter>();
        wall.AddComponent<MeshRenderer>();
        wall.GetComponent<MeshFilter>().mesh = mesh;
        return wall;
    } 
    void AddTreasureChests()
    {
        for (int i = 1; i < dungeon.GetLength(0); i++)
        {
            for (int j = 1; j < dungeon.GetLength(1); j++)
            {
                if (dungeon[i, j].treasureChest)
                {
                    GameObject chest = 
                        Instantiate(treasureChest, new Vector3((j - 1) * cellWidth + cellWidth / 2, 0, (i - 1) * cellLength + cellLength / 2), Quaternion.identity);
                    chest.transform.position = chest.transform.position - new Vector3(resX * cellWidth / 2, 0, resZ * cellLength / 2);
                    float scaleFactor = Mathf.Min(cellLength, cellWidth);
                    chest.transform.localScale *= scaleFactor;
                }
            }
        }
    }

    void CreateFloorPlan()
    {
        GameObject fp = new GameObject("Floor Plan");
        fp.AddComponent<MeshFilter>();
        fp.AddComponent<MeshRenderer>();
        fp.GetComponent<MeshFilter>().mesh = CreateFloorPlanMesh();

        fp.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);

        fp.GetComponent<Renderer>().material.mainTexture = CreateFloorPlanTexture();
    }

    Mesh CreateFloorPlanMesh()
    {
        Mesh floorPlan = new Mesh();
        float x = resX * cellWidth;
        x -= x / 2;
        float z = resZ * cellLength;
        z -= z / 2;
        Vector3[] verts = 
        {
            new Vector3(x, 0, -z),
            new Vector3(-x, 0, -z),
            new Vector3(-x, 0, z),
            new Vector3(x, 0, z)
        };
        int[] tris = { 0, 1, 2, 2, 3, 0 };
        Vector2[] uv = new Vector2[verts.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(verts[i].x > 0 ? 1 : 0, verts[i].z > 0 ? 1 : 0);
        }
        floorPlan.vertices = verts;
        floorPlan.triangles = tris;
        floorPlan.uv = uv;

        floorPlan.RecalculateNormals();

        return floorPlan;
    }

    Texture2D CreateFloorPlanTexture()
    {
        int textureWidth = textureScale * resX;
        int textureHeight = textureScale * resZ;
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        Color[] colors = new Color[textureWidth * textureHeight];
        
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        // left wall
        for (int i = 0; i < textureHeight; i++)
        {
            colors[i * textureWidth] = Color.black;
        }  
        // bottom wall
        for (int i = 0; i < textureWidth; i++)
        {
            colors[i] = Color.black;
        }
        
        for (int i = 1; i < dungeon.GetLength(0); i++)
        {
            for (int j = 1; j < dungeon.GetLength(1); j++)
            {
                if (dungeon[i, j].northWall)
                {
                    for (int k = 0; k < textureScale; k++)
                    {
                        colors[((i * textureScale) - 1) * textureWidth + (j - 1) * textureScale + k] = Color.black;
                    }
                }
                if (dungeon[i, j].eastWall)
                {
                    for (int k = 0; k < textureScale; k++)
                    {
                        colors[(i - 1) * textureScale * textureWidth + k * textureWidth + ((textureScale * j) - 1)] = Color.black;
                    }
                    if (i > 1)
                    {
                        colors[((i - 1) * textureScale - 1) * textureWidth + ((textureScale * j) - 1)] = Color.black;
                    }
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;

    }
}

class DungeonTile
{
    public static int total = 0;

    public bool northWall = true;
    public bool eastWall = true;
    public int id;
    public bool treasureChest;
    public DungeonTile(float treasureChestProb)
    {
        id = total++;
        treasureChest = Random.value <= treasureChestProb;
    }

    public void BreakNorthWall()
    {
        northWall = false;
    }
    public void BreakEastWall()
    {
        eastWall = false;
    }
}

enum DungeonWall
{
    NORTH = 0,
    EAST = 1
}