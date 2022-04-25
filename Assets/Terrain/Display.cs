using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public Renderer mapRender;
    public MeshFilter filter;
    public MeshRenderer meshrend;
    public MeshCollider meshCollider;
    public void DrawMap(Texture2D map)
    {
        
        mapRender.sharedMaterial.mainTexture = map;
        mapRender.transform.localScale = new Vector3(map.width, 1, map.height);
    }
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        filter.sharedMesh = meshData.CreateMesh();
        meshrend.sharedMaterial.mainTexture = texture;
        meshCollider.sharedMesh = meshData.CreateMesh();
    }
}
