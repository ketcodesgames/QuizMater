using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter new question text here";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;

    public string GetQuestion() 
    {
        return question;
    }

    public string GetAnswer(int index) 
    {
        return answers[index];
    }

    public string GetCorrectAnswer() 
    {
        return answers[correctAnswerIndex];
    }

    public int GetCorrectAnswerIndex() 
    {
        return correctAnswerIndex;
    }
}
