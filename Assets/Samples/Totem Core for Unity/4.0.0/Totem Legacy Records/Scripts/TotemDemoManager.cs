using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TotemEntities;
using TotemEntities.DNA;
using TotemServices.DNA;
using TMPro;

namespace TotemDemo
{
    public class TotemDemoManager : MonoBehaviour
    {
        public static TotemDemoManager Instance;
        private TotemCore totemCore;

        /// <summary>
        /// Id of your game, used for legacy records identification. 
        /// Note, that if you are targeting mobile platforms you also have to use this id for deepLink generation in
        /// *Window > Totem Generator > Generate Deep Link* menu
        /// </summary>
        [Header("Demo")]
        public string _gameId = "TotemDemo"; 

        [SerializeField] private GameObject loginButton;

        [Header("Login UI")]
        [SerializeField] private GameObject googleLoginObject;
        [SerializeField] private GameObject profileNameObject;
        [SerializeField] private TextMeshProUGUI profileNameText;

        [Header("Legacy UI")]
        [SerializeField] private TMP_InputField legacyGameIdInput;
        [SerializeField] private TMP_InputField dataToCompoareInput;
        [SerializeField] private UIAssetsList assetList;
        [SerializeField] private UIAssetLegacyRecordsList legacyRecordsList;
        [SerializeField] private Animator popupAnimator;

        //Meta Data
        private TotemUser _currentUser;
        private List<TotemDNADefaultAvatar> _userAvatars;

        //Default Avatar reference - use for your game
        private TotemDNADefaultAvatar firstAvatar;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initializing TotemCore
        /// </summary>
        void Start()
        {
            totemCore = new TotemCore(_gameId);

            legacyGameIdInput.onEndEdit.AddListener(OnGameIdInputEndEdit);
        }

        #region USER AUTHENTICATION
        public void OnLoginButtonClick()
        {
            UILoadingScreen.Instance.Show();

            //Login user
            totemCore.AuthenticateCurrentUser(Provider.GOOGLE, OnUserLoggedIn);
        }

        private void OnUserLoggedIn(TotemUser user)
        {
            //Using default filter with a default avatar model. You can implement your own filters and/or models
            totemCore.GetUserAvatars<TotemDNADefaultAvatar>(user, TotemDNAFilter.DefaultAvatarFilter, (avatars) =>
            {
                googleLoginObject.SetActive(false);
                profileNameObject.SetActive(true);
                profileNameText.SetText(user.Name);

                //UI
                assetList.ClearList();
                legacyRecordsList.ClearList();
                

                //Avatars
                _userAvatars = avatars;
                firstAvatar = avatars[0];
                //

                //UI Example Methods
                BuildAvatarList();
                UILoadingScreen.Instance.Hide();
                // ShowAvatarRecords();

            });
        }


        public void ShowAvatarRecords()
        {
            GetLegacyRecords(firstAvatar, (records) =>
            {
                UIAssetLegacyRecordsList.Instance.BuildList(firstAvatar, records);
                UILoadingScreen.Instance.Hide();
            });
        }
        #endregion

        #region LEGACY RECORDS
        /// <summary>
        /// Add a new Legacy Record to a specific Totem Asset.
        /// </summary>
        public void AddLegacyRecord(object asset, int data)
        {
            UILoadingScreen.Instance.Show();
            totemCore.AddLegacyRecord(asset, data.ToString(), (record) =>
            {
                legacyRecordsList.AddRecordToList(record, true);
                UILoadingScreen.Instance.Hide();
                popupAnimator.Play("Write Legacy");
            });
        }

        /// <summary>
        /// Add a new Legacy Record to the first Totem Avatar.
        /// </summary>
        public void AddLegacyToFirstAvatar(int data)
        {
            AddLegacyRecord(firstAvatar, data);
        }

        public void GetLegacyRecords(object asset, UnityAction<List<TotemLegacyRecord>> onSuccess)
        {
            totemCore.GetLegacyRecords(asset, onSuccess, legacyGameIdInput.text);
        }

        public void GetLastLegacyRecord(UnityAction<TotemLegacyRecord> onSuccess)
        {
            GetLegacyRecords(firstAvatar, (records) => { onSuccess.Invoke(records[records.Count - 1]); });
        }

        public void CompareLastLegacyRecord()
        {
            GetLastLegacyRecord((record) =>
            {
                string valueToCheckText = dataToCompoareInput.text;
                if (valueToCheckText.Equals(record.data))
                {
                    popupAnimator.Play("Read Legacy");
                }
            }
            );
        }
        #endregion

        #region UI EXAMPLE METHOD

        private void BuildAvatarList()
        {
            assetList.BuildList(_userAvatars);
        }

        private void OnGameIdInputEndEdit(string text)
        {
            ShowAvatarRecords();
        }

        #endregion
    }
}