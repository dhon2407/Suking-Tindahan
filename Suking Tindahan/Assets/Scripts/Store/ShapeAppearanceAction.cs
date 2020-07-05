using Player;
using Player.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Store
{
    [CreateAssetMenu(fileName = "Change to", menuName = "Action/Appearance Change Shape", order = 0)]
    public class ShapeAppearanceAction : BaseAppearanceAction
    {
        [EnumToggleButtons]
        public Shape shape;
        public override string Name => shape.ToString();
        
        public override void Execute(ShapePlayer player)
        {
            player.ChangeShape(shape);
        }

    }
}