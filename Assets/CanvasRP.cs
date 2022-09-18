using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRP : MonoBehaviour
{
    public GameObject molduraMontagemCorreta;
    public GameObject molduraMontagemErrada;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void Desativar()
    {
        gameObject.SetActive(false);

    }

    public void Ativar()
    {
        gameObject.SetActive(true);


    }

    public void SinalizaMontagemCorreta()
    {
        molduraMontagemErrada.SetActive(false);
        molduraMontagemCorreta.SetActive(true);
    }

    public void SinalizaMontagemErrada()
    {
        molduraMontagemErrada.SetActive(true);
        molduraMontagemCorreta.SetActive(false);
    }
}

