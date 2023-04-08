using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    [SerializeField] float defaultLifetime = 5f;
    [SerializeField] float extendedLifetime = 10f;
    Quaternion rotationAdjust = Quaternion.Euler(90f, 0f, 0f);

    public void ControlProjectileLife(GameObject projectile, ProjectileCollision collisionScript, TrailRenderer currentTrail, Transform parent)
    {
        StartCoroutine(ControlProjectileLifeCoroutine(projectile, collisionScript, currentTrail, parent));
    }

    IEnumerator ControlProjectileLifeCoroutine(GameObject projectile, ProjectileCollision collisionScript, TrailRenderer currentTrail, Transform parent)
    {
        currentTrail.emitting = true; 
        float lifetimeSinceFiring = 0f;
        float maxLifetime = defaultLifetime;

        while (lifetimeSinceFiring < maxLifetime)
        {
            if (collisionScript.HasImpaled)
            {
                if (!collisionScript.HitObject.activeSelf) //if object you impaled is disabled, all arrows in it are disabled
                {
                    currentTrail.emitting = false;
                    DeactivateObject(projectile, parent);
                    yield break;
                }

                if (collisionScript.CreatureWasHit) //if creature is hit, arrow life extends
                {
                    maxLifetime = extendedLifetime;
                }
            }

            lifetimeSinceFiring += Time.deltaTime;
            yield return null;
        }

        currentTrail.emitting = false;
        DeactivateObject(projectile, parent);
    }

    void DeactivateObject(GameObject projectile, Transform parent)
    {
        projectile.SetActive(false);
        projectile.transform.SetParent(parent);
        projectile.transform.rotation = parent.transform.rotation * rotationAdjust;
        projectile.transform.position = parent.transform.position;
    }
}
