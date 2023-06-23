using Kinetix;
using Kinetix.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private Camera defaultCamera;

    private InputAction click;

    private Character[] characters;

    private void Awake()
    {
        characters = FindObjectsByType<Character>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        LightsOff();
    }

    void LightsOff()
    {
        foreach (Character character in characters)
        {
            character.Light.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.performed += ctx => {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(defaultCamera.ScreenPointToRay(coor), out hit) && !KinetixUI.IsShown)
            {
                Transform objectHit = hit.transform;
                Logger.Instance.LogInfo($"Object selected: {objectHit.name}");

                // all lights off
                LightsOff();

                Character character = objectHit.GetComponent<Character>();
                if(character?.Animator != null)
                {
                    Logger.Instance.LogInfo($"Asigning animator to {objectHit.name}");
                    character.Light.gameObject.SetActive(true);

                    //The UnregisterLocalPlayer keeps creating multiple game objects which is not good
                    //I tried to call UnregisterLocalPlayer to delete the previous object but it generates
                    //exceptions saying that a glb is used by another process so for now is commented
                    //KinetixCore.Animation.UnregisterLocalPlayer();
                    KinetixCore.Animation.RegisterLocalPlayerAnimator(character.Animator);
                }
            }
        };
        click.Enable();
    }
}