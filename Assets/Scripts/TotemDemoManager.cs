using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TotemEntities;
using TotemEntities.DNA;
using TotemServices.DNA;
using TMPro;
using DG.Tweening;

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
        [SerializeField] private UIItemsList itemsList;
        [SerializeField] private UIAvatarsList avatarsList;

        //Meta Data
        private TotemUser _currentUser;
        private List<TotemDNADefaultAvatar> _userAvatars;
        private List<TotemDNADefaultItem> _userItems;

        //Default Avatar reference - use for your game
        private TotemDNADefaultAvatar firstAvatar;
        private TotemDNADefaultItem firstItem;

        private bool _isAvatarsLoaded = false;
        private bool _isItemsLoaded = false;

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
                profileNameText.SetText(user.Name);

                avatarsList.Clear();

                _userAvatars = avatars;
                firstAvatar = avatars.Count > 0 ? avatars[0] : null;
               // AddLegacyRecord(firstAvatar, TotemAssetType.avatar, 2);

                if (_userAvatars.Count > 0)
                {
                    BuildAvatarList();
                }

                _isAvatarsLoaded = true;
                CheckLoadingEnded();
            });

            //Using default filter with a default item model. You can implement your own filters and/or models
            totemCore.GetUserItems<TotemDNADefaultItem>(user, TotemDNAFilter.DefaultItemFilter, (items) =>
            {
                _userItems = items;
                firstItem = items.Count > 0 ? items[0] : null;
               AddLegacyRecord(firstItem, TotemAssetType.item, 1);

                itemsList.Clear();

                if (_userItems.Count > 0)
                {
                    BuildItemList();
                }

                _isItemsLoaded = true;
                CheckLoadingEnded();
            });
        }

        private void Test(List<TotemLegacyRecord> legacyRecords)
        {
            Debug.Log(legacyRecords.Count);
        }

        private void CheckLoadingEnded()
        {
            if(_isAvatarsLoaded && _isItemsLoaded)
            {
                googleLoginObject.SetActive(false);
                profileNameObject.SetActive(true);
                UILoadingScreen.Instance.Hide();
            }
        }
        #endregion

        #region LEGACY RECORDS
        /// <summary>
        /// Add a new Legacy Record to a specific Totem Asset.
        /// </summary>
        public void AddLegacyRecord(object asset, TotemAssetType assetType, int data)
        {
            UILoadingScreen.Instance.Show();
            totemCore.AddLegacyRecord(asset, assetType, data.ToString(), (record) =>
            {
                UILoadingScreen.Instance.Hide();
            });
        }

        public void GetLegacyRecords(object asset, TotemAssetType assetType, UnityAction<List<TotemLegacyRecord>> onSuccess)
        {
            totemCore.GetLegacyRecords(asset, assetType, onSuccess, _gameId);
        }

        public void GetLastLegacyRecord(UnityAction<TotemLegacyRecord> onSuccess)
        {
            GetLegacyRecords(firstAvatar, TotemAssetType.avatar, (records) => { onSuccess.Invoke(records[records.Count - 1]); });
        }
        #endregion

        #region UI EXAMPLE METHOD

        private void BuildAvatarList()
        {
            foreach (var avatar in _userAvatars)
            {
                avatarsList.AddUIItem(avatar);
            }

            avatarsList.LoadLegacy();
        }

        private void BuildItemList()
        {
            foreach (var item in _userItems)
            {
                itemsList.AddUIItem(item);
            }

            itemsList.LoadLegacy();
        }

        #endregion
    }
}