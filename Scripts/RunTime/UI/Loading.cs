using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] float step = 0.01f;
    [SerializeField] float delay = 0.01f;
    int executNumbers = 1000;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Animation());

    }
    IEnumerator Animation()
    {
        while (executNumbers>0)
        {
            executNumbers--;
            //Debug.Log(executNumbers);
            yield return new WaitForSeconds(delay);
            image.fillAmount += step;
            /*if (image.fillAmount >= 1 || image.fillAmount <= 0)
            {
                step *= -1;
                image.fillClockwise = !image.fillClockwise;
            }*/

        }
    }
}
