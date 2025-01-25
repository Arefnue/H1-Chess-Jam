using System;
using System.Collections.Generic;
using System.Text;
using _NueCore.AudioSystem;
using _NueCore.Common.ReactiveUtils;
using _NueCore.SaveSystem;
using _NueExtras.CheatSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _NueCore.SettingsSystem
{
    public class SettingsUIController : MonoBehaviour
    {
        [SerializeField, TabGroup("Settings", "Common")]
        private Button closeButton;

        [SerializeField, TabGroup("Settings", "Common")]
        private Button openButton;

        [SerializeField, TabGroup("Settings", "Common")]
        private Transform settingsPanelRoot;

        [SerializeField, TabGroup("Settings", "Common")]
        private Button cheatButton;

        [SerializeField, TabGroup("Settings", "Notice")]
        private TMP_Text noticeVersionText;

        #region Cache

        private bool _isSettingsPanelOpened;

        public static BoolReactiveProperty IsHapticActive = new BoolReactiveProperty(true);

        #endregion

        #region Setup

        private void Awake()
        {
            openButton.gameObject.SetActive(true);
            settingsPanelRoot.gameObject.SetActive(false);
            SetVersion();
            cheatButton.OnClickAsObservable().TakeUntilDisable(gameObject)
                    .Timestamp()
                    .Pairwise((prev, current) => (current.Timestamp - prev.Timestamp).TotalMilliseconds <= 300)
                    .Where(fastEnough => fastEnough)
                    .Subscribe(x =>  RBuss.Publish(new CheatREvents.EnableCheatREvent()));
          
            cheatButton.onClick.AddListener(() => {  });
            closeButton.onClick.AddListener(() =>
            {
                AudioStatic.PlayFx(DefaultAudioDataTypes.ClosePanel);
                SaveSettings();
                CloseSettings();
            });
            
            openButton.onClick.AddListener(() =>
            {
                AudioStatic.PlayFx(DefaultAudioDataTypes.OpenPanel);
                OpenSettings();
            });
            openButton.gameObject.SetActive(true);
            hapticToggle.ChangeAction += b =>
            {
                IsHapticActive.Value = b;
            };
            
        }

        private void Start()
        {
            //StartSettings();
        }

        private void SetVersion()
        {
            var str = new StringBuilder();
            str.Append("Version: ");

#if UNITY_IOS
            str.Append("IOS").Append("_").Append(Application.version);
#endif

#if UNITY_ANDROID
            str.Append("DRD").Append("_").Append(Application.version);
#endif

#if UNITY_EDITOR
            str.Append("EDT").Append("_").Append(Application.version);
#endif

            noticeVersionText.SetText(str.ToString());
        }

        #endregion

        #region Process

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isSettingsPanelOpened)
                {
                    CloseSettings();
                    SaveSettings();
                }
                else
                {
                    OpenSettings();
                }
            }
        }

        #endregion

        #region Common Methods

        public void OpenSettings()
        {
            //ARIF Remove from asset
            Time.timeScale = 0f;
            openButton.gameObject.SetActive(false);
            _isSettingsPanelOpened = true;
            settingsPanelRoot.gameObject.SetActive(true);
            RBuss.Publish(new SettingsREvents.SettingsOpenedREvent());
        }

        public void CloseSettings()
        {
            Time.timeScale = 1f;
            openButton.gameObject.SetActive(true);
            _isSettingsPanelOpened = false;
            settingsPanelRoot.gameObject.SetActive(false);
            RBuss.Publish(new SettingsREvents.SettingsClosedREvent());
            SaveStatic.Save();
        }

        #endregion

        #region Audio Settings

        [SerializeField, TabGroup("Settings", "Audio")]
        private AudioMixer audioMixer;
        
        [SerializeField, TabGroup("Settings", "Audio")]
        private SettingsSlider musicVolumeSlider;

        [SerializeField, TabGroup("Settings", "Audio")]
        private SettingsSlider sfxVolumeSlider;

        [SerializeField, TabGroup("Settings", "Audio")]
        private SettingsToggle hapticToggle;
        
        private float _currentMasterVolume = 100;
        private float _currentMusicVolume = 100;
        private float _currentSfxVolume = 100;

        public void SetMasterVolume(float volume)
        {
            var convertedVolume = Mathf.InverseLerp(0, 100, volume);
            if (convertedVolume < 0.0001)
                convertedVolume = 0.0001f;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(convertedVolume) * 20);
            _currentMasterVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            var convertedVolume = Mathf.InverseLerp(0, 100, volume);
            if (convertedVolume < 0.0001)
                convertedVolume = 0.0001f;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(convertedVolume) * 20);
            _currentMusicVolume = volume;
        }

        public void SetSfxVolume(float volume)
        {
            var convertedVolume = Mathf.InverseLerp(0, 100, volume);
            if (convertedVolume < 0.0001)
                convertedVolume = 0.0001f;
            audioMixer.SetFloat("SfxVolume", Mathf.Log10(convertedVolume) * 20);
            _currentSfxVolume = volume;
        }

        #endregion

        #region Video Settings

        [SerializeField,HideInInspector, TabGroup("Settings", "Video")]
        private TMP_Dropdown resolutionDropdown;

        [SerializeField,HideInInspector, TabGroup("Settings", "Video")]
        private TMP_Dropdown qualityDropdown;

        [SerializeField,HideInInspector, TabGroup("Settings", "Video")]
        private Toggle fullscreenToggle;

        private List<Resolution> ResolutionList { get; set; } = new List<Resolution>();
        private List<string> OptionTextList { get; set; } = new List<string>();
        private List<string> QualityTextList { get; set; } = new List<string>();

        private int _currentResolutionIndex;

        private void InitVideoSettings()
        {
            return;
            InitResolutionDropdown();

            InitQualityDropdown();
        }

        private void InitQualityDropdown()
        {
            qualityDropdown.ClearOptions();
            var allQualities = QualitySettings.names;
            for (int i = 0; i < allQualities.Length; i++)
            {
                var item = allQualities[i];
                QualityTextList.Add(item);
            }

            qualityDropdown.AddOptions(QualityTextList);
            qualityDropdown.RefreshShownValue();

            qualityDropdown.onValueChanged.AddListener(SetQuality);
        }

        private void InitResolutionDropdown()
        {
            resolutionDropdown.ClearOptions();
            var allResolutions = Screen.resolutions;
            for (int i = 0; i < allResolutions.Length; i++)
            {
                var item = allResolutions[i];
                if (ResolutionList.Contains(item))
                    continue;
                ResolutionList.Add(item);
            }


            for (int i = 0; i < ResolutionList.Count; i++)
            {
                var option = ResolutionList[i].width + " x " + ResolutionList[i].height;

                OptionTextList.Add(option);
                if (ResolutionList[i].width == Screen.currentResolution.width
                    && ResolutionList[i].height == Screen.currentResolution.height)
                    _currentResolutionIndex = i;
            }

            resolutionDropdown.AddOptions(OptionTextList);
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void SetResolution(int res)
        {
            Resolution resolution = ResolutionList[res];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetTextureQuality(int textureIndex)
        {
            QualitySettings.globalTextureMipmapLimit = textureIndex;
            qualityDropdown.value = 6;
        }

        public void SetAntiAliasing(int aaIndex)
        {
            QualitySettings.antiAliasing = aaIndex;
            qualityDropdown.value = 6;
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            qualityDropdown.value = qualityIndex;
        }

        #endregion

        #region Save

        public void LoadSettings()
        {
            InitVideoSettings();
            var saveData = NSaver.GetSaveData<SettingsSave>();
            // qualityDropdown.value =
            //     saveData.qualityIndex >= 0 ? saveData.qualityIndex : QualitySettings.GetQualityLevel();
            // resolutionDropdown.value =
            //     saveData.resolutionIndex >= 0 ? saveData.resolutionIndex : _currentResolutionIndex;
            // fullscreenToggle.isOn = saveData.isFullscreen;
            // SetFullscreen(saveData.isFullscreen);
            SetMasterVolume(100);
            //masterVolumeSlider.Build(saveData.masterVolume, );
            if (saveData.isHapticActive)
                hapticToggle.Activate();
            else
                hapticToggle.DeActivate();
            IsHapticActive.Value = saveData.isHapticActive;
            musicVolumeSlider.Slider.minValue = 0;
            musicVolumeSlider.Slider.maxValue = 100;
            musicVolumeSlider.Build(saveData.musicVolume, SetMusicVolume);
            sfxVolumeSlider.Slider.minValue = 0;
            sfxVolumeSlider.Slider.maxValue = 100;
            sfxVolumeSlider.Build(saveData.sfxVolume, SetSfxVolume);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                StartSettings();
            }, false);
        }

        public void StartSettings()
        {
            var saveData = NSaver.GetSaveData<SettingsSave>();
            SetMasterVolume(saveData.masterVolume);
            SetMusicVolume(saveData.musicVolume);
            SetSfxVolume(saveData.sfxVolume);
        }

        public void SaveSettings()
        {
            var saveData = NSaver.GetSaveData<SettingsSave>();
            saveData.masterVolume = _currentMasterVolume;
            saveData.musicVolume = _currentMusicVolume;
            saveData.sfxVolume = _currentSfxVolume;
            saveData.isHapticActive = hapticToggle.IsActive;
            saveData.Save();
        }

        #endregion
    }
}