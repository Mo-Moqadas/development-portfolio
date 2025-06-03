using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {
    
    [Header ("Question UI settings")]
    public RTLTextMeshPro QuestionText;
    public RTLTextMeshPro FeedbackText;
    [Tooltip ("by default add 4  buttons for answering")] public Toggle[] AnswerButtons;
    public GameObject ContinueBtn;
    [Header ("Question Data")]
    [Tooltip ("add scriptable object of questions data here")] public QuestionData QuestionsData;
    private int _currentQuestionIndex, _answerIndex = 0;

    private void ShowQuestion (int questionIndex) {
        // Set the question text
        Question question = QuestionsData.questions[questionIndex];
        QuestionText.text = question.QuestionText;

        // Set the text for each answer button
        for (int i = 0; i < 4; i++) {
            AnswerButtons[i].GetComponentInChildren<RTLTextMeshPro> ().text = question.Answers[i];
            AnswerButtons[i].isOn = false;
        }
        //by activating a the select trigaer action call will call OnanswerSelected
        ContinueBtn.SetActive (false);

        // Clear feedback text
        FeedbackText.text = "";
    }
    public void OnAnswerSelected (int _answerIndex) {
        this._answerIndex = _answerIndex;
        ContinueBtn.SetActive (true);
        SceneStateMachine.Inctance.CprSound.PlaySound ("click");
    }
    public void OnSelectContinue () {
        bool isAnswerTrue = false;
        // Check if the selected answer is correct
        if (_answerIndex == QuestionsData.questions[_currentQuestionIndex].CorrectAnswerIndex) {
            FeedbackText.text = "درست !";
            isAnswerTrue = true;
            SceneStateMachine.Inctance.CprSound.PlaySound ("true");
        } else {
            FeedbackText.text = "غلط !";
            SceneStateMachine.Inctance.CprSound.PlaySound ("false");
        }
        Debug.Log ("curect= " + QuestionsData.questions[_currentQuestionIndex].CorrectAnswerIndex +
            "answer= " + _answerIndex);

        SceneStateMachine.Inctance.ScoreCalculator.CheckAnswer (isAnswerTrue, _answerIndex);
        SceneStateMachine.Inctance.ContinueLastState ();
    }
    public void NextQuestion () {
        _currentQuestionIndex++;
        ContinueBtn.SetActive (false);
    }

    public void CallQuestion () {
        if (_currentQuestionIndex < QuestionsData.questions.Length) {
            ShowQuestion (_currentQuestionIndex);
        } else {
            FeedbackText.text = "Quiz Complete!";
            // You can add any logic here for finishing the quiz
        }
    }
}