using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text timeText;

    void Start()
    {
        ResetTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(CountTimer());
    }

    IEnumerator CountTimer()
    {
        float t = 0f;
        while (true)
        {
            t += Time.fixedDeltaTime;

            int s = Mathf.FloorToInt(t % 60);
            int m = Mathf.FloorToInt(t / 60);

            timeText.text = string.Format("{0:00}:{1:00}", m, s);

            yield return new WaitForFixedUpdate();
        }
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        timeText.text = "00:00";
    }

}
