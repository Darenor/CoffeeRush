using System.Collections.Generic;

namespace CoffeeRush
{
    public static class RecipeValidator
    {
        /// <summary>Order-independent multiset comparison: duplicates are counted correctly.</summary>
        public static bool IsCorrect(IReadOnlyList<IngredientType> selected, RecipeData recipe)
        {
            if (selected == null || recipe == null || recipe.Ingredients == null)
                return false;
            if (selected.Count != recipe.Ingredients.Length)
                return false;

            var counts = new Dictionary<IngredientType, int>();
            foreach (IngredientType item in recipe.Ingredients)
            {
                counts.TryGetValue(item, out int count);
                counts[item] = count + 1;
            }

            foreach (IngredientType item in selected)
            {
                if (!counts.TryGetValue(item, out int count) || count == 0)
                    return false;
                counts[item] = count - 1;
            }
            foreach (int count in counts.Values)
                if (count != 0) return false;
            return true;
        }
    }
}
