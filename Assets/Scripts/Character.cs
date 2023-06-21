using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Light characterLight;

    [SerializeField] 
    private Animator characterAnimator;

    public Light Light { get { return characterLight; } }

    public Animator Animator { get { return characterAnimator; } }
}
