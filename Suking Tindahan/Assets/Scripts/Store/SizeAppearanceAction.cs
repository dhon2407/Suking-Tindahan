using Helpers;
using Player;
using Player.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Store
{
    [CreateAssetMenu(fileName = "Change to", menuName = "Action/Appearance Change Size", order = 0)]
    public class SizeAppearanceAction : BaseAppearanceAction
    {
        [EnumToggleButtons]
        public Size size;
        public override string Name => size.ToNiceString();
        
        public override void Execute(ShapePlayer player)
        {
            player.ChangeSize(size);
        }
    }
}