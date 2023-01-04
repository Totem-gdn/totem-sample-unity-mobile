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
    [SerializeField] private GameObject pickerBlocker;

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
    /// Method that works when OnItemSelected event is raised
    /// </summary>
    /// <param name="selectedItemIndex">Index of a selected item</param>
    private void ItemSelected(int selectedItemIndex)
    {
        if (selectedItemIndex != _currentItemIndex)
        {
            pickerBlocker.SetActive(true);
            SwitchAsset(selectedItemIndex);

            _currentItemIndex = selectedItemIndex;
            LoadLegacy();
        }
    }

    /// <summary>
    /// Animation for asset switching in the list
    /// </summary>
    /// <param name="selectedItemIndex">Index of a selected item</param>
    private void SwitchAsset(int selectedItemIndex)
    {
        float animationDistance = (_currentItemIndex - selectedItemIndex) * _distanceBetweenItems;
        Vector2 contentEndPoint = new Vector2(contentTransform.localPosition.x + animationDistance, contentTransform.localPosition.y);

        contentTransform.DOLocalMove(contentEndPoint, itemSwitchingDuration)
            .OnComplete(() => pickerBlocker.SetActive(false));
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
    public void AddUIItem(T assetData)
    {
        var uiItem = Instantiate(uiItemPrefab, contentTransform).GetComponent<UIItemBase<T>>();
        _uiItems.Add(uiItem);

        uiItem.Initialize(assetData, _uiItems.Count - 1);
        uiItem.OnItemSelected += ItemSelected;
    }

    /// <summary>
    /// Clears all items from the list
    /// </summary>
    public void Clear()
    {
        foreach(var item in _uiItems)
        {
            item.OnItemSelected -= ItemSelected;
            Destroy(item.gameObject);
        }

        _uiItems.Clear();
    }
}
