using Palapal.MainMenu;
using RTLTMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SummeryUiManager : MonoBehaviour
{
    [Header("Question UI settings")]
    public GameObject PanaelEndGame;
    public GameObject PanelQuizInfo;
    public RTLTextMeshPro QuestionText, FeedbackText, QuesTionNumber;
    [Tooltip("UI Buttons for each possible answer  ")] public Toggle[] AnswerButtons;

    [Header("Question Data")]
    [Tooltip("add scriptable object of questions data here")] public QuestionData questionsData;

    [SerializeField, Tooltip("textures for right or wrong answer icon")] private Texture2D[] _texture2D;

    private int _questionIndex;

    void Start()
    {
        if (!SceneStateMachine.Inctance.IsTest)
        {

            for (int i = 10; i < 13; i++)
                transform.GetChild(0).GetChild(i).gameObject.SetActive(false); //these are game objects for test section scenario summery
        }
        else
            ShowQuestion(_questionIndex);
    }
    public void OnbtnQuizInfoSelect()
    {
        SceneStateMachine.Inctance.CprSound.PlaySound("click");
        PanaelEndGame.SetActive(false);
        PanelQuizInfo.SetActive(true);
    }
    public void OnbtnScoreInfoSelect()
    {
        SceneStateMachine.Inctance.CprSound.PlaySound("click");
        PanaelEndGame.SetActive(true);
        PanelQuizInfo.SetActive(false);
    }
    public void OnbtnBackToMenuSelected()
    {
        SceneStateMachine.Inctance.CprSound.PlaySound("click");
        // SceneManager.LoadScene ("StartScene");
        ExitToMainMenu();
    }
    /// <summary>
    /// this s function is called when we wantto back to the main menu of firefightr game
    /// </summary>
    private void ExitToMainMenu()
    {
        if (OpenLevel.Instance)
            OpenLevel.Instance.LoadScene(0);
        else
            SceneManager.LoadSceneAsync(0);
    }
    public void OnNextQuestion()
    {
        if (_questionIndex < questionsData.questions.Length - 1)
            _questionIndex++;

        ShowQuestion(_questionIndex);
    }
    public void OnprevQuestionBtn()
    {
        if (_questionIndex > 0)
            _questionIndex--;

        ShowQuestion(_questionIndex);
    }
    private void ShowQuestion(int questionIndex)
    {
        // Set the question text
        Question question = questionsData.questions[questionIndex];
        QuesTionNumber.text = (questionIndex + 1).ToString();
        QuestionText.text = question.QuestionText;
        FeedbackText.text = question.QuestionReason;
        int userAnswerIndex = SceneStateMachine.Inctance.ScoreCalculator.questionUserAnswers[questionIndex];

        // Set the text for each answer button
        for (int i = 0; i < 4; i++)
        {
            AnswerButtons[i].GetComponentInChildren<RTLTextMeshPro>().text = question.Answers[i];
            //clean question signs
            AnswerButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            AnswerButtons[i].transform.GetChild(1).gameObject.SetActive(false); //hideSign

        }
        AnswerButtons[question.CorrectAnswerIndex].transform.GetChild(0).gameObject.SetActive(false);
        AnswerButtons[question.CorrectAnswerIndex].transform.GetChild(1).GetComponent<RawImage>().texture = _texture2D[0]; //green sign
        AnswerButtons[question.CorrectAnswerIndex].transform.GetChild(1).gameObject.SetActive(true); //showSign

        if (question.CorrectAnswerIndex != userAnswerIndex)
        {
            AnswerButtons[userAnswerIndex].transform.GetChild(0).gameObject.SetActive(false);
            AnswerButtons[userAnswerIndex].transform.GetChild(1).GetComponent<RawImage>().texture = _texture2D[1]; //red sign
            AnswerButtons[userAnswerIndex].transform.GetChild(1).gameObject.SetActive(true); //showSign
        }
    }
}