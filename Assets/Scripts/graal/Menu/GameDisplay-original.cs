using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameDisplayOriginal: MonoBehaviour {
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    //chave de acionamento da gamificação
    public Boolean gamificacao;
    //public GameObject displayLogin;
    public GameObject emojiSorrir;
    public GameObject emojiPensar;
    public GameObject emojiSerio;
    public TMPro.TMP_Text displayTextoUsuarioG;
    public TMPro.TMP_Text displayTextoPlacaEtapaPasso;

    public Usuario usuario;
    public SystemControl systemControl;
    public FimDaEtapa fimDaEtapa;
    public InicioDaEtapa inicioDaEtapa;
    public float valorPadrao;
    public float resultado;
    public int intervaloEntrePlacas;
    


    public void Start()
   {
    
    usuario = FindObjectOfType<Usuario>();
    
        gamificacao = false;
        emojiSorrir.SetActive(false);
        emojiPensar.SetActive(false);
        emojiSerio.SetActive(false);
        intervaloEntrePlacas = 3;

        displayTextoUsuarioG.text = "Usuário: " + usuario.login + " G: "+gamificacao.ToString();

       Debug.Log("DisplayLogin():login vale: " + displayTextoUsuarioG.text);

    }

    public void Update()
    {
        usuario = FindObjectOfType<Usuario>();
        systemControl = FindObjectOfType<SystemControl>();
        inicioDaEtapa = FindObjectOfType<InicioDaEtapa>();
        fimDaEtapa = FindObjectOfType<FimDaEtapa>();

        displayTextoPlacaEtapaPasso.text = SceneManager.GetActiveScene().name + " Passo: " + (systemControl.step).ToString() + "/" + (systemControl.steps.Length - 2).ToString() + " M: " + systemControl.assemblyId.ToString();
        displayTextoUsuarioG.text = "Usuário: " + usuario.login + " G: " + gamificacao.ToString();
        Debug.Log("DisplayLogin().Update():login vale: " + displayTextoUsuarioG.text+"sbudegas1");

        if (fimDaEtapa.isActiveAndEnabled)
        {
            Debug.Log("DisplayLogin().Update():fim da Etapasbudegas2");
            displayTextoPlacaEtapaPasso.text = SceneManager.GetActiveScene().name + " Passo: "+ (systemControl.steps.Length - 2).ToString() +"/"+ (systemControl.steps.Length - 2).ToString()+ " M: "+systemControl.assemblyId.ToString();

        }
       
          
       if (systemControl.assemblyId%intervaloEntrePlacas==0 && fimDaEtapa.isActiveAndEnabled)//múltiplo de intervaloEntrePlacas e porque precisa terminar todos os passos antes de mostrar resultados.
        {
            //Debug.Log("Hora de mostrar o desempemho depois de "+systemControl.assemblyId.ToString()+" placas montadas");

        }

        Debug.Log("DisplayLogin().Update():login vale: " + displayTextoUsuarioG.text + "sbudegas2");

        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.M) && gamificacao == true)
        {
            Debug.Log("GameDisplay(): Reconheceu as teclas RAM");
            gamificacao = false;
            displayTextoUsuarioG.text = "Usuário: " + usuario.login + " G: " + gamificacao.ToString();
            //Debug.Log("DisplayLogin():login vale: " + displayTextoUsuarioG.text);

            emojiPensar.SetActive(false); emojiSerio.SetActive(false); emojiSorrir.SetActive(false);
        }
            
            

        if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.M) && gamificacao == false)
        {
            Debug.Log("GameDisplay(): Reconheceu as teclas GAM");
            gamificacao = true;
            displayTextoUsuarioG.text = "Usuário: " + usuario.login + " G: " + gamificacao.ToString();
            emojiPensar.SetActive(true); emojiSerio.SetActive(false);emojiSorrir.SetActive(false);
         }

           
                 
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.P) )
        {
           // Debug.Log("DisplayLogin().Update():Saindo do sistema....!");
            systemControl.GraalProducaoSender();
            Application.Quit();
        }
         
        

        if (fimDaEtapa.isActiveAndEnabled && gamificacao == true)
        {
           // Debug.Log("DisplayLogin().Update():End está ativo!");

            valorPadrao = 12.4f;

            resultado = systemControl.assemblyTime;

            Debug.Log("Tempo de Montagem Padrão: "+valorPadrao.ToString()+" e tempo de montagem foi: "+resultado.ToString());
                   

            if (resultado> valorPadrao)
            {
                emojiPensar.SetActive(false); emojiSerio.SetActive(true); emojiSorrir.SetActive(false);
            }
            else
            {
                emojiPensar.SetActive(false); emojiSerio.SetActive(false); emojiSorrir.SetActive(true);
            }
                        
        }

       if (Input.GetKey(KeyCode.R))
            {
          //  Debug.Log("Apertou R");

        }


    }// fim do update







}
