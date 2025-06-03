using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculator : MonoBehaviour {
    const int MaxScore = 100;

    [Tooltip ("score ui text")] public RTLTextMeshPro avrageMassage, avrageMassageSCore, handError, handErrorScore, totalTime, totalScore, successText;

    [SerializeField, Tooltip ("partions of micro scores")] private Transform massagePartsNumber, massagePartsSCore, quizPartsNumber, quizPartsScore;
    [SerializeField, Tooltip ("textures for showing right or wrong")] private Texture2D[] texture2D;

    private int _negativeScore = 0;
    private int _wrongHandNumber, _totalScoreNumber;
    private int _massagePartIndex, _quizPartIndex;
    private float numberOfMassage;

    readonly List<float> massagePartMassage = new ();

    /// <summary>
    /// the listr that holds user answers for each question
    /// </summary>
    public List<int> questionUserAnswers { get; private set; } = new ();

    /// <summary>
    /// update massage score calculation
    /// </summary>
    /// <param name="timer">time between each massage</param>
    public void UpdateMassageCalculator (float timer) {
        MassagePerMinutesCalcualtor (timer);
        MassagePerMinutesChecker (false);
        massagePartMassage.Add (numberOfMassage);
    }
    private void MassagePerMinutesCalcualtor (float timer) {
        numberOfMassage = HaertMassageState.massage_limit * 60 / timer;
    }

    private void MassagePerMinutesChecker (bool _isAvarage) {
        //is between standar time or not 120-100 times per 60 sec so for 30 times is this
        if (numberOfMassage > 130 || numberOfMassage < 90) {
            if (_isAvarage)
                SetAvarageText (false);
            else
                SetEachPartText (false);

            _negativeScore -= 5;
        } else {
            if (_isAvarage)
                SetAvarageText (true);
            else
                SetEachPartText (true);
        }
    }
    /// <summary>
    /// calculate avarage massage score 
    /// </summary>
    public void CalculateAvarageMassageScore () {
        float sum = 0;
        foreach (float item in massagePartMassage) {
            sum += item;
        }
        numberOfMassage = sum / 2;
        MassagePerMinutesChecker (true);
    }

    private void SetEachPartText (bool _isOk) {
        if (_isOk) {
            massagePartsNumber.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().color = Color.green;
            massagePartsSCore.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().color = Color.green;
            massagePartsSCore.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().text = "0";
        } else {
            massagePartsNumber.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().color = Color.red;
            massagePartsSCore.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().color = Color.red;
            massagePartsSCore.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().text = "-5";
        }
        massagePartsNumber.GetChild (_massagePartIndex).GetComponent<RTLTextMeshPro> ().text = Mathf.RoundToInt (numberOfMassage).ToString ();
        _massagePartIndex += 1;
    }

    private void SetAvarageText (bool _isOk) {
        if (_isOk) {
            avrageMassage.color = Color.green;
            avrageMassageSCore.color = Color.green;
            avrageMassageSCore.text = "0";
        } else {
            avrageMassageSCore.color = Color.red;
            avrageMassage.color = Color.red;
            avrageMassageSCore.text = "-5";
        }

        avrageMassage.text = Mathf.RoundToInt (numberOfMassage).ToString ();
    }

    /// <summary>
    /// update wrong massage hand and head position
    /// </summary>
    public void UpdateWrongMassage () {
        _wrongHandNumber += 1;
        _negativeScore -= 1;
    }

    /// <summary>
    /// check quiz answer and update texures and score text
    /// </summary>
    /// <param name="isRight"></param>
    /// <param name="answerIndex"></param>
    public void CheckAnswer (bool isRight, int answerIndex) {
        if (isRight) {
            quizPartsScore.GetChild (_quizPartIndex).GetComponent<RTLTextMeshPro> ().color = Color.green;
            quizPartsScore.GetChild (_quizPartIndex).GetComponent<RTLTextMeshPro> ().text = "0";
            quizPartsNumber.GetChild (_quizPartIndex).GetComponent<RawImage> ().texture = texture2D[0];
        } else {
            quizPartsScore.GetChild (_quizPartIndex).GetComponent<RTLTextMeshPro> ().color = Color.red;
            quizPartsScore.GetChild (_quizPartIndex).GetComponent<RTLTextMeshPro> ().text = "-5";
            quizPartsNumber.GetChild (_quizPartIndex).GetComponent<RawImage> ().texture = texture2D[1];
            _negativeScore -= 5;
        }
        _quizPartIndex += 1;
        questionUserAnswers.Add (answerIndex);
    }

    /// <summary>
    /// calculate total score
    /// </summary>
    public void CalculateTotalScore () {
        CalculateAvarageMassageScore ();
        _totalScoreNumber = MaxScore + _negativeScore;
        //if total score is above 70 pass else fail
    }

    /// <summary>
    /// show all calculated data in ui text and sounds 1-success 2-fail 3-hand error 4-total score 5-total time 6-success text 
    /// </summary>
    public void ShowAllData () {
        if (_wrongHandNumber > 0) {
            handError.color = Color.red;
            handErrorScore.color = Color.red;
        } else {
            handError.color = Color.green;
            handErrorScore.color = Color.green;
        }
        handError.text = _wrongHandNumber.ToString ();
        handErrorScore.text = -_wrongHandNumber + "";
        totalScore.text = _totalScoreNumber + "از 100";
        UpdateMissionTimeText (SceneStateMachine.Inctance.CountdownTimer.GetMissionTime ());
        if (_totalScoreNumber > 70) {
            SceneStateMachine.Inctance.CprSound.PlaySound ("success");
            successText.text = "موفق شدی";
            successText.color = Color.green;
        } else {
            SceneStateMachine.Inctance.CprSound.PlaySound ("fail");
            successText.text = "عملکرد خوبی نداشتی";
            successText.color = Color.red;
        }
    }
    void UpdateMissionTimeText (float missionTime) {
        // Format time to minutes:seconds
        int minutes = Mathf.FloorToInt (missionTime / 60F);
        int seconds = Mathf.FloorToInt (missionTime % 60F);
        totalTime.text = string.Format ("{0:00}:{1:00}", minutes, seconds);

    }
}