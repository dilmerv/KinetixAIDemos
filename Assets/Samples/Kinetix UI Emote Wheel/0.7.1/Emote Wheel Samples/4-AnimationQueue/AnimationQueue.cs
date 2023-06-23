using UnityEngine;
using System.Linq;
using Kinetix;
using Kinetix.UI;
using Kinetix.UI.EmoteWheel;

namespace Kinetix.Sample
{
    public class AnimationQueue : MonoBehaviour
    {
        [SerializeField] private string virtualWorldKey;
        [SerializeField] private Animator localPlayerAnimator;

        private void Awake()
        {
            KinetixCore.OnInitialized += OnKinetixInitialized;
            KinetixCore.Initialize(new KinetixCoreConfiguration()
            {
                VirtualWorldKey = virtualWorldKey,
                PlayAutomaticallyAnimationOnAnimators = true,
                ShowLogs                              = true
            });
        }

        private void OnDestroy()
        {
            KinetixCore.OnInitialized -= OnKinetixInitialized;
        }

        private void OnKinetixInitialized()
        {
            KinetixUIEmoteWheel.Initialize(new KinetixUIEmoteWheelConfiguration()
            {
                enabledCategories = new []
                {
                    EKinetixUICategory.INVENTORY,
                    EKinetixUICategory.EMOTE_SELECTOR
                }
            });

            // EVENTS
            KinetixCore.Animation.OnPlayedAnimationQueueLocalPlayer     += OnPlayedAnimationQueueLocal;
            KinetixCore.Animation.OnAnimationStartOnLocalPlayerAnimator += OnAnimationStartOnLocalPlayerAnimator;
            KinetixCore.Animation.OnAnimationEndOnLocalPlayerAnimator   += OnAnimationEndOnLocalPlayerAnimator;

            KinetixCore.Animation.RegisterLocalPlayerAnimator(localPlayerAnimator);

            KinetixCore.Account.ConnectAccount("sdk-sample-user-id", OnAccountConnected);
        }

        private void OnAccountConnected()
        {
            KinetixCore.Account.AssociateEmotesToUser("d228a057-6409-4560-afd0-19c804b30b84");
            KinetixCore.Account.AssociateEmotesToUser("bd6749e5-ac29-46e4-aae2-bb1496d04cbb");
            KinetixCore.Account.AssociateEmotesToUser("7a6d483e-ebdc-4efd-badb-12a2e210e618");

            KinetixCore.Metadata.GetUserAnimationMetadatas(animations =>
            {
                AnimationIds[] animationIDs = animations.Select(metadata => metadata.Ids).Take(2).ToArray();
                KinetixCore.Animation.LoadLocalPlayerAnimations(animationIDs, "AnimationQueueSampleImplementation", () => { KinetixCore.Animation.PlayAnimationQueueOnLocalPlayer(animationIDs, true); });
            });
        }


        private void OnPlayedAnimationQueueLocal(AnimationIds[] _AnimationIdsArray)
        {
            string animationStr = "";
            _AnimationIdsArray.ToList().ForEach(animationIds => animationStr += animationIds.UUID + "\n");
            Debug.Log("[LOCAL] Played Animation queue : \n" + animationStr);
        }

        private void OnAnimationStartOnLocalPlayerAnimator(AnimationIds _AnimationIds)
        {
            Debug.Log("[LOCAL] Animation Started : " + _AnimationIds.UUID);
        }

        private void OnAnimationEndOnLocalPlayerAnimator(AnimationIds _AnimationIds)
        {
            Debug.Log("[LOCAL] Animation Ended : " + _AnimationIds.UUID);
        }
    }
}
