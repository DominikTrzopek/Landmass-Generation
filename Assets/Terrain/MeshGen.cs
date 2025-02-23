using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen 
{
   public static MeshData GenerateTerrain(float[,] heightMap, float heightMulti, AnimationCurve animationCurve, int LOD)
    {

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftY = (height - 1) / 2f;
        int vertexIndex = 0;
        int meshSimplificationIncrement = (LOD == 0)?1:LOD * 2;
        int verticlePerLine = (width - 1) / meshSimplificationIncrement + 1;
        MeshData meshData = new MeshData(verticlePerLine, verticlePerLine);
        for(int y = 0; y < height; y+= meshSimplificationIncrement)
        {
            for(int x = 0; x < width; x+= meshSimplificationIncrement)
            {
                meshData.verticles[vertexIndex] = new Vector3(topLeftX + x, animationCurve.Evaluate(heightMap[x, y]) * heightMulti, topLeftY - y);
                //Debug.Log(heightMap[x, y]);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                if(x < width - 1 && y < height -1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticlePerLine + 1, vertexIndex + verticlePerLine);
                    meshData.AddTriangle(vertexIndex + 1 + verticlePerLine, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }

        return meshData;
   }
}
public class MeshData
{
    public Vector3[] verticles;
    public int[] triangles;
    int triangleIndex;
    public Vector2[] uvs;
    public MeshData(int mapWidth, int mapHeight)
    {
        verticles = new Vector3[mapWidth * mapHeight];
        uvs = new Vector2[mapWidth * mapHeight];
        triangles = new int[(mapHeight - 1) * (mapWidth - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}