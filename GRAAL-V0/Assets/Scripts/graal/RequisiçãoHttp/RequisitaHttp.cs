using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RequisitaHttp: MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/tela/teste");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log("O servidor retornou: "+ www.downloadHandler.text);



            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            Debug.Log("O servidor retornou: " + www.downloadHandler.data);

        }
    }

}

