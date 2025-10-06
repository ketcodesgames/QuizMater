using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO question;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;
    
    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;
    public bool isComplete;

    void Awake()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update() 
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion) 
        {
            if (progressBar.value == progressBar.maxValue) 
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void GetNextQuestion() 
    {
        if (questions.Count > 0) 
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        } 
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        question = questions[index];
        if (questions.Contains(question)) 
        {
            questions.Remove(question);
        }
    }

    void DisplayQuestion() 
    {
        questionText.text = question.GetQuestion();

        for (int i=0; i<answerButtons.Length; i++) 
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = question.GetAnswer(i);
        }

        SetButtonState(true);
    }

    void SetButtonState(bool state) 
    {
        foreach (GameObject answerButton in answerButtons) 
        {
            Button button = answerButton.GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites() 
    {
        foreach (GameObject answerButton in answerButtons) 
        {
            Image buttonImage = answerButton.GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;
        int correctAnswerIndex = question.GetCorrectAnswerIndex();

        if (index == correctAnswerIndex) 
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;

            scoreKeeper.IncrementCorrectAnswers();
        } 
        else 
        {
            string correctAnswer = question.GetCorrectAnswer();
            questionText.text = "Sorry, the correct answer was; \n" + correctAnswer;

            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    public void OnAnswerSelected(int index) 
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }
}
