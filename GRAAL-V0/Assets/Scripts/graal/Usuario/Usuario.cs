using System;
using UnityEngine;

[Serializable]
public class Usuario: MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public string id;
    public string nome;
    public string cadastro;
    public string login;
    public string senha;
    public string dataInicio;
    
}