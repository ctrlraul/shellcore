﻿using UnityEngine;

/// <summary>
/// The targeting system of each weapon ability
/// </summary>
public class WeaponTargetingSystem : ITargetingSystem
{
    public WeaponAbility ability; // owner ability of the targeting system
    public Transform target; // target of the targeting system

    /// <summary>
    /// Get the target of the targeting system
    /// </summary>
    /// <param name="findNew">Whether or not the targeting system should find a new target</param>
    /// <returns>The target of the targeting system</returns>
    public Transform GetTarget()
    {
        Transform tmp = ability?.Core?.GetTargetingSystem()?.GetTarget(); // get the core's target if it has one

        if (tmp != null && tmp && IsValidTarget(tmp))
        {
            target = tmp;
            return target; // if the manual target is compatible it overrides everything
        }

        if (!IsValidTarget(target))
        {
            TargetManager.Enqueue(this, ability.category);
            return null;
        }

        return target; // return the target
    }

    // checks for: if it is the same faction as the ability entity, 
    // if it's dead, if it is weapon-compatible, if it is invisible
    bool IsValidTarget(Transform t)
    {
        if (t == null || !t || !ability || !ability.Core)
        {
            return false;
        }

        IDamageable damageable = t.GetComponent<IDamageable>();
        return (damageable != null
                && !damageable.GetIsDead()
                && damageable.GetTransform() != ability.Core.GetTransform()
                && ability.Core != damageable as Entity
                && !FactionManager.IsAllied(damageable.GetFaction(), ability.Core.faction)
                && ability.CheckCategoryCompatibility(damageable)
                && (t.position - ability.transform.position).magnitude <= ability.GetRange()
                && !damageable.GetInvisible());
    }

    public Entity GetEntity()
    {
        return (ability && ability.Core) ? ability.Core : null;
    }

    public WeaponAbility GetAbility()
    {
        return ability;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
