using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class SendUsuario : MonoBehaviour
{
    // Start is called before the first frame update
    public string jsonString;
    public Usuario usuario;
    public string url;

    void Start()
    {
       //usuario = new Usuario();

        usuario.id = "32";
        usuario.nome = "Francisca da Silva";
        usuario.cadastro = "324WEG43";
        usuario.login = "chica";
        usuario.senha = "1243";
        usuario.dataInicio = "19/05/2022";
              
        Debug.Log("Objeto Usuario: "+" "+ usuario.id + " " + usuario.nome + " " + usuario.cadastro + " " + usuario.login + " " + usuario.senha + " " + usuario.dataInicio);

        //transforma o objeto Usuario em uma string no formato Json
        jsonString = JsonUtility.ToJson(usuario);

        Debug.Log("A string Usuario vale: " + jsonString);

        url = "http://127.0.0.1:8080/usuarios";
        Debug.Log("A url do post vale vale: " + url);

        StopAllCoroutines();
        StartCoroutine(postRequest(url, jsonString));
    }

        IEnumerator postRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
