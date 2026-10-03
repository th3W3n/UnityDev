using UnityEngine;

public class Rotator : MonoBehaviour
{
    private void Update()
    {
        float x = Random.Range(10f, 20f);
        float y = Random.Range(25f, 35f);
        float z = Random.Range(40f, 50f);
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);
    }
}