using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion;
    public float fillFraction;
    float timerValue;

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isAnsweringQuestion) 
        {
            if (timerValue > 0) 
            {
                fillFraction = timerValue / timeToCompleteQuestion;
            }
            else
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
            }
        } 
        else 
        {
            if (timerValue > 0) 
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                timerValue = timeToCompleteQuestion;
                isAnsweringQuestion = true;
                loadNextQuestion = true;
            }
        }
    }

    public void CancelTimer() 
    {
        timerValue = 0;
    }
}
