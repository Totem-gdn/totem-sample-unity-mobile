using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetsListBase : MonoBehaviour
{
    private List<UIItemBase> _uiItems = new List<UIItemBase>();

    [SerializeField]
    protected TotemAssetType totemAssetType;
    [SerializeField]
    protected GameObject uiItemPrefab;

    public virtual void BuildUIAssetsList()
    {

    }
}
