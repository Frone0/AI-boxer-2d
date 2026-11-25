using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AgentResult
{
    public float Reward;
    public Neuron[][] Neirons;
}

public enum MutationsTypes
{
    ExactlySame,LittleMutation,MidMutation,StrongMutation,AllTimeBest
}

public class GenerationManager : MonoBehaviour
{

    [SerializeField] int _CurrentGeneration = 0;
    [SerializeField] SettingTheNetwork settingTheNetwork;
    [SerializeField]int _CountOfAliveAgents = 0;
    [SerializeField] int _AgentsPerGeneration = 50;
    [SerializeField] private GameObject _AgentPrefab;

    public static GenerationManager Instance;

    public GameObject Finish;

    public List<AgentResult> results = new List<AgentResult>();

    public AgentResult AllTimeBest;

    public bool TestModeForNextGen;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNewGeneration();
    }

    public void StartNewGeneration()
    {   
        for(int i = 0; i < _AgentsPerGeneration; i++)
        {
             GameObject newAgent = SpawnAgent();
             AIBrain brain = newAgent.GetComponent<AIBrain>();
             MutationsTypes typeOfMutation = MutationsTypes.ExactlySame;
           
            if(i >= 35 || _CurrentGeneration == 0)
            {
              brain.SetRandomNeurons();
            }
            else
            {
              typeOfMutation =  GetMutationsTypeByID(i);
              brain.SetNeurons(GetAmountByMutation(typeOfMutation),typeOfMutation == MutationsTypes.AllTimeBest ? AllTimeBest.Neirons : results[0].Neirons);
            }

            if(i == 1)
            {
              SetAgentForVizualization(brain);
              brain.DebugData = true;
            }

            newAgent.name = "Agent_" + _CurrentGeneration + "_" + i + "_" + GetMutationName(typeOfMutation);
        }

        _CurrentGeneration++;
        results.Clear();
        UpdateUi();
    }

    GameObject SpawnAgent()
    {
        _CountOfAliveAgents++;
        return Instantiate(_AgentPrefab, transform.position, Quaternion.identity);
    }

    void SetAgentForVizualization(AIBrain brain)
    {
        settingTheNetwork.SetBrainForVisualization(brain,brain._InputsControl);
    }

    void StartTestMode()
    {
            GameObject newAgent = SpawnAgent();
            AIBrain brain = newAgent.GetComponent<AIBrain>();
            brain.DebugData = true;
            brain.SetNeurons(GetAmountByMutation(MutationsTypes.ExactlySame),AllTimeBest.Neirons);

            SetAgentForVizualization(brain);

            TestModeForNextGen = false;
    }

   float GetAmountByMutation(MutationsTypes type) => type switch
    {
    MutationsTypes.ExactlySame => 0f,
    MutationsTypes.LittleMutation => 0.05f,
    MutationsTypes.MidMutation => 0.1f,
    MutationsTypes.StrongMutation => 0.15f,
    MutationsTypes.AllTimeBest => 0f,
    _ => 0f
    };

    string GetMutationName(MutationsTypes type) => type switch
    {
         MutationsTypes.ExactlySame => "PreviusLeader",
         MutationsTypes.LittleMutation => "LittleMut",
         MutationsTypes.MidMutation => "MidMut",
         MutationsTypes.StrongMutation => "StrongMut",
         MutationsTypes.AllTimeBest => "AllTimeBest",
         _ => "Null"
    };

    MutationsTypes GetMutationsTypeByID(int i)
    {
       MutationsTypes type = MutationsTypes.AllTimeBest;

       type = i < 3 ? MutationsTypes.AllTimeBest :
        i < 5 ? MutationsTypes.ExactlySame :
        i < 15 ? MutationsTypes.LittleMutation : 
        i < 25 ? MutationsTypes.MidMutation : 
        i < 35 ? MutationsTypes.StrongMutation : 
        MutationsTypes.ExactlySame;

        return type;
    }

    public void GenIsOver()
    {
       SortResults();

       if (AllTimeBest == null || AllTimeBest.Neirons == null || results[0].Reward > AllTimeBest.Reward)
           AllTimeBest = results[0];
        
       if(TestModeForNextGen)
        {
            StartTestMode();
            return;
        }
        
        StartNewGeneration();
    }

    void SortResults()
    {
        results.Sort((a, b) => b.Reward.CompareTo(a.Reward));
    }

    public void AgentDied(float reward,AIBrain brain)
    {
        results.Add( new AgentResult{Reward = reward, Neirons =  GetNeironsFromBrain(brain)});
        _CountOfAliveAgents--;

        if(_CountOfAliveAgents <= 0)
        {
           GenIsOver();  
        }
    }

    public Neuron[][] GetNeironsFromBrain(AIBrain brain)
    {
        Neuron[][] CopyNeirons = new Neuron[brain.networkData.NeironsPerLayer.Length][];

        for(int i = 0; i < brain.networkData.NeironsPerLayer.Length; i++)
        {
            CopyNeirons[i] = new Neuron[brain._Neurons[i].Length];

            for (int j = 0; j < brain._Neurons[i].Length; j++)
            {
                CopyNeirons[i][j] = brain._Neurons[i][j].Clone();
            }
        }

        return CopyNeirons;
    }

    void UpdateUi()
    {
        UIManager.Instance.SetGenNumber(_CurrentGeneration);
    }
}
