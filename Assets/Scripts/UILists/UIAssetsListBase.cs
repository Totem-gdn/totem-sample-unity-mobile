using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetsListBase<T> : MonoBehaviour
{
    [SerializeField]
    private Transform contentTransform;

    private int _currentItemIndex = 0;

    [SerializeField]
    protected TotemAssetType totemAssetType;
    [SerializeField]
    protected GameObject uiItemPrefab;

    protected List<UIItemBase<T>> _uiItems = new List<UIItemBase<T>>();

    protected void AddUIItem(T assetData)
    {
        var uiItem = Instantiate(uiItemPrefab, contentTransform).GetComponent<UIItemBase<T>>();
        _uiItems.Add(uiItem);

        uiItem.Initialize(assetData, _uiItems.Count - 1);
        uiItem.OnItemSelected += ItemSelected;
    }

    private void ItemSelected(int selectedItemIndex)
    {
        _currentItemIndex = selectedItemIndex;
        //Block pick
        //Animate
        //Unblock pick
        //Load Legacy
    }

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
