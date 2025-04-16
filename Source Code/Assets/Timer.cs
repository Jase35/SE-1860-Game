using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float RemainingTime;
    [SerializeField] TextMeshProUGUI WinText;

    // Update is called once per frame
    void Update()
    {
        if (RemainingTime >0){
         RemainingTime -= Time.deltaTime;
        }
        else if (RemainingTime<=0){
         RemainingTime = 0;
            WinText.text = "You Win!";
            Application.Quit();
        }
        int minutes = Mathf.FloorToInt(RemainingTime / 60);
        int seconds = Mathf.FloorToInt(RemainingTime%60);
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
}
