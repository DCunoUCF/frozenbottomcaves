using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    public Item item;

    public void destroyItem()
    {
        Destroy(this.gameObject);
    }
}
