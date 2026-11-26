using System;

namespace CoffeeRush
{
    [Serializable]
    public sealed class Order
    {
        public string Id { get; }
        public RecipeData Recipe { get; }
        public float RemainingSeconds { get; private set; }
        public bool IsResolved { get; private set; }

        public Order(string id, RecipeData recipe, float patienceSeconds)
        {
            Id = id;
            Recipe = recipe;
            RemainingSeconds = patienceSeconds;
        }

        public void Tick(float deltaTime)
        {
            if (!IsResolved) RemainingSeconds = Math.Max(0f, RemainingSeconds - deltaTime);
        }

        public bool MarkResolved()
        {
            if (IsResolved) return false;
            IsResolved = true;
            return true;
        }
    }
}
