//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Player2
//{
//    // Hero Name
//    public string name;

//    // Hero Weapons
//    public Weapon weapon01;
//    public Weapon weapon02;

//    // Hero Stats
//    public int stamina;
//    public int maxStamina;

//    public int luck;
//    public int maxLuck;

//    public int skill;
//    public int maxSkill;

//    // Hero Quest
//    public string quest;

//    // Hero Bio
//    public string bio;

//    public Inventory inventory;

//    public Player2(string name)
//    {
//        string quest;
//        string bio;

//        if(name == "Knight")
//        {
//            quest = "You have to defeat the dragon";
//            bio = "You are a Knight. Too lazy to type out an entire bio, but if you did it should work.";
//            this.weapon01 = new Weapon(Weapon.WeaponType.KnightSword);
//            this.weapon02 = new Weapon(Weapon.WeaponType.KnightShield);
//            this.quest = quest;
//            this.bio = bio;
//            this.stamina = 20;
//            this.maxStamina = 20;
//            this.skill = 10;
//            this.maxSkill = 10;
//            this.luck = 9;
//            this.maxLuck = 9;
            
//        }
//        else if(name == "Mage")
//        {
//            quest = "You must find the book of secrets";
//            bio = "You are a Mage. Too lazy to type out an entire bio";
//            this.weapon01 = new Weapon(Weapon.WeaponType.MageStaff);
//            this.weapon02 = new Weapon(Weapon.WeaponType.MageVeil);
//            this.quest = quest;
//            this.bio = bio;
//            this.stamina = 20;
//            this.maxStamina = 20;
//            this.skill = 10;
//            this.maxSkill = 10;
//            this.luck = 9;
//            this.maxLuck = 9;
//        }
//        else if(name == "Monk")
//        {
//            this.weapon01 = new Weapon(Weapon.WeaponType.MonkFist);
//            this.weapon02 = new Weapon(Weapon.WeaponType.MonkPendant);
//            bio = "You are a Monk. Too lazy to type out an entire bio";
//            quest = "You must find the shawlin temple";
//            this.quest = quest;
//            this.bio = bio;
//            this.stamina = 20;
//            this.maxStamina = 20;
//            this.skill = 10;
//            this.maxSkill = 10;
//            this.luck = 9;
//            this.maxLuck = 9;
//        }
//        else
//        {
//            quest = "You have no name and must figure out your identity";
//            bio = "You have no bio. You must figure this out on your own.";
//            this.quest = quest;
//            this.bio = bio;
//            this.stamina = 20;
//            this.maxStamina = 20;
//            this.skill = 10;
//            this.maxSkill = 10;
//            this.luck = 9;
//            this.maxLuck = 9;
//        }
        
//    }
//}
