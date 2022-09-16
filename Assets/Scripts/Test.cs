using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Test : MonoBehaviour
{

    private XMLHandler _xmlHandler;
   
    void Start()
    {
        _xmlHandler = new XMLHandler();

        XmlElement myElement = _xmlHandler.CreateXMLElement("Save", 10);

        _xmlHandler.CreateRootElement("MyRoot");
        _xmlHandler.AppendElementToRoot(myElement);
        _xmlHandler.WriteXMLDocument("Hello");
    }

   
}
