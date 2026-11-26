using UnityEngine;
using System;
using System.IO;

[Serializable] public class SavedNeuron  { public float[] weights; public float bias; }
[Serializable] public class SavedLayer   { public SavedNeuron[] neurons; }
[Serializable] public class SavedNetwork
{
    public float reward;
    public int generation;
    public int inputCount;      // used to check the save still fits the prefab
    public SavedLayer[] layers; // replaces Neuron[][]
}


public class SavingSystem : MonoBehaviour
{
    string path;
    [SerializeField] NetworkData networkData;
    public void SaveAllTimeBest()
    {
        SavedNetwork savedNetwork = new SavedNetwork();
        savedNetwork.layers = new SavedLayer[networkData.NeironsPerLayer.Length];
        
        

        for(int i = 0;i < networkData.NeironsPerLayer.Length;i++)
        {
            savedNetwork.layers[i] = new SavedLayer();
            savedNetwork.layers[i].neurons = new SavedNeuron[networkData.NeironsPerLayer[i]]; 

            for(int j = 0;j < networkData.NeironsPerLayer[i];j++)
            {   
             savedNetwork.layers[i].neurons[j] = new SavedNeuron();
             savedNetwork.layers[i].neurons[j].bias = GenerationManager.Instance.AllTimeBest.Neirons[i][j]._Bias;
             savedNetwork.layers[i].neurons[j].weights = (float[])GenerationManager.Instance.AllTimeBest.Neirons[i][j]._Weights.Clone();
            }
        }

        savedNetwork.reward = GenerationManager.Instance.AllTimeBest.Reward;

        path = Path.Combine(Application.persistentDataPath, "best_brain.json");
        File.WriteAllText(path, JsonUtility.ToJson(savedNetwork, true));
    }

    public AgentResult LoadAllTimeBest()
    {
        SavedNetwork LoadedNetwork = new SavedNetwork();
        LoadedNetwork.layers = new SavedLayer[networkData.NeironsPerLayer.Length]; 
        path = Path.Combine(Application.persistentDataPath, "best_brain.json");


        if(!File.Exists(path))
        {
            return null;
        }

        string Json = File.ReadAllText(path);
        LoadedNetwork = JsonUtility.FromJson<SavedNetwork>(Json);

        Neuron[][] LoadedNeurons = new Neuron[LoadedNetwork.layers.Length][];

        for(int i = 0;i < networkData.NeironsPerLayer.Length;i++)
        {
            LoadedNeurons[i] = new Neuron[LoadedNetwork.layers[i].neurons.Length];

            for(int j = 0;j < networkData.NeironsPerLayer[i];j++)
            {   
             LoadedNeurons[i][j] = new Neuron();
             LoadedNeurons[i][j]._Bias = LoadedNetwork.layers[i].neurons[j].bias;
             LoadedNeurons[i][j]._Weights = (float[])LoadedNetwork.layers[i].neurons[j].weights.Clone();
            }
        }

        float reward = LoadedNetwork.reward;

        return new AgentResult{Neirons = LoadedNeurons,Reward = reward};

    }
}
