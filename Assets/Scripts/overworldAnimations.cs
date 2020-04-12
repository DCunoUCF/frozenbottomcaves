using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldAnimations : MonoBehaviour
{
    public OverworldManager om;
    public GameManager gm;
    public IEnumerator events(int id)
    {
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.om.dm.Panel.SetActive(false);
        switch (id)
        {
            case 13:
                GameObject obj1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/goblin") as GameObject, om.player.transform.position, Quaternion.identity);
                obj1.transform.Rotate(0, 0, -90);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hit, this.gm.sm.effectsVolume);
                while (obj1.transform.position != om.player.transform.position + new Vector3(1.25f, .4f, 0))
                {
                    obj1.transform.position = Vector3.MoveTowards(obj1.transform.position, om.player.transform.position + new Vector3(1.25f, .4f, 0), 2f * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(.75f);
                break;
            case 15:
                GameObject obj2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/goblin") as GameObject, om.player.transform.position, Quaternion.identity);
                obj2.transform.Rotate(0, 0, -90);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hit, this.gm.sm.effectsVolume);
                while (obj2.transform.position != om.player.transform.position + new Vector3(1.25f, .4f, 0))
                {
                    obj2.transform.position = Vector3.MoveTowards(obj2.transform.position, om.player.transform.position + new Vector3(1.25f, .4f, 0), 2f * Time.deltaTime);
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
