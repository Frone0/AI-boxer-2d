using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text _GenCounterTxT;
    [SerializeField] TMP_Text _TimeSpeedTxT;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetGenNumber(int Number)
    {
        _GenCounterTxT.text = "Generation:" + Number.ToString();
    }

    public void SetTimeSpeed(float Number)
    {
        float Rounded = Mathf.Round(Number * 10) / 10f;
        _TimeSpeedTxT.text = "Time Speed:" + Rounded.ToString() + "x";
    }
}
