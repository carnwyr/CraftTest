using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CraftTest
{
    public class RecipeView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private RectTransform _ingredientContainer;
        [SerializeField] private Button _createButton;
        [SerializeField] private IngredientView _ingredientPrefab;
        [SerializeField] private GameObject _plusPrefab;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private RecipeViewModel _viewModel;
        
        public void Initialize(RecipeViewModel vm)
        {
            _viewModel = vm;

            _icon.sprite = _viewModel.Image;
            _name.text = _viewModel.RecipeData.Name;
            _price.text = _viewModel.RecipeData.Price.ToString();

            IObservable<bool> canCreateObservable = new ReactiveProperty<bool>(true);

            for (var i = 0; i < _viewModel.RecipeData.Ingredients.Count; i++)
            {
                if (i > 0)
                {
                    var plus = Instantiate(_plusPrefab, _ingredientContainer);
                    plus.gameObject.SetActive(true);
                }
                
                var ingredient = _viewModel.RecipeData.Ingredients[i];
                var ingredientInInventory = _viewModel.AmountGetter.Invoke(ingredient.Id);
                var ingredientVm = new IngredientViewModel(ingredient, ingredientInInventory);
                var ingredientView = Instantiate(_ingredientPrefab, _ingredientContainer);
                ingredientView.Initialize(ingredientVm);
                ingredientView.gameObject.SetActive(true);

                var enoughIngredient = ingredientInInventory.Select(x => x >= ingredient.Amount);
                canCreateObservable = canCreateObservable
                    .CombineLatest(enoughIngredient, (x, y) => x && y);
            }

            canCreateObservable
                .Subscribe(x => _createButton.interactable = x)
                .AddTo(this);

            _createButton.OnClickAsObservable()
                .Subscribe(_ => _viewModel.Create.Execute(Unit.Default))
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}