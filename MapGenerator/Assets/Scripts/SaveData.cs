//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//[System.Serializable]
//public class SaveData
//{
//    [SerializeField]
//    int width;
//    [SerializeField]
//    int height;

//    [SerializeField]
//    private Tile[,] tiles;

//    public void SaveMap()
//    {
//        var watch = System.Diagnostics.Stopwatch.StartNew();

//        string path = "SaveData.json";

//        width = Map.width;

//        height = Map.height;

//        tiles = Map.tiles;

//        string json = JsonUtility.ToJson(this);
//        File.WriteAllText(path, json + System.Environment.NewLine);

//        foreach (Tile t in Map.tiles)
//        {
//            json = t.SerializeTile();
//            File.AppendAllText(path, json + System.Environment.NewLine);
//        }

//        watch.Stop();
//        Debug.Log("Time to serialize is:" + watch.Elapsed + "s");
//    }

//    public void LoadMap()
//    {
//        string path = EditorUtility.OpenFilePanel("Select save file", "", "json");
//        if (path.Length != 0)
//        {
//            SaveData sd = JsonUtility.FromJson<SaveData>(path);
//            Debug.Log("width:" + sd.width);
//            Debug.Log("height:" + sd.height);


//        }
//    }
//}
