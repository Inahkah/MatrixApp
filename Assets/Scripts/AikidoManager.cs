﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AikidoManager : MonoBehaviour
{
    public TextMeshProUGUI AikidoTMP;
    public Color[] AikidoColors;
    public string[] AikidoTexts;
    public int indexScreens = 0;
    public UIManager UIScript;
    public Image AikidoFond;
    public Image[] screenCounters;
    public Color screenCounterTransparencyLow;
    public Color screenCounterTransparencyHigh;
    public float lerpValue;
    public float lerpTime;
    public bool isFading;

    public ScoreManager scoreScript;
    public int scoreFinirAikido;

    public Vector2 mousePosBeginSwipe;
    public Vector2 mousePosEndSwipe;
    public int swipeXThreshold;
    public bool swipe;
    
    void Start()
    {
        UIScript = Camera.main.GetComponent<UIManager>();
        AikidoFond = GetComponent<Image>();
        AikidoTMP.text = AikidoTexts[indexScreens];
        AikidoFond.color = AikidoColors[indexScreens];
        foreach (Image screenCounter in screenCounters)
        { screenCounter.color = screenCounterTransparencyLow; }
        screenCounters[indexScreens].color = screenCounterTransparencyHigh;
    }
    private void OnMouseDown()
    {
        mousePosBeginSwipe = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    private void OnMouseUp()
    {
        mousePosEndSwipe = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (Vector2.Distance(mousePosBeginSwipe, mousePosEndSwipe) > swipeXThreshold)
        { swipe = true; }
        
        if (!isFading) //lorsque l'user clique sur le fond, si l'écran n'est pas déjà en train de changer, il change
        {
            if (!swipe)
            {
                if (Input.mousePosition.x < Screen.width / 2)
                { ScreenChange(false); }
                else
                { ScreenChange(true); }
            }
            else
            {
                if (mousePosEndSwipe.x > mousePosBeginSwipe.x + swipeXThreshold)
                { ScreenChange(true); }
                else if (mousePosEndSwipe.x < mousePosBeginSwipe.x - swipeXThreshold)
                { ScreenChange(false); }
            }
        }
        swipe = false;
    }

    private void OnDisable() // quand l'user quitte l'Aikido, les marqueurs, le texte, la couleur, la lerpValue et l'index se reset
    {
        screenCounters[indexScreens].color = screenCounterTransparencyLow;
        indexScreens = 0;
        AikidoTMP.text = AikidoTexts[indexScreens];
        AikidoFond.color = AikidoColors[indexScreens];
        screenCounters[0].color = screenCounterTransparencyHigh;
        lerpValue = 0;
        isFading = false;
    }
    // Update is called once per frame
    void ScreenChange(bool forward)
    {
        if (forward)
        {
            if (indexScreens < AikidoTexts.Length - 1) // si l'aikido n'est pas fini, update les marqueurs/index/texte, fade de la couleur
            {
                isFading = true;
                screenCounters[indexScreens].color = screenCounterTransparencyLow;
                StartCoroutine(FadeColor(AikidoColors[indexScreens], AikidoColors[indexScreens + 1], lerpTime));
                indexScreens++;
                AikidoTMP.text = AikidoTexts[indexScreens];
                AikidoFond.color = AikidoColors[indexScreens];
                screenCounters[indexScreens].color = screenCounterTransparencyHigh;
            }
            else // si l'aikido est fini, désactiver l'objet
            {
                UIScript.ActivateUI(UIScript.uIObjects[0]);
            }
        }
        else
        {
            if (indexScreens > 0) // si l'aikido n'est pas fini, update les marqueurs/index/texte, fade de la couleur
            {
                isFading = true;
                screenCounters[indexScreens].color = screenCounterTransparencyLow;
                StartCoroutine(FadeColor(AikidoColors[indexScreens], AikidoColors[indexScreens - 1], lerpTime));
                indexScreens--;
                AikidoTMP.text = AikidoTexts[indexScreens];
                AikidoFond.color = AikidoColors[indexScreens];
                screenCounters[indexScreens].color = screenCounterTransparencyHigh;
            }
            else 
            {
            }
        }
    }

    public IEnumerator FadeColor(Color from,Color to, float lerpTimeCoroutine)
    {
        if(lerpValue < 1) // si le lerp n'est pas encore fini, augmenter lerpValue et changer la couleur
        {
            lerpValue += Time.deltaTime / lerpTimeCoroutine;
            AikidoFond.color = Color.Lerp(from, to, lerpValue);
            yield return new WaitForSeconds(0.001f);
            StartCoroutine(FadeColor(from, to, lerpTimeCoroutine));
        }
        else
        {
            isFading = false;
            lerpValue = 0;
        }
    }
}
