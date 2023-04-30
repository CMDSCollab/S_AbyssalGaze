using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_GroundMesh : MonoBehaviour
{
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Mesh newMesh;

    public float xRange;
    public float zRange;
    public int xVerticesDensity;
    private int depthIndex;

    private List<Transform> spawnSpots = new List<Transform>();
    private Dictionary<Transform, MineralType> getMineralType = new Dictionary<Transform, MineralType>();

    public void InitializeGroundValue(int targetDepth)
    {
        newMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = newMesh;
        CreateMeshShape();
        UpdateMeshShape();
        Transform triggerParent = M_Mineral.Instance.GenerateMineSpot(transform.position.y);
        foreach (SphereCollider sc in triggerParent.GetComponentsInChildren<SphereCollider>()) spawnSpots.Add(sc.transform);
        depthIndex = targetDepth;
        MineralSpotsGeneration();
    }

    void CreateMeshShape()
    {
        //Vertices generation
        int xRandomDensity = Random.Range(xVerticesDensity / 2, xVerticesDensity);
        vertices = new Vector3[xRandomDensity + 4];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 0, Random.Range(zRange / 2, zRange));
        vertices[vertices.Length - 2] = new Vector3(xRange, 0, Random.Range(zRange / 2, zRange));
        vertices[vertices.Length - 1] = new Vector3(xRange, 0, 0);

        float horiRange = xRange / xRandomDensity;
        float horiBefore = 0;
        for (int i = 2; i < vertices.Length - 2; i++)
        {
            float horiToExpend = Random.Range(horiRange - horiRange / 2, horiRange);
            vertices[i] = new Vector3(horiBefore + horiToExpend, 0, Random.Range(zRange / 2, zRange));
            while (Angle_360(Vector3.zero, vertices[i]) > Angle_360(Vector3.zero, vertices[i - 1]))
                vertices[i] = new Vector3(horiBefore + horiToExpend, 0, Random.Range(zRange / 2, zRange));
            horiBefore += horiToExpend;
        }

        //Expand Horizontal Vertices Distance
        if (vertices[vertices.Length - 2].x - vertices[vertices.Length - 3].x > 0.5f)
        {
            float equalValue = (vertices[vertices.Length - 2].x - vertices[vertices.Length - 3].x) / (vertices.Length + vertices.Length / 5);
            for (int i = 2, j = 1; i < vertices.Length - 2; i++)
            {
                vertices[i] = new Vector3(vertices[i].x + equalValue * j, vertices[i].y, vertices[i].z);
                j++;
            }
        }

        //Reset the position of the vertex in the right upper corner
        if (Angle_360(Vector3.zero, vertices[vertices.Length - 2]) > Angle_360(Vector3.zero, vertices[vertices.Length - 3]))
            vertices[vertices.Length - 2] = new Vector3(xRange, 0, Random.Range(0.1f, vertices[vertices.Length - 3].z));

        //UVs generation
        uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / xRange, vertices[i].z / zRange);
        }

        //Triangles generation
        triangles = new int[3 * (xRandomDensity + 2)];
        for (int i = 0, vert = 0; vert < triangles.Length; i++)
        {
            triangles[vert] = 0;
            triangles[vert + 1] = i + 1;
            triangles[vert + 2] = i + 2;
            vert += 3;
        }

        float Angle_360(Vector3 from_, Vector3 to_)
        {
            //两点的x、y值
            float x = from_.x - to_.x;
            float z = from_.z - to_.z;

            //斜边长度
            float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(z, 2f));

            //求出弧度
            float cos = x / hypotenuse;
            float radian = Mathf.Acos(cos);

            //用弧度算出角度    
            float angle = 180 / (Mathf.PI / radian);

            if (z < 0) angle = -angle;
            else if ((z == 0) && (x < 0)) angle = 180;

            return angle;
        }
    }

    void UpdateMeshShape()
    {
        newMesh.vertices = vertices;
        newMesh.uv = uvs;
        newMesh.triangles = triangles;
        newMesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = newMesh;
    }

    private void MineralSpotsGeneration()
    {
        float mineDistributionRatio = Random.Range(0.1f, 0.2f);
        int spotsInTotal = spawnSpots.Count;
        int spotsForMine = Mathf.RoundToInt(spotsInTotal * mineDistributionRatio);
        //Debug.Log(spotsInTotal + "  " + spotsForMine);
        List<Transform> residueSpots = spawnSpots;
        List<Transform> toMineSpots = new List<Transform>();

        List<MineralInfo> permitedMinerals = GetMineralList(depthIndex);
        int totalPossibility = 0;
        foreach (MineralInfo mine in permitedMinerals) totalPossibility += mine.emergePossibility;

        for (int i = 0; i < spotsForMine; i++)
        {
            MineralInfo mineralToSpawn = permitedMinerals[GetRandomMineral()];

            int randomSpot = Random.Range(0, residueSpots.Count);
            Transform newMineSpot = residueSpots[randomSpot];
            toMineSpots.Add(newMineSpot);
            residueSpots.Remove(newMineSpot);

            newMineSpot.tag = "Mineral";
            getMineralType.Add(newMineSpot, mineralToSpawn.type);

            //GameObject lightingObj = new GameObject();
            //lightingObj.transform.SetParent(newMineSpot);
            //lightingObj.transform.localPosition = new Vector3(0, 5, 0);
            //Light newLighting = lightingObj.AddComponent<Light>();
            //newLighting.type = LightType.Spot;
            //newLighting.color = mineralToSpawn.lightColor;
            //newLighting.intensity = 40;
            //lightingObj.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        foreach (Transform transform in residueSpots) Destroy(transform.gameObject, 0.2f);

        int GetRandomMineral()
        {
            int counter = 0;
            int randomMineralArea = Random.Range(0, totalPossibility);
            for (int i = 0; i < permitedMinerals.Count; i++)
            {
                counter += permitedMinerals[i].emergePossibility;
                if (randomMineralArea <= counter) return i;
                else continue;
            }
            return 0;
        }
    }

    private List<MineralInfo> GetMineralList(int depth)
    {
        List<MineralInfo> permitedMinerals = new List<MineralInfo>();
        foreach (MineralInfo mineralInfo in M_Major.Instance.repository.minerals)
        {
            if (mineralInfo.minDepth <= depth && mineralInfo.maxDepth >= depth)
            {
                permitedMinerals.Add(mineralInfo);
            }
        }
        return permitedMinerals;
    }

    private void OnDestroy()
    {
        //if (triggerParent!= null) Destroy(triggerParent.gameObject);
    }

    public MineralType GetMineralType(Transform transform)
    {
        if (getMineralType.ContainsKey(transform)) return getMineralType[transform];
        else return MineralType.M10;
    }
}
