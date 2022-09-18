using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class SystemControl : MonoBehaviour
{
    public GameObject[] steps;
    public GameObject[] objDemo;
    public int step;
    public float[] steptimes;
    public float assemblyTime;
    public int assemblyId;
    public string idWorker;
    public string url;
    public Producao producao;
    public Usuario usuario;
    public SendProducao sendProducao;

   
    private void Awake()
    {
       
    }


    // Start is called before the first frame update
    void Start()
    {
        
        assemblyId = 1;
        steptimes = new float[steps.Length];
        
    }

    // Update is called once per frame
    void Update()
    {
        // Tempo geral
        if (step > 0 && step < (objDemo.Length-1))
        {
            assemblyTime += Time.deltaTime;
        }

        // Tempo para cada um dos componentes.
        steptimes[step] += Time.deltaTime;


        // Incrementa o passo de montagem.
        if (Input.GetButtonDown("Fire1"))//***TODO: verificar se há impedimento para alterar para apenas a tecla ENTER para barrar a tecla ctl
        {
                       

            step++;

            OnActive();
        }

        // Decrementa o passo de montagem. 
        if (Input.GetButtonDown("Fire2") && step!= 0)
        {
            step--;

            OnActive();
        }
    }

    public void OnActive()
    {
        //desativar instruções que não são do passo atual
        foreach (GameObject a in steps)
        {
            a.SetActive(false);
        }
        // Desativa os objetos que não fazem parte do passo atual.
        foreach (GameObject a in objDemo)
        {
            a.SetActive(false);
        }

        // Caso o passo atual seja maior que o tamanho do vetor de passos.
        if (step >= objDemo.Length)
        {
            // reiniciar a montagem 
            step = 0;

            // armazenar tempos
            Writter();
            //manda os dados de produção para o servidor
            GraalProducaoSender();

            // Reseta o tempo 
            assemblyTime = 0;

            // Reseta os tempos para cada um dos passos.
            for (int i = 0; i< steptimes.Length; i++)
            {
                steptimes[i] = 0;
            }

            // Avança a chave.
            assemblyId++;
        }

        // O passo de montagem atual é ativo.
        steps[step].SetActive(true);
        // O objeto de montagem atual é ativo.
        objDemo[step].SetActive(true);
    }

    public void Writter()
    {
        Debug.Log("idWorker: " + idWorker);
        
        string bah = Application.persistentDataPath + "/" + idWorker + ".csv";
        Debug.Log("string bah vale: "+bah);
        FileStream fappend = File.Open(bah, FileMode.Append);
        TextWriter tw = new StreamWriter(fappend);

        // Escrever no arquivo.
        tw.WriteLine("sep= . ");
        tw.WriteLine("Montagem: " + assemblyId + " . " + " Data: " + System.DateTime.Now + " . " + SceneManager.GetActiveScene().name);
        
        for (int e = 0; e < steptimes.Length; e++)
        {
            // O tempo para cada um dos passos.
            // (e + 1): 1, 2, 3...
            tw.WriteLine("Tempo Passo " + (e + 1) + " . " + steptimes[e].ToString());
        }

        // O tempo total de montagem.
        tw.WriteLine("Tempo Total de Montagem " + " . " + assemblyTime);

        tw.Close();
        
    }

    public void StopStepWritter()
    {
        FileStream fappend = File.Open(idWorker + ".csv", FileMode.Append);
        TextWriter tw = new StreamWriter(fappend);

        // Escrever no arquivo
        tw.WriteLine("sep= . ");
        tw.WriteLine("Montagem: " + assemblyId + " . " + " Data: " + System.DateTime.Now + " . " + SceneManager.GetActiveScene().name);

        for (int e = 0; e < steptimes.Length; e++)
        {
            tw.WriteLine("Tempo Passo " + (e + 1) + " . " + steptimes[e].ToString());
        }

        tw.WriteLine("Tempo Total de Montagem " + " . " + assemblyTime);
        tw.WriteLine("Montagem interrompida" + " . " + assemblyTime);

        tw.Close();
        
    }

    public  void GraalProducaoSender()
    {
        Debug.Log("Iniciando GraalProducaoSender");
        //Pega a Producao persistida e salva os dados da montagem nela
        producao = FindObjectOfType<Producao>();

        Debug.Log("Inicializando as variáveis de produção");
            
        //variáveis confirmadas pela WEG
        producao.cartao = "P500B41";
        producao.op = "F32O7DA";
        producao.linha = "32";

        //O posto está associado a uma Etapa de montagem. Por exemplo, o Posto 4 pode montar a Etapa 2 da montagem completa da placa.
        producao.posto = "4";
        
        //data e hora de conclusão da montagem de cada etapa 
        producao.dataTarefa = System.DateTime.Now.ToString();

        producao.etapa = SceneManager.GetActiveScene().name;
        producao.tempoTarefa = assemblyTime.ToString();

        // o random de RP ficou no gamedisplay.cs, no momento de gerar as molduras de montagem correta ou montagem errada
    
        usuario= FindObjectOfType<Usuario>();
        Debug.Log("Id do usuário vale: " + usuario.id);
        producao.usuarioId = usuario.id;
        Debug.Log("producao.usuarioId vale: " + producao.usuarioId);


        string url = "http://127.0.0.1:8080/producao";
        Debug.Log("A url do post vale vale: " + url);

        //transforma o objeto Produção em uma string no formato Json
        Debug.Log("Objeto Produção tem cartão= " + producao.cartao);

        string jsonString = JsonUtility.ToJson(producao);
        Debug.Log("A string Produção vale: " + jsonString);

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
