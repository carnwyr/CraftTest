using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CraftTest
{
    public class IngredientView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _counter;
        
        private IngredientViewModel _viewModel;
        
        public void Initialize(IngredientViewModel vm)
        {
            _viewModel = vm;

            _icon.sprite = _viewModel.Image;
            UpdateCountText(_viewModel.IngredientInInventory.Value, _viewModel.Amount);

            _viewModel.IngredientInInventory
                .Subscribe(x => UpdateCountText(x, _viewModel.Amount))
                .AddTo(this);
        }

        private void UpdateCountText(int currentValue, int targetValue)
        {
            if (currentValue < targetValue)
            {
                _counter.text = $"<color=\"red\">{currentValue}</color>/{targetValue}";
            }
            else
            {
                _counter.text = $"{currentValue}/{targetValue}";
            }
        }
    }
}