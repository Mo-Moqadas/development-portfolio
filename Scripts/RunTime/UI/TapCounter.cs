using RTLTMPro;
using UnityEngine;

public class TapCounter : MonoBehaviour
{
   // public RTLTextMeshPro TapCountText;
   [Tooltip("the arrow object that indecate the speed of massaging")] public Transform SpeedSignArrow;

    private float  _lastPushTime, _massageTime = 0f;  
    private bool _isMassgingTime, _isFirstTrigger;
    private float _barWidth;

    private void Start()
    {
      _isFirstTrigger=true;
       //temp solution
       _barWidth=3f;
       // barWidth = speedSingArrow.parent.GetComponent<Renderer>().bounds.size.x;
       
    }
    void FixedUpdate()
    {
        if (_isMassgingTime)
        {
            _massageTime += Time.fixedDeltaTime;
        }
    }
    
    /// <summary>
    /// sets massaging time on or off by pushing down or up for calculating speed
    /// </summary>
    /// <param name="isMassaging"></param>
    public void SetMassagingTime(bool isMassaging)
    {
        if (isMassaging)
        {
            //restart just once then it will restart on auto
            if (_isFirstTrigger)
                ResetCounter();
            else
                UpdateSpeedArrow();
        }
        else
        {
            _isFirstTrigger = true;
            _massageTime = 0;
            _lastPushTime=0;
        }

        _isMassgingTime = isMassaging;
      //  OnChestPushed();
    }

    private void UpdateSpeedArrow()
    {
        float betweenTime = _massageTime - _lastPushTime;
        float arrowXPosition = .55f;
        if (betweenTime >= 1)
        {
            arrowXPosition = _barWidth ;
        }
        else if (betweenTime <= 0)
        {
            arrowXPosition = -_barWidth ;
        }
        else
        {
            arrowXPosition = (betweenTime - .55f) * _barWidth;
        }
         
        SpeedSignArrow.localPosition = new(arrowXPosition, SpeedSignArrow.localPosition.y, SpeedSignArrow.localPosition.z);
        _lastPushTime = _massageTime;
    }
    private void ResetCounter()
    {
        // _tapCount = 0;
        // _elapsedTime = 0f;
        _isFirstTrigger = false;
    }
}