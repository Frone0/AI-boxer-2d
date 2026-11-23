using UnityEngine;

public class HeadCollisionCheck : MonoBehaviour
{

   [SerializeField] private AIBrain _Brain;
   void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.CompareTag("Floor"))
      {
      _Brain.Died();
      }

      if(collision.gameObject.CompareTag("Finish"))
      {
        _Brain._RewardCounter.TouchedTheFinish();
      }
   }
}
