using TMPro;
using UnityEngine;

namespace CoffeeRush
{
    public sealed class IngredientButton : MonoBehaviour
    {
        [SerializeField] private IngredientType ingredient;
        [SerializeField] private TMP_Text label;
        [SerializeField] private BrewController brewController;

        private void Start()
        {
            if (label != null) label.text = IngredientDisplayName(ingredient);
        }

        public void Select() => brewController?.SelectIngredient(ingredient);

        private static string IngredientDisplayName(IngredientType type)
        {
            switch (type)
            {
                case IngredientType.Coffee: return "Кофе";
                case IngredientType.Water: return "Вода";
                case IngredientType.Milk: return "Молоко";
                case IngredientType.MilkFoam: return "Молочная пена";
                case IngredientType.Tea: return "Чай";
                case IngredientType.Lemon: return "Лимон";
                case IngredientType.Chocolate: return "Шоколад";
                case IngredientType.Ice: return "Лёд";
                default: return type.ToString();
            }
        }
    }
}
