using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RewardCounter : MonoBehaviour
{
    [SerializeField] private GenerationManager _GenerationManager;
    [SerializeField] AIBrain _AIBrain;
    [SerializeField] GameObject _Body;
    [SerializeField] GameObject _Head;
    public float _Reward = 0f;
    [SerializeField] float RewardFromHeadUp;
    [SerializeField] float RewardFromFeet;
    float rewardFortime;
    bool _ReachedTheFinish;
    float time = 0f;

    float MaxDistanceToFinish;

    GameObject finish;

    void Start()
    {
       finish = GenerationManager.Instance.Finish;
       MaxDistanceToFinish = Math.Abs(finish.transform.position.x - _Body.transform.position.x);

       PosXInLastStep = _Body.transform.position.x;
    }

    int LastFoot;
    float PosXInLastStep;

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        if(_Head.transform.position.y > 0f)
        {
            RewardFromHeadUp += 200f * Time.fixedDeltaTime;
        }
        else
        {
            RewardFromHeadUp -= 100f * Time.fixedDeltaTime;
        }

        if(_Body.transform.rotation.z < -80f)
        {
            RewardFromHeadUp -= 300 * Time.fixedDeltaTime;
        }

        
        bool L = _AIBrain._InputsControl.LeftFootOnFloor;
        bool R = _AIBrain._InputsControl.RightFootOnFloor;

        int CurrentFoot = (L && !R) ? -1 : (R && !L) ? 1 : 0;

        if(LastFoot != CurrentFoot && CurrentFoot != 0)
        {
            float x = _Body.transform.position.x;
            if(x - PosXInLastStep > 0.3f)
                RewardFromFeet += 300;
                PosXInLastStep = x;

            LastFoot = CurrentFoot;
        }



        if(!L && !R)
        {
            RewardFromFeet -= 300f * Time.fixedDeltaTime;
        }

        if(time > 15f)
        {
            _AIBrain.Died();
            rewardFortime -= 1000;

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

        float RewardFromDistance = (MaxDistanceToFinish - distance) * 100;
        _Reward = RewardFromDistance + (_ReachedTheFinish ? (4000 - time * 150) : 0) + RewardFromHeadUp + RewardFromFeet + rewardFortime; 

        if(_AIBrain.DebugData)
        {
            Debug.Log(_Reward);
        }

    }
}
