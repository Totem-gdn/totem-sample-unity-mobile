using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities.DNA;
using UnityEngine;

public class UIAssetItem : UIItemBase<TotemDNADefaultItem>
{
    [SerializeField]
    private TextMeshPro classicalElementTMP;
    [SerializeField]
    private TMP_Text damageTMP;
    [SerializeField]
    private TMP_Text rangeTMP;
    [SerializeField]
    private TMP_Text powerTMP;
    [SerializeField]
    private TMP_Text magicalExpTMP;
    [SerializeField]
    private TMP_Text weaponMaterialTMP;
    [SerializeField]
    private TMP_Text primaryColorTMP;

    public override void Initialize(TotemDNADefaultItem assetsListParent, int index)
    {
        base.Initialize(assetsListParent, index);
    }
}
