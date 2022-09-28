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
        if((spawnObjects.Count - this._lastMeshCount) <= this.MeshInstantUpdateMargin)
           return false;

        bool result = false;

        this._currentMeshCount = spawnObjects.Count;

        int validLastMeshCount = Mathf.Clamp(this._currentMeshCount, 0, meshPoints.Count);

        for (int i = this._lastMeshCount; i < validLastMeshCount; i++)
        {
            GameObject spawnedObject = GameObject.Instantiate(spawnObjects[i]);
            spawnedObject.transform.position = meshPoints[i];
            this._spawnedObjects.Add(spawnedObject);
        }

        this._lastMeshCount = this._currentMeshCount;

        result = true;

        Debug.Log("Valid Last Mesh Count : " + validLastMeshCount);
        Debug.Log("Actual Last Mesh Count : " + this._lastMeshCount);

        #region Unoptimized Code
        /*
           if (this._currentMeshCount <= meshPoints.Count)
           {

                for (int i = this._lastMeshCount; i < this._currentMeshCount; i++)
                {
                   GameObject spawnedObject = GameObject.Instantiate(spawnObjects[i]);
                   spawnedObject.transform.position = meshPoints[i];
                   this._spawnedObjects.Add(spawnedObject);
                }

                this._lastMeshCount = this._currentMeshCount;


               result = true;

           }
           else
           {

               int validLastMeshCount = Mathf.Clamp(this._currentMeshCount, 0, meshPoints.Count);

               for (int i = this._lastMeshCount; i < validLastMeshCount; i++)
               {
                   GameObject spawnedObject = GameObject.Instantiate(spawnObjects[i]);
                   spawnedObject.transform.position = meshPoints[i];
                   this._spawnedObjects.Add(spawnedObject);
               }

               this._lastMeshCount = this._currentMeshCount;

               result = true;

               //Debug.Log("Spawned Mesh Count : " + validLastMeshCount);
           }

           Debug.Log("Last Mesh Count : " + this._lastMeshCount);
       */

        #endregion

        return result; 

    }

    
    public List<MeshData<GameObject, Vector3, Vector3>> GetManualUpdatableMeshes(List<GameObject> spawnObjects, List<Vector3> startPoints, List<Vector3> meshPoints)
    {
        if ((spawnObjects.Count - this._lastMeshCount) == 0 || (spawnObjects.Count - this._lastMeshCount) > this.MeshInstantUpdateMargin)
            return null;

            List<MeshData<GameObject, Vector3, Vector3>> result = new List<MeshData<GameObject, Vector3, Vector3>>();

        if (this._lastMeshCount + (spawnObjects.Count - this._lastMeshCount) <= meshPoints.Count)
        {

            for (int i = 0; i < (spawnObjects.Count - this._lastMeshCount); i++)
            {
                MeshData<GameObject, Vector3, Vector3> meshData = new MeshData<GameObject, Vector3, Vector3>();
                meshData.GameObject = spawnObjects[this._lastMeshCount + i];
                meshData.StartPoint = startPoints[this._lastMeshCount + i];
                meshData.EndPoint   = meshPoints[this._lastMeshCount + i];
                result.Add(meshData);
            }
            Debug.Log("Mesh Points Not Full then return next available meshPoints");
        }
        else
        {
            for (int j = 0; j < (spawnObjects.Count - this._lastMeshCount); j++)
            {
                MeshData<GameObject, Vector3, Vector3> meshData = new MeshData<GameObject, Vector3, Vector3>();
                meshData.GameObject = spawnObjects[this._lastMeshCount + j];
                meshData.StartPoint = startPoints[this._lastMeshCount + j];
                int randomMeshPointID = Random.Range(0, meshPoints.Count);
                meshData.EndPoint = meshPoints[randomMeshPointID];
                result.Add(meshData);
            }
            Debug.Log("Mesh Points Full then return meshPoints from used meshpoints");
        }


        return result;
        

    }

    public void UpdateMeshInstant(MeshData<GameObject, Vector3, Vector3> meshData, List<Vector3> meshPoints)
    {
        if((this._lastMeshCount + 1) < meshPoints.Count)
        {
            GameObject spawnedObject = GameObject.Instantiate(meshData.GameObject);
            spawnedObject.transform.position = meshData.EndPoint;
            this._spawnedObjects.Add(spawnedObject);
            this._currentMeshCount++;
            this._lastMeshCount = this._currentMeshCount;
            Debug.Log("Added Single Mesh Instant!");
        }
        else
        {
            GameObject spawnedObject = GameObject.Instantiate(meshData.GameObject);
            spawnedObject.transform.position = meshData.EndPoint;
            this._currentMeshCount++;
            this._lastMeshCount = this._currentMeshCount;
            GameObject.DestroyImmediate(spawnedObject);
            Debug.Log("Destroy Single Mesh because of list is full");
        }
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
            //combiners[i].mesh.uv = meshFilterList[i].sharedMesh.uv;
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
