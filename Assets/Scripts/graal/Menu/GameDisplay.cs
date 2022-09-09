using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameDisplay : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    //chave de acionamento da gamifica��o
    public Boolean gamificacao;
    public GameObject emojiSorrir;
    public GameObject emojiPensar;
    public GameObject emojiSerio;
    public GameObject montagemCorreta;
    public GameObject montagemErrada;

    public TMPro.TMP_Text displayTextoUsuarioG;
    public TMPro.TMP_Text displayTextoPostoPasso;
    public TMPro.TMP_Text displayTextoMenorTempo;
    public TMPro.TMP_Text displayTextoMaiorTempo;
    public TMPro.TMP_Text displayTextoConsecutivas;
    public TMPro.TMP_Text headerDesempenho;
    public TMPro.TMP_Text ritmoMontagem;
    public TMPro.TMP_Text headerTemposErros;
     

    public Usuario usuario;
    public Producao producao;

    public SystemControl systemControl;
    public FimDaEtapa fimDaEtapa;
    public InicioDaEtapa inicioDaEtapa;

    public int numeroRegularidades;
    public float taxaRegularidade;

    public int intervaloEntrePlacas;
    public Desempenho desempenho;
    public CanvasDesempenho canvasDesempenho;
    public CanvasRP canvasRP;

    public float tempo;

    //mostra grafico regularidade no rodap�
    public RawImage passo1;

    //public SortedDictionary<float, int> temposEtapas = new SortedDictionary<float, int>();

    public void Start()
    {

        canvasDesempenho.Desativar();
        canvasRP.Desativar();

        usuario = FindObjectOfType<Usuario>();
        producao= FindObjectOfType<Producao>();
        /*
        //inicia com gamifica��o desligada
        gamificacao = false;
        emojiSorrir.SetActive(false);
        emojiPensar.SetActive(false);
        emojiSerio.SetActive(false);
        */

        //inicia com gamifica��o ligada
        gamificacao = true;
        emojiSorrir.SetActive(true);
        emojiPensar.SetActive(false);
        emojiSerio.SetActive(false);


        intervaloEntrePlacas = 3;


        displayTextoUsuarioG.text = "Usu�rio: " + usuario.login + " G: " + gamificacao.ToString();
        // Debug.Log("DisplayLogin():login vale: " + displayTextoUsuarioG.text);

    }
    
    public void Update()
    {
        //Debug.Log("Tempo de montagem: " + systemControl.assemblyTime.ToString());

        usuario = FindObjectOfType<Usuario>();
        systemControl = FindObjectOfType<SystemControl>();
        inicioDaEtapa = FindObjectOfType<InicioDaEtapa>();
        fimDaEtapa = FindObjectOfType<FimDaEtapa>();

        ConfereTeclas();
        MostraRodape();
        ConfereMontagem();
       




        // mostra informa��es de desempenho ap�s n placas ***********************

        if (systemControl.assemblyId % intervaloEntrePlacas == 0 && fimDaEtapa.isActiveAndEnabled && gamificacao == true)
        {
            //m�ltiplo de intervaloEntrePlacas e porque precisa terminar todos os passos antes de mostrar resultados.
            Debug.Log("Hora de mostrar o desempemho depois de " + systemControl.assemblyId.ToString() + " placas montadas");

            //Liga a tela de desempenho
            canvasDesempenho.Ativar();

            //Linha de texto no cabe�alho da p�gina de desempenho
            headerDesempenho.text = "Seu desempenho ap�s " + systemControl.assemblyId.ToString() + " montagens";


            //Linha de texto no cabe�alho da Tabela de desempenho
            headerTemposErros.text = "Posto: " +SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2) ;


            //menor tempo  de montagem, como sendo a soma de todos os passos daquela montagem.
            tempo = desempenho.GetMenorTempo();
            Debug.Log("GameDisplay-Tempo do primeiro item do Dicion�rio Sorted vale: " + tempo.ToString());
            displayTextoMenorTempo.text = "Meu menor tempo no Posto " +SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2)+":  " + tempo.ToString()+" s.";


            //maior tempo de montagem
            tempo = desempenho.GetMaiorTempo();
            displayTextoMaiorTempo.text = "Meu maior tempo no Posto " + SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2) + ":  " + tempo.ToString() + " s.";

        
            //sugest�o Marcelo:
            ritmoMontagem.text = "Minha regularidade no Posto " 
                +SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2) + ":  "
                + desempenho.GetRitmo().ToString()
                + " %.";


            //TODO: esta linha de texto abaixo � para quando o m�dulo RP estiver funcionando
            // displayTextoConsecutivas.text = "Montagens sem erro: " + "nd.";

            //sugest�o Marcelo:
            displayTextoConsecutivas.text = "Minha taxa de acertos no Posto "
                + SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2)
                + ": " + "xx" + " %.";

            ConfereRitmo();
            

        }

        else
        {
            canvasDesempenho.Desativar();
        }


    }// fim do update


    void ConfereTeclas()
    {

        //Muda estado para RAM e desliga a gamifica��o
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.M) && gamificacao == true)
        {
            // Debug.Log("GameDisplay(): Reconheceu as teclas RAM");
            gamificacao = false;
            displayTextoUsuarioG.text = "Usu�rio: " + usuario.login + " G: " + gamificacao.ToString();
            //Debug.Log("DisplayLogin():login vale: " + displayTextoUsuarioG.text);

            emojiPensar.SetActive(false); emojiSerio.SetActive(false); emojiSorrir.SetActive(false);
        }


        //Muda estado para GAM   e liga a gamifica��o
        if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.M) && gamificacao == false)
        {
            // Debug.Log("GameDisplay(): Reconheceu as teclas GAM");
            gamificacao = true;
            displayTextoUsuarioG.text = "Usu�rio: " + usuario.login + " G: " + gamificacao.ToString();
            emojiPensar.SetActive(true); emojiSerio.SetActive(false); emojiSorrir.SetActive(false);
        }


        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.P))
        {
            // Debug.Log("DisplayLogin().Update():Saindo do sistema....!");
            systemControl.GraalProducaoSender();
            Application.Quit();
        }

    }//fim do ConfereTeclas()

    public void MostraRodape()
    {
        //mostra as informa��es b�sicas da tela *****************************
        displayTextoPostoPasso.text = "Posto: " + SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2) + " Passo: " + (systemControl.step).ToString() + "/" + (systemControl.steps.Length - 2).ToString() + " M: " + systemControl.assemblyId.ToString();
        displayTextoUsuarioG.text = "Usu�rio: " + usuario.login + " G: " + gamificacao.ToString();
        // Debug.Log("DisplayLogin().Update():login vale: " + displayTextoUsuarioG.text+"sbudegas1");


        if (systemControl.step > (systemControl.steps.Length - 2))
        {

            displayTextoPostoPasso.text = "Posto: " + SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 2) + " Passo: " + (systemControl.step - 1).ToString() + "/" + (systemControl.steps.Length - 2).ToString() + " M: " + systemControl.assemblyId.ToString();
            displayTextoUsuarioG.text = "Usu�rio: " + usuario.login + " G: " + gamificacao.ToString();

        }

        /*
         *TODO:
        //teste para criar a regularidade mais granular, a n�vel de passos
        
        if(systemControl.steptimes[1] <14.0f && systemControl.steptimes[1] >= 10.0f)
        {
            passo1.color = Color.green;
        }
        else
        {
            passo1.color = Color.gray;
        }
        */
    }


    public void ConfereRitmo()
    {

        if (desempenho.GetRitmo() >= 60.0f)
        {
            emojiSorrir.SetActive(true); emojiPensar.SetActive(false); emojiSerio.SetActive(false);
        }
        else
        {
            if (desempenho.GetRitmo() < 60.0f && desempenho.GetRitmo() >= 30.0f)
            {
                emojiSorrir.SetActive(false); emojiPensar.SetActive(true); emojiSerio.SetActive(false);
            }
            else
            {
                if (desempenho.GetRitmo() < 30.0f) emojiSorrir.SetActive(false); emojiPensar.SetActive(false); emojiSerio.SetActive(true);
            }

        }
    }

    public void ConfereMontagem()
    {
        if(fimDaEtapa.isActiveAndEnabled && gamificacao == true)
        {
            canvasRP.Ativar();
        }
        if (ifimDaEtapa.isActiveAndEnabled==false)// && gamificacao == true)
        {
            canvasRP.Desativar();
        }


    }

          
   
}