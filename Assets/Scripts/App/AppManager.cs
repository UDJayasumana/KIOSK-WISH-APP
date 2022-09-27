using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public GameObject SpawnObjectA, SpawnObjectB;
    private List<GameObject> _spawnObjects;

    private MeshDataManager _meshDataManager;
    private XMLFileManager _xmlFileManager;
    private DatabaseManager _databaseManager;
    private WishesManager _wishesManager;

    private GameObject _instaceWishMesh;
   
    void Awake()
    {
        Initialize();  
    }


    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.P))
        {
            SpawnObjects(15);
        }

        if(_instaceWishMesh == null)
           _instaceWishMesh = new GameObject("Wish Mesh Instance");

        _meshDataManager.MeshInstancer.CreateInstanceMesh(_instaceWishMesh);
        */
          
    }

    void Initialize()
    {
        
       _meshDataManager = GetComponent<MeshDataManager>();
       _xmlFileManager = GetComponent<XMLFileManager>();
       _databaseManager = GetComponent<DatabaseManager>();
       _wishesManager = GetComponent<WishesManager>();

        
       CreateOrLoadRandomMeshPoints();

       UpdateWishesData();


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


    void SpawnObjects(int spawnCount)
    {
        for(int i = 0; i < spawnCount; i++)
        {
            if (i % 2 == 0)
                _spawnObjects.Add(SpawnObjectA);
            else
                _spawnObjects.Add(SpawnObjectB);
        }

        bool isUpdateMeshes = _meshDataManager.MeshInstancer.UpdateMeshesInstant(_spawnObjects, _meshDataManager.RandomMeshPointList);

        Debug.Log("Mesh Update Result : " + isUpdateMeshes);

        if (!isUpdateMeshes)
        {
            List<Vector3> randomStartPoints = new List<Vector3>();

            foreach (GameObject go in _spawnObjects)
            {
                float rx = Random.Range(-200f, 200f);
                float ry = Random.Range(-200f, 200f);
                float rz = Random.Range(-200f, 200f);
                randomStartPoints.Add(new Vector3(rx, ry, rz));
            }
            List<MeshData<GameObject, Vector3, Vector3>> meshDataList = _meshDataManager.MeshInstancer.GetManualUpdatableMeshes(_spawnObjects, randomStartPoints, _meshDataManager.RandomMeshPointList);

            Debug.Log("meshData List " + ((meshDataList == null) ? "is null" : ("count : " + meshDataList.Count)));

            //Update Single Meshes into the Mesh list
            //If mesh limit increase after adding the current meshes, then update only the count
            //Otherwise added these meshes also into mesh list
            if (meshDataList != null)
            {
                foreach (MeshData<GameObject, Vector3, Vector3> md in meshDataList)
                {
                    _meshDataManager.MeshInstancer.UpdateMeshInstant(md, _meshDataManager.RandomMeshPointList);
                }
            }

        }

       
    }


}
