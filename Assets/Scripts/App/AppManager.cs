using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    public Text DebugTX;

    //For Manual Updatable Spawn Objects
    private List<GameObject> _manualSpawnObjects;
    //End of For Manual Updatable Spawn Objects

    private MeshDataManager _meshDataManager;
    private XMLFileManager _xmlFileManager;
    private DatabaseManager _databaseManager;
    private WishesManager _wishesManager;

   
    void Awake()
    {
        Initialize();  
    }


    void Update()
    {
        //For Manual Updatable Spawn Objects
        if (Input.GetKeyDown(KeyCode.P))
        {
            try
            {
                ManualUpdateWishData(15);
            }
            catch(Exception e)
            {
                DebugTX.text = e.Message;
            }
           
        }
        //End of For Manual Updatable Spawn Objects  
    }

    void Initialize()
    {
        
       _meshDataManager = GetComponent<MeshDataManager>();
       _xmlFileManager = GetComponent<XMLFileManager>();
       _databaseManager = GetComponent<DatabaseManager>();
       _wishesManager = GetComponent<WishesManager>();

        
       CreateOrLoadRandomMeshPoints();
        
        //Comment for Activate Manual Update
       //UpdateWishesData();


        //For Manual Updatable Spawn Objects
        _manualSpawnObjects = new List<GameObject>();
        //End of For Manual Updatable Spawn Objects

    }



    /// <summary>
    /// Create or Load MeshPoint Data
    /// </summary>
    void CreateOrLoadRandomMeshPoints()
    {
        bool isXmlFound = _xmlFileManager.CheckXMLFile("MyXML");

        if (isXmlFound)
        {
            _xmlFileManager.ReadXML();
            _xmlFileManager.LoadXMLRawData();

            _meshDataManager.RandomMeshPointList = new List<Vector3>();

            foreach (string xmlData in _xmlFileManager.XMLRawDataList)
            {
                Vector3 point = _xmlFileManager.GetXMLValue<Vector3>(xmlData);
                _meshDataManager.RandomMeshPointList.Add(point);
            }

            Debug.Log("Read Mesh Points : " + _meshDataManager.RandomMeshPointList.Count);
        }
        else
        {
            _meshDataManager.GenerateMeshPoints();
            _meshDataManager.RandomMeshPointList = MeshPoints.RandomizeMeshPoints(_meshDataManager.RandomMeshPointList);

            _xmlFileManager.CreateNewXML("MyRoot");

            foreach (Vector3 point in _meshDataManager.RandomMeshPointList)
            {
                _xmlFileManager.CreateXMLElement<Vector3>("Save", point);
            }

            _xmlFileManager.AppendXMLElementsToRoot();
            _xmlFileManager.WriteXML();

            Debug.Log("New Mesh Points : " + _meshDataManager.RandomMeshPointList.Count);
        }

    }


    /// <summary>
    /// Sync with database abd Update new wishes data
    /// </summary>
    async void UpdateWishesData()
    {

        if (_databaseManager.WishesInfo == null)
            await _databaseManager.GetLatestWishesInfo();

        Debug.Log(_databaseManager.WishesInfo.Count);

        _wishesManager.LoadWishPrefabs();

        bool isWishesUpdated = _wishesManager.UpdateWishesList();

        Debug.Log("Wishes Update Status : " + isWishesUpdated);
    }


    //For Manual Updatable Spawn Objects
    void ManualUpdateWishData(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
              
            int randomId = UnityEngine.Random.Range(0, 3);

            GameObject spawnObj = (randomId == 0) ? _wishesManager.W_BringCupHome : ((randomId == 1) ? _wishesManager.W_SriLankaCan : _wishesManager.W_JayaApatai);

            _manualSpawnObjects.Add(spawnObj);
                   
        }

        bool isUpdateMeshes = _meshDataManager.MeshInstancer.UpdateMeshesInstant(_manualSpawnObjects, _meshDataManager.RandomMeshPointList);

        if(isUpdateMeshes)
           _wishesManager.UpdateInstanceMesh();


        if (!isUpdateMeshes)
        {
            Debug.Log("Time for manual Update");

            List<Vector3> randomStartPoints = new List<Vector3>();

            foreach (GameObject go in _manualSpawnObjects)
            {
                randomStartPoints.Add(_wishesManager.GetRandomScreenPoint());
            }

            List<MeshData<GameObject, Vector3, Vector3>> meshDataList = _meshDataManager.MeshInstancer.GetManualUpdatableMeshes(_manualSpawnObjects, randomStartPoints, _meshDataManager.RandomMeshPointList);

            Debug.Log("meshData List " + ((meshDataList == null) ? "is null" : ("count : " + meshDataList.Count)));


            StartCoroutine(_wishesManager.SpawnManualWishes(meshDataList));

            //Update Single Meshes into the Mesh list
            //If mesh limit increase after adding the current meshes, then update only the count
            //Otherwise added these meshes also into mesh list
            /*
            if (meshDataList != null)
            {
                foreach (MeshData<GameObject, Vector3, Vector3> md in meshDataList)
                {
                    _meshDataManager.MeshInstancer.UpdateMeshInstant(md, _meshDataManager.RandomMeshPointList);
                    
                }
            }
            */
            

        }
        //_wishesManager.UpdateInstanceMesh();
    }
    //End of For Manual Updatable Spawn Objects

}
