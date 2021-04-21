using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looteable : Interactive
{
    public int gold;
    private TriggerSpawner zone;

    private Animator anim;
    private int hitHashCode;
    private int destroyHashCode;

    public void Initialize(int minG, int maxG, TriggerSpawner zone)
    {
        anim = GetComponent<Animator>();
        hitHashCode = Animator.StringToHash("Hit");
        destroyHashCode = Animator.StringToHash("Destroy");

        gold = Random.Range(minG, maxG);
        this.zone = zone;
    }

    public override void Interact()
    {
        if (zone.enemiesAlive != 0)
            anim.SetTrigger(hitHashCode);
        else
        {
            anim.SetTrigger(destroyHashCode);
            player.CurrentGold += gold;
            col.enabled = false;
        }
    }
}
