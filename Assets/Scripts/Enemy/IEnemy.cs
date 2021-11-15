using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void KillEnemy();

    void DamageEnemy(float dmg);

    enum AiState
    {
        Idle,
        Search,
        Move,
        Attack
    }
}
