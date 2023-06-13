using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZincirController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject whip = Instantiate(weaponData.Prefab);
        whip.transform.position = transform.position+ new Vector3(1f,0,0);
        whip.transform.parent = transform;
    }
}
