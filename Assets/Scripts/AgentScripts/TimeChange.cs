using UnityEngine;

public class TimeChange : MonoBehaviour
{
   public float TimeFlowSpeed;

   void FixedUpdate()
    {
        Time.timeScale = TimeFlowSpeed;
    }
}
