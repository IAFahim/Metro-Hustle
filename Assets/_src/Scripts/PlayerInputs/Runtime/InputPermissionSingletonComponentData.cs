using BovineLabs.Core.ObjectManagement;
using Unity.Entities;

namespace _src.Scripts.PlayerInputs.Runtime
{
    public struct InputPermissionSingletonComponentData : IComponentData
    {
        [ObjectCategories] public int CategoriesDirectionCurrentXZ;
        [ObjectCategories] public int OrCategoriesDirectionFixedXZ;
        [ObjectCategories] public int CategoriesDash;
        [ObjectCategories] public int CategoriesJump;
    }
}