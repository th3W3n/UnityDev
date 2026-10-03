using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ToggleFireParticle : MonoBehaviour
{
    [Header("Particle Systems")]
    [Tooltip("List of particle systems to toggle on/off")]
    public List<ParticleSystem> fireParticleSystems = new List<ParticleSystem>();
    
    [Tooltip("Particle effect played when toggling state")]
    public ParticleSystem toggleEffectParticle;

    [Header("Lights")]
    [Tooltip("List of lights to toggle on/off")]
    public List<Light2D> lights = new List<Light2D>();

    [Header("Input Settings")]
    [Tooltip("Key to toggle fire on/off")]
    public Key toggleKey = Key.Space;

    private bool isPlaying = true;
    private InputAction toggleAction;

    private void Awake()
    {
        // Create input action with the specified key binding
        toggleAction = new InputAction(
            name: "ToggleFire",
            binding: $"<Keyboard>/{toggleKey.ToString().ToLower()}"
        );
        
        toggleAction.performed += OnTogglePerformed;
    }

    private void OnEnable()
    {
        toggleAction?.Enable();
    }

    private void OnDisable()
    {
        toggleAction?.Disable();
    }

    private void OnDestroy()
    {
        if (toggleAction != null)
        {
            toggleAction.performed -= OnTogglePerformed;
            toggleAction.Dispose();
        }
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        if (isPlaying)
        {
            // Turn off fire
            foreach (ParticleSystem ps in fireParticleSystems)
            {
                if (ps != null)
                    ps.Stop();
            }

            foreach (Light2D light in lights)
            {
                if (light != null)
                    light.enabled = false;
            }

            if (toggleEffectParticle != null)
                toggleEffectParticle.Play();

            isPlaying = false;
        }
        else
        {
            // Turn on fire
            foreach (ParticleSystem ps in fireParticleSystems)
            {
                if (ps != null)
                    ps.Play();
            }

            foreach (Light2D light in lights)
            {
                if (light != null)
                    light.enabled = true;
            }

            if (toggleEffectParticle != null)
                toggleEffectParticle.Play();

            isPlaying = true;
        }
    }

    // Allow runtime key rebinding
    public void SetToggleKey(Key newKey)
    {
        toggleKey = newKey;
        toggleAction.ApplyBindingOverride($"<Keyboard>/{newKey.ToString().ToLower()}");
    }
}