using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    // This script and an Audio Source component are on the Player prefab
    // UI sounds are played by UI buttons which have OnClick() components added via the Inspector
    // Gameplay sounds are played via classes in the GameManager script 

    // Sound when clicking a button
    public AudioClip clickSound;
    private AudioSource clickAudio;

    // Sound when picking up a token
    public AudioClip tokenSound;
    private AudioSource tokenAudio;

    // Sound when eating food or getting power up
    public AudioClip otherSound;
    private AudioSource otherAudio;

    // Sound when destroying a NIMBY
    public AudioClip destroyNimbySound;
    private AudioSource destroyNimbyAudio;

    // Sound when being attacked by a NIMBY
    public AudioClip nimbyAttackSound;
    private AudioSource nimbyAttackAudio;

    // Start is called before the first frame update
    void Start()
    {
        clickAudio = GetComponent<AudioSource>();
        tokenAudio = GetComponent<AudioSource>();
        otherAudio = GetComponent<AudioSource>();
        destroyNimbyAudio = GetComponent<AudioSource>();
        nimbyAttackAudio = GetComponent<AudioSource>();
    }

    public void playClickSound()
    {
        clickAudio.PlayOneShot(clickSound);
    }

    public void playTokenSound()
    {
        tokenAudio.PlayOneShot(tokenSound);
    }

    public void playOtherSound()
    {
        otherAudio.PlayOneShot(otherSound);
    }

    public void playDestroyNimbySound()
    {
        destroyNimbyAudio.PlayOneShot(destroyNimbySound, 1.0f);
    }

    public void playNimbyAttackSound()
    {
        nimbyAttackAudio.PlayOneShot(nimbyAttackSound, 1.0f);
    }
}
