using System;
using UnityEngine;

namespace Core.Movement.Data
{
    [Serializable]
    public class JumpData
    {
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public int GravityScale { get; private set; }
        [field: SerializeField] public SpriteRenderer Shadow { get; private set; }
        [field: SerializeField] public float ShadowSizeModificator { get; private set; }
        [field: SerializeField] public float ShadowAlphaModificator { get; private set; }
    }
}