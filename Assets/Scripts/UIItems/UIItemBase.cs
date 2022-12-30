using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemBase<T> : MonoBehaviour
{
    public event Action<int> OnItemSelected;

    public int ItemIndex { get; protected set; }

    public virtual void Initialize(T assetsData, int index)
    {
        ItemIndex = index;
    }

    private void OnMouseDown()
    {
        OnItemSelected?.Invoke(ItemIndex);
    }
}
