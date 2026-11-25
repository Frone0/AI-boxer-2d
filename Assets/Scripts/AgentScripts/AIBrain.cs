using System;
using UnityEngine;

public class AIBrain : MonoBehaviour
{  
    [Header("Links")]
    [SerializeField] public InputsControl _InputsControl;
    [SerializeField] private BodyPartsControl _BodyPartsControl;
    [SerializeField] private BodyPartsContainer _BodyPartsContainer;
    [SerializeField] public RewardCounter _RewardCounter;
    [SerializeField] private GenerationManager _GenerationManager;

    bool died;

    public bool DebugData;

    [Header("Neurons Settings")]
    //Neirons
    public Neuron[][] _Neurons;

    public NetworkData networkData;

    public void SetRandomNeurons()
    {
        _Neurons = new Neuron[networkData.NeironsPerLayer.Length][];

        networkData.NeironsPerLayer[networkData.NeironsPerLayer.Length - 1] = _BodyPartsContainer._BodyParts.Length;
        
        for (int i = 0; i < networkData.NeironsPerLayer.Length; i++)
        {
            _Neurons[i] = new Neuron[networkData.NeironsPerLayer[i]];
        }

        for (int i = 0; i < networkData.NeironsPerLayer.Length; i++)
        {
            for (int j = 0; j < _Neurons[i].Length; j++)
            {
                _Neurons[i][j] = new Neuron();
                int WeightsCount = (i == 0) ? _InputsControl._Inputs.Length : networkData.NeironsPerLayer[i - 1];
                _Neurons[i][j].InitializeWeights(WeightsCount);
            }
        }
    }

    public void SetNeurons(float amount, Neuron[][] Parent)
{
    _Neurons = new Neuron[Parent.Length][];

    for (int i = 0; i < Parent.Length; i++)
    {
        _Neurons[i] = new Neuron[Parent[i].Length];

        for (int j = 0; j < _Neurons[i].Length; j++)
        {
            _Neurons[i][j] = new Neuron();               
            _Neurons[i][j].BecameSameNeuron(Parent[i][j]);
            _Neurons[i][j].Mutate(amount);
        }
    }
}

    void FixedUpdate() {PassForward(_InputsControl._Inputs, 0); }

    public void Died()
    {
        if(died) return;
        died = true;
        _RewardCounter.SetReward();
        GenerationManager.Instance.AgentDied(_RewardCounter._Reward,this);
        Destroy(gameObject);
    }


    public float TanhActivation(float input)
    {
        return (float)Math.Tanh(input);
    }


    public void PassForward(float[] input, int LayerIndex)
    {
       float[] outputs = new float[_Neurons[LayerIndex].Length];
    
       for(int i = 0; i < _Neurons[LayerIndex].Length; i++)
       {
       outputs[i] = _Neurons[LayerIndex][i].GetOutput(input);
       }       

       if(LayerIndex != _Neurons.Length - 1)
         {
         PassForward(outputs, LayerIndex + 1);
         }
         else
         {
         SetOutput(outputs);  
         }
    }

    public void SetOutput(float[] output)
    {
        _BodyPartsControl.Apply(output);
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
