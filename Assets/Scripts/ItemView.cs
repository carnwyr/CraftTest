using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace CraftTest
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        private ItemViewModel _viewModel;
        
        public void Initialize(ItemViewModel vm)
        {
            _viewModel = vm;

            _icon.sprite = _viewModel.Image;
            _viewModel.Amount
                .Subscribe(x => _count.text = x.ToString())
                .AddTo(this);
        }
    }
}