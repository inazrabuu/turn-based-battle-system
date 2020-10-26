using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLauncherDemo : MonoBehaviour
{
    [SerializeField]
    private List<Character> enemies, players;
    [SerializeField]
    private BattleLauncher _battleLauncher;

    public void Launch()
    {
        _battleLauncher.PrepareBattle(enemies, players);
    }
}
