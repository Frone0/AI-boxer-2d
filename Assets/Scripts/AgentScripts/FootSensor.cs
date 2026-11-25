using UnityEngine;

public class FootSensor : MonoBehaviour
{
    [SerializeField] AIBrain aIBrain;
    [SerializeField] bool IsRight;

void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Floor"))
    {
       aIBrain._InputsControl.SetFootSensorData(true,IsRight);
    }
}

void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Floor"))
    {
       aIBrain._InputsControl.SetFootSensorData(false,IsRight);
    }
}
}
