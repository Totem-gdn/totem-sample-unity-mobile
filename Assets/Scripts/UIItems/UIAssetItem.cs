using TMPro;
using TotemEntities.DNA;
using UnityEngine;

public class UIAssetItem : UIItemBase<TotemDNADefaultItem>
{
    [SerializeField] private TMP_Text classicalElementTMP;
    [SerializeField] private TMP_Text damageTMP;
    [SerializeField] private TMP_Text rangeTMP;
    [SerializeField] private TMP_Text powerTMP;
    [SerializeField] private TMP_Text magicalExpTMP;
    [SerializeField] private TMP_Text weaponMaterialTMP;
    [SerializeField] private TMP_Text primaryColorTMP;

    /// <summary>
    /// Initialization of UI fields with Item data
    /// </summary>
    /// <param name="itemData">Totem Item data</param>
    /// <param name="index">Item's index in the global list</param>
    public override void Initialize(TotemDNADefaultItem itemData, int index)
    {
        base.Initialize(itemData, index);

        classicalElementTMP.text = itemData.classical_element;
        damageTMP.text = itemData.damage_nd.ToString();
        rangeTMP.text = itemData.range_nd.ToString();
        powerTMP.text = itemData.power_nd.ToString();
        magicalExpTMP.text = itemData.magical_exp.ToString();
        weaponMaterialTMP.text = itemData.weapon_material;
        primaryColorTMP.text = itemData.primary_color.ToString();
    }
}
