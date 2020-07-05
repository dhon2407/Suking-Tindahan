using Player;
using Player.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Store
{
    [CreateAssetMenu(fileName = "Change to", menuName = "Action/Appearance Change Color", order = 0)]
    public class ColorAppearanceAction : BaseAppearanceAction
    {
        [EnumToggleButtons]
        public SkinColor color;
        public override string Name => color.ToString();
        
        public override void Execute(ShapePlayer player)
        {
            player.ChangeSkinColor(color);
        }
    }
}