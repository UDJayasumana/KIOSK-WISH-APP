﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Test : MonoBehaviour
{

    private XMLHandler _xmlHandler;
   
    void Start()
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
        foreach(Vector3 d in myDataList)
        {
            Debug.Log(d);
        }
        
        
   

    }

   
}
