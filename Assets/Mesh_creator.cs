using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mesh_creator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] points;
    int[] triangles;
    public int xSize = 40;
    public int zSize = 40;
    void GenerateMesh()
    {
        points = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(z * .3f, x * .3f) * 2f;
                points[i] = new Vector3(x, y, z);
                i++;
            }
        }
        int tris = 0, vert = 0;
        triangles = new int[xSize * zSize * 6];
        for (int z = 0; z < zSize; z++)
        {
            for (int i = 0; i < xSize; i++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + 2 + xSize;
                vert++;
                tris += 6;
            }
            vert++;
        }
        
        
    }
    void UseMesh()
    {
        mesh.Clear();
        mesh.vertices = points;
        mesh.triangles = triangles;
    }
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GenerateMesh();
        UseMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
       
    }
   
}
