using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelInfo : ScriptableObject
{
    [TextArea]
    public string[] LevelInfos;
}
