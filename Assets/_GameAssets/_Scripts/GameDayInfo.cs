using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameDay")]
public class GameDayInfo : ScriptableObject
{
    public List<ClientCharacter> Characters;
    
}
