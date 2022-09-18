using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FimDaEtapa : MonoBehaviour
{
    public string nameScene;
    public SystemControl sis;

    public void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            sis.Writter();
            SceneManager.LoadScene(nameScene);
        }
        if (Input.GetButtonDown("Exit"))
        {
            sis.Writter();
            Application.Quit();
        }
    }
    public bool GetState()
    {
        if (gameObject.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
