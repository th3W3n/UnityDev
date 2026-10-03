using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public List<AudioClip> grassSteps = new List<AudioClip>();
    public List<AudioClip> waterSteps = new List<AudioClip>();
    public List<AudioClip> caveSteps = new List<AudioClip>();
    
    private enum Surface { grass, water, cave, none };
    private Surface currentSurface = Surface.none;
    private List<AudioClip> currentList;
    private AudioSource source;
    
    // Track which surfaces we're currently touching
    private HashSet<Surface> activeSurfaces = new HashSet<Surface>();
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
        // Default to grass surface
        currentSurface = Surface.grass;
        SelectStepList();
    }
    
    public void PlayStep()
    {
        if (currentList == null || currentList.Count == 0)
            return;
        
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);
    }
    
    private void SelectStepList()
    {
        switch (currentSurface)
        {
            case Surface.grass:
                currentList = grassSteps;
                break;
            case Surface.water:
                currentList = waterSteps;
                break;
            case Surface.cave:
                currentList = caveSteps;
                break;
            default:
                currentList = grassSteps; // Default to grass
                break;
        }
    }
    
    private Surface GetHighestPrioritySurface()
    {
        // Priority order: cave > water > grass
        if (activeSurfaces.Contains(Surface.cave))
            return Surface.cave;
        else if (activeSurfaces.Contains(Surface.water))
            return Surface.water;
        else if (activeSurfaces.Contains(Surface.grass))
            return Surface.grass;
        else
            return Surface.grass; // Default fallback
    }
    
    // 2D collision detection using triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        Surface detectedSurface = TagToSurface(other.tag);
        if (detectedSurface != Surface.none)
        {
            activeSurfaces.Add(detectedSurface);
            UpdateCurrentSurface();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Surface detectedSurface = TagToSurface(other.tag);
        if (detectedSurface != Surface.none)
        {
            activeSurfaces.Remove(detectedSurface);
            UpdateCurrentSurface();
        }
    }
    
    private Surface TagToSurface(string tag)
    {
        if (tag == "Grass")
            return Surface.grass;
        else if (tag == "Water")
            return Surface.water;
        else if (tag == "Cave")
            return Surface.cave;
        else
            return Surface.none;
    }
    
    private void UpdateCurrentSurface()
    {
        Surface newSurface = GetHighestPrioritySurface();
        
        if (newSurface != currentSurface)
        {
            currentSurface = newSurface;
            SelectStepList();
        }
    }
}