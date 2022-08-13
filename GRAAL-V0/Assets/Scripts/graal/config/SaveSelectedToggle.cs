using UnityEngine.UI;
using UnityEngine;

public class SaveSelectedToggle : MonoBehaviour
{
    //You Can Also Use An Array If More Easy To Use.
    [SerializeField] private Toggle toggle1, toggle2;
    //[SerializeField] private ToggleGroup allowSwitch;

    void Start()
    {
                  
            //toggle1.isOn = false;
           // toggle2.isOn = false;
           
       
    }

    //You Can Choose To Do Something When These Methods Are Called
    //For Example Change Graphic Settings or Change Languages etc.
    public void Toggle1Selected()
    {
        PlayerPrefs.SetInt("seletor", 0);
        Debug.Log("Escolheu RAMM");

       // toggle1.isOn = true;
       // toggle2.isOn = false;

        //Write Code Below To Choose What It Should Do Besides Saving Toggle Selected.
    }

    public void Toggle2Selected()
    {
        PlayerPrefs.SetInt("seletor", 1);
        Debug.Log("Escolheu GRAAL");

       // toggle1.isOn = false;
       // toggle2.isOn = true;

        //Write Code Below To Choose What It Should Do Besides Saving Toggle Selected.
    }

 }
