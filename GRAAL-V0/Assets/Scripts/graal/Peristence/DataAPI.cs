using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class DataAPI : MonoBehaviour
{

   
    // Start is called before the first frame update
    void Start()
    {
        Usuario usuario = new Usuario();
        Debug.Log("Inicializando as variáveis de usuário");


        usuario.id = "32";
        usuario.nome = "Francisca da Silva";
        usuario.cadastro= "324WEG43";
        usuario.login = "chica";
        usuario.senha = "1243";
        usuario.dataInicio = "19/05/2022";

        Producao producao = new Producao();
        Debug.Log("Inicializando as variáveis de produção");

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

        string jsonString = "";

        //************** Usuario ******************************

        //transforma o objeto Usuario em uma string no formato Json
        Debug.Log("Objeto Usuario tem login= " + usuario.login);

         jsonString = JsonUtility.ToJson(usuario);
        Debug.Log("A string Usuario vale: " + jsonString);

        string url = "http://127.0.0.1:8080/usuarios";
        Debug.Log("A url do post vale vale: " + url);

        //envia o objeto usuario serializado para o servidor Tomcat
        StopAllCoroutines();
        StartCoroutine(PostRequest(url, jsonString));



        //*** Producao *****************************************

        //transforma o objeto Produção em uma string no formato Json
        Debug.Log("Objeto Produção tem cartão= " + producao.cartao);

        jsonString = JsonUtility.ToJson(producao);
        Debug.Log("A string Produção vale: " + jsonString);

        url = "http://127.0.0.1:8080/producao";
        Debug.Log("A url do post vale vale: " + url);

        //envia o objeto producao serializado para o servidor Tomcat
        StopAllCoroutines();
        StartCoroutine(PostRequest(url, jsonString));

    }

   //Faz post no servidor, enviandos dados em json
IEnumerator PostRequest(string url, string json)
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
