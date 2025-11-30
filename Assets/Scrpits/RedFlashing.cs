using UnityEngine;

public class RedFlashing : MonoBehaviour
{
    public Light targetLight;      // Assign the Light here
    public float speed = 1f;       // How fast to transition
    public bool pingPong = true;   // Should it go back and forth?
    private Color colorA = (Color.red + Color.yellow)/4f;
    private Color colorB = Color.red;
    private float t;

    void Update()
    {
        if (targetLight == null) return;

        // Increase t over time
        t += Time.deltaTime * speed;

        // Ping-pong or loop the value
        float lerpValue = pingPong ? Mathf.PingPong(t, 1f) : Mathf.Repeat(t, 1f);

        // Apply color
        targetLight.color = Color.Lerp(colorA, colorB, lerpValue);
    }
}