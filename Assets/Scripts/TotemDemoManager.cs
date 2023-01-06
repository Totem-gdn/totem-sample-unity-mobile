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
using UnityEngine.UI;
using UnityEngine.Networking;

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

        [Header("Login/Logout UI")]
        [SerializeField] private RawImage profileImage;
        [SerializeField] private GameObject profileInfoObject;
        [SerializeField] private TMP_Text profilePublicKey;
        [SerializeField] private TextMeshProUGUI profileNameText;

        [Header("Legacy UI")]
        [SerializeField] private UIItemsList itemsList;
        [SerializeField] private UIAvatarsList avatarsList;

        //Meta Data
        private List<TotemDNADefaultAvatar> _userAvatars;
        private List<TotemDNADefaultItem> _userItems;

        //Default Avatar reference - use for your game
        private TotemDNADefaultAvatar firstAvatar;
        private TotemDNADefaultItem firstItem;

        private bool _isAvatarsLoaded = false;
        private bool _isItemsLoaded = false;

        private Coroutine profileImageCoroutine;

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
            totemCore.AuthenticateCurrentUser(OnUserLoggedIn);
        }

        public void OnLogoutButtonClick()
        {
            if(profileImageCoroutine != null)
            {
                StopCoroutine(profileImageCoroutine);
                profileImageCoroutine = null;
            }

            avatarsList.Clear();
            itemsList.Clear();

            profileImage.texture = null;
            profileImage.enabled = false;

            profileInfoObject.SetActive(false);

            loginButton.SetActive(true);
            avatarsList.SetLoginPanelsVisibility(true);
            itemsList.SetLoginPanelsVisibility(true);
        }

        private void OnUserLoggedIn(TotemUser user)
        {
            //Using default filter with a default avatar model. You can implement your own filters and/or models
            totemCore.GetUserAvatars<TotemDNADefaultAvatar>(user, TotemDNAFilter.DefaultAvatarFilter, (avatars) =>
            {
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    profileImageCoroutine = StartCoroutine(LoadProfileImage(user.ProfileImageUrl));
                }
                profileNameText.SetText(user.Name);
                profilePublicKey.SetText(user.PublicKey);

                avatarsList.Clear();

                _userAvatars = avatars;
                firstAvatar = avatars.Count > 0 ? avatars[0] : null;
                GetLegacyRecords(firstAvatar, TotemAssetType.avatar, TestSuccess);
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
              // AddLegacyRecord(firstItem, TotemAssetType.item, 1);

                itemsList.Clear();

                if (_userItems.Count > 0)
                {
                    BuildItemList();
                }

                _isItemsLoaded = true;
                CheckLoadingEnded();
            });
        }

        private void TestSuccess(List<TotemLegacyRecord> legacyRecords)
        {
            Debug.Log(legacyRecords.Count);
        }

        private void CheckLoadingEnded()
        {
            if(_isAvatarsLoaded && _isItemsLoaded)
            {
                loginButton.SetActive(false);
                profileInfoObject.SetActive(true);

                avatarsList.SetLoginPanelsVisibility(false);
                itemsList.SetLoginPanelsVisibility(false);
                UILoadingScreen.Instance.Hide();
            }
        }

        private IEnumerator LoadProfileImage(string imageUrl)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return www.SendWebRequest();
                if(www.result == UnityWebRequest.Result.Success)
                {
                    profileImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    profileImage.enabled = true;
                    profileImageCoroutine = null;
                }
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

        public void GetLastLegacyRecord(object asset, TotemAssetType assetType, UnityAction<TotemLegacyRecord> onSuccess)
        {
            GetLegacyRecords(asset, assetType, (records) => { onSuccess.Invoke(records[records.Count - 1]); });
        }
        #endregion

        #region UI EXAMPLE METHOD

        private void BuildAvatarList()
        {
            avatarsList.InitializeAssetsList(_userAvatars);
            avatarsList.LoadLegacy();
        }

        private void BuildItemList()
        {
            itemsList.InitializeAssetsList(_userItems);
            itemsList.LoadLegacy();
        }

        #endregion
    }
}