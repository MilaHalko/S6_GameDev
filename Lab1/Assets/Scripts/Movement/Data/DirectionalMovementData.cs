using System;
using Core.Enum;
using UnityEngine;

namespace Movement.Data
{
    [Serializable]
    public class DirectionalMovementData
    {
        // TODO:
        // [field: SerializeField] public float SlideSpeed { get; private set; }
        // [field: SerializeField] public float SpeedWhileAttack { get; private set; }
        
        [field: SerializeField] public Direction Direction { get; private set; }
        [field: SerializeField] public float MinSize { get; private set; }
        [field: SerializeField] public float MaxSize { get; private set; }
        [field: SerializeField] public float MinVerticalPosition { get; private set; }
        [field: SerializeField] public float MaxVerticalPosition { get; private set; }
    }
}