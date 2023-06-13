using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position; // firlatilacagi yeri player yaptik
        spawnedKnife.GetComponent<KnifeBehaviour>().DirectionChecker(playerMovement.lastMovementVector);   //yon icin referans
    }
}