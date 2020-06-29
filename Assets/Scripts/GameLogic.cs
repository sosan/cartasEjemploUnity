﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;
using System;

using GlobalActions;
using UnityEngine.InputSystem;
using UnityEditor;

public class GameLogic : MonoBehaviour
{


    private InputActions inputActions;

    [SerializeField] private int currentPositionPlayer = -1;
    [SerializeField] private int lastPositionPlayer = -1;
    [SerializeField] private Image[] cartas = null;
    [SerializeField] private RectTransform[] cartasRect = null;
    [SerializeField] private List<Sprite> poolImages = new List<Sprite>();
    [SerializeField] private Sprite[] imagesCartas = null;

    [SerializeField] private RectTransform[] posicionPlayer = null;
    [SerializeField] private RectTransform recPlayer = null;
    [SerializeField] private Animation anim = null;

    [SerializeField] private Animation[] animsCards = null;
    [SerializeField] private Outline[] outlineCards = null;
    [SerializeField] private Sprite malClickSprite = null;
    [SerializeField] private Sprite backgroundCard = null;

    private List<int> listCards = new List<int>();
    public bool clickedCard = false;
    public bool isBegin = true;

    private short rangoXMin = -400;
    private short rangoXMax = 400;

    private short rangoYMin = -240;
    private short rangoYMax = 240;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        inputActions = new InputActions();
        //inputActions.PlayerMovement.clicked.performed += Clicked

    

        for (ushort i = 0; i < cartas.Length; i++)
        {


            outlineCards[i].enabled = false;

        }

