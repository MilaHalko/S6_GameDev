using System.Drawing;
using Items.Enum;
using Color = UnityEngine.Color;

namespace Items.Rarity
{
    public interface IItemRarityColor
    {
        ItemRarity ItemRarity { get; }
        Color Color { get; }
    }
}