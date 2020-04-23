using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldAnimations : MonoBehaviour
{
    public OverworldManager om;
    public GameManager gm;

    public IEnumerator EarlyEvents(int id)
    {
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.om.dm.Panel.SetActive(false);
        GameObject obj;
        GameObject player = GameObject.Find("TheWhiteKnight1(Clone)");
        Vector3 origPlayerPos;
        Vector3 playerMoveTar;
        switch (id)
        {
            case 39:
            case 40:
                playerMoveTar = this.gm.om.GetCurrentNode().GetComponent<WorldNode>().transform.position;
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 41:
            case 42:
                playerMoveTar = this.gm.om.GetCurrentNode().GetComponent<WorldNode>().transform.position;
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            default:
                break;
        }
        yield break;

    }

    public IEnumerator LateEvents(int id)
    {
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.om.dm.Panel.SetActive(false);
        GameObject obj;
        GameObject player = GameObject.Find("TheWhiteKnight1(Clone)");
        Vector3 origPlayerPos;
        Vector3 playerMoveTar;
        switch (id)
        {
            case 13: // 1st Goblin Bush kicked
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/goblin") as GameObject, om.player.transform.position, Quaternion.identity);
                obj.transform.Rotate(0, 0, -90);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hit, this.gm.sm.effectsVolume);
                while (obj.transform.position != om.player.transform.position + new Vector3(1.25f, .4f, 0))
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, om.player.transform.position + new Vector3(1.25f, .4f, 0), 2f * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(.75f);
                break;
            case 15: // 2nd Goblin Bush kicked
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/goblin") as GameObject, om.player.transform.position, Quaternion.identity);
                obj.transform.Rotate(0, 0, -90);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hit, this.gm.sm.effectsVolume);
                while (obj.transform.position != om.player.transform.position + new Vector3(3.25f, 5f, 0))
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, om.player.transform.position + new Vector3(3.25f, 5f, 0), 2f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;
            case 30:
                this.gm.om.TurnPlayer(this.gm.om.player, 2);
                yield return new WaitForSeconds(.75f);
                break;
            case 31: // Crow flies away
                obj = GameObject.Find("crow");
                Vector3 crowFliesAway = obj.transform.position + new Vector3(-2, 2f, 0);
                this.gm.om.TurnPlayer(obj, 2);
                while (obj.transform.position != crowFliesAway)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowFliesAway, 2f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return null;
                break;
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
            case 246: // Crow flies and misses player
                obj = GameObject.Find("crow");
                origPlayerPos = player.transform.position;
                Vector3 crowFleeTar = origPlayerPos + new Vector3(2f, -2f, 0);
                playerMoveTar = player.transform.position + new Vector3(0.15f, -0.15f, 0f);
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowFleeTar || player.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowFleeTar, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return new WaitForSeconds(.75f);
                break;
            case 247: // Crow attacks player
                obj = GameObject.Find("crow");
                Vector3 crowAttack1 = player.transform.position + new Vector3(0, 0.25f, 0);
                Vector3 crowAttack2 = crowAttack1 + new Vector3(-0.25f, 0.25f, 0);
                Vector3 crowFlee2 = obj.transform.position + new Vector3(2, -2.5f, 0);
                origPlayerPos = player.transform.position;
                playerMoveTar = origPlayerPos + new Vector3(0.15f, -0.15f, 0);
                while (obj.transform.position != crowAttack1)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowAttack1, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowAttack2 || player.transform.position != playerMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowAttack2, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowAttack1 || player.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowAttack1, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowAttack2 || player.transform.position != playerMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowAttack2, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowAttack1 || player.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowAttack1, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowAttack2 || player.transform.position != playerMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowAttack2, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != crowFlee2 || player.transform.position != origPlayerPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowFlee2, 2f * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return new WaitForSeconds(.75f);
                break;
            case 34:
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/crow_twine") as GameObject, new Vector3(20.5f,4.75f,0), Quaternion.identity);
                this.om.TurnPlayer(obj, 0);
                crowMoveTar = new Vector3(23.5f, 4.25f, 0);
                while (obj.transform.position != crowMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                this.om.TurnPlayer(player, 1);
                yield return new WaitForSeconds(.75f);
                break;
            case 35:
                obj = GameObject.Find("crow_twine(Clone)");
                this.om.TurnPlayer(obj, 2);
                crowMoveTar = new Vector3(20.5f, 4.75f, 0);
                while (obj.transform.position != crowMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return new WaitForSeconds(.75f);
                break;
            case 36:
                obj = GameObject.Find("crow_twine(Clone)");
                GameObject temp = Instantiate(Resources.Load("Prefabs/OverworldCharacters/crow") as GameObject, obj.transform.position, Quaternion.identity);
                Destroy(obj);
                obj = temp;
                this.om.TurnPlayer(obj, 0);
                yield return new WaitForSeconds(.75f);
                break;
            case 55:
                GameObject bandit1 = GameObject.Find("bandit_scout 1");
                GameObject bandit2 = GameObject.Find("bandit_thug");
                GameObject bandit3 = GameObject.Find("bandit_thug (1)");
                GameObject bandit4 = GameObject.Find("bandit_thug (2)");
                bandit1.transform.Rotate(0, 0, -75);
                bandit2.transform.Rotate(0, 0, 60);
                bandit3.transform.Rotate(0, 0, -40);
                bandit4.transform.Rotate(0, 0, 80);
                bandit1.transform.position = new Vector3(30,15.25f, 0);
                bandit2.transform.position = new Vector3(29.75f,15.25f, 0);
                bandit3.transform.position = new Vector3(30.1f,15.05f, 0);
                bandit4.transform.position = new Vector3(30,15.65f, 0);
                yield return new WaitForSeconds(.75f);
                break;
            case 63:
                obj = GameObject.Find("bandit_leader");
                obj.transform.Rotate(0, 0, 75);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.fall, this.gm.sm.effectsVolume);
                yield return new WaitForSeconds(.75f);
                break;
            default:
                break;
        }
        yield break;

    }
}
