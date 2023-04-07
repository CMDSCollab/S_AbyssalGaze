using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHandler : MonoBehaviour
{
    public Material mat_Reef;

    public int xSize;
    public int zSize;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Mesh newMesh;

    public float xRange;
    public float zRange;
    public int xVerticesDensity;
    // Start is called before the first frame update
    void Start()
    {
        newMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = newMesh;
        CreateMeshShape();
    }

    void Update()
    {
        UpdateMeshShape();
    }

    void CreateMeshShape()
    {
        //StartCoroutine( CreateGridMap());

        StartCoroutine(CreateRandomShape());

        IEnumerator CreateRandomShape()
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
            for (int i = 2; i < vertices.Length-2; i++)
            {
                float horiToExpend = Random.Range(horiRange - horiRange / 2, horiRange);
                vertices[i] = new Vector3(horiBefore + horiToExpend, 0, Random.Range(zRange / 2, zRange));
                while (Angle_360(Vector3.zero, vertices[i] )> Angle_360(Vector3.zero, vertices[i - 1]))
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
                uvs[i] = new Vector2(vertices[i].x/xRange, vertices[i].z/zRange);
            }
            //uvs[uvs.Length - 1] = new Vector2(uvs.Length - 1, uvs.Length-1);
            //uvs[uvs.Length - 1] = new Vector2(uvs.Length - 1, 0);
            //uvs[0] = new Vector2(0, 0);
            //uvs[1] = new Vector2(0, 1);
            //uvs[2] = new Vector2(0, 2);
            //uvs[3] = new Vector2(0, 3);

            //Triangles generation
            triangles = new int[3 * (xRandomDensity + 2)];
            for (int i = 0, vert = 0; vert < triangles.Length; i++)
            {
                triangles[vert] = 0;
                triangles[vert + 1] = i + 1;
                triangles[vert + 2] = i + 2;
                vert += 3;
                yield return new WaitForSeconds(0.1f);
            }
            //FindObjectOfType<M_GroundMesh>().InstantiateNewGround();
            //Destroy(gameObject, 0.1f);
            
            //triangles[0] = 0;
            //triangles[1] = 1;
            //triangles[2] = 2;

            //triangles[3] = 0;
            //triangles[4] = 2;
            //triangles[5] = 3;

            //Debug.Log("DrawNext");

            //vertices = new Vector3[(xSize + 1) * (zSize + 1)];
            //for (int i = 0, z = 0; z <= zSize; z++)
            //{
            //    for (int x = 0; x <= xSize; x++)
            //    {
            //        vertices[i] = new Vector3(x, 0, z);
            //        i++;
            //    }
            //}

            //uvs = new Vector2[(xSize + 1) * (zSize + 1)];
            //for (int i = 0, z = 0; z <= zSize; z++)
            //{
            //    for (int x = 0; x <= xSize; x++)
            //    {
            //        uvs[i] = new Vector2(x, z);
            //        i++;
            //    }
            //}

            //uvs = new Vector2[4];
            //uvs[0] = new Vector2(0, 1);
            //uvs[1] = new Vector2(1, 1);
            //uvs[2] = new Vector2(0, 0);
            //uvs[3] = new Vector2(1, 0);

            //triangles = new int[xSize * zSize * 6];
            //int vert = 0;
            //int tris = 0;
            //for (int z = 0; z < zSize; z++)
            //{
            //    for (int x = 0; x < xSize; x++)
            //    {
            //        triangles[tris + 0] = vert + 0;
            //        triangles[tris + 1] = vert + xSize + 1;
            //        triangles[tris + 2] = vert + 1;
            //        triangles[tris + 3] = vert + 1;
            //        triangles[tris + 4] = vert + xSize + 1;
            //        triangles[tris + 5] = vert + xSize + 2;

            //        vert++;
            //        tris += 6;
            //        yield return new WaitForSeconds(0.1f);
            //    }
            //    vert++;
            //}
        }

        //IEnumerator CreateGridMap()
        //{
        //    vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        //    for (int i = 0, z = 0; z <= zSize; z++)
        //    {
        //        for (int x = 0; x <= xSize; x++)
        //        {
        //            vertices[i] = new Vector3(x, 0, z);
        //            i++;
        //        }
        //    }

        //    uvs = new Vector2[(xSize + 1) * (zSize + 1)];
        //    for (int i = 0, z = 0; z <= zSize; z++)
        //    {
        //        for (int x = 0; x <= xSize; x++)
        //        {
        //            uvs[i] = new Vector2(x, z);
        //            i++;
        //        }
        //    }

        //    //uvs = new Vector2[4];
        //    //uvs[0] = new Vector2(0, 1);
        //    //uvs[1] = new Vector2(1, 1);
        //    //uvs[2] = new Vector2(0, 0);
        //    //uvs[3] = new Vector2(1, 0);

        //    triangles = new int[xSize * zSize * 6];
        //    int vert = 0;
        //    int tris = 0;
        //    for (int z = 0; z < zSize; z++)
        //    {
        //        for (int x = 0; x < xSize; x++)
        //        {
        //            triangles[tris + 0] = vert + 0;
        //            triangles[tris + 1] = vert + xSize + 1;
        //            triangles[tris + 2] = vert + 1;
        //            triangles[tris + 3] = vert + 1;
        //            triangles[tris + 4] = vert + xSize + 1;
        //            triangles[tris + 5] = vert + xSize + 2;

        //            vert++;
        //            tris += 6;
        //            yield return new WaitForSeconds(0.1f);
        //        }
        //        vert++;
        //    }
        //}

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
        //newMesh.Clear();
        newMesh.vertices = vertices;
        newMesh.uv = uvs;
        newMesh.triangles = triangles;

        //Debug.Log(newMesh.uv.Length);
       
        newMesh.RecalculateNormals();

        //transform.localScale = new Vector3(xSize, 1, zSize);
        //GetComponent<MeshFilter>().mesh = newMesh;
        //GetComponent<MeshRenderer>().material = mat_Reef;
        //GetComponent<MeshCollider>().sharedMesh = newMesh;
    }

    private void OnDrawGizmos()
    {
        if (vertices!=null)
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
