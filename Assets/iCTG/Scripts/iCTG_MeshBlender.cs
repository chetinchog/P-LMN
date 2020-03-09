using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iCTG_MeshBlender : MonoBehaviour
{
    [SerializeField]
    public Tetrahedron tetrahedron;

    private void Start()
    {
        StartCoroutine(BlendMesh());
    }

    private IEnumerator BlendMesh()
    {
        yield return new WaitForEndOfFrame();
        if (tetrahedron)
        {
            tetrahedron.p0 = tetrahedron.p0 + tetrahedron.p0 * .001f;
            tetrahedron.p1 = tetrahedron.p1 + tetrahedron.p1 * .001f;
            tetrahedron.p2 = tetrahedron.p2 + tetrahedron.p2 * .001f;
            tetrahedron.Rebuild();
        }
        StartCoroutine(BlendMesh());
    }

    public void Generate()
    {
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter not found!");
            return;
        }
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            meshFilter.mesh = new Mesh();
            mesh = meshFilter.sharedMesh;
        }
        mesh.Clear();


        mesh.vertices = new Vector3[]{
                p0,p1,p2,
                p0,p2,p3,
                p2,p1,p3,
                p0,p3,p1
            };

        mesh.triangles = new int[]{
                0,1,2,
                3,4,5,
                6,7,8,
                9,10,11
            };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }
}
