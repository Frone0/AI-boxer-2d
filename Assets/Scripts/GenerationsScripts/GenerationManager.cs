using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]int _CountOfAliveAgents = 0;
    [SerializeField] int _AgentsPerGeneration = 50;
    [SerializeField] private GameObject _AgentPrefab;

    public static GenerationManager Instance;

    public GameObject Finish;

    public List<AgentResult> results = new List<AgentResult>();

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
            if(_CurrentGeneration > 0)
            {
               if(i < 5)
               {
                   brain.SetNeurons(GetAmountByMutation(MutationsTypes.ExactlySame),results[0].Neirons);
               }
               else if(i < 20)
               {
                   brain.SetNeurons(GetAmountByMutation(MutationsTypes.LittleMutation),results[0].Neirons); 
               }
               else if(i < 30)
               {
                   brain.SetNeurons(GetAmountByMutation(MutationsTypes.MidMutation),results[0].Neirons); 
               }
                else if(i < 35)
               {
                   brain.SetNeurons(GetAmountByMutation(MutationsTypes.MidMutation),results[0].Neirons); 
               }
               else
               {
                   brain.SetRandomNeurons();
               }
            }
            else
            {
                brain.SetRandomNeurons();
            }


            newAgent.name = "Agent_" + _CurrentGeneration + "_" + i;
            _CountOfAliveAgents++;
        }


        _CurrentGeneration++;

        results.Clear();
    }

   float GetAmountByMutation(MutationsTypes type) => type switch
{
    MutationsTypes.ExactlySame => 0f,
    MutationsTypes.LittleMutation => 0.1f,
    MutationsTypes.MidMutation => 0.25f,
    MutationsTypes.StrongMutation => 0.4f,
    _ => 0f
};

    public void GenIsOver()
    {
        SortResults();
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
    
}
