using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CraftTest
{
    public class GameManager : MonoBehaviour
    {
        private const string _inventoryDataPath = "inventoryData";
        private const string _recipeDataPath = "recipeData";

        [SerializeField] private Canvas _canvas;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private Button _alchemyButton;
        [SerializeField] private AlchemyView _alchemyViewPrefab;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private InventoryViewModel _inventoryViewModel;
        private AlchemyView _alchemyView;

        private void Start()
        {
            LoadInventory();

            _alchemyButton.OnClickAsObservable()
                .Subscribe(_ => OpenAlchemyWindow())
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void LoadInventory()
        {
            var inventoryJson = Resources.Load<TextAsset>(_inventoryDataPath);
            var inventoryData = JsonUtility.FromJson<InventoryData>(inventoryJson.text);
            _inventoryViewModel = new InventoryViewModel(inventoryData);
            _inventoryView.Initialize(_inventoryViewModel);
        }

        private void OpenAlchemyWindow()
        {
            var recipeJson = Resources.Load<TextAsset>(_recipeDataPath);
            var recipeData = JsonUtility.FromJson<RecipeCollectionData>(recipeJson.text);
            var alchemyViewModel = new AlchemyViewModel(recipeData, _inventoryViewModel.GetCurrentAmount);

            alchemyViewModel.Create
                .Subscribe(CreateItem)
                .AddTo(alchemyViewModel.Disposable);
            
            if (_alchemyView == null)
            {
                _alchemyView = Instantiate(_alchemyViewPrefab, _canvas.transform);
            }
            else
            {
                _alchemyView.gameObject.SetActive(true);
            }
            
            _alchemyView.Initialize(alchemyViewModel);
        }

        private void CreateItem(RecipeData recipeData)
        {
            if (_inventoryViewModel.TrySpendItems(recipeData.Ingredients))
            {
                _inventoryViewModel.AddItem(recipeData.Id);
            }
        }
    }
}