using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XMLFileManager : MonoBehaviour
{
    public string XMLFileName = "";

    public List<string> XMLRawDataList
    {
        get { return _xmlRawDataList; }
    }

    private XMLHandler _xmlHandler;
    private List<XmlElement> _xmlElements;

    private List<string> _xmlRawDataList;

    public bool CheckXMLFile(string fileName)
    {
        return File.Exists(fileName + ".xml");
    }

    #region XML Creation Methods

    public void CreateNewXML(string rootElementName)
    {
        _xmlHandler = new XMLHandler();
        _xmlHandler.CreateRootElement(rootElementName);
        _xmlElements = new List<XmlElement>();
    }

    public void CreateXMLElement<T>(string elementName, T value)
    {
        if (_xmlElements == null)
            _xmlElements = new List<XmlElement>();

        if (value is string)
        {
          XmlElement xmlElement = _xmlHandler.CreateXMLElement(elementName, (string)(object)value);
          _xmlElements.Add(xmlElement);
        }

        if (value is int)
        {
            XmlElement xmlElement = _xmlHandler.CreateXMLElement(elementName, (int)(object)value);
            _xmlElements.Add(xmlElement);
        }

        if (value is float)
        {
            XmlElement xmlElement = _xmlHandler.CreateXMLElement(elementName, (float)(object)value);
            _xmlElements.Add(xmlElement);
        }

        if (value is Vector3)
        {
            XmlElement xmlElement = _xmlHandler.CreateXMLElement(elementName, (Vector3)(object)value);
            _xmlElements.Add(xmlElement);
        }

        

    }

    public void AppendXMLElementsToRoot()
    {
        try
        {
            foreach (XmlElement xmlElement in _xmlElements)
            {
                _xmlHandler.AppendElementToRoot(xmlElement);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Faield Append To Root : " + e.Message);
        }
    }

    #endregion

    public void WriteXML()
    {
        if (string.IsNullOrEmpty(XMLFileName))
        {
            Debug.LogError("XML File Name is Null or Empty.");
            return;
        }

        _xmlHandler.WriteXMLDocument(XMLFileName);
    }

    public void WriteXML(string fileName)
    {
        _xmlHandler.WriteXMLDocument(fileName);
    }


    public void ReadXML()
    {
        if (string.IsNullOrEmpty(XMLFileName))
        {
            Debug.LogError("XML File Name is Null or Empty.");
            return;
        }

        if (_xmlHandler == null)
            _xmlHandler = new XMLHandler();


        _xmlHandler.OpenXMLDocument(XMLFileName);

    }

    public void ReadXML(string fileName)
    {
        if (_xmlHandler == null)
            _xmlHandler = new XMLHandler();


        _xmlHandler.OpenXMLDocument(fileName);
    }

    #region XML Load Methods

    public void LoadXMLRawData()
    {
        _xmlRawDataList = _xmlHandler.GetXMLRawData();
    }

    public T GetXMLValue<T>(string xmlStrValue)
    {

    
        if (typeof(T) == typeof(int))
        {
            int result = _xmlHandler.GetIntData(xmlStrValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        if(typeof(T) == typeof(float))
        {
            float result = _xmlHandler.GetFloatData(xmlStrValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        if (typeof(T) == typeof(Vector3))
        {
            Vector3 result = _xmlHandler.GetVector3Data(xmlStrValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }


        return (T)Convert.ChangeType(null, typeof(T));

    }

    #endregion

}
