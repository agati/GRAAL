using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class JsonManager : MonoBehaviour
{
    public static string directory = "/SaveData/";
    public static string filename = "MyData.txt";
    public static string dir= Application.persistentDataPath + directory;

    public static void Save(ObjectToSave so)
    {
        Debug.Log("O diret�rio para salvar o arquivo vale: " + directory);
        Debug.Log("O nome do arquivo vale: " + filename);

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(so);
        File.WriteAllText(dir + filename, json);

    }

    public static ObjectToSave Load()
    {
        Debug.Log("O diret�rio para carregar o arquivo vale: " + dir);
        Debug.Log("O nome do arquivo vale: " + filename);

        ObjectToSave so = new ObjectToSave();

       if (File.Exists(dir))
        {
            string json = File.ReadAllText(dir+filename);
            so = JsonUtility.FromJson<ObjectToSave>(json);
            Debug.Log("O objeto recuperado �: " + so);
            return so;
        }
        else
        {
            Debug.Log("Save file n�o existe");
            return so;

        }

    }
}
