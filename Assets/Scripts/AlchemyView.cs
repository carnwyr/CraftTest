using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CraftTest
{
    public class AlchemyView: MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _recipesContainer;
        [SerializeField] private RecipeView _recipePrefab;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly List<RecipeView> _recipes = new List<RecipeView>();
        
        private AlchemyViewModel _viewModel;
        
        public void Initialize(AlchemyViewModel vm)
        {
            _viewModel = vm;

            foreach (var recipe in _viewModel.Recipes)
            {
                var recipeView = Instantiate(_recipePrefab, _recipesContainer);
                recipeView.Initialize(recipe);
                _recipes.Add(recipeView);
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
            foreach (var recipe in _recipes)
            {
                Destroy(recipe.gameObject);
            }
            _recipes.Clear();
            _viewModel.Dispose();
            _viewModel = null;
        }

        private void Start()
        {
            _closeButton.OnClickAsObservable()
                .Subscribe(_ => Close())
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}