        GenerateCartasMezcla();

    }




    private void GenerateCartasMezcla()
    { 
        AnimationClip clip = new AnimationClip();
        clip.name = "cartas_mezcla_runtime";
        clip.legacy = true;
    
        clip.wrapMode = WrapMode.Once;

        for(ushort i = 0; i < cartas.Length; i++ )
        { 
            Keyframe[] keysX = new Keyframe[6];
            Keyframe[] keysY = new Keyframe[6];

            float timeRot = 1.05f;
        

            keysX[0] = new Keyframe(0f, UnityEngine.Random.Range(rangoXMin, rangoXMax) );
            keysX[1] = new Keyframe(0.18f, UnityEngine.Random.Range(rangoXMin, rangoXMax));
            keysX[2] = new Keyframe(0.38f, UnityEngine.Random.Range(rangoXMin, rangoXMax));
            keysX[3] = new Keyframe(0.58f, UnityEngine.Random.Range(rangoXMin, rangoXMax));
            keysX[4] = new Keyframe(timeRot, 500);
            keysX[5] = new Keyframe(1.40f, posicionPlayer[i].anchoredPosition.x);

            keysY[0] = new Keyframe(0f, UnityEngine.Random.Range(rangoYMin, rangoYMax) );
            keysY[1] = new Keyframe(0.18f, UnityEngine.Random.Range(rangoYMin, rangoYMax) );
            keysY[2] = new Keyframe(0.38f, UnityEngine.Random.Range(rangoYMin, rangoYMax) );
            keysY[3] = new Keyframe(0.58f, UnityEngine.Random.Range(rangoYMin, rangoYMax) );
            keysY[4] = new Keyframe(timeRot, 250 );
            keysY[5] = new Keyframe(1.40f, posicionPlayer[i].anchoredPosition.y);


            Keyframe[] keysRotX = new Keyframe[11];
            Keyframe[] keysRotY = new Keyframe[11];
            Keyframe[] keysRotZ = new Keyframe[11];
            Keyframe[] keysRotW = new Keyframe[11];

        

            Quaternion angle0 = Quaternion.Euler(0, 0, 0);
            Quaternion angle90 = Quaternion.AngleAxis(90, Vector3.forward);
            Quaternion angle180 = Quaternion.Euler(0, 0, 180);
            Quaternion angle270 = Quaternion.Euler(0, 0, 270);
            Quaternion angle360 = Quaternion.Euler(0, 0, 360);

            Quaternion rot = Quaternion.identity;
            float angle = 90f;
            Vector3 axis = Vector3.forward;
            float timeSumed = 0.03f;

            for(int k = 0; k < 7; k++)
            {
                rot = Quaternion.AngleAxis(angle * k, axis);
            

                //create the keys
                keysRotX[k] = new Keyframe(timeRot, rot.x);
                keysRotY[k] = new Keyframe(timeRot, rot.y);
                keysRotZ[k] = new Keyframe(timeRot, rot.z);
                keysRotW[k] = new Keyframe(timeRot, rot.w);

                timeRot += timeSumed;
            }


            keysRotX[10] = new Keyframe(timeRot, angle0.x);
            keysRotY[10] = new Keyframe(timeRot, angle0.y);
            keysRotZ[10] = new Keyframe(timeRot, angle0.z);
            keysRotW[10] = new Keyframe(timeRot, angle0.w);


            //keysRotX[1] = new Keyframe(, angle90.x );
            //keysRotY[1] = new Keyframe(timeRot += timeSumed, angle90.y );
            //keysRotZ[1] = new Keyframe(timeRot += timeSumed, angle90.z );
            //keysRotW[1] = new Keyframe(timeRot += timeSumed, angle90.w );


        
                // keysRotZ[2] = new Keyframe(timeRot += timeSumed, angle180.z);
                // keysRotZ[3] = new Keyframe(timeRot += timeSumed, angle270.z);
                // keysRotZ[4] = new Keyframe(timeRot += timeSumed, angle360.z);
                // keysRotZ[5] = new Keyframe(timeRot += timeSumed, angle90.z);
                // keysRotZ[6] = new Keyframe(timeRot += timeSumed, angle180.z);
                // keysRotZ[7] = new Keyframe(timeRot += timeSumed, angle270.z);
                // keysRotZ[8] = new Keyframe(timeRot += timeSumed, angle360.z);
                // keysRotZ[9] = new Keyframe(timeRot += timeSumed, angle90.z);
                // keysRotZ[10] = new Keyframe(timeRot += timeSumed, angle0.z);

            


                AnimationCurve curvex = new AnimationCurve(keysX);
                AnimationCurve curvey = new AnimationCurve(keysY);

                AnimationUtility.SetKeyRightTangentMode(curvex, 0, AnimationUtility.TangentMode.Free);
                AnimationUtility.SetKeyRightTangentMode(curvey, 0, AnimationUtility.TangentMode.Free);

                AnimationCurve curveRotx = new AnimationCurve(keysRotX);
                AnimationCurve curveRoty = new AnimationCurve(keysRotY);
                AnimationCurve curveRotz = new AnimationCurve(keysRotZ);
                AnimationCurve curveRotw = new AnimationCurve(keysRotW);
            


                string nombreCarta = "carta_" + (i + 1);
                clip.SetCurve(nombreCarta, typeof(RectTransform), "m_AnchoredPosition.x", curvex);
                clip.SetCurve(nombreCarta, typeof(RectTransform), "m_AnchoredPosition.y", curvey);



                clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.x", curveRotx);
                clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.y", curveRoty);
                clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.z", curveRotz);
                clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.w", curveRotw);

                //clip.EnsureQuaternionContinuity();

        }
        anim.AddClip(clip, clip.name);
            
    }



    // Start is called before the first frame update
    private async void Start()
    {
        isBegin = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        Click_NewGame();
    }


    public async void Click_Carta(int posicion)
    { 

        if (isBegin == true || clickedCard == true || posicion < 0 || posicion > 9) return;

        if (lastPositionPlayer == -1 )
        { 
            lastPositionPlayer = posicion - 1;

        }
        else
        {

            if (lastPositionPlayer == posicion - 1) return; 
           
            switch(lastPositionPlayer)
            {
                case 0: if (posicion == 5 || posicion == 6 || posicion == 8 || posicion == 9) { MalClick(posicion); return; } break;
                case 1: if (posicion == 4 || posicion == 6 || posicion == 7 || posicion == 9) { MalClick(posicion); return; } break;
                case 2: if (posicion == 4 || posicion == 5 || posicion == 7 || posicion == 8) { MalClick(posicion); return; } break;
                case 3: if (posicion == 2 || posicion == 3 || posicion == 8 || posicion == 9) { MalClick(posicion); return; } break;
                case 4: if (posicion == 1 || posicion == 3 || posicion == 7 || posicion == 9) { MalClick(posicion); return; } break;
                case 5: if (posicion == 1 || posicion == 2 || posicion == 7 || posicion == 8) { MalClick(posicion); return; } break;
                case 6: if (posicion == 2 || posicion == 3 || posicion == 5 || posicion == 6) { MalClick(posicion); return; } break;
                case 7: if (posicion == 1 || posicion == 3 || posicion == 4 || posicion == 6) { MalClick(posicion); return; } break;
                case 8: if (posicion == 1 || posicion == 2 || posicion == 4 || posicion == 5) { MalClick(posicion); return; } break;
            
            }



            outlineCards[lastPositionPlayer].GetComponent<Image>().enabled = false;

            //string nombreclipLastPosition = "carta_giro_" + (lastPositionPlayer + 1);

            AnimationClip clipTemp = GenerateClipAnimation(lastPositionPlayer, "giro_carta_lastposition");

            anim.AddClip(clipTemp, clipTemp.name);
            anim.Play(clipTemp.name);

            //anim.Play(nombreclipLastPosition);
            await UniTask.Delay(TimeSpan.FromSeconds(anim.GetClip(clipTemp.name).length));
            //cartas[lastPositionPlayer].color = Color.black;
            cartas[lastPositionPlayer].sprite = backgroundCard;
            anim.RemoveClip(clipTemp);

        }

        clickedCard = true;

        AnimationClip clip = GenerateClipAnimation(posicion - 1, "giro_carta_siguiente");
        
        float fullDurationClip = clip.length * 1000; //anim.GetClip("carta_giro").length * 1000;
        float restDurationClip = fullDurationClip - 100;
        
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);
        await UniTask.Delay(TimeSpan.FromMilliseconds(fullDurationClip - 100));
        
        currentPositionPlayer = posicion - 1;
        cartas[currentPositionPlayer].sprite = poolImages[currentPositionPlayer];

        await UniTask.Delay(TimeSpan.FromMilliseconds(restDurationClip));
        
        anim.RemoveClip(clip);

        animsCards[lastPositionPlayer].Stop();
        outlineCards[lastPositionPlayer].enabled = false;
        outlineCards[lastPositionPlayer].GetComponent<Image>().enabled = false;
        
        
        lastPositionPlayer = currentPositionPlayer;

        
        cartas[currentPositionPlayer].color = Color.white;
        outlineCards[currentPositionPlayer].enabled = true;
        outlineCards[currentPositionPlayer].GetComponent<Image>().enabled = true;

        animsCards[currentPositionPlayer].Play("carta_outline");
        await UniTask.Delay(TimeSpan.FromSeconds(animsCards[currentPositionPlayer].GetClip("carta_outline").length));
        clickedCard = false;
        
    
    }

    private AnimationClip GenerateClipAnimation(int posicion, string nombreClip)
    { 
        AnimationClip clip = new AnimationClip();
        clip.name = nombreClip;
        clip.legacy = true;
        clip.wrapMode = WrapMode.Once;

        Keyframe[] keysX = new Keyframe[3];
        Keyframe[] keysY = new Keyframe[1];
        Keyframe[] keysZ = new Keyframe[1];

        float x = cartasRect[posicion].anchoredPosition.x;

        keysX[0] = new Keyframe(0f, x );
        keysX[1] = new Keyframe(0.05f, x -= 44);
        keysX[2] = new Keyframe(0.15f, x += 44);
        
        keysY[0] = new Keyframe(0f, cartasRect[posicion].anchoredPosition.y );
        keysZ[0] = new Keyframe(0f, 0);

        
        Keyframe[] keysRotX = new Keyframe[1];
        Keyframe[] keysRotY = new Keyframe[4];
        Keyframe[] keysRotZ = new Keyframe[1];
        Keyframe[] keysRotW = new Keyframe[1];

        //float rotX = cartasRect[posicion - 1].localRotation.x;

        Quaternion angle0 = Quaternion.Euler(0, 0, 0);
        Quaternion angle90 = Quaternion.Euler(0, 92, 0);

        keysRotX[0] = new Keyframe(0f, angle0.x );
        keysRotY[0] = new Keyframe(0f, angle0.y );
        keysRotZ[0] = new Keyframe(0f, angle0.z );
        keysRotW[0] = new Keyframe(0f, angle0.w );

        
        keysRotY[1] = new Keyframe(0.03f, angle0.y);
        keysRotY[2] = new Keyframe(0.10f, angle90.y);
        keysRotY[3] = new Keyframe(0.20f, angle0.y );
        
        


        AnimationCurve curvex = new AnimationCurve(keysX);
        AnimationCurve curvey = new AnimationCurve(keysY);
        AnimationCurve curvez = new AnimationCurve(keysZ);

        AnimationCurve curveRotx = new AnimationCurve(keysRotX);
        AnimationCurve curveRoty = new AnimationCurve(keysRotY);
        AnimationCurve curveRotz = new AnimationCurve(keysRotZ);
        AnimationCurve curveRotw = new AnimationCurve(keysRotW);

        string nombreCarta = "carta_" + (posicion + 1);
        clip.SetCurve(nombreCarta, typeof(Transform), "localPosition.x", curvex);
        clip.SetCurve(nombreCarta, typeof(Transform), "localPosition.y", curvey);
        clip.SetCurve(nombreCarta, typeof(Transform), "localPosition.z", curvez);

        clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.x", curveRotx);
        clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.y", curveRoty);
        clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.z", curveRotz);
        clip.SetCurve(nombreCarta, typeof(Transform), "localRotation.w", curveRotw);

        clip.EnsureQuaternionContinuity();

        return clip;
    
    
    
    }

    private async void MalClick(int posicion)
    { 
    
        int pos = posicion - 1;
        var tempSprite = cartas[pos].sprite;
        cartas[pos].sprite = malClickSprite;
        await UniTask.Delay(TimeSpan.FromMilliseconds(500));
        cartas[pos].sprite = tempSprite;




    }

    public async void Click_NewGame()
    { 
        poolImages.Clear();
        isBegin = true;
        lastPositionPlayer = -1;
        currentPositionPlayer = -1;

        for (ushort i = 0; i < cartas.Length; i++)
        {
            outlineCards[i].enabled = false;

        }


        { 
        
            anim.RemoveClip("cartas_mezcla_runtime");
            GenerateCartasMezcla();
        }
        

        anim.Play("cartas_mezcla_runtime");
        await UniTask.Delay(TimeSpan.FromSeconds( anim.GetClip("cartas_mezcla_runtime").length ));

        listCards.Clear();
        for(ushort i = 0; i < cartas.Length; i++)
        {
            cartasRect[i].rotation = Quaternion.Euler(0,0,0);
            InsertarCarta(i);
           
        
        }
        isBegin = false;
    }

    private void InsertarCarta(int i)
    { 
        bool insertado = false;

        while(insertado == false)
        { 
            int rnd = UnityEngine.Random.Range(0, imagesCartas.Length);
            if (listCards.Contains(rnd) == false)
            { 
            
                listCards.Add(rnd);
                //cartas[i].color = Color.black;
                poolImages.Add(imagesCartas[rnd]);
                //cartas[i].sprite = imagesCartas[rnd];
                
                insertado = true;
            }
                       
        }

    
    }


    public void QuitGame()
    { 
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    
    
    }

}