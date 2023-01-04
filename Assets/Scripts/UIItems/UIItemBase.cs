using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemBase<T> : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> OnItemSelected;

    public int ItemIndex { get; protected set; }
    public T ItemData { get; protected set; }

    /// <summary>
    /// Initialization of UI fields with Asset data
    /// </summary>
    /// <param name="itemData">Totem Asset data</param>
    /// <param name="index">Asset's index in the global list</param>
    public virtual void Initialize(T assetData, int index)
    {
        ItemIndex = index;
        ItemData = assetData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemSelected?.Invoke(ItemIndex);
    }
}
