using System;
using System.Collections.Generic;

namespace CraftTest
{
    [Serializable]
    public struct RecipeData
    {
        public string Id;
        public List<ItemData> Ingredients;
        public int Price;
        public string Name;
    }
}