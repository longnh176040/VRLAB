using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public static float timer = 15f;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
        timerText.text = Mathf.RoundToInt(timer).ToString();
        else timerText.text = "0";
    }
}
