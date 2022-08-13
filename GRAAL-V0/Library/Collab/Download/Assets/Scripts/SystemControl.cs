using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
public class SystemControl : MonoBehaviour
{
    public GameObject[] steps;
    public GameObject[] objDemo;
    public int step;
    public float[] steptimes;
    public float assemblyTime;
    public int assemblyId;
    public string idWorker;

    // Start is called before the first frame update
    void Start()
    {
        assemblyId = 1;
        steptimes = new float[steps.Length];
    }

    
    // Update is called once per frame
    void Update()
    {
        if (step > 0 && step < (objDemo.Length-1))
        {
            assemblyTime += Time.deltaTime;
        }

        steptimes[step] += Time.deltaTime;


        if (Input.GetButtonDown("Fire1"))
        {
            step++;

            OnActive();
        }
        if (Input.GetButtonDown("Fire2") && step!= 0)
        {
            step--;

            OnActive();
        }

    }
    public void OnActive()
    {
        //desativar itens que não são da etapa
        foreach (GameObject a in steps)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in objDemo)
        {
            a.SetActive(false);
        }

        if (step >= objDemo.Length)
        {
            // reiniciar a montagem 
            step = 0;

            // armazenar tempos
            Writter();




            //zerar tempos

            assemblyTime = 0;
            
            for (int i = 0; i< steptimes.Length; i++)
            {
                steptimes[i] = 0;
            }

            //avançar a id
            assemblyId++;
        }

        steps[step].SetActive(true);
        objDemo[step].SetActive(true);
    }



    public void Writter()
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

        tw.Close();
    }
}
