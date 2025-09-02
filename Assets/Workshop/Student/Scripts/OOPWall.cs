using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// OOPWall aka "Demon Wall"
public class OOPWall : Identity
{
    public string Name;
    public int positionX;
    public int positionY;
    public OOPMapGenerator mapGenerator;
    public override void Hit()
    {
        mapGenerator.player.TakeDamage(10);
        Destroy(this.gameObject);
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
    }
}