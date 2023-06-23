// // ----------------------------------------------------------------------------
// // <copyright file="CustomMain.cs" company="Kinetix SAS">
// // Kinetix Unity SDK - Copyright (C) 2022 Kinetix SAS
// // </copyright>
// // ----------------------------------------------------------------------------

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Kinetix;
using Kinetix.UI;
using Kinetix.UI.EmoteWheel;

namespace Kinetix.Sample
{
    public class Theme : MonoBehaviour
    {
        [SerializeField] private string virtualWorldKey;
        [SerializeField] private Animator           localPlayerAnimator;
        [SerializeField] private KinetixCustomTheme kinetixCustomTheme;
        [SerializeField] private ECustomTheme       defaultConfig = ECustomTheme.DARK_MODE;

        [SerializeField] private TMP_Dropdown dropdown;
        private                  List<string> dropdownTheme;

        private void Awake()
        {
            KinetixCore.OnInitialized += OnKinetixInitialized;
            KinetixCore.Initialize(new KinetixCoreConfiguration()
            {
                VirtualWorldKey = virtualWorldKey,
                PlayAutomaticallyAnimationOnAnimators = true,
                ShowLogs                              = true,
                EnableAnalytics                       = true
            });
        }

        private void Start()
        {
            dropdownTheme = new List<string>();

            dropdownTheme.Add(ECustomTheme.LIGHT_MODE.ToString());
            dropdownTheme.Add(ECustomTheme.DARK_MODE.ToString());
            dropdownTheme.Add("CUSTOM THEME");

            dropdown.AddOptions(dropdownTheme);

            dropdown.value = dropdownTheme.IndexOf(defaultConfig.ToString());
        }

        public void OnThemeDropdownChanged()
        {
            if (dropdown.value < System.Enum.GetValues(typeof(ECustomTheme)).Length)
            {
                KinetixUIEmoteWheel.UpdateTheme((ECustomTheme)dropdown.value);
            }
            else if (dropdown.options[dropdown.value].text == "CUSTOM THEME")
            {
                KinetixUIEmoteWheel.UpdateThemeOverride(kinetixCustomTheme);
            }
        }

        private void OnDestroy()
        {
            KinetixCore.OnInitialized -= OnKinetixInitialized;
        }

        private void OnKinetixInitialized()
        {
            KinetixUIEmoteWheel.Initialize(new KinetixUIEmoteWheelConfiguration()
            {
                customThemeOverride = null,
                customTheme         = defaultConfig,
                baseLanguage        = SystemLanguage.English,
                enabledCategories = new []
                {
                    EKinetixUICategory.INVENTORY,
                    EKinetixUICategory.EMOTE_SELECTOR
                }
            });

            KinetixCore.Animation.RegisterLocalPlayerAnimator(localPlayerAnimator);

            KinetixCore.Account.ConnectAccount("sdk-sample-user-id", OnAccountConnected);
        }

        private void OnAccountConnected()
        {
            KinetixCore.Account.AssociateEmotesToUser("d228a057-6409-4560-afd0-19c804b30b84");
            KinetixCore.Account.AssociateEmotesToUser("bd6749e5-ac29-46e4-aae2-bb1496d04cbb");
            KinetixCore.Account.AssociateEmotesToUser("7a6d483e-ebdc-4efd-badb-12a2e210e618");
        }
    }
}
