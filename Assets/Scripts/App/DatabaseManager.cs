using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    private const string WishCountURL = "http://localhost/cwc-api/fullcount.php";
    private const string WishDataURL = "http://localhost/cwc-api/";

    public Text UIText;

    async Task<string> AsyncGetRequest(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            await Task.Yield();
        }

        string[] pages = url.Split('/');
        int page = pages.Length - 1;

        string result = "";

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
                result = webRequest.downloadHandler.text;
                break;

        }

        return result;

    }

    async Task<string> AsyncPutRequest(string url, byte[] rawData)
    {
        UnityWebRequest webRequest = UnityWebRequest.Put(url, rawData);
        webRequest.SendWebRequest();

        while(!webRequest.isDone)
        {
            await Task.Yield();
        }

        string[] pages = url.Split('/');
        int page = pages.Length - 1;

        string result = "";

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
                result = webRequest.downloadHandler.text;
                break;

        }

        return result;
    }


    async void Awake()
    {

        string latestWishCount = await RequestLatestWishCount();     
       
        WishCount wishCount = JsonUtility.FromJson<WishCount>(latestWishCount);

        Debug.Log("Latest Wish Count : " + wishCount.full_count);

         string wishes = await RequestWishes(1, 100);
      
         WishesContainer wishesContainer = JsonUtility.FromJson<WishesContainer>(wishes);

         Debug.Log("Status : " + wishesContainer.status);
         Debug.Log("Desc : " + wishesContainer.desc);
         Debug.Log("row_start : " + wishesContainer.row_start);
         Debug.Log("row_end : " + wishesContainer.row_end);
         Debug.Log("row_count : " + wishesContainer.row_count);
         Debug.Log("Data Count : " + wishesContainer.data.Count);
        
   

       

    }
   
    
    public async Task<string> RequestLatestWishCount()
    {

        return await AsyncGetRequest(WishCountURL);

    }
    

    public async Task<string> RequestWishes(int wishStartNum, int wishEndNum)
    {


        WWWForm form = new WWWForm();
        form.AddField("row_start", wishStartNum);
        form.AddField("row_end", wishEndNum);

        WishDataRequest wishDataRequest = new WishDataRequest { row_start = wishStartNum, row_end = wishEndNum };
        string postData = JsonUtility.ToJson(wishDataRequest);
        byte[] bytes = Encoding.UTF8.GetBytes(postData);

        string data = await AsyncPutRequest(WishDataURL, bytes);

        return data;

       
    }

   
}


public class Save
{
    public string PlayerName;
    public int Score;
    public List<string> NickNames;
}

