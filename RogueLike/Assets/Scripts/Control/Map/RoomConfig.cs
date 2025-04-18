using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfig
{
    /*Esta classe serve para configurar itens de drop, portas de saida, id da room e mais*/
    private int idRoom;
    private int directionDoors; // 0000 -> cima, direita, baixo, esquerda
    private bool isChest;

    public int IdRoom{
        get => idRoom;
        set => idRoom = value;
    } 
    public int DirectionDoors{
        get => directionDoors;
        set => directionDoors = value;
    }
    public bool IsChest{
        get => isChest;
        set => isChest = value;
    }
}
