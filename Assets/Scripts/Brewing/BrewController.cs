
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeRush
{
    public sealed class BrewController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text ingredientsText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text activeCustomerText;
        [SerializeField] private TMP_Text activeOrderText;
        [SerializeField] private Button serveButton;

        private readonly List<IngredientType> selected =
            new List<IngredientType>();

        private OrderCardView selectedOrderCard;
        private bool isBrewing;
        private bool isReadyToServe;

        public IReadOnlyList<IngredientType> SelectedIngredients => selected;
        public bool IsBrewing => isBrewing;
        public bool IsReadyToServe => isReadyToServe;

        private void Start()
        {
            UpdateIngredientsUI();
            UpdateServeButton();

            if (resultText != null)
                resultText.text = string.Empty;
        }

        public void SetSelectedOrder(OrderCardView card)
        {
            if (isBrewing || isReadyToServe)
                return;

            selectedOrderCard = card;

            if (activeCustomerText != null)
                activeCustomerText.text = card != null
                    ? card.CustomerName
                    : "Заказ не выбран";

            if (activeOrderText != null)
                activeOrderText.text = card != null
                    ? card.RecipeName
                    : string.Empty;

            UpdateServeButton();
        }

        public void SelectIngredient(IngredientType ingredient)
        {
            if (isBrewing || isReadyToServe ||
                gameManager == null ||
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
            if (isBrewing || isReadyToServe ||
                selected.Count == 0)
                return;

            selected.RemoveAt(selected.Count - 1);
            UpdateIngredientsUI();
        }

        public void ClearIngredients()
        {
            if (isBrewing || isReadyToServe)
                return;

            selected.Clear();
            UpdateIngredientsUI();
        }

        public void BrewSelected()
        {
            if (isBrewing || isReadyToServe ||
                gameManager == null ||
                selectedOrderCard == null ||
                string.IsNullOrEmpty(selectedOrderCard.OrderId) ||
                selected.Count == 0)
                return;

            StartCoroutine(BrewRoutine(
                selectedOrderCard.OrderId));
        }

        private IEnumerator BrewRoutine(string orderId)
        {
            isBrewing = true;
            UpdateServeButton();

            if (resultText != null)
                resultText.text = "Готовим напиток...";

            float duration =
                gameManager.GetPreparationDuration(selected.Count);

            yield return new WaitForSeconds(duration);

            isBrewing = false;
            isReadyToServe = true;

            if (resultText != null)
                resultText.text = "Напиток готов! Можно отдавать.";

            UpdateServeButton();
        }

        public void ServePreparedOrder()
        {
            if (!isReadyToServe ||
                gameManager == null ||
                selectedOrderCard == null ||
                string.IsNullOrEmpty(selectedOrderCard.OrderId))
                return;

            string orderId = selectedOrderCard.OrderId;

            IngredientType[] ingredients = selected.ToArray();

            gameManager.ServeOrder(orderId, ingredients);

            selected.Clear();
            selectedOrderCard = null;
            isReadyToServe = false;

            if (activeCustomerText != null)
                activeCustomerText.text = "Заказ не выбран";

            if (activeOrderText != null)
                activeOrderText.text = string.Empty;

            UpdateIngredientsUI();

            if (resultText != null)
                resultText.text = "Заказ передан!";

            UpdateServeButton();
        }

        private void UpdateServeButton()
        {
            if (serveButton == null)
                return;

            serveButton.interactable = isReadyToServe;
        }

        private void UpdateIngredientsUI()
        {
            if (ingredientsText == null)
                return;

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
