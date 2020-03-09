using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iCTG_MeshCreator : MonoBehaviour
{
    public Vector3[] listVector;
    public Vector3[] listTriangle;
    public Material material;
    public float duration;
    public float startY;
    public float endY;
    public float velocity;
    public bool onStart;

    private Vector3[] m_listTriangle;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private bool direction;

    private void Start()
    {
        if (onStart)
            Render(duration);
    }

    private void Update()
    {
        Render(0f);
        float amount = Time.time * velocity * .001f;
        for (int i = 0; i < listVector.Length; i++)
        {
            if (i % 2 == 0)
            {
                if (listVector[i].y <= startY)
                    direction = true;
                else if (listVector[i].y >= endY)
                    direction = false;

                if (direction)
                    listVector[i].y += amount;
                else
                    listVector[i].y -= amount;
            }
        }
    }

    public void Render(float time = 0f)
    {
        PrepareMesh();
        foreach (Vector3 triangle in listTriangle)
        {
            StartCoroutine(WaitForCreate(triangle, UnityEngine.Random.Range(0f, time)));
        }
    }

    private IEnumerator WaitForCreate(Vector3 triangle, float time = 0)
    {
        yield return new WaitForSeconds(time);
        AddTriangle(triangle);
    }

    private void PrepareMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            meshFilter.mesh = new Mesh();
            mesh = meshFilter.sharedMesh;
        }
        mesh.Clear();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        if (material)
            meshRenderer.material = material;
    }

    private void AddTriangle(Vector3 triangle)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length + 3];
        int[] triangles = new int[mesh.triangles.Length + 3];
        if (mesh.vertices.Length > 0)
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices[i] = mesh.vertices[i];
                triangles[i] = mesh.triangles[i];
            }

        vertices[mesh.vertices.Length] = listVector[(int)triangle.x];
        vertices[mesh.vertices.Length + 1] = listVector[(int)triangle.y];
        vertices[mesh.vertices.Length + 2] = listVector[(int)triangle.z];

        triangles[mesh.triangles.Length] = mesh.triangles.Length;
        triangles[mesh.triangles.Length + 1] = mesh.triangles.Length + 1;
        triangles[mesh.triangles.Length + 2] = mesh.triangles.Length + 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    public int GetCantTriangles()
    {
        return listTriangle.Length;
    }
}
