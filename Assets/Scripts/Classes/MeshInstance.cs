using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshInstance
{
    public Material InstanceMaterial;
    public int MeshInstantUpdateMargin;

    private int _lastMeshCount = 0;
    private int _currentMeshCount = 0;
    private List<GameObject> _spawnedObjects;
    private GameObject _meshCombinedInstance;

    public MeshInstance()
    {
        this._spawnedObjects = new List<GameObject>();
        this._meshCombinedInstance = null;
        this.MeshInstantUpdateMargin = 20;
        
    }

    public bool UpdateMeshesInstant(List<GameObject> spawnObjects, List<Vector3> meshPoints)
    {
        if((spawnObjects.Count - this._lastMeshCount) < this.MeshInstantUpdateMargin)
           return false;

        bool result = false;

        this._currentMeshCount = spawnObjects.Count;

     
        if (this._currentMeshCount <= meshPoints.Count)
        {

             for (int i = this._lastMeshCount; i < this._currentMeshCount; i++)
             {
                GameObject spawnedObject = GameObject.Instantiate(spawnObjects[i]);
                spawnedObject.transform.position = meshPoints[i];
                this._spawnedObjects.Add(spawnedObject);
             }

             this._lastMeshCount = this._currentMeshCount;
            //this._lastMeshCount = this._spawnedObjects.Count;


            result = true;
           
        }
        else
        {
            result = false;
        }


        return result; 

    }


   

    public void CreateInstanceMesh(GameObject instanceObject)
    {
        if (_spawnedObjects.Count < 20)
            return;

        Mesh finalMesh = new Mesh();

        List<MeshFilter> meshFilterList = new List<MeshFilter>();

        if(instanceObject.GetComponent<MeshFilter>() != null)
           meshFilterList.Add(instanceObject.GetComponent<MeshFilter>());

        foreach (GameObject spawnedObject in _spawnedObjects)
        {
            meshFilterList.Add(spawnedObject.GetComponent<MeshFilter>());
        }

        CombineInstance[] combiners = new CombineInstance[meshFilterList.Count];


        for (int i = 0; i < meshFilterList.Count; i++)
        {
            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = meshFilterList[i].sharedMesh;
            combiners[i].transform = meshFilterList[i].transform.localToWorldMatrix;
        }

        finalMesh.indexFormat = IndexFormat.UInt32;
        finalMesh.CombineMeshes(combiners);

        if(instanceObject.GetComponent<MeshRenderer>() == null)
           instanceObject.AddComponent<MeshRenderer>();

        if(instanceObject.GetComponent<MeshFilter>() == null)
           instanceObject.AddComponent<MeshFilter>();

        instanceObject.GetComponent<MeshFilter>().sharedMesh = finalMesh;
        instanceObject.GetComponent<MeshRenderer>().material = InstanceMaterial;

        meshFilterList.Clear();

        foreach(GameObject go in _spawnedObjects)
        {
            GameObject.DestroyImmediate(go);
        }

        _spawnedObjects.Clear();

    }



}
