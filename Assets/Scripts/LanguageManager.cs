﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Cysharp.Threading.Tasks;

public class LanguageManager : MonoBehaviour
{


    [SerializeField] private Animation anim = null;

    [SerializeField] private GameLogic gameLogic = null;
    //[SerializeField] private Color selectColor = Color.white;
    //[SerializeField] private Color notSelectColor = Color.white;
    [SerializeField] private TextMeshProUGUI[] textos = null;
    [SerializeField] public ParticleSystem[] particulas = null;
    //[SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroupSelectPersonajes = null;
    [SerializeField] private CanvasGroup canvasGroupSelectLanguage = null;

    [SerializeField] private RectTransform[] posiciones = null;
    [SerializeField] private RectTransform cuadro = null;


    private void Awake()
    {
        print("idioma=" + PlayerPrefs.HasKey("idioma"));
        if (PlayerPrefs.HasKey("idioma") == true)
        {

            string t = PlayerPrefs.GetString("idioma");

            switch (t)
            {

                case "espanol": Localization.language = t; break;
                case "english": Localization.language = t; break;
                default: Debug.LogError("NOT SET IDIOMA"); return;

            }

            DesactivarCanvasIdiomas();
            gameLogic.Click_NewGame(false);

        }
        else
        {
            anim.Play("aparecerCanvasIdiomas");

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void DesactivarCanvasIdiomas()
    {

        canvasGroupSelectLanguage.gameObject.SetActive(false);
        canvasGroupSelectLanguage.alpha = 0;
        canvasGroupSelectLanguage.blocksRaycasts = false;
        canvasGroupSelectLanguage.interactable = false;

    }

    public async void ClickedSetEnglish()
    {
        DesactivarCanvasIdiomas();
        Localization.language = "english";
        PlayerPrefs.SetString("idioma", "english");
        particulas[0].Play();
        await UniTask.Delay(300);
        gameLogic.Click_NewGame(false);
    }


    public async void ClickedSetSpanish()
    {

        DesactivarCanvasIdiomas();
        Localization.language = "espanol";
        PlayerPrefs.SetString("idioma", "espanol");
        particulas[1].Play();
        await UniTask.Delay(300);

        
        gameLogic.Click_NewGame(false);

    }

    

}
