using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public static class GlobalItemManager {
    static readonly Dictionary<int, Item> items = new();
    static readonly string jsonFilepath = "/Scripts/Items/items.json";
    static readonly string imageFilepath = "Sprites/Items/";
    static bool initialized = false;
    public static Sprite item_none;

    public static void Init(){
        if (!initialized){
            Import();
            initialized = true;
        }
    }

    private static void Import() {
        item_none = Resources.Load<Sprite>(imageFilepath + "none");
        ItemJsonObject temp = new();
        StreamReader reader = new StreamReader(Application.dataPath + jsonFilepath);
        JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), temp);
        reader.Close();
        foreach (JsonMat obj in temp.mats_list){
            Debug.Log(imageFilepath + obj.image);
            Item item = new() {
                id = obj.id,
                name = obj.name,
                maxStack = obj.max_stack,
                sprite = Resources.Load<Sprite>(imageFilepath + obj.image)
            };
            items.Add(obj.id, item);
        } 
        foreach (JsonItem obj in temp.item_list){
            Item item = new() {
                id = obj.id,
                name = obj.name,
                maxStack = obj.max_stack,
                sprite = Resources.Load<Sprite>(imageFilepath + obj.image)
            };
            items.Add(obj.id, item);
        } 
    }
    private static void Write(){
        /*
        StreamWriter writer = new(Application.dataPath + jsonFilepath, false);

        ItemJsonObject temp = new();
        temp.item_list.Add(new JsonItem());
        string a = JsonUtility.ToJson(temp, true);
        
        writer.WriteLine(a);
        writer.Close();
        */
    }

    public static Item Clone(int id){
        //Debug.Log(items.GetValueOrDefault<int,Item>(id, null));
        return items.GetValueOrDefault<int,Item>(id, null);
    }
}

[System.Serializable]
public class ItemJsonObject{
    public List<JsonMat> mats_list = new();
    public List<JsonItem> item_list = new();
    public List<JsonItem> tool_list = new();
}

[System.Serializable]
public class JsonMat {
    public int id;
    public string name;
    public string desc;
    public int max_stack;
    public string image;
}

[System.Serializable]
public class JsonItem {
    public int id;
    public string name;
    public string desc;
    public int max_stack;
    public string image;
    public bool consumed;
    public JsonItemScript script;
}

[System.Serializable]
public class JsonItemScript {
    public string class_name = "null";
    public string class_type = "null";
}
