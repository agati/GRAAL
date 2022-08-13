using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilitaGamificacao : MonoBehaviour
{
    public GameDisplay gameDisplay;



    public void OnEnable()
    {
        gameDisplay = FindObjectOfType<GameDisplay>();
        gameDisplay.gamificacao = true;

    }

    



}
