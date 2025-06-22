using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GridMesh : MonoBehaviour
{
    public int GridSize;

    private void Reset()
    {
        CreateMesh();
    }

    void Awake()
    {
        CreateMesh();
    }

    private void CreateMesh()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();        
        var mesh = new Mesh();
        var verticies = new Vector3[3];
        var uv = new Vector2[3];
        var triangles = new int[3];
        
        verticies[0] = new Vector3(0, 0, 0);
        verticies[1] = new Vector3(0, 0, 100);
        verticies[2] = new Vector3(100, 0, 100);
        
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        
        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;
        filter.mesh = mesh;
    }
}