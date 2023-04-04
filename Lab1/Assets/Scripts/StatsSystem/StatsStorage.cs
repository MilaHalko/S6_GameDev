using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu(fileName = "StatsStorage", menuName = "Stats")]
    public class StatsStorage : ScriptableObject
    {
        [field: SerializeField] public List<Stat> Stats { get; private set; }
    }
}