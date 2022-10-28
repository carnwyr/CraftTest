using System;
using UniRx;
using UnityEngine;

namespace CraftTest
{
    public class RecipeViewModel
    {
        public RecipeData RecipeData { get; }
        public Sprite Image { get; }
        public ReactiveCommand<Unit> Create { get; } = new ReactiveCommand<Unit>();

        public Func<string, ReactiveProperty<int>> AmountGetter { get; }

        public RecipeViewModel(RecipeData recipeData, Func<string, ReactiveProperty<int>> amountGetter)
        {
            RecipeData = recipeData;
            Image = Resources.Load<Sprite>($"items/{RecipeData.Id}");
            AmountGetter = amountGetter;
        }
    }
}