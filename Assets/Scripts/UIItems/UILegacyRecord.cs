using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities;
using UnityEngine;

public class UILegacyRecord : MonoBehaviour
{
    [SerializeField] private TMP_Text legacyRecordType;
    [SerializeField] private TMP_Text assetId;
    [SerializeField] private TMP_Text gameId;
    [SerializeField] private TMP_Text data;

    public void Initalize(TotemLegacyRecord legacyData)
    {
        legacyRecordType.text = legacyData.legacyRecordType.ToString();
        assetId.text = legacyData.assetId;
        gameId.text = legacyData.gameId;
        data.text = legacyData.data;
    }
}
