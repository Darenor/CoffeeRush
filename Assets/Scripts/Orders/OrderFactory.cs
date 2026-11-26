using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeRush
{
    public sealed class OrderFactory
    {
        private readonly System.Random random = new System.Random();

        public Order Create(IReadOnlyList<RecipeData> recipes, float patienceSeconds)
        {
            if (recipes == null || recipes.Count == 0)
                throw new InvalidOperationException("OrderFactory requires at least one recipe.");
            RecipeData recipe = recipes[random.Next(recipes.Count)];
            return new Order(Guid.NewGuid().ToString("N"), recipe, patienceSeconds);
        }
    }
}
