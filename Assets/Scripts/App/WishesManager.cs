using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishesManager : MonoBehaviour
{
    public GameObject W_SriLankaCan, W_BringCupHome, W_JayaApatai;

    public Material WishesSetMaterial;

    public Camera Camera2D;

    public List<GameObject> WishPrefabs { get; set; }

    private MeshDataManager _meshDataManager;
    private DatabaseManager _databaseManager;
    private GameObject _instaceWishMesh;


    void Awake()
    {
        _meshDataManager = GetComponent<MeshDataManager>();
        _databaseManager = GetComponent<DatabaseManager>();

        
    }


    public void UpdateInstanceMesh()
    {
        if (_instaceWishMesh == null)
            _instaceWishMesh = new GameObject("Wish Mesh Instance");

        if(WishesSetMaterial != null)
           _meshDataManager.MeshInstancer.InstanceMaterial = WishesSetMaterial;

        if(_meshDataManager.MeshInstancer != null)
           _meshDataManager.MeshInstancer.CreateInstanceMesh(_instaceWishMesh);
    }

    public void LoadWishPrefabs()
    {
        //WishPrefabs = new List<GameObject>();

        foreach(WishData wd in _databaseManager.WishesInfo)
        {
            switch(wd.wish)
            {
                case "sri_lanka_can":
                    WishPrefabs.Add(W_SriLankaCan);
                    break;

                case "bring_the_cup_home":
                    WishPrefabs.Add(W_BringCupHome);
                    break;

                case "jaya_apatai":
                    WishPrefabs.Add(W_JayaApatai);
                    break;
            }
        }

        Debug.Log("Wish Prefab Count : " + WishPrefabs.Count);
    }

    public bool UpdateWishesList()
    {
        return _meshDataManager.MeshInstancer.UpdateMeshesInstant(WishPrefabs, _meshDataManager.RandomMeshPointList);

    }



    public Vector3 GetRandomScreenPoint()
    {
        Vector3 Bounds = Camera2D.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float minX = -Bounds.x;
        float maxX = Bounds.x;
        float minY = -Bounds.y;
        float maxY = Bounds.y;

        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        return new Vector3(pos.x, pos.y, 4);

    }

    public IEnumerator SpawnWishesWithAnimation(List<MeshData<GameObject, Vector3, Vector3>> meshDataList)
    {
        foreach(MeshData<GameObject, Vector3, Vector3> md in meshDataList)
        {
            _meshDataManager.MeshInstancer.UpdateMeshInstant(md, _meshDataManager.RandomMeshPointList);
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(1);

        UpdateInstanceMesh();

        Debug.Log("Spawn Done!");
    }

}
