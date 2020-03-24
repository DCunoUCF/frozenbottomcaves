using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMashingTest : MonoBehaviour
{
    private float max = 100f;
    private float start = 50f;
    private float current;
    private bool done, ready;

    // Start is called before the first frame update
    void Start()
    {
        current = start;
        ready = false;
        StartCoroutine("decrementTimer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
            current += 3f;

        if (!done)
            transform.localScale = new Vector3(current / max, 1f, 1f);

        if (current <= 0)
        {
            transform.localScale = new Vector3(0f, 1f, 1f);
            done = true;
            print("YOU LOSER :)");
        }
        else if (current >= 100f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            done = true;
            print("YOU WINNER :)");
        }
    }

    IEnumerator decrementTimer()
    {
        while (!done)
        {
            yield return new WaitForSeconds(.015f);
            current -= .15f;
        }
        yield break;
    }
}
