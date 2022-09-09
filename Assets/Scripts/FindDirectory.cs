using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDirectory : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Diretório para salvar serialização: "+Application.persistentDataPath);
        }
    
}

