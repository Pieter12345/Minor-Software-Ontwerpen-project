using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour
{

    public float frameDelay;
    public Texture[] frame;
    private int i = 0;
    private int j = 0;

    void Update()
    {
        j++;
        
        if (frame[i] != renderer.material.GetTexture("_MainTex"))
        {
            renderer.material.SetTexture("_MainTex", frame[i]);
        }

        if (j >= frameDelay)
        {
            i++;
            j = 0;

            if (i >= frame.Length)
            {
                i = 0;
            }
        }


    }
}
