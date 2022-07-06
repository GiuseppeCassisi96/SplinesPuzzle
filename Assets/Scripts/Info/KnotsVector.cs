using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnotsVector
{
    public List<float> nodes = new List<float>();
    Dictionary<float, int> multiplicityDict = new Dictionary<float, int>();

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
            if (multiplicityDict[oldValue] == 1)
            {
                multiplicityDict.Remove(oldValue);
            }
            else if (multiplicityDict[oldValue] > 1)
            {
                multiplicityDict[oldValue]--;
            }
            nodes[index] = value;
        }
        
    }
}
