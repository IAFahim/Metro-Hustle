using BovineLabs.Core.ObjectManagement;
using Unity.Entities;
using UnityEngine;

namespace _Root.Scripts.MainInputProviders.Runtime
{
    public class InputPermissionSingletonComponentDataAuthoring : MonoBehaviour
    {
        [ObjectCategories] public int categoriesMoveCurrentXZ;
        [ObjectCategories] public int categoriesMoveXZ;
        [ObjectCategories] public int categoriesDash;
        [ObjectCategories] public int categoriesJump;

        public class InputPermissionSingletonComponentDataBaker : Baker<InputPermissionSingletonComponentDataAuthoring>
        {
            public override void Bake(InputPermissionSingletonComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new InputPermissionSingletonComponentData
                {
                    CategoriesDirectionCurrentXZ = authoring.categoriesMoveCurrentXZ,
                    OrCategoriesDirectionFixedXZ = authoring.categoriesMoveXZ,
                    CategoriesDash = authoring.categoriesDash,
                    CategoriesJump = authoring.categoriesJump
                });
            }
        }
    }
}