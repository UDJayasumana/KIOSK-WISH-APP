using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

public class XMLHandler
{
    private XmlDocument _xmlDocument;
    private XmlElement _xmlRootElement;


    #region Class Constructors

    /// <summary>
    /// Default constructor of XMLHandler class.<br/>
    /// Create a new XML Document.
    /// </summary>
    public XMLHandler()
    {
        this._xmlDocument = new XmlDocument();
    }


    /// <summary>
    /// Create a new XML Document with the root element.
    /// </summary>
    public XMLHandler(string rootElementName)
    {
        this._xmlDocument = new XmlDocument();
        CreateRootElement(rootElementName);
    }

    #endregion

    #region XML Document Creation Methods

    /// <summary>
    /// Create a root element for the XML Document
    /// </summary>
    public void CreateRootElement(string rootElementName)
    {
        this._xmlRootElement = this._xmlDocument.CreateElement(rootElementName);
        appendRootToXMLDocument();
    }

    #region Create XML Elements

    /// <summary>
    /// Create XML element with a string value
    /// </summary>
    public XmlElement CreateXMLElement(string elementName, string value)
    {
        XmlElement attr = _xmlDocument.CreateElement(elementName);
        attr.InnerText = value;

        return attr;
    }

    /// <summary>
    /// Create XML element with an integer value
    /// </summary>
    public XmlElement CreateXMLElement(string elementName, int value)
    {
        string elementValue = string.Format("{0}", value);

        XmlElement attr = _xmlDocument.CreateElement(elementName);
        attr.InnerText = elementValue;

        return attr;
    }

    /// <summary>
    /// Create XML element with a floating point value
    /// </summary>
    public XmlElement CreateXMLElement(string elementName, float value)
    {
        string elementValue = string.Format("{0}", value);

        XmlElement attr = _xmlDocument.CreateElement(elementName);
        attr.InnerText = elementValue;

        return attr;
    }

    /// <summary>
    /// Create XML element with a vector3 value
    /// </summary>
    public XmlElement CreateXMLElement(string elementName, Vector3 value)
    {
        string elementValue = string.Format("{0} {1} {2}", value.x, value.y, value.z);

        XmlElement attr = _xmlDocument.CreateElement(elementName);
        attr.InnerText = elementValue;

        return attr;
    }

    #endregion

    /// <summary>
    /// Append element as a child element of the root element.
    /// </summary>
    public void AppendElementToRoot(XmlElement element)
    {
        try
        {
            this._xmlRootElement.AppendChild(element);
        }
        catch (Exception e)
        {
            Debug.LogError("Append to root Faield : " + e.Message);
        }

    }


    /// <summary>
    /// Append XML element to the XML Document as a root element
    /// </summary>
    private void appendRootToXMLDocument()
    {
        try
        {
            this._xmlDocument.AppendChild(this._xmlRootElement);
        }
        catch (Exception e)
        {
            Debug.LogError("Append to XML Document Faield : " + e.Message);
        }

    }

    /// <summary>
    /// Write XML data into a xml file.
    /// </summary>
    public void WriteXMLDocument(string xmlDocName)
    {

        try
        {
            this._xmlDocument.Save(xmlDocName + ".xml");
            Debug.Log("XML File Saved Succesfully");
        }
        catch (Exception e)
        {
            Debug.LogError("XML Write Faield : " + e.Message);
        }

    }

    #endregion

    #region XML Document Load Methods


    /// <summary>
    /// Create an existing XML Document.
    /// </summary>
    public void OpenXMLDocument(string xmlDocName)
    {
        try
        {
            this._xmlDocument.Load(xmlDocName + ".xml");
            loadRootElement();
            Debug.Log("XML File Opened Succesfully");
        }
        catch (Exception e)
        {

            Debug.LogError("XML Document Open Faield : " + e.Message);

        }

    }


    /// <summary>
    /// Get the root element from the XML document
    /// </summary>
    private void loadRootElement()
    {
        this._xmlRootElement = this._xmlDocument.DocumentElement;
    }


    /// <summary>
    /// Get the Raw string data from all the child nodes
    /// </summary>
    public List<string> GetXMLRawData()
    {
        List<string> result = new List<string>();

        try
        {
            for (int i = 0; i < this._xmlRootElement.ChildNodes.Count; i++)
            {
                XmlNode attr = this._xmlRootElement.ChildNodes[i].FirstChild;
                result.Add(attr.Value);
            }

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("XML Data Receiving Error : " + e.Message);

            return null;
        }


    }

    #region Get Data from XML string


    /// <summary>
    /// Convert Raw string int type data back into a Integer type 
    /// </summary>
    public int GetIntData(string resultText)
    {
        return int.Parse(resultText);
    }

    /// <summary>
    /// Convert Raw string float type data back into a Floating type 
    /// </summary>
    public float GetFloatData(string resultText)
    {
        return float.Parse(resultText);
    }

    /// <summary>
    /// Convert Raw string Vector3 type data back into a Vector3 type 
    /// </summary>
    public Vector3 GetVector3Data(string resultText)
    {
        char[] whitespace = new char[] { ' ', '\t' };
        string[] ssizes = resultText.Split(whitespace);

        Vector3 point = Vector3.zero;

        for (int i = 0; i < ssizes.Length; i++)
        {
            switch (i)
            {
                case 0:
                    point.x = float.Parse(ssizes[i]);
                    break;

                case 1:
                    point.y = float.Parse(ssizes[i]);
                    break;

                case 2:
                    point.z = float.Parse(ssizes[i]);
                    break;
            }
        }

        return point;
    }

    #endregion

    #endregion























}
