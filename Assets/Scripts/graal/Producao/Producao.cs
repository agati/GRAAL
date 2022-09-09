using System;
using UnityEngine;

[Serializable]
public class Producao : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public string cartao;
    public string op;
    public string linha;
    public string posto;
    public string dataTarefa;
    public string etapa;
    public string tempoTarefa;
    public string correta;
    public string tempoRetrabalho;
    public string usuarioId;
      

}


