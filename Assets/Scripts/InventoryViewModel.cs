using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace CraftTest
{
    public class InventoryViewModel
    {
        private readonly ReactiveDictionary<string, ItemViewModel> _items = new ReactiveDictionary<string, ItemViewModel>();

        public IReadOnlyReactiveDictionary<string, ItemViewModel> Items => _items;

        public InventoryViewModel(InventoryData inventoryData)
        {
            foreach (var itemData in inventoryData.Items)
            {
                var item = new ItemViewModel(itemData.Id, itemData.Amount);
                _items.Add(itemData.Id, item);
            }
        }

        public bool TrySpendItems(List<ItemData> items)
        {
            if (items.Any(item => !_items.ContainsKey(item.Id) || _items[item.Id].Amount.Value < item.Amount))
            {
                return false;
            }
            
            foreach (var item in items)
            {
                _items[item.Id].Amount.Value -= item.Amount;
                if (_items[item.Id].Amount.Value == 0)
                {
                    _items.Remove(item.Id);
                }
            }

            return true;
        }

        public void AddItem(string itemId)
        {
            if (_items.ContainsKey(itemId))
            {
                _items[itemId].Amount.Value++;
            }
            else
            {
                var item = new ItemViewModel(itemId, 1);
                _items.Add(itemId, item);
            }
        }

        public ReactiveProperty<int> GetCurrentAmount(string itemId)
        {
            return !_items.ContainsKey(itemId) ? new ReactiveProperty<int>() : _items[itemId].Amount;
        }
    }
}