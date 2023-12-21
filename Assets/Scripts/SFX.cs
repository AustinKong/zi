using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance;
    private void Awake()
    {
        instance = this;
    }

    public AudioSource hitHurt;
    public AudioSource pickup;
    public AudioSource death;
    public AudioSource altarUse;
    public AudioSource fire;
    public AudioSource rock;
    public AudioSource poison;

    public void PlayHitHurt() => hitHurt.Play();

    public void PlayDeath() => death.Play();

    public void PlayPickup() => pickup.Play();

    public void PlayAltarUse() => altarUse.Play();

    public void PlayFire() => fire.Play();

    public void PlayRock() => rock.Play();

    public void PlayPoison() => poison.Play();
}
