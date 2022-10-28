using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace CraftTest
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private ItemView _itemPrefab;
        
        private InventoryViewModel _viewModel;
        private readonly IDictionary<string, ItemView> _views = new Dictionary<string, ItemView>();
        
        public void Initialize(InventoryViewModel vm)
        {
            _viewModel = vm;

            foreach (var itemVm in _viewModel.Items)
            {
                AddItem(itemVm.Key, itemVm.Value);
            }

            _viewModel.Items.ObserveAdd()
                .Subscribe(x => AddItem(x.Key, x.Value))
                .AddTo(this);

            _viewModel.Items.ObserveRemove()
                .Subscribe(x => RemoveItem(x.Key))
                .AddTo(this);
        }

        private void AddItem(string id, ItemViewModel itemVm)
        {
            var itemView = Instantiate(_itemPrefab, _itemContainer);
            itemView.Initialize(itemVm);
            _views.Add(id, itemView);
        }

        private void RemoveItem(string id)
        {
            Destroy(_views[id].gameObject);
            _views.Remove(id);
        }
    }
}