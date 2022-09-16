using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
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

    /// <summary>
    /// Create a root element for the XML Document
    /// </summary>
    public void CreateRootElement(string rootElementName)
    {
        this._xmlRootElement = this._xmlDocument.CreateElement(rootElementName);
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
        catch(Exception e)
        {
     
            if(e.GetType() == typeof(NullReferenceException))
            {
                Debug.LogError("Append to root Faield : " + "Didn't find any root element.");
            }
            else
            {
                Debug.LogError("Append to root Faield : " + e.Message);
            }
            
    
        }

    }


    /// <summary>
    /// Write XML data into a xml file.
    /// </summary>
    public void WriteXMLDocument(string xmlDocName)
    {
        try
        {
            this._xmlDocument.AppendChild(this._xmlRootElement);
            this._xmlDocument.Save(xmlDocName + ".xml");
            Debug.Log("XML File Saved Succesfully");
        }
        catch(Exception e)
        {
            if (e.GetType() == typeof(NullReferenceException))
            {
                Debug.LogError("XML Write Faield : " + "Didn't find any root element.");
            }
            else
            {
                Debug.LogError("XML Write Faield : " + e.Message);
            }
        }
    }


}
