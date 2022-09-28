using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDataManager : MonoBehaviour, IMeshPointGenerate
{
    #region Members of IMeshPointGenerate

    [field: SerializeField]
    public GameObject[] MeshObjects { get; set; }

    [field: Range(1, 100), SerializeField]
    public int PointsPerTriangle { get; set; }

    [field: Range(-2, 2), SerializeField]
    public float PointMinOffset { get; set; }

    [field: Range(-2, 2), SerializeField]
    public float PointMaxOffset { get; set; }

    [field: SerializeField]
    public MeshPoints.MeshOffsetDirection MeshesOffsetDirection { get; set; }

    public MeshPoints[] MeshPointSet { get; set; }

    public List<Vector3> RandomMeshPointList { get; set; }

    #endregion

    public MeshInstance MeshInstancer;
    

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        MeshInstancer = new MeshInstance();
    }

    #region Methods of IMeshPointGenerate

    public void GenerateMeshPoints()
    {
        if(MeshObjects == null || MeshObjects.Length == 0)
        {
            Debug.LogError("Not Found any Mesh Objects in the MeshObjects Array.");
            return;
        }

        try
        {

            MeshPointSet = new MeshPoints[MeshObjects.Length];

            RandomMeshPointList = new List<Vector3>();

            for (int i = 0; i < MeshObjects.Length; i++)
            {
                MeshPointSet[i] = new MeshPoints(MeshObjects[i]);
                MeshPointSet[i].GenerateRandomPointsOnMesh(PointsPerTriangle, PointMinOffset, PointMaxOffset, MeshesOffsetDirection);
            
            }

            if (RandomMeshPointList == null)
                RandomMeshPointList = new List<Vector3>();

            foreach (MeshPoints mp in MeshPointSet)
            {
                foreach (Vector3 point in mp.MeshPointList)
                {
                    RandomMeshPointList.Add(point);
                }
            }

        }
        catch(Exception e)
        {
            Debug.LogError("Generate Random Mesh Points Faield : " + e.Message);
        }

    }

    #endregion
}
