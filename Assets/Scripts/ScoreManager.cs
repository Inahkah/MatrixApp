﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int arrosoirScore;
    public TextMeshProUGUI txt_ArrosoirScore;

    public int totemScore;
    public TextMeshProUGUI txt_TotemScore;

    //public float compteurTemps;
    public float timeBetweenIncreases;
    public int increases;
    public bool arrosageLoop;

    public GameObject obj_arbre;
    public GameObject obj_Arrosoire;
    public Renderer[] renderer_ArrosoireArray;
    public GameObject obj_ArrosezTexte;

    public float totemScale;

    public AnimationCurve arrosageIncreaseCurve;

    public IEnumerator Arrosage()
    {
        if (arrosageLoop) //augmente le score du totem en baissant celui de l'arrosage. Le temps entre chaque augmentation dépend de la curve
        {
            timeBetweenIncreases = 1/(arrosageIncreaseCurve.Evaluate(increases));
            if (arrosoirScore > 0)
            {
                ArrosoirScoreUpdate(-1);
                TotemScoreUpdate(+1);
                increases++;
                yield return new WaitForSecondsRealtime(timeBetweenIncreases);
                StartCoroutine(Arrosage());
            }
        }
    }

    public void ArrosoirScoreUpdate (int scoreToAddArrosoir) //augmente le score de l'arrosoir
    {
        arrosoirScore += scoreToAddArrosoir;
        txt_ArrosoirScore.text = "Arrosoir : " + arrosoirScore.ToString();
        //foreach (Renderer arrosoirePartRenderer in renderer_ArrosoireArray)
        //{ arrosoirePartRenderer.material.color = new Color(0, 0, arrosoirScore * 1.5f); }
        if (arrosoirScore == 0) // désactive le texte disant d'arroser si le score arrosoir passe à 0
        {
            obj_ArrosezTexte.SetActive(false);
        }
    }

    public void TotemScoreUpdate (int scoreToAddTotem) // augmente le score du totem, agrandit le totem et la taille du score
    {
        totemScore += scoreToAddTotem;
        txt_TotemScore.text = totemScore.ToString();
        txt_TotemScore.fontSize = 90 + totemScore * (210 / 500);
        totemScale = 0.35f + totemScore * (1.35f / 500);
        obj_arbre.transform.localScale = new Vector3 (totemScale, totemScale, totemScale);
    }

    public void ArrosageReset()
    {
        increases = 0;
    }
}
