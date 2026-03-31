using UnityEngine;

[CreateAssetMenu(fileName = "New Affix", menuName = "Game/Affix")]
public class Affix : ScriptableObject
{
    public string affixName;
    public string description;
    public float multiplierBonus;
    public bool isActive;
    public Sprite icon;
}
