using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

public class Character : Identity
{
    public int energy;
    public int attackPoint;
    protected bool isFreeze;

    //public virtual void Move(Vector2 direction)
    //{
    //    int toX = positionX + (int)direction.x;
    //    int toY = positionY + (int)direction.y;

    //    if (mapGenerator == null) { Debug.LogError("mapGenerator is null", this); return; }

    //    // เดินได้เมื่อว่าง หรือเป็น Potion/Exit (ตัวอย่าง)
    //    if (!HasPlacement(toX, toY) || IsPotion(toX, toY) || IsExit(toX, toY))
    //    {
    //        positionX = toX;
    //        positionY = toY;
    //        transform.position = new Vector3(positionX, positionY, -0.1f);
    //        mapGenerator.potions[toX, toY].Hit();
    //        mapGenerator.walls[toX, toY].Hit();
    //    }
    //    // else: ติดกำแพง/วัตถุอื่น ก็ไม่ขยับ
    //}
    public virtual void Move(Vector2 direction)
    {
        int toX = (int)(positionX + direction.x);
        int toY = (int)(positionY + direction.y);

        if (HasPlacement(toX, toY))
        {
            if (IsPotion(toX, toY))
            {
                positionX = toX;
                positionY = toY;
                transform.position = new Vector3(positionX, positionY, 0);
                mapGenerator.potions[toX, toY].Hit();
            }
            else if (IsDemonWalls(toX, toY))
            {
                positionX = toX;
                positionY = toY;
                transform.position = new Vector3(positionX, positionY, 0);
                mapGenerator.walls[toX, toY].Hit();
            }
        }
        else
        {
            positionX = toX;
            positionY = toY;
            transform.position = new Vector3(positionX, positionY, 0);
        }
    }

    public virtual void TakeDamage(int dmg)
    {
        energy -= dmg;
        Debug.Log("Current Energy : " + energy);
        CheckDead();
    }

    public virtual void TakeDamage(int dmg, bool freeze)
    {
        energy -= dmg;
        isFreeze = freeze;
        GetComponent<SpriteRenderer>().color = Color.blue;
        Debug.Log("Current Energy : " + energy);
        Debug.Log("you is Freeze");
        CheckDead();
    }

    public void Heal(int healPoint)
    {
        energy += healPoint;
        // Debug.Log("Current Energy : " + energy);
        // เราสามารถเรียกใช้ฟังก์ชัน Heal โดยกำหนดให้ Bonuse = false ได้ เพื่อที่จะให้ logic ในการ heal อยู่ที่ฟังก์ชัน Heal อันเดียวและไม่ต้องเขียนซ้ำ
        Heal(healPoint, false);
    }
    public void Heal(int healPoint, bool Bonuse)
    {
        energy += healPoint * (Bonuse ? 2 : 1);
        Debug.Log("Current Energy : " + energy);
    }

    protected virtual void CheckDead()
    {
        if (energy <= 0) Destroy(gameObject);
    }

    // ---------- Map helpers ----------
    public bool HasPlacement(int x, int y)
    {
        string mapData = mapGenerator.GetMapData(x, y);
        return mapData != mapGenerator.empty; // invalid/out-of-range ก็ไม่เท่ากับ empty => true
    }

    public bool IsDemonWalls(int x, int y)
    {
        string mapData = mapGenerator.GetMapData(x, y);
        return mapData == mapGenerator.demonWall;
    }

    public bool IsPotion(int x, int y)
    {
        string mapData = mapGenerator.GetMapData(x, y);
        return mapData == mapGenerator.potion;
    }

    public bool IsExit(int x, int y)
    {
        string mapData = mapGenerator.GetMapData(x, y);
        return mapData == mapGenerator.exit;
    }
}