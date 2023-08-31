using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomizerStatus", menuName = "ScriptableObjects/Randomizer Status Object", order = 1)]
public class RandomizerStatus : ScriptableObject
{
    [SerializeField] public List<RandomizerMap> blockChances;
}

[Serializable]
public class RandomizerMap
{
    [SerializeField] public BlockTypeEnum type;
    [SerializeField] [Range(0.0f,1.0f)]public float chance;
}
