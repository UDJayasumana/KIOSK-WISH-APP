using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestJSONReadWrite : MonoBehaviour
{
    
    void Start()
    {
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

           


        }
    }

    
}
