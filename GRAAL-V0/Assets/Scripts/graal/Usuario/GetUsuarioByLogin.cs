using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Windows.Forms.DataVisualization.Charting;
using UnityEngine;
using UnityEngine.Networking;

public  class GetUsuarioByLogin :MonoBehaviour
{
    public static Usuario usuario;
    public static string login;

    public void Start()
    {
        usuario = FindObjectOfType<Usuario>();
        Debug.Log("Usuario vale: "+ usuario.login+" e senha vale: "+usuario.senha);

        StopAllCoroutines();
        StartCoroutine(GetUsuario(usuario.login, usuario.senha));
    }


    public IEnumerator GetUsuario(string login, string senha)
    {
        string url = "http://127.0.0.1:8080/usuarios/por-login" + "?login=" + login + "&senha=" + senha; ;

        Debug.Log("BuscaUsuarioPorLogin(): A url de acesso ao servidor vale: " + url);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

        }
        else
        {
            // Show results as text
            Debug.Log("O servidor retornou: " + www.downloadHandler.text);
            string jsonString = www.downloadHandler.text.ToString();
            JsonUtility.FromJsonOverwrite(jsonString,usuario);
            Debug.Log(" O Json do usuário recebido convertido para objeto tem usuário id: " + usuario.id);
        }

    }
}

