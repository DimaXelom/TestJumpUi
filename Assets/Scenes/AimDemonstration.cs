using UnityEngine;
using UnityEngine.UI;

public class AimDemonstration : MonoBehaviour
{
    [SerializeField] private AimingBase _aimingBase;
    [SerializeField] private Text _valueText;

    private void Update()
    {
        _valueText.text = _aimingBase.IsHit + ":" + _aimingBase.HitPoint;
    }
}
internal class AimingBase
{
    public string IsHit { get; set; }
    public string HitPoint { get; set; }
}