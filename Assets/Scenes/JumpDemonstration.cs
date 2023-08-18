using UnityEngine;
using UnityEngine.UI;

public interface IJumpable
{
    void Jump();
}

public abstract class JumpableBase : MonoBehaviour, IJumpable
{
    public abstract void Jump();
}


public class JumpDemonstration : JumpableBase
{
    private int _jumpCount = 0;

    [SerializeField] private Text _jumpCountText;

    public override void Jump()
    {
        _jumpCount++;
        _jumpCountText.text = "Jump Count: " + _jumpCount;
    }
}