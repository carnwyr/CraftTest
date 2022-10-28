using UniRx;
using UnityEngine;

namespace CraftTest
{
    public class ItemViewModel
    {
        public Sprite Image { get; }
        public ReactiveProperty<int> Amount { get; }

        public ItemViewModel(string id, int amount)
        {
            Image = Resources.Load<Sprite>($"items/{id}");
            Amount = new ReactiveProperty<int>(amount);
        }
    }
}