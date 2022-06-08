using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private float flickerTimer = 3f;
    [SerializeField] private float seizureTimer;
    [SerializeField] private int percentageChanceOfFlicker = 75;
    [SerializeField] private float darknessDurationModifier = 1f;
    
    private float seizureCooldown;
    private float seizureDice;
    private bool seizure = false;
    private float flickerCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        flickerCooldown = flickerTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (flickerCooldown >= 0)
        {
            flickerCooldown -= Time.deltaTime * darknessDurationModifier;
        }
        else
        {
            seizureDice = Random.Range(1, 100);
            seizure = seizureDice > percentageChanceOfFlicker;

            flickerCooldown = flickerTimer;
            SwitchLightState();
        }

        if (seizureCooldown >= 0)
        {
            seizureCooldown -= Time.deltaTime;
        }

        if (seizure && seizureCooldown <= 0)
        {
            SwitchLightState();
        }
    }

    private void SwitchLightState()
    {
        lightSource.enabled = !lightSource.enabled;
        seizureCooldown = seizureTimer;
    }
}
