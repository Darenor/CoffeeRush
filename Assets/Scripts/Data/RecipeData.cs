using UnityEngine;

namespace CoffeeRush
{
    [CreateAssetMenu(fileName = "Recipe_New", menuName = "Coffee Rush/Recipe")]
    public sealed class RecipeData : ScriptableObject
    {
        [SerializeField] private string recipeId;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private IngredientType[] ingredients = new IngredientType[2];
        [SerializeField, Min(1)] private int baseReward = 20;
        [SerializeField, Min(0.1f)] private float preparationSeconds = 2f;

        public string RecipeId => recipeId;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public IngredientType[] Ingredients => ingredients;
        public int BaseReward => baseReward;
        public float PreparationSeconds => preparationSeconds;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(recipeId))
                recipeId = name.Trim().ToLowerInvariant().Replace(" ", "_");
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = name;
            if (ingredients == null)
                ingredients = new IngredientType[2];
        }
#endif
    }
}
