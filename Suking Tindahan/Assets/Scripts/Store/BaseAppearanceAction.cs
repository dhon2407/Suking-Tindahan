using Player;
using UnityEngine;

namespace Store
{
    public abstract class BaseAppearanceAction : ScriptableObject, IAppearanceAction
    {
        public abstract string Name { get; }
        public abstract void Execute(ShapePlayer player);
    }
}