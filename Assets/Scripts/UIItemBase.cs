using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemBase : MonoBehaviour
{
    private event Action<int> OnItemPicked;

    private int itemIndex;

    public virtual void Initialize(UIAssetsListBase assetsListParent, int index)
    {
        itemIndex = index;
    }

    private void OnMouseDown()
    {
        OnItemPicked?.Invoke(itemIndex);
    }
}
