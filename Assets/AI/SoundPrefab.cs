using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrefab : MonoBehaviour
{
    [SerializeField]
    GameObject soundInstancePrefab = new GameObject();

    #region Constructors
    SoundPrefab(Mesh newMesh)
    {
        try
        {
            GetComponent<MeshFilter>().mesh = newMesh;
        }
        catch { }
    }
    SoundPrefab(Mesh newMesh, Material newMaterial)
    {
        try
        {
            GetComponent<MeshFilter>().mesh = newMesh;
            GetComponent<MeshRenderer>().material = newMaterial;
        }
        catch { }
    }
    SoundPrefab(Mesh newMesh, Material[] newMaterials)
    {
        try
        {
            GetComponent<MeshFilter>().mesh = newMesh;
            GetComponent<MeshRenderer>().materials = newMaterials;
        }
        catch { }
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(soundInstancePrefab, transform);
    }
}
