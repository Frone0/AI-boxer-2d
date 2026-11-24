using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingTheNetwork : MonoBehaviour
{
    [SerializeField] GameObject NeuronVisualPref;
    [SerializeField] Transform StartPoint;

    InputsControl _Inputs;
    List<NeuronVizualizer> _InputVisuals = new List<NeuronVizualizer>();

    public void SetBrainForVisualization(AIBrain brain, InputsControl inputs)
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        _InputVisuals.Clear();
        _Inputs = inputs;

        for (int k = 0; k < inputs._Inputs.Length; k++)
        {
            Vector3 pos = new Vector3(StartPoint.position.x, StartPoint.position.y - k * 100 - 500, 0);
            GameObject obj = Instantiate(NeuronVisualPref, pos, transform.rotation, transform);
            _InputVisuals.Add(obj.GetComponent<NeuronVizualizer>());
        }

        
        Neuron[][] Network = brain._Neurons;
        for (int i = 0; i < Network.Length; i++)
        {
            float Xpos = StartPoint.position.x + (i + 1) * 450;
            float distancebetweenY = Math.Clamp(1500 / Network[i].Length,100,200);
            float YstartPointOffset = Math.Clamp(500 - Network[i].Length * 30,0,1000);
            for (int j = 0; j < Network[i].Length; j++)
            {
                float Ypos = StartPoint.position.y - j * distancebetweenY - YstartPointOffset;
                GameObject obj = Instantiate(NeuronVisualPref, new Vector3(Xpos, Ypos, 0), transform.rotation, transform);
                Network[i][j].SetVisualizer(obj.GetComponent<NeuronVizualizer>());
            }
        }
    }

    void Update()
    {
        if (_Inputs == null) return;


        for (int k = 0; k < _InputVisuals.Count && k < _Inputs._Inputs.Length; k++)
            if (_InputVisuals[k] != null)
                _InputVisuals[k].SetColor(_Inputs._Inputs[k]);
    }
}