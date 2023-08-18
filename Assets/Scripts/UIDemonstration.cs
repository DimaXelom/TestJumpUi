using UnityEngine;
using UnityEngine.UI;

public class UIDemonstration : MonoBehaviour
{
    [SerializeField] private JumpableBase _jumpable;
    [SerializeField] private Button _jumpButton;

    private void Start()
    {
        _jumpButton.onClick.AddListener(HandleJumpButtonClick);
    }

    private void HandleJumpButtonClick()
    {
        _jumpable.Jump();
    }
}
