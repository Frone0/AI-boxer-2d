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
    ExactlySame,LittleMutation,MidMutation,StrongMutation
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
            GameObject newAgent = Instantiate(_AgentPrefab, transform.position, Quaternion.identity);
             AIBrain brain = newAgent.GetComponent<AIBrain>();
             MutationsTypes typeOfMutation = MutationsTypes.ExactlySame;
            if(_CurrentGeneration > 0)
            {
               if(i < 5)
               {
                 typeOfMutation = MutationsTypes.ExactlySame;
               }
               else if(i < 20)
               {
                  typeOfMutation = MutationsTypes.LittleMutation;
               }
               else if(i < 30)
               {
                  typeOfMutation = MutationsTypes.MidMutation;
               }
                else if(i < 35)
               {
                    typeOfMutation = MutationsTypes.StrongMutation;
               }
               else
               {
                   brain.SetRandomNeurons();
               }

                brain.SetNeurons(GetAmountByMutation(MutationsTypes.MidMutation),results[0].Neirons);
            }
            else
            {
                brain.SetRandomNeurons();
            }

            if(i == 1)
            {
                settingTheNetwork.SetBrainForVisualization(brain,newAgent.GetComponent<InputsControl>());
            }


            newAgent.name = "Agent_" + _CurrentGeneration + "_" + i + "_" + GetMutationName(typeOfMutation);
            _CountOfAliveAgents++;
        }


        _CurrentGeneration++;

        results.Clear();
        UpdateUi();
    }

   float GetAmountByMutation(MutationsTypes type) => type switch
{
    MutationsTypes.ExactlySame => 0f,
    MutationsTypes.LittleMutation => 0.05f,
    MutationsTypes.MidMutation => 0.1f,
    MutationsTypes.StrongMutation => 0.15f,
    _ => 0f
};

    string GetMutationName(MutationsTypes type) => type switch
    {
         MutationsTypes.ExactlySame => "PreviusLeader",
         MutationsTypes.LittleMutation => "LittleMut",
         MutationsTypes.MidMutation => "MidMut",
         MutationsTypes.StrongMutation => "StrongMut",
         _ => "Null"
    };

    public void GenIsOver()
    {
        SortResults();

        if(results[0].Reward > AllTimeBest.Reward)
        {
            AllTimeBest = results[0];
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
        Neuron[][] CopyNeirons = new Neuron[brain.NeironsPerLayer.Length][];

        for(int i = 0; i < brain.NeironsPerLayer.Length; i++)
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
