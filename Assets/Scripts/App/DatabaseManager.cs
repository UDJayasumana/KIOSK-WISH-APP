using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour, IGetRequest
{
    private const string WishCountURL = "http://localhost/cwc-api/fullcount.php";

    public Text UIText;

    public IEnumerator GetRequest(string url, Action<string> callback = null)
    {

        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    callback.Invoke(webRequest.downloadHandler.text);
                    break;

            }
        }

    }

    void Awake()
    {

        //RequestLatestWishCount();

        if (!File.Exists("MyJSON.txt"))
        {
            Save save = new Save();

            save.PlayerName = "Sameera Rathnayaka";
            save.Score = 1555;
            save.NickNames = new List<string>();

            save.NickNames.Add("Maria");
            save.NickNames.Add("Julia");
            save.NickNames.Add("Salman");
            save.NickNames.Add("Akshaya");

            string JsonString = JsonUtility.ToJson(save);

            StreamWriter sw = new StreamWriter("MyJSON.txt");
            sw.Write(JsonString);
            sw.Close();
        }

        if (File.Exists("MyJSON.txt"))
        {
            StreamReader sr = new StreamReader("MyJSON.txt");

            string JsonString = sr.ReadToEnd();

            sr.Close();

            Save save = JsonUtility.FromJson<Save>(JsonString);

            string myResult = save.PlayerName + "\n";
            myResult += save.Score + "\n";

            foreach (string str in save.NickNames)
            {
                myResult += str + "\n";
            }

            UIText.text = myResult;


            /*
            Debug.Log(save.PlayerName);
            Debug.Log(save.Score);

            foreach(string str in save.NickNames)
            {
                Debug.Log(str);
            }
            */
        }

       

    }
   
    public void RequestLatestWishCount()
    {
        StartCoroutine(GetRequest(WishCountURL, returnValue =>
        {

            Debug.Log(returnValue);


        }));
    }

    
}


public class Save
{
    public string PlayerName;
    public int Score;
    public List<string> NickNames;
}

