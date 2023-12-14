using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public Text textoCronometro;
    public Text score;
    public static float time = 0F;
    public static int scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + Mathf.Round(scoreCount);
        time += Time.deltaTime;
        ActualizarCronometro();
    }
    void ActualizarCronometro()
    {
        int horas = (int)(time / 3600);
        int minutos = (int)((time % 3600) / 60);
        int segundos = (int)(time % 60);

        string formatoCronometro = string.Format("{0:00}:{1:00}:{2:00}", horas, minutos, segundos);

        textoCronometro.text = formatoCronometro;
    }
}
