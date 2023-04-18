using System;
using Items.Enum;
using Items.Rarity;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Items.Data
{
    [Serializable]
    public class RarityDescriptor : IItemRarityColor
    {
        [field: SerializeField] public ItemRarity ItemRarity { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public UnityEngine.Color Color { get; private set; }
    }
}