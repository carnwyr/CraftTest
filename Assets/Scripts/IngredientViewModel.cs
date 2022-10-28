using UniRx;
using UnityEngine;

namespace CraftTest
{
    public class IngredientViewModel
    {
        public Sprite Image { get; }
        public int Amount { get; }
        public ReactiveProperty<int> IngredientInInventory { get; }

        public IngredientViewModel(ItemData ingredientData, ReactiveProperty<int> ingredientInInventory)
        {
            Image = Resources.Load<Sprite>($"items/{ingredientData.Id}");
            Amount = ingredientData.Amount;
            IngredientInInventory = ingredientInInventory;
        }
    }
}