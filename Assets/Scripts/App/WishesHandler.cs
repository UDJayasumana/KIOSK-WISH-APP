using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishesHandler : MonoBehaviour, IMeshPointGenerate
{

    [field: SerializeField]
    public GameObject[] MeshObjects { get; set; }

    [field: Range(1, 100), SerializeField]
    public int PointsPerTriangle { get; set; }
    public float PointMinOffset { get; set; }
    public float PointMaxOffset { get; set; }
    public MeshPoints.MeshOffsetDirection MeshesOffsetDirection { get; set; }
    public MeshPoints[] MeshPointSet { get; set; }
    public List<Vector3> RandomMeshPointList
    {
        get
        {
            List<Vector3> result = new List<Vector3>();

            if (MeshPointSet == null)
                result = null;
            else
            {
                foreach (MeshPoints mp in MeshPointSet)
                {
                    foreach (Vector3 point in mp.MeshPointList)
                    {
                        result.Add(point);
                    }
                }
            }

            return result;
        }
    }

    void Awake()
    {
        Initialize();
    }


    void Initialize()
    {
        if(MeshObjects != null)
        {
            MeshPointSet = new MeshPoints[MeshObjects.Length];
        }

       GenerateMeshPoints();

        Debug.Log("Mesh Points : " + RandomMeshPointList.Count);
    }


    public void GenerateMeshPoints()
    {
        for(int i = 0; i < MeshObjects.Length; i++)
        {
            MeshPointSet[i] = new MeshPoints(MeshObjects[i]);
            MeshPointSet[i].GenerateRandomPointsOnMesh(PointsPerTriangle, PointMinOffset, PointMaxOffset, MeshesOffsetDirection);
        }
    }


}
