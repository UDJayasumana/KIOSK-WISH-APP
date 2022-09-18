using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Test : MonoBehaviour
{

    public GameObject MeshObjA;
    public GameObject MeshObjB;
    public GameObject CubePF;

    private XMLHandler _xmlHandler;


    private List<Vector3> allRandomPoints;
    private GameObject[] cubes;

    void Start()
    {
        
        //XMLTest();

        GenerateMeshPoints();

    }

    void GenerateMeshPoints()
    {
        /*
        MeshPoints meshPoints = new MeshPoints(MeshObjB);
        List<Vector3> randomPoints = meshPoints.GenerateRandomPointsOnMesh(10, -0.04f, 0.04f, MeshPoints.MeshOffsetDirection.X);

        foreach (Vector3 point in randomPoints)
        {
            GameObject cube = Instantiate(CubePF);
            cube.transform.position = point;
        }
        */

        MeshPoints meshPointsA = new MeshPoints(MeshObjA);
        List<Vector3> randomPointsA = meshPointsA.GenerateRandomPointsOnMesh(10, -0.04f, 0.04f, MeshPoints.MeshOffsetDirection.X);

        MeshPoints meshPointsB = new MeshPoints(MeshObjB);
        List<Vector3> randomPointsB = meshPointsB.GenerateRandomPointsOnMesh(10, -0.04f, 0.04f, MeshPoints.MeshOffsetDirection.X);

        allRandomPoints = meshPointsA + meshPointsB;

        /*
        foreach (Vector3 point in allRandomPoints)
        {
            GameObject cube = Instantiate(CubePF);
            cube.transform.position = point;
        }
        */
        cubes = new GameObject[allRandomPoints.Count];

        for (int i = 0; i < allRandomPoints.Count; i++)
        {
            // GameObject cube = Instantiate(CubePF);
            //cube.transform.position = allRandomPoints[i];
            cubes[i] = Instantiate(CubePF);
            cubes[i].transform.position = allRandomPoints[i];

            if (i == 0)
            {
                cubes[i].GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }

       
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            allRandomPoints = MeshPoints.RandomizeMeshPoints(allRandomPoints);

            for (int i = 0; i < allRandomPoints.Count; i++)
            {
                cubes[i].transform.position = allRandomPoints[i];
            }
        }
    }

    void XMLTest()
    {
        //Creating a new XML file and Writing
        /*
        _xmlHandler = new XMLHandler();
        
        //Create XML child elements
        XmlElement myElement = _xmlHandler.CreateXMLElement("Save", new Vector3(5.26f, 5.68f, 8.12f));
        XmlElement myElement2 = _xmlHandler.CreateXMLElement("Save", new Vector3(7.26f, 1.68f, 0.12f));

        //Create and append XML root element
        _xmlHandler.CreateRootElement("MyRoot");

        //Append xml elements to the root element
        _xmlHandler.AppendElementToRoot(myElement);
        _xmlHandler.AppendElementToRoot(myElement2);

        //write xml data to a xml file
        _xmlHandler.WriteXMLDocument("Hello");
        */


        //Open Existing XML file and Reading

        _xmlHandler = new XMLHandler();

        //Open an existing XML file
        _xmlHandler.OpenXMLDocument("Hello");

        //Get data of all the child elements in the xml file
        List<string> dataList = _xmlHandler.GetXMLRawData();

        //Create List in Vector3 type
        List<Vector3> myDataList = new List<Vector3>();

        //If XML Raw string data is in vector3, then desirealize those data back into a Vector3 format
        // using "GetVector3Data" method in the XMLHandler class
        foreach (string res in dataList)
        {
            //Deserialize Vector3 string data back to the Vector3 data
            myDataList.Add(_xmlHandler.GetVector3Data(res));
        }

        //Read those deserialized Vector3 data
        foreach (Vector3 d in myDataList)
        {
            Debug.Log(d);
        }

    }


}
