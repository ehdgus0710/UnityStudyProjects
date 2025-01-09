using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
class SaveData2
{
    public string pritiveName;
    public PrimitiveType primitiveType;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color color;
}

class SaveDataList
{
    public List<SaveData2> saveDatas = new List<SaveData2>();
}

public class JsonTest3 : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();

    public static readonly string fileName = "cube.json";
    // public static readonly string fileFullPath = Path.Combine(Application.persistentDataPath, fileName);

    public static readonly JsonSerializerSettings setting = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        Converters = { new Vector3Converter(), new QuaternionConverter(), new ColorConverter() }
    };

    public void OnCreateObject()
    {
        var gameObject = GameObject.CreatePrimitive((PrimitiveType)Random.Range((int)PrimitiveType.Sphere, (int)PrimitiveType.Quad));
        gameObject.transform.position = Random.insideUnitSphere * 10f;
        gameObject.transform.rotation = Random.rotation;
        gameObject.transform.localScale = Random.insideUnitSphere * 10f;
        gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
        gameObjects.Add(gameObject);
    }


    public void OnClearObjects()
    {
        foreach (var obj in gameObjects)
        {
            Destroy(obj);
        }
        gameObjects.Clear();
    }

    public void Save()
    {
        // SaveDataList saveDataList = new SaveDataList(); 
        List<SaveData2> saveData2s = new List<SaveData2>();

        foreach (var gameobj in gameObjects)
        {
            SaveData2 saveData = new SaveData2();
            saveData.pritiveName = gameobj.GetComponent<MeshFilter>().mesh.name;
            saveData.position = gameobj.transform.position;
            saveData.rotation = gameobj.transform.rotation;
            saveData.scale = gameobj.transform.localScale;
            saveData.color = gameobj.GetComponent<Renderer>().material.color;

            saveData2s.Add(saveData);
        }

        var json = JsonConvert.SerializeObject(saveData2s, setting);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName), json);
    }

    public void Load()
    {
        var fileFullPath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(fileFullPath))
            return;

        var json = File.ReadAllText(fileFullPath);

        List<SaveData2> loadData = JsonConvert.DeserializeObject<List<SaveData2>>(json, setting);

        foreach (var data in loadData)
        {
            var name = data.pritiveName.Split();
            GameObject gameObject = null;

            for (int i = 0; i < (int)PrimitiveType.Quad; ++i)
            {
                if (name[0] == ((PrimitiveType)i).ToString())
                {
                    gameObject = GameObject.CreatePrimitive((PrimitiveType)i);
                    break;
                }
            }

            gameObject.transform.position = data.position;
            gameObject.transform.rotation = data.rotation;
            gameObject.transform.localScale = data.scale;
            gameObject.GetComponent<Renderer>().material.color = data.color;
        }       
    }

}
