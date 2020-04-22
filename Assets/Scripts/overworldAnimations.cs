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
        GameObject obj;
        GameObject player = GameObject.Find("TheWhiteKnight1(Clone)");
        switch (id)
        {
            case 13: // 1st Goblin Bush kicked
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
            case 15: // 2nd Goblin Bush kicked
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
/*            case 31: // Crow flies away
                obj = GameObject.Find("crow");
                while (obj.transform.position != om.player.transform.position)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, om.player.transform.position + new Vector3(1.25f, .4f, 0), 2f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;*/
            case 244: // Crow flies halfway towards character, AGI roll
                obj = GameObject.Find("crow");
                Vector3 crowMoveTar = obj.transform.position + (om.player.transform.position - obj.transform.position) / 2;
                while (obj.transform.position != crowMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;
/*            case 246: // Crow flies and misses player
                obj = GameObject.Find("crow");
                Vector3 origPlayerPos = player.transform.position;
                while (player.transform.position != player.transform.position + new Vector3(0.5f, -0.25f, 0f))
                {
                    obj.transform.position = Vector3.MoveTowards(player.transform.position, player.transform.position + new Vector3(0.5f, -0.25f, 0f), 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != obj.transform.position + new Vector3(5f, 10f, 0))
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, obj.transform.position + new Vector3(5f, 10f, 0), 2f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;*/
            default:
                break;
        }
        yield break;

    }
}
