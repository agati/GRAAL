using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRP : MonoBehaviour
{
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
}
