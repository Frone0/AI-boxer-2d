using System;
using UnityEngine;

public class RewardCounter : MonoBehaviour
{
    [SerializeField] private GenerationManager _GenerationManager;
    [SerializeField] AIBrain _AIBrain;
    [SerializeField] GameObject _Body;
    public float _Reward = 0f;
    bool _ReachedTheFinish;
    float time = 0f;

    float MaxDistanceToFinish;

    GameObject finish;

    void Start()
    {
        finish = GenerationManager.Instance.Finish;

       MaxDistanceToFinish = Math.Abs(finish.transform.position.x - _Body.transform.position.x);
    }

    void Update()
    {
       time += Time.deltaTime;

       if(time > 15f)
       {
           _AIBrain.Died();
       }
    }

    public void TouchedTheFinish()
    {
         SetReward(); 
        _AIBrain.Died();
    }

    public void SetReward()
    {
        float distance = Math.Abs(finish.transform.position.x - _Body.transform.position.x);

        float RewardFromDistance = (MaxDistanceToFinish - distance) * 10;
        _Reward = time + RewardFromDistance + (_ReachedTheFinish ? 1000 : 0);
    }
}
