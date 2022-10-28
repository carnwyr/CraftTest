using System;
using System.Collections.Generic;
using UniRx;

namespace CraftTest
{
    public class AlchemyViewModel : IDisposable
    {
        private readonly List<RecipeViewModel> _recipes = new List<RecipeViewModel>();

        public CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveCommand<RecipeData> Create { get; } = new ReactiveCommand<RecipeData>();

        public IReadOnlyList<RecipeViewModel> Recipes => _recipes;
        
        public AlchemyViewModel(RecipeCollectionData recipeData, Func<string, ReactiveProperty<int>> amountGetter)
        {
            foreach (var recipe in recipeData.Recipes)
            {
                var recipeVm = new RecipeViewModel(recipe, amountGetter);
                _recipes.Add(recipeVm);

                recipeVm.Create
                    .Subscribe(x => Create.Execute(recipe))
                    .AddTo(Disposable);
            }
        }

        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}