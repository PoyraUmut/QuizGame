using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float TimeToCompleteQuestion = 10f;
    [SerializeField] float TimeToShowCorrectAnswer = 5f;
    public float timerValue;
    public float fillFraction;
    public bool LoadNextQuestion = false;
    public bool isAnsweringQuestion = false;
    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer(){
        timerValue = 0;
    }
    void UpdateTimer(){
        timerValue-=Time.deltaTime;
        if(isAnsweringQuestion){
             if(timerValue>0){
                fillFraction = timerValue / TimeToCompleteQuestion;
            }
            else{
                isAnsweringQuestion = false;
                timerValue = TimeToShowCorrectAnswer;
            }
        }
        else{
            if(timerValue>0){
                fillFraction = timerValue / TimeToShowCorrectAnswer;
            }
            else{
            isAnsweringQuestion = true;
            timerValue = TimeToCompleteQuestion;
            LoadNextQuestion = true;}
        }
        Debug.Log(isAnsweringQuestion + ": " + timerValue+ "= " + fillFraction);
    }
}
