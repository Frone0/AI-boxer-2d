using UnityEngine;
using UnityEngine.UI;

public class NeuronVizualizer : MonoBehaviour
{
    [SerializeField] Color _DarkColor = Color.black;
    [SerializeField] Color _BrightColor = Color.white;
    [SerializeField] Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetColor(float OutPutValue)
    {
        float t = (OutPutValue + 1f) * 0.5f;  
        image.color = Color.Lerp(_DarkColor, _BrightColor, t);
    }
}
