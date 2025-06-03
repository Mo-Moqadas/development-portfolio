using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {
    public TextMeshProUGUI timerText;

    [SerializeField] private Image loadingImage;

    private float currentTime, missionTime;

    public bool IstimerStart { get; set; }
    public bool IsMissionTime { get; set; }
    public float CountdownTimeBase { get; set; } = 3f; // Time in seconds for the countdown

    void FixedUpdate () {
        if (IstimerStart) {
            if (currentTime > 0) {
                currentTime -= Time.fixedDeltaTime;
                UpdateTimerText ();

                if (currentTime <= 0) {
                    TimerEnd ();
                }
            }
        }

        if (IsMissionTime)
            missionTime += Time.fixedDeltaTime;
    }

    void UpdateTimerText () {
        // Format time to minutes:seconds
        int minutes = Mathf.FloorToInt (currentTime / 60F);
        int seconds = Mathf.FloorToInt (currentTime % 60F);
        timerText.text = string.Format ("{0:00}:{1:00}", minutes, seconds);
        loadingImage.fillAmount = currentTime / CountdownTimeBase;

    }
    /// <summary>
    /// set the starting number of seconds for the countdown timer and update the timer text.
    /// </summary>
    /// <param name="_countdownTime">the time we want to set</param>
    public void SetTimer (float _countdownTime) {
        currentTime = _countdownTime;
        CountdownTimeBase = _countdownTime;
        UpdateTimerText ();
    }

    /// <summary>
    /// get the current time of the countdown timer.
    /// </summary>
    /// <returns>the time</returns>
    public float GetCurentTime () => currentTime;

    /// <summary>
    /// get the whole procces of cpr time
    /// </summary>
    public float GetMissionTime () => missionTime;
    void TimerEnd () {
        currentTime = 0;
        timerText.text = "00:00"; // Ensure the timer shows 00:00 at the end
    }
}