using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRespawning : MonoBehaviour
{
    public GameObject parent;
    public TMP_Text timerText;
    public Image timerImg; 

    public void TriggerUIRespawning(float maxTime)
    {
        parent.SetActive(true);
        StartCoroutine(UIResp(maxTime));
    }

    IEnumerator UIResp(float time)
    {
        float t = time;
        while (t > 0)
        {
            t -= Time.fixedDeltaTime;

            timerText.text = Mathf.Round(t).ToString();
            timerImg.fillAmount = t / time;
            yield return new WaitForFixedUpdate();
        }

        Hide();
    }

    public void Hide()
    {
        parent.SetActive(false);
    }
}
