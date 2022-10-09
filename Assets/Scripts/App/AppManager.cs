using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class AppManager : MonoBehaviour
{


    public bool IsManualUpdateWishes = false;

    public bool IsDBAutoUpdate = false;
    
    public float AutoDBUpdateInterval = 1f;

    public float CamDemoAnimationInterval = 1.5f;

    public PlayableDirector MainCamTimeline;

    private MeshDataManager _meshDataManager;
    private XMLFileManager _xmlFileManager;
    private DatabaseManager _databaseManager;
    private WishesManager _wishesManager;

   
    void Awake()
    {
        Initialize();

        
    }


    async void Update()
    {

        //If Wishes Data Update by Manually

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsManualUpdateWishes)
                UpdateManualWishesData(15);
            else
                await UpdateDBWishesData();
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

        }
        else
        {
            _wishesManager.WishPrefabs = new List<GameObject>();
        }


        if (IsDBAutoUpdate)
            Invoke("UpdateDBWithDelayAsync", AutoDBUpdateInterval);


        Invoke("PlayCameraDemoAnimation", CamDemoAnimationInterval);


    }

    async void  UpdateDBWithDelayAsync()
    {
       try
        {
            await UpdateDBWishesData();
        }
        catch(Exception e)
        {
            Debug.LogError("Error : " + e.Message);
        }
        finally
        {
            Invoke("UpdateDBWithDelayAsync", AutoDBUpdateInterval);
        }
       

        
    }


    void PlayCameraDemoAnimation()
    {
        MainCamTimeline.Play();

        Invoke("PlayCameraDemoAnimation", CamDemoAnimationInterval);
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

        _wishesManager.WishPrefabs = new List<GameObject>();

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

                if (meshDataList != null)
                    StartCoroutine(_wishesManager.SpawnWishesWithAnimation(meshDataList));
                else
                    Debug.Log("Mesh Data is Null");
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
