using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
class SaveData4
{
    public Vector3      position;
    public Quaternion   rotation;
    public Vector3      scale;
    public Color        color;
}

public class JsonTest2 : MonoBehaviour
{
    public static readonly string fileName = "cube.json";
    // public static readonly string fileFullPath = Path.Combine(Application.persistentDataPath, fileName);
    public GameObject target;
    public Color color;

    public static readonly JsonSerializerSettings setting = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        Converters = { new Vector3Converter(), new QuaternionConverter(), new ColorConverter() }
    };

    private void Start()
    {
        Load();

        // GameObject.CreatePrimitive(PrimitiveType.Plane)
    }

    public void Save()
    {
        SaveData4 saveData = new SaveData4();
        saveData.position = target.transform.position;
        saveData.rotation = target.transform.rotation;
        saveData.scale = target.transform.localScale;
        saveData.color = target.GetComponent<Renderer>().material.color;

        var json = JsonConvert.SerializeObject(saveData, setting);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName), json);
    }

    public void Load()
    {
        // var json = JsonConvert.DeserializeObject(path);

        var fileFullPath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(fileFullPath))
            return;

        var json = File.ReadAllText(fileFullPath);
        SaveData4 loadData = JsonConvert.DeserializeObject<SaveData4>(json, setting);
        target.transform.position = loadData.position;
        target.transform.rotation = loadData.rotation;
        target.transform.localScale = loadData.scale;
        target.GetComponent<Renderer>().material.color = loadData.color;
    }

    public void OnChangeColor()
    {
        target.GetComponent<Renderer>().material.color = color;
    }
}
