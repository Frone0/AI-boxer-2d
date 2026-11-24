using System;
using UnityEngine;

public class RewardCounter : MonoBehaviour
{
    [SerializeField] private GenerationManager _GenerationManager;
    [SerializeField] AIBrain _AIBrain;
    [SerializeField] GameObject _Body;
    [SerializeField] GameObject _Head;
    public float _Reward = 0f;
    [SerializeField] float RewardFromHeadUp;
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

        if(_Head.transform.position.y > 0f)
        {
            RewardFromHeadUp += 200f * Time.deltaTime;
        }
       else
        {
            RewardFromHeadUp -= 100f * Time.deltaTime;
        }

        if(time > 15f)
        {
            _AIBrain.Died();
        }
    }

    public void TouchedTheFinish()
    {
         SetReward(); 
         _ReachedTheFinish = true;
        _AIBrain.Died();
    }

    public void SetReward()
    {
        float distance = Math.Abs(finish.transform.position.x - _Body.transform.position.x);

        float RewardFromDistance = (MaxDistanceToFinish - distance) * 10;
        _Reward = RewardFromDistance + (_ReachedTheFinish ? (3000 - time * 80) : 0) + RewardFromHeadUp;
    }
}
