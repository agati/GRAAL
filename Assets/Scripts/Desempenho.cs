using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Desempenho : MonoBehaviour 
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public Usuario usuario;
    public SystemControl systemControl;
    public FimDaEtapa fimDaEtapa;
    public InicioDaEtapa inicioDaEtapa;
       
    
    public float[] temposMontagem;
    public float tempo;
    public float tempoMontagemPadrao;
    public float deltaAcimaTempoMontagemPadrao;
    public float deltaAbaixoTempoMontagemPadrao;
    public float toleranciaTempoMontagem;
    public float taxaRegularidade;

    public float resultadoMontagem;
    public float tempoControle;

    public int numeroRegularidades;
    public int regularidadeMontagem;
    public int numeroPassosMontagem;
    public int intervaloEntrePlacas;
    public int montagemControle;
    public int numeroMontagensIncorretas;//no momento, esse valor está sendo simulado no Gamidisplay() e salvo nesta variável.Deve vir do módulo RP no futuro.
    public int numeroMontagensCorretas;//no momento, esse valor está sendo simulado no Gamidisplay() e salvo nesta variável.Deve vir do módulo RP no futuro.


    public SortedDictionary<float, int> temposEtapas; //por default, não aparecem no inspector!!! 
    
   
    // Start is called before the first frame update
    void Start()
    {
        usuario = FindObjectOfType<Usuario>();
        systemControl = FindObjectOfType<SystemControl>();
        inicioDaEtapa = FindObjectOfType<InicioDaEtapa>();
        fimDaEtapa = FindObjectOfType<FimDaEtapa>();
        temposEtapas = new SortedDictionary<float, int>();

        //temposEtapas.Add(337.0f, 100);
        //Debug.Log("Desempenho-Start(): Primeira inserção em temposEtapas vale: " + temposEtapas.First().ToString());              
       
        //Desativar();
                       
        toleranciaTempoMontagem = 4.0f;
        //numeroRegularidades = 0;
        //taxaRegularidade = 0;
        

        tempoMontagemPadrao = 12.0f;
        tempoControle = 0.0f;
        montagemControle = 0;
        
    }

    private void Update()
    {
        usuario = FindObjectOfType<Usuario>();
        systemControl = FindObjectOfType<SystemControl>();
        inicioDaEtapa = FindObjectOfType<InicioDaEtapa>();
        fimDaEtapa = FindObjectOfType<FimDaEtapa>();
               
        IncrementaListaTemposEtapas();

    }

    public void Desativar()
    {
        gameObject.SetActive(false);
       
    }

    public void Ativar()
    {
        gameObject.SetActive(true);
               
    }

    public void AddRegularidade()
    {
        regularidadeMontagem++;
    }

    public int GetRegularidade()
    {
        return regularidadeMontagem;
    }

      public float GetMenorTempo()
    {
        return (float)Math.Truncate((double)temposEtapas.FirstOrDefault().Key*100)/100;
       
    }

    public float GetMaiorTempo()
    {
        return (float)Math.Truncate((double)temposEtapas.LastOrDefault().Key * 100) / 100;
       
    }

    public float GetRitmo()
    {
        float taxaRegularidade = (float)regularidadeMontagem / systemControl.assemblyId;
        return (float)Math.Truncate((double)taxaRegularidade*100);
        
    }

    public float GetTaxaErros()
    {
        return 100 * numeroMontagensIncorretas / systemControl.assemblyId;
    }

    public float GetTaxaAcertos()
    {
        return 100 * numeroMontagensCorretas / systemControl.assemblyId;
    }

    public  void IncrementaListaTemposEtapas()
    {

        if (fimDaEtapa.isActiveAndEnabled && tempoControle != systemControl.assemblyTime)
        {
            numeroPassosMontagem = systemControl.steps.Length-2;
            Debug.Log("Número de passos para esta montagem: " + numeroPassosMontagem.ToString());


            temposMontagem = systemControl.steptimes;
            Debug.Log("Desempenho():O tempo de montagem para o passo 2 etapa vale: " + temposMontagem[1].ToString());
            
            //regularidadeMontagem = 0;

            toleranciaTempoMontagem = 4.0f;
            //numeroRegularidades = 0;
            //taxaRegularidade = 0;

            tempoMontagemPadrao = 12.0f;
            tempoControle = 0.0f;
            //montagemControle = 0;

            // incrementa uma lista (tempo total de montagem, número da montagem) quando termina a última montagem 
            
            temposEtapas.TryAdd (systemControl.assemblyTime, systemControl.assemblyId);//adiciona (tempo total da montagem, número da montagem) no dicionário (key,value)
            //a probabilidade de existirem dois tempos de montagem iguais sendo float é praticamente impossível, mas verificar melhor isso depois (try entry)

            tempoControle = systemControl.assemblyTime;//não é uma variável repetida. Tem que ser diferente de resultado para funcionar corretamente
                                                       //montagemControle = systemControl.assemblyId;

            Debug.Log("Desempenho-IncrementaLista()-Incluído mais um tempo e etapa na SortedList");

            //esta consulta funcionou:temposEtapas.Last().Key.ToString() + " e M= " + temposEtapas.Last().Value.ToString());

            //atualiza a regularidade após o término da montagem

            //calculo do ritmo: quantas montagens ficaram dentro da faixa de tempo definida/todas as montagens efetuadas até o momento
            resultadoMontagem = systemControl.assemblyTime;
            Debug.Log("Resultado da montagem: " + resultadoMontagem.ToString());

            deltaAcimaTempoMontagemPadrao = tempoMontagemPadrao + (toleranciaTempoMontagem / 2);
            Debug.Log("Delta acima: " + deltaAcimaTempoMontagemPadrao.ToString());

            deltaAbaixoTempoMontagemPadrao = tempoMontagemPadrao - (toleranciaTempoMontagem / 2);
            Debug.Log("Delta abaixo: " + deltaAbaixoTempoMontagemPadrao.ToString());


            //compara tempo com margens superior e inferior
            if (resultadoMontagem <= deltaAcimaTempoMontagemPadrao && resultadoMontagem >= deltaAbaixoTempoMontagemPadrao)
            {
               Debug.Log("Valor da regularidade acumulada: " + GetRegularidade().ToString());

                AddRegularidade();

                Debug.Log("Valor da regularidade acrescentada: " + GetRegularidade().ToString());
                Debug.Log("taxa de regularidade vale:" + GetRitmo().ToString());

                Debug.Log("Desempenho-GetMenorTempo() do Dicionário vale: " + temposEtapas.First().Key.ToString());
                Debug.Log("Desempenho-GetMenorTempo() do Dicionário vale: " + GetMenorTempo().ToString());
            }
           
        }

    }//fim do incrementaLista()

  
}
