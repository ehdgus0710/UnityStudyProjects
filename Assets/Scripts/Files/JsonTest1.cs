using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// 직렬화를 할 때 Serializable가 없는 경우 직렬화에서 제외가 됨
[System.Serializable]
public class MyClass
{
    public int level;
    public float timeElapsed;
    public string playerName;
    
    [JsonConverter(typeof(Vector3Converter))]
    public Vector3 position;
}


public class JsonTest1 : MonoBehaviour
{
    private void Start()
    {
        MyClass myObject = new MyClass();
        myObject.level = 1;
        myObject.timeElapsed = 47.5f;
        myObject.playerName = "Dr Charles Francis";
        myObject.position = transform.position;

        //var settings = new JsonSerializerSettings
        //{
        //    Formatting = Formatting.Indented,
        //    Converters = { new Vector3Converter() }
        //};

        string json = JsonConvert.SerializeObject(myObject, new Vector3Converter());
        // string json = JsonConvert.SerializeObject(myObject, settings);
        Debug.Log(json);


        // Application.dataPath 가 asset 경로를 반환
        var path = System.IO.Path.Combine(Application.persistentDataPath, "test.json");
        System.IO.File.WriteAllText(path, json);

        //path = System.IO.Path.Combine(Application.persistentDataPath, "test2.json");
        //json = System.IO.File.ReadAllText(path);

        //MyClass myObject2 = JsonConvert.DeserializeObject<MyClass>(json, new Vector3Converter());
        //Debug.Log(myObject2.playerName);

    }
}
