using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnotsVector
{
    public List<float> nodes
    {
        get => _nodes;
    }

    public Dictionary<float, int> multiplicityDict
    {
        get => _multiplicityDict;
    }


    List<float> _nodes = new List<float>();
    Dictionary<float, int> _multiplicityDict = new Dictionary<float, int>();

    public void Add(float value)
    {
        List<float> dictKeys = new List<float>(multiplicityDict.Keys);
        if(dictKeys.Contains(value))
        {
            multiplicityDict[value]++;
        }
        else
        {
            multiplicityDict.Add(value, 1);
        }

        nodes.Add(value); 
    }

    public void Substituite(float value, int index)
    {
        float oldValue = nodes[index];
        if(value != oldValue)
        {
            List<float> keys = new List<float>(multiplicityDict.Keys);
            if (multiplicityDict[oldValue] == 1)
            {
                multiplicityDict.Remove(oldValue);
            }
            else if (multiplicityDict[oldValue] > 1)
            {
                multiplicityDict[oldValue]--;
            }

            if(keys.Contains(value))
            {
                multiplicityDict[value]++;
            }
            else
            {
                multiplicityDict.Add(value, 1);
            }
            nodes[index] = value;
        }
        
    }
}
