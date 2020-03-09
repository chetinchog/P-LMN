using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iCTG_MeshCreator : MonoBehaviour
{
    [Header("| Data")]
    public Vector3[] listVector;
    public Vector3[] listTriangle;
    public Material material;
    [Header("| Config")]
    public float duration;
    public float startY;
    public float endY;
    public float velocity;
    [Header("| Runners")]
    public bool onStart;
    public bool onUpdate;

    private Vector3[] m_listTriangle;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Mesh mesh;
    private bool direction;

    private void Start()
    {
        if (onStart)
            Render(duration);
    }

    private void Update()
    {
        if (onUpdate)
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
        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

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
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.uv = uvs;

        meshCollider.sharedMesh = mesh;

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }
}
