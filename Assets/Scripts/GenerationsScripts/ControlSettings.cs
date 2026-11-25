using UnityEngine;

public class ControlSettings : MonoBehaviour
{

    public void SpawnOnlyBestAgent()
    {
        GenerationManager.Instance.TestModeForNextGen = true;
    }
}
