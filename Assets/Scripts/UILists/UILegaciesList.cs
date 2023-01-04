using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class UILegaciesList : MonoBehaviour
{
    [SerializeField] private GameObject noLegaciesPanel;
    [SerializeField] private GameObject legacyRecordPrefab;
    [SerializeField] private Transform contentRoot;

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
    }
}
