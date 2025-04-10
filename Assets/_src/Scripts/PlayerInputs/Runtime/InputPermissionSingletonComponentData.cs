using BovineLabs.Core.ObjectManagement;
using Unity.Entities;
using Unity.Mathematics;

namespace _Root.Scripts.MainInputProviders.Runtime
{
    public struct InputPermissionSingletonComponentData : IComponentData
    {
        [ObjectCategories] public int CategoriesDirectionCurrentXZ;
        [ObjectCategories] public int OrCategoriesDirectionFixedXZ;
        [ObjectCategories] public int CategoriesDash;
        [ObjectCategories] public int CategoriesJump;
    }
}