using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public GameObject MeshObj1;
    public GameObject MeshObj2;
    public GameObject SpawnObjA;
    public GameObject SpawnObjB;

    private MeshPoints meshPoints1;
    private MeshPoints meshPoints2;
    private List<Vector3> randomPoints;

    private MeshInstance meshInstance;
    List<GameObject> spawnObjects;

    void Start()
    {
        meshPoints1 = new MeshPoints(MeshObj1);
        meshPoints1.GenerateRandomPointsOnMesh(10, -0.04f, 0.04f, MeshPoints.MeshOffsetDirection.X);

        meshPoints2 = new MeshPoints(MeshObj2);
        meshPoints2.GenerateRandomPointsOnMesh(10, -0.04f, 0.04f, MeshPoints.MeshOffsetDirection.X);

        randomPoints = meshPoints1 + meshPoints2;
        randomPoints = MeshPoints.RandomizeMeshPoints(randomPoints);

        meshInstance = new MeshInstance();
        spawnObjects = new List<GameObject>();

        Debug.Log("Random Points : " + randomPoints.Count);

        /*
        for(int j = 0; j < spawnObjects.Count; j++)
        {
            GameObject go = Instantiate(spawnObjects[j]);
            go.transform.position = randomPoints[j];
        }
        */

        //MeshInstance mesh = new MeshInstance();
        //mesh.UpdateMeshesInstant(spawnObjects, randomPoints);


        //mesh.UpdateMeshes(spawnObjects, randomPoints);

        /*
        GameObject go = new GameObject("Hello");

        List<GameObject> gameObjList = new List<GameObject>();

        gameObjList.Add(go);
        gameObjList.Add(go);
        gameObjList.Add(go);

        Debug.Log("Actual Obj Name : " + go.name);

        foreach(GameObject obj in gameObjList)
        {
            Debug.Log("In List : " + obj.name);
        }
        */
    }


    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            
            for (int i = 0; i < 800; i++)
            {
                if (i % 2 == 0)
                    spawnObjects.Add(SpawnObjA);
                else
                    spawnObjects.Add(SpawnObjB);
            }

            
            bool decision = meshInstance.UpdateMeshesInstant(spawnObjects, randomPoints);

            
            List<Vector3> randomStartPoints = new List<Vector3>();

            foreach(GameObject go in spawnObjects)
            {
                float rx = Random.Range(-200f, 200f);
                float ry = Random.Range(-200f, 200f);
                float rz = Random.Range(-200f, 200f);
                randomStartPoints.Add(new Vector3(rx, ry, rz));
            }

            //Debug.Log("randomStartPoints count : " + randomStartPoints.Count);
            List<MeshData<GameObject, Vector3, Vector3>> meshDataList = meshInstance.GetManualUpdatableMeshes(spawnObjects, randomStartPoints, randomPoints);

            Debug.Log("meshData List " + ((meshDataList == null) ? "is null" : ("count : " + meshDataList.Count)));

            if (meshDataList != null)
            {
                foreach (MeshData<GameObject, Vector3, Vector3> md in meshDataList)
                {
                    meshInstance.UpdateMeshInstant(md, randomPoints);
                }
            }

            //List<MeshData<GameObject, Vector3, Vector3>> meshData =  meshInstance.GetManualUpdatableMeshes(spawnObjects)
            // Debug.Log(decision);

        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < 15; i++)
            {
                if (i % 2 == 0)
                    spawnObjects.Add(SpawnObjA);
                else
                    spawnObjects.Add(SpawnObjB);
            }


            bool decision = meshInstance.UpdateMeshesInstant(spawnObjects, randomPoints);
            // Debug.Log(decision);
            
            List<Vector3> randomStartPoints = new List<Vector3>();

            foreach (GameObject go in spawnObjects)
            {
                float rx = Random.Range(-200f, 200f);
                float ry = Random.Range(-200f, 200f);
                float rz = Random.Range(-200f, 200f);
                randomStartPoints.Add(new Vector3(rx, ry, rz));
            }

            //Debug.Log("randomStartPoints count : " + randomStartPoints.Count);
            List<MeshData<GameObject, Vector3, Vector3>> meshDataList = meshInstance.GetManualUpdatableMeshes(spawnObjects, randomStartPoints, randomPoints);
            Debug.Log("meshData List " + ((meshDataList == null) ? "is null" : ("count : " + meshDataList.Count)));

            if (meshDataList != null)
            {
                foreach (MeshData<GameObject, Vector3, Vector3> md in meshDataList)
                {
                    meshInstance.UpdateMeshInstant(md, randomPoints);
                }
            }
        }

        meshInstance.CreateInstanceMesh(gameObject);
    }

}
