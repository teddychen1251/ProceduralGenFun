using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour
{
    const float BASE_GROW_DIST = .5f;
    const int MAX_ORDER = 5;
    const float TRUNK_RADIUS_BASELINE = .2f;
    const float K = 0.2f;

    public int seed = 0;
    public Color color1 = new Color(139 / 256f, 69 / 256f, 19 / 256f);
    public Color color2 = new Color(255 / 256f, 228 / 256f, 205 / 256f);
    public bool orthotropic = true;
    public int branchResolution = 8;

    private Color color;

    void Start()
    {
        Random.InitState(seed);

        List<Node> roots = new List<Node>();
        // Node root = new Node(new Vector3(), Vector3.up, 0, 0, false);
        int x = 0;
        for (int i = 0; i < 3; i++)
        {
            color = Color.Lerp(color1, color2, Random.value);
            Node root = new Node(new Vector3(x, 0, 0), Vector3.up, 0, 0, orthotropic);
            roots.Add(root);
            for (int j = 0; j < 14; j++)
            {
                root.Step();
            }
            List<List<Vector3>>[] branches = root.GetBranches();
            for (int k = 0; k < branches.Length; k++)
            {
                float radius = TRUNK_RADIUS_BASELINE / (k + 1);
                branches[k].ForEach(branch => CreateBranch(branch, radius, branchResolution));
            }
            x += 8;
        }
        
    }

    void CreateBranch(List<Vector3> points, float radius, int segments)
    {
        GameObject branch = new GameObject("branch");
        branch.AddComponent<MeshFilter>();
        branch.AddComponent<MeshRenderer>();
        
        Mesh branchMesh = new Mesh();
        Vector3[] verts = new Vector3[points.Count * segments + 1];
        int[] tris = new int[(points.Count - 1) * segments * 2 * 3 + segments * 3];

        Vector3 tan = (points[1] - points[0]).normalized;
        // calculate any normal
        Vector3 norm = new Vector3(tan.z == 0 ? 0 : -1, 0, tan.z == 0 ? 1 : tan.x / tan.z).normalized;
        Vector3 biNorm = Vector3.Cross(tan, norm);
        // vertices
        int vIndex = 0;
        float angle = 2 * Mathf.PI / segments;
        for (int i = 0; i < points.Count; i++)
        {
            if (i < points.Count - 1)
            {
                tan = (points[i + 1] - points[i]).normalized;
                biNorm = Vector3.Cross(tan, norm).normalized;
                norm = Vector3.Cross(biNorm, tan);
            }
            for (float thetaFac = 0; thetaFac < segments; thetaFac++)
            {
                verts[vIndex++] = points[i] + (radius) * (norm * Mathf.Cos(thetaFac * angle) + biNorm * Mathf.Sin(thetaFac * angle));
            }
        }
        // triangles
        int tIndex = 0;
        for (int i = 1; i < points.Count; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                tris[tIndex++] = (i - 1) * segments + j % segments;
                tris[tIndex++] = i * segments + (j + 1) % segments;
                tris[tIndex++] = i * segments + j % segments;
                tris[tIndex++] = (i - 1) * segments + j % segments;
                tris[tIndex++] = (i - 1) * segments + (j + 1) % segments;
                tris[tIndex++] = i * segments + (j + 1) % segments;
            }
        }
        // closing the cylinder
        verts[vIndex++] = points[points.Count - 1];
        for (int i = 0; i < segments; i++)
        {
            tris[tIndex++] = verts.Length - 1;
            tris[tIndex++] = verts.Length - 2 - ((i + 1) % segments);
            tris[tIndex++] = verts.Length - 2 - i;
        }


        branchMesh.vertices = verts;
        branchMesh.triangles = tris;
        branchMesh.RecalculateNormals();
        branch.GetComponent<MeshFilter>().mesh = branchMesh;
        branch.GetComponent<Renderer>().material.color = color;
    }

    class Node
    {

        Vector3 pos;
        bool ortho; // ortho or plagio
        Bud apical;
        Node next;
        List<Bud> axillaries = new List<Bud>();
        List<Node> axNodes = new List<Node>();

        static float k = K;

        public Node(Vector3 pos, Vector3 dir, int order, int axCount, bool ortho)
        {
            this.pos = pos;
            this.ortho = ortho;
            this.apical = new Bud(dir, 0, 0, order);
            if (axCount > 0)
            {
                float angleInc = 2 * Mathf.PI / axCount;
                float initAngle = Random.value * 2 * Mathf.PI;
                Vector3 norm = new Vector3(dir.z == 0 ? 0 : -1, 0, dir.z == 0 ? 1 : dir.x / dir.z).normalized;
                Vector3 biNorm = Vector3.Cross(dir, norm).normalized;
                for (int i = 0; i < axCount; i++)
                {
                    float orthoBaseBias = 0.3f;
                    float varianceBaseBias = 1 - orthoBaseBias;
                    float orthoBias = Random.value * varianceBaseBias;
                    float apicalBias = varianceBaseBias - orthoBias;
                    Vector3 orthogonal = norm * Mathf.Cos(initAngle + angleInc * i) + biNorm * Mathf.Sin(initAngle + angleInc * i);
                    Vector3 axDir = (orthoBaseBias + orthoBias) * orthogonal + apicalBias * dir;
                    axillaries.Add(
                        new Bud(
                            axDir, 0, 0, order + 1
                        )
                    );
                }
            }
        }

        public void Step()
        {
            if (next == null)
            {
                if (apical.live)
                {
                    bool grew = apical.Step();
                    if (grew)
                    {
                        int axCount = 0;
                        if (apical.order == 0)
                        {
                            axCount = Mathf.FloorToInt(2 * Mathf.Pow(Random.value, .5f));
                        }
                        else if (apical.order == 1)
                        {
                            axCount = Mathf.FloorToInt(3 * Random.value);
                        }
                        Vector3 newDir = apical.dir;
                        if (apical.order > 0)
                        {
                            if (ortho)
                            {
                                newDir = (newDir + k * Vector3.up).normalized;
                            }
                            else
                            {
                                Vector3 hor = new Vector3(newDir.x, 0, newDir.z).normalized;
                                newDir = (newDir + k * hor).normalized;
                            }
                        }
                        float growDist = BASE_GROW_DIST / (.25f * apical.order + 1);
                        Vector3 destination = pos + growDist * apical.dir;
                        next = new Node(destination, newDir, apical.order, axCount, ortho);
                    }
                }
            } 
            else
            {
                next.Step();
            }
            axNodes.ForEach(axNode => axNode.Step());
            for (int i = 0; i < axillaries.Count; i++)
            {
                Bud bud = axillaries[i];
                bool grew = bud.Step();
                if (grew)
                {
                    int axCount = 0;
                    if (bud.order == 0)
                    {
                        axCount = Mathf.FloorToInt(2 * Mathf.Pow(Random.value, .5f));
                    }
                    else if (bud.order == 1)
                    {
                        axCount = Mathf.FloorToInt(3 * Random.value);
                    }
                    Vector3 newDir = bud.dir;
                    if (ortho)
                    {
                        newDir = (newDir + k * Vector3.up).normalized;
                    }
                    else
                    {
                        Vector3 hor = new Vector3(newDir.x, 0, newDir.z).normalized;
                        newDir = (newDir + k * hor).normalized;
                    }
                    float growDist = BASE_GROW_DIST / (.25f * bud.order + 1);
                    Vector3 destination = pos + growDist * bud.dir;
                    Node axNode = new Node(destination, newDir, bud.order, axCount, ortho);
                    axNodes.Add(axNode);
                    axillaries.RemoveAt(i);
                    i--;
                }
            }
            
        }

        public List<List<Vector3>>[] GetBranches()
        {
            List<List<Vector3>>[] branches = new List<List<Vector3>>[MAX_ORDER];
            for (int i = 0; i < branches.Length; i++) {
                branches[i] = new List<List<Vector3>>();
            }
            List<Vector3> trunk = new List<Vector3>();
            branches[0].Add(trunk);
            rGetBranches(trunk, branches);

            return branches;
        }
        private void rGetBranches(List<Vector3> currBranch, List<List<Vector3>>[] branches)
        {
            currBranch.Add(pos);
            if (next != null)
            {
                next.rGetBranches(currBranch, branches);
            }
            // handle axillaries
            foreach (Node node in axNodes)
            {
                List<Vector3> axBranch = new List<Vector3>();
                axBranch.Add(pos);
                branches[node.apical.order].Add(axBranch);
                node.rGetBranches(axBranch, branches);
            }
        }
    }
    class Bud
    {
        public Vector3 dir;
        float die;
        float pause;
        public int order;
        public bool live = true;

        public Bud(Vector3 dir, float dieProb, float pauseProb, int order)
        {
            this.dir = dir;
            this.die = dieProb;
            this.pause = pauseProb;
            this.order = order;
        }

        public bool Step()
        {
            if (!live) return false;
            
            float prob = Random.value;
            if (prob <= die)
            {
                live = false;
                return false;
            }
            else if (prob > pause)
            {
                return true;
            }
            return false;
        }

    }

}
