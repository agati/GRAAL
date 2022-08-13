using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class  SendProducao : MonoBehaviour
{
    public Producao producao;
    void Start()
    {
        
        Debug.Log("Inicializando as variáveis de produção fake");

        producao.cartao = "XPTO3334";
        producao.op = "F32O7DA";
        producao.linha = "32";
        producao.posto = "4";
        producao.dataTarefa = "2022-05-27";
        producao.etapa = "5";
        producao.tempoTarefa = "2,42";
        producao.correta = "1";
        producao.tempoRetrabalho = "3,27";
        producao.usuarioId = "57";


        //transforma o objeto Produção em uma string no formato Json
        Debug.Log("Objeto Produção tem cartão= " + producao.cartao);

        string jsonString = JsonUtility.ToJson(producao);
        Debug.Log("A string Produção vale: " + jsonString);

        string url = "http://127.0.0.1:8080/producao";
        Debug.Log("A url do post vale vale: " + url);

        StopAllCoroutines();
        StartCoroutine(PostRequest(url, jsonString));
    }

    public IEnumerator PostRequest(string url, string json)
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
