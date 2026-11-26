using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoffeeRush
{
    public sealed class BrewController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text ingredientsText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text activeCustomerText;
        [SerializeField] private TMP_Text activeOrderText;

        private readonly List<IngredientType> selected =
            new List<IngredientType>();

        private OrderCardView selectedOrderCard;
        private bool isBrewing;

        public IReadOnlyList<IngredientType> SelectedIngredients => selected;
        public bool IsBrewing => isBrewing;

        public void SetSelectedOrder(OrderCardView card)
        {
            if (isBrewing) return;

            selectedOrderCard = card;

            if (activeCustomerText != null)
                activeCustomerText.text = card != null
                    ? card.CustomerName
                    : "Заказ не выбран";

            if (activeOrderText != null)
                activeOrderText.text = card != null
                    ? card.RecipeName
                    : string.Empty;
        }

        public void SelectIngredient(IngredientType ingredient)
        {
            if (isBrewing || gameManager == null ||
                gameManager.State != GameState.Playing)
                return;

            selected.Add(ingredient);
            UpdateIngredientsUI();

            if (resultText != null)
                resultText.text = string.Empty;

            GameEvents.RaiseNotification(
                $"Добавлен ингредиент: {ingredient}");
        }

        public void RemoveLastIngredient()
        {
            if (isBrewing || selected.Count == 0) return;

            selected.RemoveAt(selected.Count - 1);
            UpdateIngredientsUI();
        }

        public void ClearIngredients()
        {
            if (isBrewing) return;

            selected.Clear();
            UpdateIngredientsUI();
        }

        public void BrewSelected()
        {
            if (isBrewing || gameManager == null ||
                selectedOrderCard == null ||
                string.IsNullOrEmpty(selectedOrderCard.OrderId) ||
                selected.Count == 0)
                return;

            StartCoroutine(BrewRoutine(selectedOrderCard.OrderId));
        }

        private IEnumerator BrewRoutine(string orderId)
        {
            isBrewing = true;

            if (resultText != null)
                resultText.text = "Готовим напиток...";

            float duration =
                gameManager.GetPreparationDuration(selected.Count);

            yield return new WaitForSeconds(duration);

            gameManager.ServeOrder(orderId, selected.ToArray());

            selected.Clear();
            selectedOrderCard = null;

            UpdateIngredientsUI();

            if (resultText != null)
                resultText.text = "Приготовление завершено!";

            isBrewing = false;
        }

        private void UpdateIngredientsUI()
        {
            if (ingredientsText == null) return;

            if (selected.Count == 0)
            {
                ingredientsText.text = "Ингредиенты не выбраны";
                return;
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < selected.Count; i++)
                builder.AppendLine($"{i + 1}. {selected[i]}");

            ingredientsText.text = builder.ToString();
        }
    }
}