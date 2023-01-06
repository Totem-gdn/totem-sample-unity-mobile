using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities;
using UnityEngine;

public class UILegaciesList : MonoBehaviour
{
    [SerializeField] private GameObject noLegaciesPanel;
    [SerializeField] private GameObject loginTipPanel;
    [SerializeField] private GameObject legacyRecordPrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private TMP_Text legaciesCountText;

    private List<UILegacyRecord> legacyUIItems = new List<UILegacyRecord>();
    private List<TotemLegacyRecord> legacyRecordsData = new List<TotemLegacyRecord>();

    public void InitializeLegacy(List<TotemLegacyRecord> legaciesData)
    {
        if (legaciesData.Count == 0)
        {
            noLegaciesPanel.SetActive(true);

            return;
        }

        noLegaciesPanel.SetActive(false);
        legacyRecordsData = legaciesData;
        legaciesCountText.text = legacyRecordsData.Count.ToString();

        foreach (var legacyData in legacyRecordsData)
        {
            var uiLegacyRecord = Instantiate(legacyRecordPrefab, contentRoot).GetComponent<UILegacyRecord>();
            uiLegacyRecord.Initalize(legacyData);

            legacyUIItems.Add(uiLegacyRecord);
        }
    }

    public void Clear()
    {
        foreach(var uiItem in legacyUIItems)
        {
            Destroy(uiItem.gameObject);
        }

        legacyUIItems.Clear();
        legacyRecordsData.Clear();
        legaciesCountText.text = "0";

        noLegaciesPanel.SetActive(true);
    }

    public void SetLoginTipVisibility(bool isVisible)
    {
        loginTipPanel.SetActive(isVisible);
    }
}
