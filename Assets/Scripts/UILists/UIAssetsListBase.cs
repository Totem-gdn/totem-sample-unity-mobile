using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TotemDemo;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetsListBase<T> : MonoBehaviour
{
    [Min(0.1f)]
    [SerializeField] private float itemSwitchingDuration = 1f;
    [SerializeField] private TotemAssetType totemAssetType;
    [SerializeField] private UILegaciesList legaciesList;

    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject uiItemPrefab;

    [SerializeField] private Button nextItemButton;
    [SerializeField] private Button previousItemButton;

    private int _currentItemIndex = 0;
    private float _distanceBetweenItems = 0f;

    private List<UIItemBase<T>> _uiItems = new List<UIItemBase<T>>();

    private void Awake()
    {
        CalculateDistanceBetweenItems();
    }

    /// <summary>
    /// Calculates static distance between closest UI elements in the assets list
    /// </summary>
    private void CalculateDistanceBetweenItems()
    {
        var itemsSpasing = contentTransform.GetComponent<HorizontalLayoutGroup>().spacing;
        var itemWidth = uiItemPrefab.GetComponent<RectTransform>().rect.width;

        _distanceBetweenItems = itemsSpasing + itemWidth;
    }

    /// <summary>
    /// Animation for asset switching in the list
    /// </summary>
    /// <param name="selectedItemIndex">Index of a selected item</param>
    private void SwitchAssetAnim(int selectedItemIndex)
    {
        float animationDistance = (_currentItemIndex - selectedItemIndex) * _distanceBetweenItems;
        Vector2 contentEndPoint = new Vector2(contentTransform.localPosition.x + animationDistance, contentTransform.localPosition.y);

        contentTransform.DOLocalMove(contentEndPoint, itemSwitchingDuration)
            .OnComplete(() => ChangeButtonsInteractability(true));
    }

    private int GetValidItemIndex(int newItemIndex)
    {
        if (newItemIndex >= _uiItems.Count)
        {
            return 0;
        }
        if(newItemIndex < 0)
        {
            return _uiItems.Count - 1;
        }

        return newItemIndex;
    }

    private void ChangeButtonsInteractability(bool isInteractable)
    {
        nextItemButton.interactable = isInteractable;
        previousItemButton.interactable = isInteractable;
    }

    /// <summary>
    /// Method to switch currently selected item
    /// </summary>
    /// <param name="indexStep">Step to increment/decrement item index</param>
    public void SwitchAsset(int indexStep)
    {
        var selectedItemIndex = GetValidItemIndex(_currentItemIndex + indexStep);
        ChangeButtonsInteractability(false);

        SwitchAssetAnim(selectedItemIndex);

        _currentItemIndex = selectedItemIndex;
        LoadLegacy();
    }

    /// <summary>
    /// Load legacy of currently selected asset to the legaciesList
    /// </summary>
    public void LoadLegacy()
    {
        legaciesList.Clear();
        TotemDemoManager.Instance.GetLegacyRecords(_uiItems[_currentItemIndex].ItemData, totemAssetType, legaciesList.InitializeLegacy);
    }

    /// <summary>
    /// Instantiates new UI element, initializes it and adds to the list
    /// </summary>
    /// <param name="assetData">Totem Asset's data</param>
    public void InitializeAssetsList(List<T> assetData)
    {
        foreach (var data in assetData)
        {
            var uiItem = Instantiate(uiItemPrefab, contentTransform).GetComponent<UIItemBase<T>>();
            _uiItems.Add(uiItem);

            uiItem.Initialize(data, _uiItems.Count - 1);
        }

        bool isItemsSwitchingEnabled = _uiItems.Count > 1;
        ChangeButtonsInteractability(isItemsSwitchingEnabled);
    }

    /// <summary>
    /// Clears all items from the list
    /// </summary>
    public void Clear()
    {
        foreach(var item in _uiItems)
        {
            Destroy(item.gameObject);
        }

        _uiItems.Clear();
    }
}
