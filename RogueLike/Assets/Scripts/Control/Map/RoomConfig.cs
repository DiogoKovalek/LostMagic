using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfig
{
    /*Esta classe serve para configurar itens de drop, portas de saida, id da room e mais*/
    private int idRoom;
    private Byte directionDoors; // 0000 -> cima, direita, baixo, esquerda
    private bool isChest;

    public int IdRoom{
        get => idRoom;
        set => idRoom = value;
    } 
    public Byte DirectionDoors{
        get => directionDoors;
        set => directionDoors = value;
    }
    public bool IsChest{
        get => isChest;
        set => isChest = value;
    }
}
