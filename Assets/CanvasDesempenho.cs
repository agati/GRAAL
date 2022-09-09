using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDesempenho : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
