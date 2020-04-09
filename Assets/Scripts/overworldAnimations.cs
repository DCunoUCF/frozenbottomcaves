using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldAnimations : MonoBehaviour
{
    public OverworldManager om;
    public IEnumerator events(int id)
    {
        this.om.dm.Panel.SetActive(false);
        switch (id)
        {
            case 13:
                GameObject obj = Instantiate(Resources.Load("Prefabs/Enemies/goblin") as GameObject, om.player.transform.position, Quaternion.identity);
                obj.transform.Rotate(0, 0, -90);
                while (obj.transform.position != om.player.transform.position + new Vector3(1.25f, .4f, 0))
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, om.player.transform.position + new Vector3(1.25f, .4f, 0), 2f * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(.75f);
                break;
            default:
                break;
        }
        yield break;

    }
}
