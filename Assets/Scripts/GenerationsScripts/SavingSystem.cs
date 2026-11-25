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

        for(int i = 0;i < networkData.NeironsPerLayer.Length;i++)
        {
            for(int j = 0;j < networkData.NeironsPerLayer[i];j++)
            {   
             savedNetwork.layers[i].neurons[j].bias = GenerationManager.Instance.AllTimeBest.Neirons[i][j]._Bias;
             savedNetwork.layers[i].neurons[j].weights = (float[])GenerationManager.Instance.AllTimeBest.Neirons[i][j]._Weights.Clone();
            }
        }

        path = Path.Combine(Application.persistentDataPath, "best_brain.json");
        File.WriteAllText(path, JsonUtility.ToJson(savedNetwork, true));
    }
}
