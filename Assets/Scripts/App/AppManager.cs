using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    public Text DebugTX;

    public bool IsManualUpdateWishes = false;

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

        //If Wishes Data Update by Manually
        if (IsManualUpdateWishes)
        {

            if (Input.GetKeyDown(KeyCode.P))
            {
                UpdateManualWishesData(15);
            }

        }



    }


    /// <summary>
    /// Initialize App data
    /// </summary>
    async void Initialize()
    {
        
       _meshDataManager = GetComponent<MeshDataManager>();
       _xmlFileManager = GetComponent<XMLFileManager>();
       _databaseManager = GetComponent<DatabaseManager>();
       _wishesManager = GetComponent<WishesManager>();


        CreateOrLoadRandomMeshPoints();

        if (!IsManualUpdateWishes)
        {

            _databaseManager.InitializeURLs();

             await UpdateDBWishesData();

            _wishesManager.UpdateInstanceMesh();

        }
        else
        {
            _wishesManager.WishPrefabs = new List<GameObject>();
        }




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
    async Task UpdateDBWishesData()
    {

        if (_databaseManager.WishesInfo == null)
            await _databaseManager.GetLatestWishesInfo();

        Debug.Log(_databaseManager.WishesInfo.Count);

        if (_databaseManager.WishesInfo.Count != 0)
        {

            _wishesManager.LoadWishPrefabs();

            bool isWishesUpdated = _wishesManager.UpdateWishesList();

            if (isWishesUpdated)
                _wishesManager.UpdateInstanceMesh();

            if (!isWishesUpdated)
            {

                List<Vector3> randomStartPoints = new List<Vector3>();

                foreach (GameObject go in _wishesManager.WishPrefabs)
                {
                    randomStartPoints.Add(_wishesManager.GetRandomScreenPoint());
                }

                List<MeshData<GameObject, Vector3, Vector3>> meshDataList = _meshDataManager.MeshInstancer.GetManualUpdatableMeshes(_wishesManager.WishPrefabs, randomStartPoints, _meshDataManager.RandomMeshPointList);

                StartCoroutine(_wishesManager.SpawnWishesWithAnimation(meshDataList));
            }

        }

    }


    
    /// <summary>
    /// Update Wishes Data by Manually
    /// </summary>
    void UpdateManualWishesData(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
              
            int randomId = UnityEngine.Random.Range(0, 3);

            GameObject wishPrefab = (randomId == 0) ? _wishesManager.W_BringCupHome : ((randomId == 1) ? _wishesManager.W_SriLankaCan : _wishesManager.W_JayaApatai);

            _wishesManager.WishPrefabs.Add(wishPrefab);
                   
        }

        bool isUpdateMeshes = _meshDataManager.MeshInstancer.UpdateMeshesInstant(_wishesManager.WishPrefabs, _meshDataManager.RandomMeshPointList);

        if(isUpdateMeshes)
           _wishesManager.UpdateInstanceMesh();


        if (!isUpdateMeshes)
        {

            List<Vector3> randomStartPoints = new List<Vector3>();

            foreach (GameObject go in _wishesManager.WishPrefabs)
            {
                randomStartPoints.Add(_wishesManager.GetRandomScreenPoint());
            }

            List<MeshData<GameObject, Vector3, Vector3>> meshDataList = _meshDataManager.MeshInstancer.GetManualUpdatableMeshes(_wishesManager.WishPrefabs, randomStartPoints, _meshDataManager.RandomMeshPointList);


            StartCoroutine(_wishesManager.SpawnWishesWithAnimation(meshDataList));
            

        }
     
    }
   

}
