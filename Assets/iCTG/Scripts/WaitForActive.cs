using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForActive : MonoBehaviour
{
    public float delay;
    public float duration;
    public bool onStart;
    public Material material;

    void Start()
    {
        if (onStart)
            StartCoroutine(Wait());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        iCTG_MeshCreator[] listObj = GetComponentsInChildren<iCTG_MeshCreator>(true);
        yield return new WaitForSeconds(delay);
        foreach (iCTG_MeshCreator meshCreator in listObj)
        {
            meshCreator.material = material;
            meshCreator.Render(duration);
        }
    }
}
