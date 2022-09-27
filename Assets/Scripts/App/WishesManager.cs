using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishesManager : MonoBehaviour
{
    public GameObject W_SriLankaCan, W_BringCupHome, W_JayaApatai;

    public List<GameObject> WishPrefabs { get { return _wishPrefabs; } }

    private MeshDataManager _meshDataManager;
    private DatabaseManager _databaseManager;

    private GameObject _instaceWishMesh;

    private List<GameObject> _wishPrefabs;

    void Awake()
    {
        _meshDataManager = GetComponent<MeshDataManager>();
        _databaseManager = GetComponent<DatabaseManager>();    
    }

    void Update()
    {
        UpdateInstanceMesh();
    }
    void UpdateInstanceMesh()
    {
        if (_instaceWishMesh == null)
            _instaceWishMesh = new GameObject("Wish Mesh Instance");

        if(_meshDataManager.MeshInstancer != null)
           _meshDataManager.MeshInstancer.CreateInstanceMesh(_instaceWishMesh);
    }

    public void LoadWishPrefabs()
    {
        _wishPrefabs = new List<GameObject>();

        foreach(WishData wd in _databaseManager.WishesInfo)
        {
            switch(wd.wish)
            {
                case "sri_lanka_can":
                    _wishPrefabs.Add(W_SriLankaCan);
                    break;

                case "bring_the_cup_home":
                    _wishPrefabs.Add(W_BringCupHome);
                    break;

                case "jaya_apatai":
                    _wishPrefabs.Add(W_JayaApatai);
                    break;
            }
        }

        Debug.Log("Wish Prefab Count : " + _wishPrefabs.Count);
    }

    public bool UpdateWishesList()
    {
        return _meshDataManager.MeshInstancer.UpdateMeshesInstant(_wishPrefabs, _meshDataManager.RandomMeshPointList);

    }

}
