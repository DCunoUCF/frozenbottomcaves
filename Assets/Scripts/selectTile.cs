using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectTile : MonoBehaviour
{
    private void OnMouseDown()
    {
        PlayerManager.Instance.setSelectedTile(transform.position);
    }
}
