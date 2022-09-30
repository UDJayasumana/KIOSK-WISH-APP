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
    public enum APISource { Server, LocalHost }

    public APISource CurrentHost = APISource.LocalHost;

    private const string ServerBaseURL = "https://srilankacan.online/cwc-api/";
    private const string LocalHostBaseURL = "http://localhost/cwc-api/";

    private string MainBaseURL = "";

    private const string WishCountURL = "fullcount.php";

    public int LatestWishCount { get { return _latestWishCount; } }
    public List<WishData> WishesInfo { get { return _wishesInfo; } }

    private int _latestWishCount;
    private List<WishData> _wishesInfo;

   

    public void InitializeURLs()
    {
        MainBaseURL = (CurrentHost == APISource.Server) ? ServerBaseURL : LocalHostBaseURL;
    }


    private async Task<string> AsyncGetRequest(string url)
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
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                result = webRequest.downloadHandler.text;
                break;

        }

        return result;

    }

    private async Task<string> AsyncPutRequest(string url, byte[] rawData)
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
   
    private async Task RequestLatestWishCount()
    {

        string latestWishCount =  await AsyncGetRequest(MainBaseURL + WishCountURL);
        WishCount wishCount = JsonUtility.FromJson<WishCount>(latestWishCount);
        _latestWishCount = wishCount.full_count;

    }
    
    private async Task<string> RequestWishes(int wishStartNum, int wishEndNum)
    {


        WWWForm form = new WWWForm();
        form.AddField("row_start", wishStartNum);
        form.AddField("row_end", wishEndNum);

        WishDataRequest wishDataRequest = new WishDataRequest { row_start = wishStartNum, row_end = wishEndNum };
        string postData = JsonUtility.ToJson(wishDataRequest);
        byte[] bytes = Encoding.UTF8.GetBytes(postData);

        string data = await AsyncPutRequest(MainBaseURL, bytes);

        return data;

       
    }


    public async Task GetLatestWishesInfo()
    {
        int requestCount = 0;
        
        await RequestLatestWishCount();

        Debug.Log("Latest Wish Count : " + _latestWishCount);


        requestCount = (_latestWishCount < 1000) ? 1 : ((_latestWishCount % 1000 == 0) ? _latestWishCount / 1000 : (_latestWishCount / 1000) + 1);

        Debug.Log("Request Count : " + requestCount);

        _wishesInfo = new List<WishData>();

     
        
        for(int i = 0; i < requestCount; i++)
        {

             string wishes = "";

             wishes = await RequestWishes(_wishesInfo.Count + 1, _wishesInfo.Count + 1000);

             WishesContainer wishesContainer = JsonUtility.FromJson<WishesContainer>(wishes);

 
            foreach(WishData wd in wishesContainer.data)
            {
                _wishesInfo.Add(wd);
            }
            
        }


       // Debug.Log("Wish Infos Length : " + _wishesInfo.Count);


    }

   
}





