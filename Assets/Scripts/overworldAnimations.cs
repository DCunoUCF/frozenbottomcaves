using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldAnimations : MonoBehaviour
{
    public OverworldManager om;
    public GameManager gm;
    private bool alreadyProne;

    public IEnumerator EarlyEvents(int id)
    {
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.om.dm.Panel.SetActive(false);
        GameObject obj;
        GameObject player = GameObject.Find("TheWhiteKnight1(Clone)");
        Vector3 origPlayerPos;
        Vector3 origObjPos;
        Vector3 playerMoveTar;
        switch (id)
        {
            case 6: // Spin jump puddle
                float startTime = Time.time;
                float journeyTime = .275f;
                origPlayerPos = player.transform.position;
                Vector3 playerHop = this.gm.om.GetCurrentNode().GetComponent<WorldNode>().transform.position;
                while (player.transform.position != origPlayerPos)
                {
                    // Couldn't get him to spin yet... too much other stuff to do.
                    //if (Random.value < 0.25f)
                    //    this.om.TurnPlayer(player, ++i % 4);

                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = origPlayerPos - center;
                    Vector3 endRelCenter = playerHop - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                this.om.TurnPlayer(player, 2);
                yield return null;
                break;
            case 37: // Knight on driftwood
                GameObject temp = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight_Driftwood") as GameObject, player.transform.position, Quaternion.identity);
                temp.name = "TheWhiteKnight1(Clone)";
                this.gm.pm.player = temp;
                this.gm.om.player = temp;
                GameObject.Find("MainCameraOW").GetComponent<OWCamera>().target = temp.GetComponent<Transform>();
                Destroy(player);
                player = temp;
                yield return null;
                break;
            case 38: // Knight on Raft
                temp = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight_Raft") as GameObject, player.transform.position, Quaternion.identity);
                temp.name = "TheWhiteKnight1(Clone)";
                this.gm.pm.player = temp;
                this.gm.om.player = temp;
                GameObject.Find("MainCameraOW").GetComponent<OWCamera>().target = temp.GetComponent<Transform>();
                Destroy(player);
                player = temp;
                yield return null;
                break;
            case 39: // Slide across raft river
            case 40:
                playerMoveTar = this.gm.om.GetCurrentNode().GetComponent<WorldNode>().transform.position;
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 41: // Slide across raft river
            case 42:
                playerMoveTar = this.gm.om.GetCurrentNode().GetComponent<WorldNode>().transform.position;
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 151: // Slide and spin down tidal wave river
                playerMoveTar = this.gm.om.GetCurrentNode().GetComponent<WorldNode>().transform.position;
                int i = 0;
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);

                    if(Random.value < 0.25f)
                        this.om.TurnPlayer(player, ++i % 4);

                    yield return null;
                }
                this.om.TurnPlayer(player, 3);
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
        Vector3 origObjPos;
        Vector3 playerMoveTar;
        Vector3 objMoveTar;
        GameObject temp;
        switch (id)
        {
            case 7: // Player falls into the puddle
                Quaternion playerRotate = player.transform.rotation;
                player.transform.Rotate(0, 0, 90);
                yield return new WaitForSeconds(2f);
                while (player.transform.rotation.z > playerRotate.z)
                {
                    player.transform.Rotate(new Vector3(0,0,-90) * (2f * Time.deltaTime));
                    yield return null;
                }
                player.transform.Rotate(Vector3.zero); // Making sure the player is perfectly upright
                yield return null;
                break;
            case 8:
                GameObject sl1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/slime_g") as GameObject, new Vector3(1, 8.25f, 0), Quaternion.identity);
                GameObject sl2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/slime_g") as GameObject, new Vector3(1f, 7.5f, 0), Quaternion.identity);
                sl1.name = "sl1";
                sl2.name = "sl2";
                sl1.transform.SetParent(om.EntitiesParent);
                sl2.transform.SetParent(om.EntitiesParent);
                Vector3 sl1MoveTar = new Vector3(0f, 8.25f, 0);
                Vector3 sl2MoveTar = new Vector3(1f, 7.75f, 0);
                this.om.TurnPlayer(sl1, 0);
                this.om.TurnPlayer(sl2, 2);
                while (sl1.transform.position != sl1MoveTar || sl2.transform.position != sl2MoveTar)
                {
                    sl1.transform.position = Vector3.MoveTowards(sl1.transform.position, sl1MoveTar, 4f * Time.deltaTime);
                    sl2.transform.position = Vector3.MoveTowards(sl2.transform.position, sl2MoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 10:
                sl1 = GameObject.Find("sl1");
                sl2 = GameObject.Find("sl2");
                sl1.transform.Rotate(0, 0, 80);
                sl2.transform.Rotate(0, 0, -120);
                yield return null;
                break;
            case 13: // 1st Goblin Bush kicked
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/goblin") as GameObject, om.player.transform.position, Quaternion.identity);
                obj.transform.Rotate(0, 0, -90);
                obj.transform.SetParent(om.EntitiesParent);
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
                obj.transform.SetParent(om.EntitiesParent);
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
            case 41:
            case 42: // Knight gets off raft/driftwood
                yield return new WaitForSeconds(1f);
                temp = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight1") as GameObject, player.transform.position, Quaternion.identity);
                temp.name = "TheWhiteKnight1(Clone)";
                this.gm.pm.player = temp;
                this.gm.om.player = temp;
                this.om.playerX = this.om.pathMap[41].X;
                this.om.playerY = this.om.pathMap[41].Y;
                GameObject.Find("MainCameraOW").GetComponent<OWCamera>().target = temp.GetComponent<Transform>();
                Destroy(player);
                player = temp;
                yield return null;
                break;
            case 114: // Nixies approach player
                GameObject n1 = GameObject.Find("nixie");
                GameObject n2 = GameObject.Find("nixie (1)");
                GameObject n3 = GameObject.Find("nixie (2)");
                Vector3 n1MoveTar = n1.transform.position + new Vector3(0.5f,-0.25f,0);
                Vector3 n2MoveTar = n2.transform.position + new Vector3(0.5f,-0.25f,0);
                Vector3 n3MoveTar = n3.transform.position + new Vector3(0.5f,-0.25f,0);
                while (n1.transform.position != n1MoveTar || n2.transform.position != n2MoveTar || n3.transform.position != n3MoveTar)
                {
                    n1.transform.position = Vector3.MoveTowards(n1.transform.position, n1MoveTar, 2f * Time.deltaTime);
                    n2.transform.position = Vector3.MoveTowards(n2.transform.position, n2MoveTar, 2f * Time.deltaTime);
                    n3.transform.position = Vector3.MoveTowards(n3.transform.position, n3MoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 112:
            case 119: // Nixies fly into the forest
            case 120:
                n1 = GameObject.Find("nixie");
                n2 = GameObject.Find("nixie (1)");
                n3 = GameObject.Find("nixie (2)");
                n1MoveTar = n1.transform.position + new Vector3(-3.5f, 0.75f, 0);
                n2MoveTar = n2.transform.position + new Vector3(-3f, 2f, 0);
                n3MoveTar = n3.transform.position + new Vector3(-1.5f, 2f, 0);
                this.om.TurnPlayer(n1, 2);
                this.om.TurnPlayer(n2, 2);
                this.om.TurnPlayer(n3, 2);
                while (n1.transform.position != n1MoveTar || n2.transform.position != n2MoveTar || n3.transform.position != n3MoveTar)
                {
                    n1.transform.position = Vector3.MoveTowards(n1.transform.position, n1MoveTar, 4f * Time.deltaTime);
                    n2.transform.position = Vector3.MoveTowards(n2.transform.position, n2MoveTar, 4f * Time.deltaTime);
                    n3.transform.position = Vector3.MoveTowards(n3.transform.position, n3MoveTar, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(n1);
                Destroy(n2);
                Destroy(n3);
                yield return null;
                break;
            case 124: // Looking towards the wall of water approaching
                this.om.TurnPlayer(player, 0);
                yield return null;
                break;
            case 125: // Player sees wave coming up
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/tidal_wave") as GameObject, new Vector3(12, 13.5f, 0), Quaternion.identity);
                objMoveTar = new Vector3(10.75f, 14.25f, 0);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 1f * Time.deltaTime);
                    yield return null;
                }
                break;
            case 127:
            case 128: // Tidal wave washes past player.
            case 130: // STR and AGI
            case 131:
                obj = GameObject.Find("tidal_wave(Clone)");
                objMoveTar = new Vector3(3f, 19.75f, 0);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.tidalWave, this.gm.sm.effectsVolume);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 1f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                break;
            case 133: // INT Ducking underneath the wave and surviving. Tidal wave passes by
                playerRotate = player.transform.rotation;
                player.transform.Rotate(0, 0, -90);
                obj = GameObject.Find("tidal_wave(Clone)");
                objMoveTar = new Vector3(3f, 19.75f, 0);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.tidalWave, this.gm.sm.effectsVolume);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 1f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                while (player.transform.rotation.z < playerRotate.z)
                {
                    player.transform.Rotate(new Vector3(0, 0, 90) * (2f * Time.deltaTime));
                    yield return null;
                }
                player.transform.Rotate(Vector3.zero);  // Making sure the player is perfectly upright
                yield return null;
                break;
            case 134: // INT Ducking underneath the wave and failing. Tidal wave passes by
                alreadyProne = true;
                player.transform.Rotate(0, 0, -90);
                obj = GameObject.Find("tidal_wave(Clone)");
                objMoveTar = new Vector3(3f, 19.75f, 0);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.tidalWave, this.gm.sm.effectsVolume);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 1f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return null;
                break;
            case 141:
                bool gobOrig = true;
                GameObject sp = GameObject.Find("spider_small");
                GameObject gob = GameObject.Find("goblin");
                Vector3 origGobPos = gob.transform.position;
                Vector3 origSpPos = sp.transform.position;
                Vector3 gobMoveTar = origGobPos + new Vector3(-0.125f, 0.06f, 0);
                Vector3 spMoveTar = origSpPos + new Vector3(0.2f, 0, 0);
                for (int j = 0; j < 3; j++)
                {
                    while (gob.transform.position != gobMoveTar || sp.transform.position != spMoveTar)
                    {
                        if(gobOrig)
                        {
                            gob.transform.position = Vector3.MoveTowards(gob.transform.position, gobMoveTar, 4f * Time.deltaTime);

                            if (gob.transform.position == gobMoveTar)
                                gobOrig = false;
                        }
                        else
                        {
                            gob.transform.position = Vector3.MoveTowards(gob.transform.position, origGobPos, 1f * Time.deltaTime);

                            if (gob.transform.position == origGobPos)
                                gobOrig = true;
                        }

                        sp.transform.position = Vector3.MoveTowards(sp.transform.position, spMoveTar, 4f * Time.deltaTime);
                        yield return null;
                    }
                    while (gob.transform.position != origGobPos || sp.transform.position != origSpPos)
                    {
                        if (gobOrig)
                        {
                            gob.transform.position = Vector3.MoveTowards(gob.transform.position, gobMoveTar, 4f * Time.deltaTime);

                            if (gob.transform.position == gobMoveTar)
                                gobOrig = false;
                        }
                        else
                        {
                            gob.transform.position = Vector3.MoveTowards(gob.transform.position, origGobPos, 4f * Time.deltaTime);

                            if (gob.transform.position == origGobPos)
                                gobOrig = true;
                        }

                        sp.transform.position = Vector3.MoveTowards(sp.transform.position, origSpPos, 1f * Time.deltaTime);
                        yield return null;
                    }
                }
                yield return null;
                break;
            case 143:
                sp = GameObject.Find("spider_small");
                gob = GameObject.Find("goblin");
                GameObject gob2 = GameObject.Find("goblin (1)");
                sp.transform.Rotate(0, 0, -90);
                gob.transform.Rotate(0, 0, 110);
                gob2.transform.Rotate(0, 0, -90);
                yield return null;
                break;
            case 145:
                sp = GameObject.Find("spider_small");
                gob = GameObject.Find("goblin");
                gob2 = GameObject.Find("goblin (1)");
                spMoveTar = new Vector3(15f, 24.25f, 0);
                origPlayerPos = player.transform.position;
                playerMoveTar = origPlayerPos + new Vector3(0.125f,0.06f,0);
                gobMoveTar = gob.transform.position + new Vector3(-3f,3f,0);
                Vector3 gob2MoveTar = gob.transform.position + new Vector3(-3f,3f,0);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.miss, this.gm.sm.effectsVolume);
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 4f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != origPlayerPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 4f * Time.deltaTime);
                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.collide, this.gm.sm.effectsVolume);
                this.om.TurnPlayer(sp, 1);
                while (sp.transform.position != spMoveTar)
                {
                    sp.transform.position = Vector3.MoveTowards(sp.transform.position, spMoveTar, 1f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(1f);
                this.om.TurnPlayer(gob, 1);
                this.om.TurnPlayer(gob2, 1);
                yield return new WaitForSeconds(1f);
                this.om.TurnPlayer(gob, 2);
                this.om.TurnPlayer(gob2, 2);
                while (gob.transform.position != gobMoveTar || gob2.transform.position != gob2MoveTar)
                {
                    gob.transform.position = Vector3.MoveTowards(gob.transform.position, gobMoveTar, 2f * Time.deltaTime);
                    gob2.transform.position = Vector3.MoveTowards(gob2.transform.position, gob2MoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                Destroy(gob);
                Destroy(gob2);
                yield return null;
                break;
            case 147:
                sp = GameObject.Find("spider_small");
                sp.transform.Rotate(0, 0, -90);
                yield return null;
                break;
            case 135: // Coughing up water and standing up
            case 151:
                playerRotate = Quaternion.identity;
                if(!alreadyProne)
                    player.transform.Rotate(0, 0, -90);
                yield return new WaitForSeconds(2f);
                while (player.transform.rotation.z < playerRotate.z)
                {
                    player.transform.Rotate(new Vector3(0, 0, 90) * (2f * Time.deltaTime));
                    yield return null;
                }
                player.transform.Rotate(Vector3.zero); // Making sure the player is perfectly upright
                yield return null;
                break;
            case 181: // Fail AGI across troll bridge
            case 187: // Pretend to not see troll
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/troll") as GameObject, new Vector3(1.5f,21.75f,0), Quaternion.identity);
                objMoveTar = new Vector3(1,22.25f,0);
                obj.transform.SetParent(om.EntitiesParent);
                this.om.TurnPlayer(obj, 1);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 3f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 156: // Come up from troll's comfy stump 
            case 180: // Succeed AGI across troll bridge
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/troll") as GameObject, new Vector3(0.5f, 22.5f, 0), Quaternion.identity);
                objMoveTar = new Vector3(4.5f, 24.5f, 0);
                obj.transform.SetParent(om.EntitiesParent);
                this.om.TurnPlayer(obj, 3);
                this.om.TurnPlayer(player, 1);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 3f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 160: // Player sees black wolf on bone pile
                this.om.TurnPlayer(player, 1);
                yield return null;
                break;
            case 161: // Black wolf calls to other wolves
                GameObject w1 = GameObject.Find("wolf_black");
                GameObject w2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/wolf_black") as GameObject, new Vector3(-11f, 13f, 0), Quaternion.identity);
                GameObject w3 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/wolf_black") as GameObject, new Vector3(-11f, 12f, 0), Quaternion.identity);
                GameObject w4 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/wolf_black") as GameObject, new Vector3(-8f, 10f, 0), Quaternion.identity);
                w2.name = "w2";
                w3.name = "w3";
                w4.name = "w4";
                w2.transform.SetParent(om.EntitiesParent);
                w3.transform.SetParent(om.EntitiesParent);
                w4.transform.SetParent(om.EntitiesParent);
                Vector3 w2MoveTar = new Vector3(-9f, 12.75f, 0);
                Vector3 w3MoveTar = new Vector3(-8.5f, 12.5f, 0);
                Vector3 w4MoveTar = new Vector3(-7.5f, 12.5f, 0);
                this.om.TurnPlayer(w2, 3);
                this.om.TurnPlayer(w3, 3);
                this.om.TurnPlayer(w4, 3);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.wolfHowl, this.gm.sm.effectsVolume);
                yield return new WaitForSeconds(5f);
                while (w2.transform.position != w2MoveTar || w3.transform.position != w3MoveTar || w4.transform.position != w4MoveTar)
                {
                    w2.transform.position = Vector3.MoveTowards(w2.transform.position, w2MoveTar, 4f * Time.deltaTime);
                    w3.transform.position = Vector3.MoveTowards(w3.transform.position, w3MoveTar, 4f * Time.deltaTime);
                    w4.transform.position = Vector3.MoveTowards(w4.transform.position, w4MoveTar, 4.5f * Time.deltaTime);
                    yield return null;
                }
                yield return null;
                break;
            case 163: // Player beats black wolves
                w1 = GameObject.Find("wolf_black");
                w2 = GameObject.Find("w2");
                w3 = GameObject.Find("w3");
                w4 = GameObject.Find("w4");
                w2MoveTar = new Vector3(-12f, 13f, 0);
                w3MoveTar = new Vector3(-12f, 12f, 0);
                w4MoveTar = new Vector3(-12f, 11f, 0);
                this.om.TurnPlayer(w2, 1);
                this.om.TurnPlayer(w3, 1);
                this.om.TurnPlayer(w4, 1);
                w1.transform.position = player.transform.position + new Vector3(-0.5f, -0.25f,0);
                w1.transform.Rotate(0,0,100f);
                while (w2.transform.position != w2MoveTar || w3.transform.position != w3MoveTar || w4.transform.position != w4MoveTar)
                {
                    w2.transform.position = Vector3.MoveTowards(w2.transform.position, w2MoveTar, 4f * Time.deltaTime);
                    w3.transform.position = Vector3.MoveTowards(w3.transform.position, w3MoveTar, 4f * Time.deltaTime);
                    w4.transform.position = Vector3.MoveTowards(w4.transform.position, w4MoveTar, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(w2);
                Destroy(w3);
                Destroy(w4);
                yield return null;
                break;
            case 165: // Player howls, Grey wolf comes in and scares the black wolves
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.personHowl, this.gm.sm.effectsVolume);
                yield return new WaitForSeconds(3f);
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/wolf") as GameObject, new Vector3(-4f, 13.5f, 0), Quaternion.identity);
                obj.transform.SetParent(om.EntitiesParent);
                objMoveTar = new Vector3(-6f, 13.75f, 0);
                this.om.TurnPlayer(obj, 1);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 4f * Time.deltaTime);
                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.wolfSnarl, this.gm.sm.effectsVolume);
                yield return new WaitForSeconds(2.5f);
                w1 = GameObject.Find("wolf_black");
                w2 = GameObject.Find("w2");
                w3 = GameObject.Find("w3");
                w4 = GameObject.Find("w4");
                Vector3 w1MoveTar = new Vector3(-12f, 13.5f, 0);
                w2MoveTar = new Vector3(-12f, 13f, 0);
                w3MoveTar = new Vector3(-12f, 12f, 0);
                w4MoveTar = new Vector3(-12f, 11f, 0);
                this.om.TurnPlayer(w1, 1);
                this.om.TurnPlayer(w2, 1);
                this.om.TurnPlayer(w3, 1);
                this.om.TurnPlayer(w4, 1);
                while (w2.transform.position != w2MoveTar || w3.transform.position != w3MoveTar || w4.transform.position != w4MoveTar || w1.transform.position != w1MoveTar)
                {
                    w1.transform.position = Vector3.MoveTowards(w1.transform.position, w1MoveTar, 4f * Time.deltaTime);
                    w2.transform.position = Vector3.MoveTowards(w2.transform.position, w2MoveTar, 4f * Time.deltaTime);
                    w3.transform.position = Vector3.MoveTowards(w3.transform.position, w3MoveTar, 4f * Time.deltaTime);
                    w4.transform.position = Vector3.MoveTowards(w4.transform.position, w4MoveTar, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(w2);
                Destroy(w3);
                Destroy(w4);
                yield return null;
                break;
            case 166: // Grey wolf looks expectantly up at the player
                obj = GameObject.Find("wolf(Clone)");
                this.om.TurnPlayer(player, 0);
                this.om.TurnPlayer(obj, 2);
                yield return null;
                break;
            case 167: // Grey wolf nudges player's arm and whines
                // dog whine sound
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.wolfWhine, this.gm.sm.effectsVolume);
                yield return null;
                break;
            case 169:// Player kicks grey wolf and wolf runs away
                obj = GameObject.Find("wolf(Clone)");
                origPlayerPos = player.transform.position;
                origObjPos = obj.transform.position;
                playerMoveTar = origPlayerPos + new Vector3(0.125f, -0.06f, 0);
                Vector3 wolfFlinch = origObjPos + new Vector3(0.125f, -0.06f, 0);
                objMoveTar = origObjPos + new Vector3(3.5f, -0.5f, 0);
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hit, this.gm.sm.effectsVolume);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.dogYelp, this.gm.sm.effectsVolume);
                while (player.transform.position != origPlayerPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != wolfFlinch)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, wolfFlinch, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != origObjPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origObjPos, 2f * Time.deltaTime);
                    yield return null;
                }
                this.om.TurnPlayer(obj, 0);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return null;
                break;
            case 170: // Player kicks grey wolf softly and wolf bites player
                obj = GameObject.Find("wolf(Clone)");
                origPlayerPos = player.transform.position;
                origObjPos = obj.transform.position;
                playerMoveTar = origPlayerPos + new Vector3(0.125f, -0.06f, 0);
                wolfFlinch = origObjPos + new Vector3(0.125f, -0.06f, 0);
                Vector3 greyWolfBite = origObjPos + new Vector3(-0.125f, 0.06f, 0);
                Vector3 playerFlinch = origPlayerPos + new Vector3(-0.125f, 0.06f, 0);
                Vector3 wolfFlee = origObjPos + new Vector3(3.5f, -0.5f, 0);
                while (player.transform.position != playerMoveTar)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hit, this.gm.sm.effectsVolume);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.dogYelp, this.gm.sm.effectsVolume);
                while (player.transform.position != origPlayerPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != wolfFlinch)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, wolfFlinch, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != origObjPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origObjPos, 2f * Time.deltaTime);
                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.wolfBite, this.gm.sm.effectsVolume);
                while (obj.transform.position != greyWolfBite)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, greyWolfBite, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != origObjPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origObjPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != playerFlinch)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerFlinch, 2f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != origPlayerPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                this.om.TurnPlayer(obj, 0);
                while (obj.transform.position != wolfFlee)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, wolfFlee, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return null;
                break;
            case 171: // Player attempts to pet the wolf
                obj = GameObject.Find("wolf(Clone)");
                origPlayerPos = player.transform.position;
                origObjPos = obj.transform.position;
                playerMoveTar = origPlayerPos + new Vector3(0.125f, -0.06f, 0);
                wolfFlinch = origObjPos + new Vector3(0.125f, -0.06f, 0);
                greyWolfBite = origObjPos + new Vector3(-0.125f, 0.06f, 0);
                playerFlinch = origPlayerPos + new Vector3(-0.125f, 0.06f, 0);
                wolfFlee = origObjPos + new Vector3(3.5f, -0.5f, 0);
                while (player.transform.position != playerMoveTar || obj.transform.position != wolfFlinch)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerMoveTar, 2f * Time.deltaTime);
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, wolfFlinch, 2f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != origPlayerPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.wolfBite, this.gm.sm.effectsVolume);
                while (obj.transform.position != origObjPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origObjPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != greyWolfBite)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, greyWolfBite, 2f * Time.deltaTime);
                    yield return null;
                }
                while (obj.transform.position != origObjPos)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, origObjPos, 2f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != playerFlinch)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, playerFlinch, 2f * Time.deltaTime);
                    yield return null;
                }
                while (player.transform.position != origPlayerPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 2f * Time.deltaTime);
                    yield return null;
                }
                this.om.TurnPlayer(obj, 0);
                while (obj.transform.position != wolfFlee)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, wolfFlee, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return null;
                break;
            case 175: // player feeds the wolf and it runs away, content
                obj = GameObject.Find("wolf(Clone)");
                origObjPos = obj.transform.position;
                wolfFlee = origObjPos + new Vector3(3.5f, -0.5f, 0);
                this.om.TurnPlayer(obj, 0);
                while (obj.transform.position != wolfFlee)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, wolfFlee, 4f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return null;
                break;
            case 149:
            case 195:
            case 196: // Cackling gnolls
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.cackle, this.gm.sm.effectsVolume);
                yield return null;
                break; 
            case 197: // Loud gnoll cackle player turns around
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.loudCackle, this.gm.sm.effectsVolume);
                origPlayerPos = player.transform.position;
                bool up = false;
                Vector3 upVec = origPlayerPos + new Vector3(0, 0.15f, 0);
                while (!up || player.transform.position != origPlayerPos)
                {
                    if (!up)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, upVec, 1.5f * Time.deltaTime);
                        if (player.transform.position == upVec)
                            up = true;
                    }
                    else
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 1.5f * Time.deltaTime);
                    }

                    yield return null;
                }
                this.om.TurnPlayer(player, 1);
                yield return null;
                break;
            case 199:
                GameObject gnoll1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/Gnoll") as GameObject, new Vector3(12.5f, 30.5f, 0), Quaternion.identity);
                GameObject gnoll2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/Gnoll") as GameObject, new Vector3(16f, 32.75f, 0), Quaternion.identity);
                GameObject gnoll3 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/Gnoll") as GameObject, new Vector3(17f, 28f, 0), Quaternion.identity);
                GameObject gnoll4 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/Gnoll") as GameObject, new Vector3(20f, 30f, 0), Quaternion.identity);
                GameObject gnoll5 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/Gnoll") as GameObject, new Vector3(12.5f, 29.75f, 0), Quaternion.identity);
                gnoll1.name = "gnoll1";
                gnoll2.name = "gnoll2";
                gnoll3.name = "gnoll3";
                gnoll4.name = "gnoll4";
                gnoll5.name = "gnoll5";
                gnoll1.transform.SetParent(om.EntitiesParent);
                gnoll2.transform.SetParent(om.EntitiesParent);
                gnoll3.transform.SetParent(om.EntitiesParent);
                gnoll4.transform.SetParent(om.EntitiesParent);
                gnoll5.transform.SetParent(om.EntitiesParent);
                Vector3 gnoll1MoveTar = new Vector3(14.5f,30.5f,0);
                Vector3 gnoll2MoveTar = new Vector3(16f,31.25f,0);
                Vector3 gnoll3MoveTar = new Vector3(17f,29.75f,0);
                Vector3 gnoll4MoveTar = new Vector3(17.5f,30f,0);
                Vector3 gnoll5MoveTar = new Vector3(14.5f,30f,0);
                this.om.TurnPlayer(gnoll1, 0);
                this.om.TurnPlayer(gnoll2, 1);
                this.om.TurnPlayer(gnoll3, 2);
                this.om.TurnPlayer(gnoll4, 1);
                this.om.TurnPlayer(gnoll5, 3);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.cackle, this.gm.sm.effectsVolume);
                while (gnoll1.transform.position != gnoll1MoveTar || gnoll2.transform.position != gnoll2MoveTar || gnoll3.transform.position != gnoll3MoveTar || gnoll4.transform.position != gnoll4MoveTar || gnoll5.transform.position != gnoll5MoveTar)
                {
                    gnoll1.transform.position = Vector3.MoveTowards(gnoll1.transform.position, gnoll1MoveTar, 2f * Time.deltaTime);
                    gnoll2.transform.position = Vector3.MoveTowards(gnoll2.transform.position, gnoll2MoveTar, 2f * Time.deltaTime);
                    gnoll3.transform.position = Vector3.MoveTowards(gnoll3.transform.position, gnoll3MoveTar, 2f * Time.deltaTime);
                    gnoll4.transform.position = Vector3.MoveTowards(gnoll4.transform.position, gnoll4MoveTar, 2f * Time.deltaTime);
                    gnoll5.transform.position = Vector3.MoveTowards(gnoll5.transform.position, gnoll5MoveTar, 2f * Time.deltaTime);

                    yield return null;
                }
                yield return null;
                break;
            case 202:
                gnoll1 = GameObject.Find("gnoll1");
                gnoll2 = GameObject.Find("gnoll2");
                gnoll3 = GameObject.Find("gnoll3");
                gnoll4 = GameObject.Find("gnoll4");
                gnoll5 = GameObject.Find("gnoll5");
                gnoll1.transform.Rotate(0, 0, 100);
                gnoll2.transform.Rotate(0, 0, 110);
                gnoll3.transform.Rotate(0, 0, 120);
                gnoll4.transform.Rotate(0, 0, -80);
                gnoll5.transform.Rotate(0, 0, -120);
                yield return null;
                break;
            case 205:
                gnoll3 = GameObject.Find("gnoll3");
                gnoll4 = GameObject.Find("gnoll4");
                gnoll3.transform.Rotate(0, 0, -80);
                gnoll4.transform.Rotate(0, 0, -120);
                yield return null;
                break;
            case 208:
                gnoll1 = GameObject.Find("gnoll1");
                gnoll2 = GameObject.Find("gnoll2");
                gnoll5 = GameObject.Find("gnoll5");
                gnoll1.transform.Rotate(0, 0, 100);
                gnoll2.transform.Rotate(0, 0, 110);
                gnoll5.transform.Rotate(0, 0, 120);
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
            case 34: // Crow flies in with twine
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/crow_twine") as GameObject, new Vector3(20.5f,4.75f,0), Quaternion.identity);
                this.om.TurnPlayer(obj, 0);
                obj.transform.SetParent(om.EntitiesParent);

                crowMoveTar = new Vector3(23.5f, 4.25f, 0);
                while (obj.transform.position != crowMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, crowMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                this.om.TurnPlayer(player, 1);
                yield return new WaitForSeconds(.75f);
                break;
            case 35: // Crow flies away with twine
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
            case 36: // Player uses Acorn, takes twine from crow
                obj = GameObject.Find("crow_twine(Clone)");
                temp = Instantiate(Resources.Load("Prefabs/OverworldCharacters/crow") as GameObject, obj.transform.position, Quaternion.identity);
                Destroy(obj);
                obj = temp;
                obj.transform.SetParent(om.EntitiesParent);

                this.om.TurnPlayer(obj, 0);
                yield return new WaitForSeconds(.75f);
                break;
            case 55: // Bandits battle in a heap
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
            case 61: // Bandit leader tosses player
                origPlayerPos = player.transform.position;
                up = false;
                upVec = origPlayerPos + new Vector3(0, 0.25f, 0);
                while (!up || player.transform.position != origPlayerPos)
                {
                    if (!up)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, upVec, 1.5f * Time.deltaTime);
                        if (player.transform.position == upVec)
                            up = true;
                    }
                    else
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 1.5f * Time.deltaTime);
                    }

                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.thud, this.gm.sm.effectsVolume);
                break;
            case 63: // Bandit leader dies
                obj = GameObject.Find("bandit_leader");
                obj.transform.Rotate(0, 0, 75);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.fall, this.gm.sm.effectsVolume);
                yield return new WaitForSeconds(.75f);
                break;
            case 81:
                obj = GameObject.Find("gnome_log");
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.thud, this.gm.sm.effectsVolume);
                this.om.TurnPlayer(obj, 1);
                obj.transform.position = obj.transform.position + new Vector3(0,0.5f,0);
                GameObject gnome = Instantiate(Resources.Load("Prefabs/OverworldCharacters/gnome") as GameObject, new Vector3(27, 24.25f,0), Quaternion.identity);
                gnome.transform.SetParent(om.EntitiesParent);

                this.om.TurnPlayer(gnome, 0);
                yield return null;
                break;
            case 82:
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.thud, this.gm.sm.effectsVolume);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.gnomeOof, this.gm.sm.effectsVolume);
                yield return null;
                break;
            case 88: // Player obtains and wears rusty helmet
            case 248:
                temp = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight_Rusty") as GameObject, player.transform.position, Quaternion.identity);
                temp.name = "TheWhiteKnight1(Clone)";
                this.gm.pm.player = temp;
                this.gm.om.player = temp;
                GameObject.Find("MainCameraOW").GetComponent<OWCamera>().target = temp.GetComponent<Transform>();
                Destroy(player);
                player = temp;
                yield return null;
                break;
            case 93: // Gnoll logger gets crushed
                obj = GameObject.Find("gnoll_logger");
                temp = Instantiate(Resources.Load("Prefabs/OverworldCharacters/gnoll_logger_logged") as GameObject, obj.transform.position, Quaternion.identity);
                Destroy(obj);
                obj = temp;
                obj.transform.SetParent(om.EntitiesParent);

                this.om.TurnPlayer(obj, 0);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.thud, this.gm.sm.effectsVolume);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.dogYelp, this.gm.sm.effectsVolume);
                yield return new WaitForSeconds(.75f);
                break;
            case 99: // Spider under goblin log bites player
                obj = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_small") as GameObject, new Vector3(29.5f,30.5f,0), Quaternion.identity);
                obj.transform.SetParent(om.EntitiesParent);

                this.om.TurnPlayer(obj, 0);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.hiss, this.gm.sm.effectsVolume);
                objMoveTar = player.transform.position + new Vector3(-0.5f, 0.25f,0);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 2f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;
            case 100: // Spider under goblin log runs away
                obj = GameObject.Find("spider_small(Clone)");
                objMoveTar = obj.transform.position + new Vector3(2.5f, 2f,0);
                while (obj.transform.position != objMoveTar)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, objMoveTar, 3f * Time.deltaTime);
                    yield return null;
                }
                Destroy(obj);
                yield return new WaitForSeconds(.75f);
                break;
            case 217: // Player uses tower shield on boulders
                Vector3 b1MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                Vector3 b1MoveTar2 = new Vector3(26.75f, 33.75f, 0);
                Vector3 b2MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                Vector3 b2MoveTar2 = b2MoveTar1 + new Vector3(-1.5f, 5f, 0);
                Vector3 b3MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                Vector3 b3MoveTar2 = new Vector3(26.75f, 33.75f, 0);
                temp = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight_Shield") as GameObject, player.transform.position, Quaternion.identity);
                temp.name = "TheWhiteKnight1(Clone)";
                this.gm.pm.player = temp;
                this.gm.om.player = temp;
                GameObject.Find("MainCameraOW").GetComponent<OWCamera>().target = temp.GetComponent<Transform>();
                Destroy(player);
                player = temp;
                GameObject b1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder2") as GameObject, new Vector3(34.25f, 37.5f, 0), Quaternion.identity);
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.rumble, this.gm.sm.effectsVolume);
                while (b1.transform.position != b1MoveTar2)
                {
                    b1.transform.position = Vector3.MoveTowards(b1.transform.position, b1MoveTar2, 6f * Time.deltaTime);
                    b1.transform.Rotate(0, 0, 5f);

                    yield return null;
                }
                Destroy(b1);
                GameObject b2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder1") as GameObject, new Vector3(33.75f, 37.75f, 0), Quaternion.identity);
                while (b2.transform.position != b2MoveTar1)
                {
                    b2.transform.position = Vector3.MoveTowards(b2.transform.position, b2MoveTar1, 6f * Time.deltaTime);
                    b2.transform.Rotate(0, 0, 5f);

                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.metalThud, this.gm.sm.effectsVolume);
                player.transform.Rotate(0, 0, 110f);
                while (b2.transform.position != b2MoveTar2)
                {
                    b2.transform.position = Vector3.MoveTowards(b2.transform.position, b2MoveTar2, 6f * Time.deltaTime);
                    b2.transform.Rotate(0, 0, 5f);

                    yield return null;
                }
                Destroy(b2);
                GameObject b3 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder2") as GameObject, new Vector3(34.25f, 37.5f, 0), Quaternion.identity);
                while (b3.transform.position != b3MoveTar2)
                {
                    b3.transform.position = Vector3.MoveTowards(b3.transform.position, b3MoveTar2, 6f * Time.deltaTime);
                    b3.transform.Rotate(0, 0, 5f);

                    yield return null;
                }
                Destroy(b3);
                yield return null;
                break;
            case 218:
                playerRotate = Quaternion.identity;
                yield return new WaitForSeconds(1f);
                while (player.transform.rotation.z > playerRotate.z)
                {
                    player.transform.Rotate(new Vector3(0, 0, -110) * (2f * Time.deltaTime));
                    yield return null;
                }
                player.transform.Rotate(Vector3.zero); // Making sure the player is perfectly upright
                temp = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight1") as GameObject, player.transform.position, Quaternion.identity);
                temp.name = "TheWhiteKnight1(Clone)";
                this.gm.pm.player = temp;
                this.gm.om.player = temp;
                GameObject.Find("MainCameraOW").GetComponent<OWCamera>().target = temp.GetComponent<Transform>();
                Destroy(player);
                player = temp;
                yield return null;
                break;
            case 220: // Player succeeds boulder dodge
                origPlayerPos = player.transform.position;
                Vector3 playerHop = player.transform.position + new Vector3(0.5f,-0.25f,0);
                b1MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f,0);
                b1MoveTar2 = new Vector3(26.25f,34f,0);
                b2MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                b2MoveTar2 = new Vector3(26.75f,33.75f,0);
                b3MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                b3MoveTar2 = new Vector3(26.25f, 34f, 0);
                b1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder2") as GameObject, new Vector3(33.75f, 37.75f, 0), Quaternion.identity);
                float startTime = Time.time;
                float journeyTime = .275f;
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.rumble, this.gm.sm.effectsVolume);
                while (b1.transform.position != b1MoveTar2 || player.transform.position != playerHop)
                {
                    b1.transform.position = Vector3.MoveTowards(b1.transform.position, b1MoveTar2, 6f * Time.deltaTime);
                    b1.transform.Rotate(0,0,5f);

                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = origPlayerPos - center;
                    Vector3 endRelCenter = playerHop - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                Destroy(b1);
                b2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder1") as GameObject, new Vector3(34.25f, 37.5f, 0), Quaternion.identity);
                startTime = Time.time;
                journeyTime = .275f;
                while (b2.transform.position != b2MoveTar2 || player.transform.position != origPlayerPos)
                {
                    b2.transform.position = Vector3.MoveTowards(b2.transform.position, b2MoveTar2, 6f * Time.deltaTime);
                    b2.transform.Rotate(0, 0, 5f);

                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = playerHop - center;
                    Vector3 endRelCenter = origPlayerPos - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                Destroy(b2);
                b3 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder2") as GameObject, new Vector3(33.75f, 37.75f, 0), Quaternion.identity);
                startTime = Time.time;
                journeyTime = .275f;
                while (b3.transform.position != b3MoveTar2 || player.transform.position != playerHop)
                {
                    b3.transform.position = Vector3.MoveTowards(b3.transform.position, b3MoveTar2, 6f * Time.deltaTime);
                    b3.transform.Rotate(0, 0, 5f);

                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = origPlayerPos - center;
                    Vector3 endRelCenter = playerHop - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                Destroy(b3);
                startTime = Time.time;
                journeyTime = .275f;
                while (player.transform.position != origPlayerPos)
                {
                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = playerHop - center;
                    Vector3 endRelCenter = origPlayerPos - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;
            case 221: // Player Fails boulder dodge
                origPlayerPos = player.transform.position;
                playerHop = player.transform.position + new Vector3(0.5f, -0.25f, 0);
                Vector3 playerHopFail = origPlayerPos;
                b1MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                b1MoveTar2 = new Vector3(26.25f, 34f, 0);
                b2MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                b2MoveTar2 = new Vector3(26.75f, 33.75f, 0);
                b3MoveTar1 = player.transform.position + new Vector3(0.5f, 0.25f, 0);
                b3MoveTar2 = new Vector3(26.25f, 34f, 0);
                b1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder2") as GameObject, new Vector3(33.75f, 37.75f, 0), Quaternion.identity);
                startTime = Time.time;
                journeyTime = .275f;
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.rumble, this.gm.sm.effectsVolume);
                while (b1.transform.position != b1MoveTar2 || player.transform.position != playerHop)
                {
                    b1.transform.position = Vector3.MoveTowards(b1.transform.position, b1MoveTar2, 6f * Time.deltaTime);
                    b1.transform.Rotate(0, 0, 5f);

                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = origPlayerPos - center;
                    Vector3 endRelCenter = playerHop - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                Destroy(b1);
                b2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder1") as GameObject, new Vector3(34.25f, 37.5f, 0), Quaternion.identity);
                startTime = Time.time;
                journeyTime = .275f;
                while (b2.transform.position != b2MoveTar2 || player.transform.position != origPlayerPos)
                {
                    b2.transform.position = Vector3.MoveTowards(b2.transform.position, b2MoveTar2, 6f * Time.deltaTime);
                    b2.transform.Rotate(0, 0, 5f);

                    Vector3 center = (origPlayerPos + playerHop) * 0.5f;
                    center -= new Vector3(0, .35f, 0);
                    Vector3 playerRelCenter = playerHop - center;
                    Vector3 endRelCenter = origPlayerPos - center;
                    float fracComplete = (Time.time - startTime) / journeyTime;
                    player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
                    player.transform.position += center;

                    yield return null;
                }
                Destroy(b2);
                b3 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/boulder2") as GameObject, new Vector3(33.75f, 37.75f, 0), Quaternion.identity);
                startTime = Time.time;
                journeyTime = .275f;
                Vector3 b3Halfway = origPlayerPos + new Vector3(-0.25f,0.25f,0);
                up = false;
                upVec = origPlayerPos + new Vector3(0, 0.25f, 0);
                while (b3.transform.position != origPlayerPos || player.transform.position != playerHopFail)
                {
                    b3.transform.position = Vector3.MoveTowards(b3.transform.position, origPlayerPos, 6f * Time.deltaTime);
                    b3.transform.Rotate(0, 0, 5f);

                    if (!up)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, upVec, 1.5f * Time.deltaTime);
                        if (player.transform.position == upVec)
                            up = true;
                    }
                    else
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, origPlayerPos, 1.5f * Time.deltaTime);
                    }

                    yield return null;
                }
                this.gm.sm.effectChannel.PlayOneShot(this.gm.sm.thud, this.gm.sm.effectsVolume);
                player.transform.Rotate(0, 0, 110f);
                while (b3.transform.position != b3MoveTar2)
                {
                    b3.transform.position = Vector3.MoveTowards(b3.transform.position, b3MoveTar2, 6f * Time.deltaTime);
                    b3.transform.Rotate(0, 0, 5f);
                    yield return null;
                }
                Destroy(b3);
                yield return new WaitForSeconds(.75f);
                break;
            case 225: // Little spiders descend around player, queen darts across.
                GameObject s1 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_small") as GameObject, new Vector3(37.5f, 42f, 0), Quaternion.identity);
                s1.name = "s1";
                GameObject s2 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_small") as GameObject, new Vector3(38.5f, 42.25f, 0), Quaternion.identity);
                s2.name = "s2";
                GameObject s3 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_small") as GameObject, new Vector3(39.5f, 42.5f, 0), Quaternion.identity);
                s3.name = "s3";
                GameObject s4 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_small") as GameObject, new Vector3(39.5f, 42.5f, 0), Quaternion.identity);
                s4.name = "s4";
                GameObject s5 = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_small") as GameObject, new Vector3(41.5f, 42.5f, 0), Quaternion.identity);
                s5.name = "s5";
                GameObject queen = Instantiate(Resources.Load("Prefabs/OverworldCharacters/spider_queen") as GameObject, new Vector3(38.5f, 42.25f, 0), Quaternion.identity);
                s1.transform.SetParent(om.EntitiesParent);
                s2.transform.SetParent(om.EntitiesParent);
                s3.transform.SetParent(om.EntitiesParent);
                s4.transform.SetParent(om.EntitiesParent);
                s5.transform.SetParent(om.EntitiesParent);
                queen.transform.SetParent(om.EntitiesParent);

                this.om.TurnPlayer(s1, 0);
                this.om.TurnPlayer(s2, 0);
                this.om.TurnPlayer(s3, 0);
                this.om.TurnPlayer(s4, 2);
                this.om.TurnPlayer(s5, 2);
                this.om.TurnPlayer(queen, 0);
                Vector3 s1Move = new Vector3(37.5f, 40f, 0);
                Vector3 s2Move = new Vector3(38.5f, 40.5f, 0);
                Vector3 s3Move = new Vector3(39.5f, 41f, 0);
                Vector3 s4Move = new Vector3(39.5f, 39.5f, 0);
                Vector3 s5Move = new Vector3(41.5f, 40.5f, 0);
                Vector3 queenMove = new Vector3(43f, 40f, 0);
                while (s1.transform.position != s1Move || s2.transform.position != s2Move || s3.transform.position != s3Move || s4.transform.position != s4Move || s5.transform.position != s5Move || queen.transform.position != queenMove)
                {
                    s1.transform.position = Vector3.MoveTowards(s1.transform.position, s1Move, 2f * Time.deltaTime);
                    s2.transform.position = Vector3.MoveTowards(s2.transform.position, s2Move, 2.5f * Time.deltaTime);
                    s3.transform.position = Vector3.MoveTowards(s3.transform.position, s3Move, 3f * Time.deltaTime);
                    s4.transform.position = Vector3.MoveTowards(s4.transform.position, s4Move, 2.5f * Time.deltaTime);
                    s5.transform.position = Vector3.MoveTowards(s5.transform.position, s5Move, 2f * Time.deltaTime);
                    queen.transform.position = Vector3.MoveTowards(queen.transform.position, queenMove, 4f * Time.deltaTime);

                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;
            case 227: // Spider queen gets in player's face
                queen = GameObject.Find("spider_queen(Clone)");
                queenMove = player.transform.position + new Vector3(0.5f, 0.25f,0);
                this.om.TurnPlayer(queen, 1);
                while (queen.transform.position != queenMove)
                {
                    queen.transform.position = Vector3.MoveTowards(queen.transform.position, queenMove, 4f * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(.75f);
                break;
            case 240:
                s1 = GameObject.Find("s1");
                s2 = GameObject.Find("s2");
                s3 = GameObject.Find("s3");
                s4 = GameObject.Find("s4");
                s5 = GameObject.Find("s5");
                queen = GameObject.Find("spider_queen(Clone)");
                s1.transform.Rotate(0, 0, 100);
                s2.transform.Rotate(0, 0, 110);
                s3.transform.Rotate(0, 0, 120);
                s4.transform.Rotate(0, 0, -80);
                s5.transform.Rotate(0, 0, -120);
                queen.transform.Rotate(0, 0, -110);
                yield return new WaitForSeconds(.75f);
                break;
            default:
                break;
        }
        yield break;

    }
}
