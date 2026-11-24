using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text _GenCounterTxT;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetGenNumber(int Number)
    {
        _GenCounterTxT.text = "Generation:" + Number.ToString();
    }
}
