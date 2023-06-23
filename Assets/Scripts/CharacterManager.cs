using DilmerGames.Core.Singletons;
using Kinetix;
using Kinetix.UI;
using Kinetix.UI.EmoteWheel;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    [SerializeField]
    private string mockUserId = "dilmer@test.com";

    [SerializeField]
    private string virtualWorldKey = "";

    [SerializeField]
    private Animator localPlayerAnimator;

    public bool IsLocalPlayerReady { get; set; }

    private void Awake()
    {
        KinetixCore.OnInitialized += KinetixCore_OnInitialized;
        KinetixCore.Initialize(new KinetixCoreConfiguration
        {
            VirtualWorldKey = virtualWorldKey,
            PlayAutomaticallyAnimationOnAnimators = true,
            ShowLogs = true,
            EnableAnalytics = true,
        });

        KinetixCore.Animation.OnRegisteredLocalPlayer += () =>
        {
            Logger.Instance.LogInfo("OnRegisteredLocalPlayer executed...");
        };

        KinetixCore.Animation.OnAnimationStartOnLocalPlayerAnimator += (animationIds) =>
        {
            Logger.Instance.LogInfo("OnAnimationStartOnLocalPlayerAnimator executed");
        };

        KinetixCore.Animation.OnAnimationEndOnLocalPlayerAnimator += (animationIds) =>
        {
            Logger.Instance.LogInfo("OnAnimationEndOnLocalPlayerAnimator executed");
        };

    }

    private void KinetixCore_OnInitialized()
    {
        KinetixUIEmoteWheel.Initialize(new KinetixUIEmoteWheelConfiguration
        {
            enabledCategories = new []
            {
                EKinetixUICategory.INVENTORY,
                EKinetixUICategory.EMOTE_SELECTOR,
                EKinetixUICategory.CREATE
            }
        });

        KinetixCore.Animation.RegisterLocalPlayerAnimator(localPlayerAnimator);
        KinetixCore.Account.ConnectAccount(mockUserId, OnConnectedAccount);
    }

    private void OnDestroy()
    {
        KinetixCore.OnInitialized -= KinetixCore_OnInitialized;
    }

    private void OnConnectedAccount()
    {
        // d228a057-6409-4560-afd0-19c804b30b84 (anger)
        // bd6749e5-ac29-46e4-aae2-bb1496d04cbb (crying)
        // 8515cc88-9559-48d4-a4a4-9017c10d6ed4 (dancing)

        // 793885df-e0e1-45ec-846e-cebe4f3e07c3 (dilmer idle)
        // 507f5f4b-f573-4089-89fd-3c95e18db5b1 (dilmer jump)
        // ec524df7-9381-4c35-bdc6-6026dea998aa (dilmer walk)
        KinetixCore.Account.AssociateEmotesToUser("d228a057-6409-4560-afd0-19c804b30b84");
        KinetixCore.Account.AssociateEmotesToUser("bd6749e5-ac29-46e4-aae2-bb1496d04cbb");
        KinetixCore.Account.AssociateEmotesToUser("8515cc88-9559-48d4-a4a4-9017c10d6ed4");

        KinetixCore.Account.AssociateEmotesToUser("793885df-e0e1-45ec-846e-cebe4f3e07c3");
        KinetixCore.Account.AssociateEmotesToUser("507f5f4b-f573-4089-89fd-3c95e18db5b1");
        KinetixCore.Account.AssociateEmotesToUser("ec524df7-9381-4c35-bdc6-6026dea998aa");
    }
}
