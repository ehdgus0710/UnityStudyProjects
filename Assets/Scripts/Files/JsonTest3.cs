using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public struct SaveData2
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


    public class PushCommand : ICommand
    {
        private readonly List<GameObject> gameObjects;
        private readonly SaveData2 savedata;

        public PushCommand(List<GameObject> list, SaveData2 data)
        {
            this.gameObjects = list;
            this.savedata = data;
        }

        public void Execute()
        {
            var obj = GameObject.CreatePrimitive(savedata.primitiveType);
            obj.transform.position = savedata.position;
            obj.transform.rotation = savedata.rotation;
            obj.transform.localScale = savedata.scale;
            obj.GetComponent<Renderer>().material.color = savedata.color;
            gameObjects.Add(obj);
        }

        public void Undo()
        {
            Destroy(gameObjects[gameObjects.Count - 1]);
            gameObjects.RemoveAt(gameObjects.Count - 1);
        }
    }

    public class PopCommand : ICommand
    {
        private readonly List<GameObject> gameObjects;
        private SaveData2 removeData;

        public PopCommand(List<GameObject> list)
        {
            gameObjects = list;
        }

        public void Execute()
        {
            var obj = gameObjects[gameObjects.Count - 1];
            removeData.position = obj.transform.position;
            removeData.rotation = obj.transform.rotation;
            removeData.scale = obj.transform.localScale;
            removeData.color = obj.GetComponent<Renderer>().material.color;
            removeData.pritiveName = obj.GetComponent<MeshFilter>().mesh.name;

            gameObjects.RemoveAt(gameObjects.Count - 1);
        }

        public void Undo()
        {
            var name = removeData.pritiveName.Split();
            GameObject undoObject = null;

            for (int i = 0; i < (int)PrimitiveType.Quad; ++i)
            {
                if (name[0] == ((PrimitiveType)i).ToString())
                {
                    undoObject = GameObject.CreatePrimitive((PrimitiveType)i);
                    break;
                }
            }

            if (undoObject == null)
                return;

            undoObject.transform.position = removeData.position;
            undoObject.transform.rotation = removeData.rotation;
            undoObject.transform.localScale = removeData.scale;
            undoObject.GetComponent<Renderer>().material.color = removeData.color;
            gameObjects.Add(undoObject);
        }
    }

    public class ClearCommand : ICommand
    {
        private readonly List<GameObject> datas;
        private List<SaveData2> removeDatas = new List<SaveData2>();
        private SaveData2 removeData;

        public ClearCommand(List<GameObject> datas)
        {
            this.datas = datas;
        }

        public void Execute()
        {
            foreach (var obj in datas)
            {
                SaveData2 data = new SaveData2();
                data.position = obj.transform.position;
                data.rotation = obj.transform.rotation;
                data.scale = obj.transform.localScale;
                data.color = obj.GetComponent<Renderer>().material.color;
                removeData.pritiveName = obj.GetComponent<MeshFilter>().mesh.name;
                removeDatas.Add(data);
                Destroy(obj);
            }
            datas.Clear();
        }

        public void Undo()
        {
            foreach (var data in removeDatas)
            {
                var name = removeData.pritiveName.Split();
                GameObject undoObject = null;

                for (int i = 0; i < (int)PrimitiveType.Quad; ++i)
                {
                    if (name[0] == ((PrimitiveType)i).ToString())
                    {
                        undoObject = GameObject.CreatePrimitive((PrimitiveType)i);
                        break;
                    }
                }

                if (undoObject == null)
                    return;

                undoObject.transform.position = data.position;
                undoObject.transform.rotation = data.rotation;
                undoObject.transform.localScale = data.scale;
                undoObject.GetComponent<Renderer>().material.color = data.color;
                objectDatas.Add(undoObject);
            }
            removeDatas.Clear();
        }
    }


    private static List<GameObject> objectDatas = new List<GameObject>();
    private Stack<ICommand> undoStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    public void OnCreateObject()
    {
        SaveData2 data = new SaveData2();
        data.primitiveType = (PrimitiveType)Random.Range((int)PrimitiveType.Sphere, (int)PrimitiveType.Quad);
        data.position = Random.insideUnitSphere * 10f;
        data.rotation = Random.rotation;
        data.scale = Random.insideUnitSphere * 10f;
        data.color = Random.ColorHSV();

        var push = new PushCommand(objectDatas, data);
        push.Execute();
        undoStack.Push(push);
        redoStack.Clear();
    }


    public void OnClearObjects()
    {
        var clear = new ClearCommand(objectDatas);
        clear.Execute();
        undoStack.Push(clear);
        redoStack.Clear();
    }

    public void OnUndo()
    {
        if (undoStack.Count > 0)
        {
            var undoCmd = undoStack.Pop();
            undoCmd.Undo();
            redoStack.Push(undoCmd);
        }
    }

    public void OnRedo()
    {
        if (redoStack.Count > 0)
        {
            var redoCmd = redoStack.Pop();
            redoCmd.Execute();
            undoStack.Push(redoCmd);
        }
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
