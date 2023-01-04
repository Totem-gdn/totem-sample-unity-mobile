using TMPro;
using TotemEntities.DNA;
using UnityEngine;

public class UIAssetAvatar : UIItemBase<TotemDNADefaultAvatar>
{
    [SerializeField] private TMP_Text sexTMP;
    [SerializeField] private TMP_Text bodyStrengthTMP;
    [SerializeField] private TMP_Text bodyTypeTMP;
    [SerializeField] private TMP_Text humanHairColorTMP;
    [SerializeField] private TMP_Text humanEyeColorTMP;
    [SerializeField] private TMP_Text humanSkinColorTMP;
    [SerializeField] private TMP_Text primaryColorTMP;

    /// <summary>
    /// Initialization of UI fields with Avatar data
    /// </summary>
    /// <param name="itemData">Avatar Item data</param>
    /// <param name="index">Avatar's index in the global list</param>
    public override void Initialize(TotemDNADefaultAvatar avatarData, int index)
    {
        base.Initialize(avatarData, index);

        sexTMP.text = avatarData.sex_bio.ToString();
        bodyStrengthTMP.text = avatarData.body_strength.ToString();
        bodyTypeTMP.text = avatarData.body_type.ToString();
        humanHairColorTMP.text = avatarData.human_hair_color.ToString();
        humanEyeColorTMP.text = avatarData.human_eye_color.ToString();
        humanSkinColorTMP.text = avatarData.human_skin_color.ToString();
        primaryColorTMP.text = avatarData.primary_color.ToString();
    }

}
