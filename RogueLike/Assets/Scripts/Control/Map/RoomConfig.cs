using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomConfig
{
    /*Esta classe serve para configurar itens de drop, portas de saida, id da room e mais*/
    private int idRoom;
    private byte directionDoors; // 0000 -> cima, direita, baixo, esquerda
    private TypeRoom typeRoom;

    public int IdRoom{
        get => idRoom;
        set => idRoom = value;
    } 
    public byte DirectionDoors{
        get => directionDoors;
        set => directionDoors = value;
    }
    public TypeRoom TypeRoom{
        get => typeRoom;
        set => typeRoom = value;
    }
}
public enum TypeRoom{
    initial,
    basic,
    basicWithChest,
    bonusChest,
    portalFinal
}
