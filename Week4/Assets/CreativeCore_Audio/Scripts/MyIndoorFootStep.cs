using System.Collections.Generic;
using UnityEngine;

public class MyIndoorFootStep : MonoBehaviour
{
    [SerializeField] private List<AudioClip> indoorFootsteps = new();
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        AudioClip clip = indoorFootsteps[Random.Range(0, indoorFootsteps.Count)];
        source.PlayOneShot(clip);
    }
    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if (hit.transform.tag == "Grass") surface = Surface.GRASS;
    //     if (hit.transform.tag == "Water") surface = Surface.WATER;
    //     if (hit.transform.tag == "Cave") surface = Surface.CAVE;
    //     SelectStepList();
    // }
}