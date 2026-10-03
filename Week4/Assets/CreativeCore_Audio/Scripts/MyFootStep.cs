using System.Collections.Generic;
using UnityEngine;

public class MyFootStep : MonoBehaviour
{
    [SerializeField] private List<AudioClip> grassSteps = new();
    [SerializeField] private List<AudioClip> waterSteps = new();
    [SerializeField] private List<AudioClip> caveSteps = new();
    private List<AudioClip> currentAudioList;
    private AudioSource source;

    private enum Surface { GRASS, WATER, CAVE }
    private Surface surface;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        if (currentAudioList == null) return;

        AudioClip clip = currentAudioList[Random.Range(0, currentAudioList.Count)];
        source.PlayOneShot(clip);
    }
    private void SelectStepList()
    {
        switch (surface)
        {
            case Surface.GRASS: currentAudioList = grassSteps; break;
            case Surface.WATER: currentAudioList = waterSteps; break;
            case Surface.CAVE: currentAudioList = caveSteps; break;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Grass") surface = Surface.GRASS;
        if (hit.transform.tag == "Water") surface = Surface.WATER;
        if (hit.transform.tag == "Cave") surface = Surface.CAVE;
        SelectStepList();
    }
}