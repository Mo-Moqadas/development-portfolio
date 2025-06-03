using System;

[Serializable]
public class Question
{
    public string QuestionText,QuestionReason ;
    public string[] Answers ;
    public int CorrectAnswerIndex ; // The index of the correct answer in the array
}
