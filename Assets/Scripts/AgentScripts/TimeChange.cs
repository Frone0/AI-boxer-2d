using UnityEngine;
using UnityEngine.UI;

public class TimeChange : MonoBehaviour
{
    public float TimeFlowSpeed;
    [SerializeField] Slider slider;
    public void ChangeTime()
    {
        TimeFlowSpeed = slider.value;
        UIManager.Instance.SetTimeSpeed(TimeFlowSpeed);

        Time.timeScale = TimeFlowSpeed;
    }
}
