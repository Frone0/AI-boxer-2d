using UnityEngine;

public class Neuron
{
    public float _Weights;
    public float _Bias;
}
public class AIBrain : MonoBehaviour
{

    public Neuron[,] _Neurons;

    int InputsCount;

    int LayersCount;
    int NeironsPerLayer;
    void Start()
    {
        _Neurons = new Neuron[LayersCount, NeironsPerLayer];
 

        for (int i = 0; i < _Neurons.GetLength(0); i++)
        {
            for (int j = 0; j < _Neurons.GetLength(1); j++)
            {
                _Neurons[i, j] = new Neuron();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
