using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

public class Procedural
{
    private RoomConfig[,] mapGenerateRooms;

    private float chanceForCreateTrapRoom = 30.0f; // number Between 0 and 100
    private float chanceForCreateDoubleTrapRoom = 15.0f;
    public RoomConfig[,] generateMapProcedural(int size2D, int numberOfRooms, int numberOfRoomsList){
        // cria o array
        mapGenerateRooms = new RoomConfig[size2D,size2D];
        // retorna o array central
        int pointx = (int) size2D/2;
        int pointy = (int) size2D/2;
        
        mapGenerateRooms[pointx,pointy] = createRoom(1); // A sala 1 será a sala que que o player ira iniciar
        // A sala 1 tera 4 portas de saida, depois disso devese criar entao 4 ramificacoes de caminhos aleatorios
        // init branch
        List<posXY> seeds = new List<posXY>(); // cada ponto representa uma ramificacao
        for(int i = 0; i < 4; i++){ // Cria 4 sementes de ramificacao
            seeds.Add(new posXY(pointx, pointy));
        }
        for(int i = 0; i < numberOfRooms; i++){ // Para cada sala maxima da seed
            for(int j = 0; j < seeds.Count; j++){ // para cada seed

                #region Posible positions

                List<posXY> emptySpace = new List<posXY>();
                int posx = seeds[j].X;
                int posy = seeds[j].Y;
                // Possibilidades de movimento
                if(posy - 1 >= 0 && mapGenerateRooms[posx, posy - 1] == null){ // Cima
                    posXY pos = new posXY(posx, posy - 1);
                    pos.DirectionDoor |= 0b1000;
                    emptySpace.Add(pos);
                }
                if(posx + 1 <= (size2D - 1) && mapGenerateRooms[posx + 1, posy] == null){ // Direita
                    posXY pos = new posXY(posx + 1, posy);
                    pos.DirectionDoor |= 0b0100;
                    emptySpace.Add(pos);
                }
                if(posy + 1 <= (size2D - 1) && mapGenerateRooms[posx, posy + 1] == null){ // Baixo
                    posXY pos = new posXY(posx, posy + 1);
                    pos.DirectionDoor |= 0b0010;
                    emptySpace.Add(pos);
                }
                if(posx - 1 >= 0 && mapGenerateRooms[posx - 1, posy] == null){ // Esquerda
                    posXY pos = new posXY(posx - 1, posy);
                    pos.DirectionDoor |= 0b0001;
                    emptySpace.Add(pos);
                }
                if(emptySpace.Count == 0){ // nao encontrou caminho nenhum
                    continue;
                    // break para manter todas as seeds com mesmo tamanho, continue pra deixar cada uma com tamanho diferente
                } 

                #endregion
                
                #region Add doors
                // Decide o caminho
                int chosenPath = UnityEngine.Random.Range(0,emptySpace.Count);
                // Adiciona a porta ao antigo ponto
                mapGenerateRooms[seeds[j].X, seeds[j].Y].DirectionDoors |= emptySpace[chosenPath].DirectionDoor; 
                // subistitui para a porta contraria
                emptySpace[chosenPath].DirectionDoor = getOpositeDoor(emptySpace[chosenPath].DirectionDoor);
                posXY previousRoom = seeds[j]; // para alteracoes futuras no algoritimo
                seeds[j] = emptySpace[chosenPath];
                emptySpace.Remove(emptySpace[chosenPath]);
                #endregion
                
                #region Create new Room
                int roomNumber = UnityEngine.Random.Range(1,numberOfRoomsList);
                mapGenerateRooms[seeds[j].X, seeds[j].Y] = createRoom(roomNumber, seeds[j].DirectionDoor);
                //Debug.Log($"Room new({seeds[j].X},{seeds[j].Y}) Doors: {Convert.ToString(mapGenerateRooms[seeds[j].X,seeds[j].Y].DirectionDoors, 2).PadLeft(4, '0')}");
                #endregion

                #region If Trap Room 
                if(i != 0 && UnityEngine.Random.Range(0.0f,100.0f) <= chanceForCreateTrapRoom && emptySpace.Count > 0){ // Nao pode ter trapRoom na primeira sala, e como esta no final tambem nao pode ter na ultima, e tambem dever ter salas para adicionar
                    Byte maxOfTrapRooms = 1;
                    if(emptySpace.Count == 2 && UnityEngine.Random.Range(0.0f,100.0f) <= chanceForCreateDoubleTrapRoom){ // Se for duas salas, roda o loop
                        maxOfTrapRooms = 2;
                    }
                    for(Byte s = 0; s < maxOfTrapRooms; s++){
                        int numRoomChange = UnityEngine.Random.Range(0,emptySpace.Count);
                        posXY roomChange = emptySpace[numRoomChange];
                        mapGenerateRooms[previousRoom.X, previousRoom.Y].DirectionDoors |= roomChange.DirectionDoor;// Atualiza a antiga sala
                        mapGenerateRooms[roomChange.X,roomChange.Y] = createRoom(UnityEngine.Random.Range(1,numberOfRoomsList), getOpositeDoor(roomChange.DirectionDoor)); // Cria a nova sala
                        emptySpace.Remove(emptySpace[numRoomChange]);
                    }
                }
                #endregion

                
            }
        }
        return mapGenerateRooms;
    }
    private Byte getOpositeDoor(Byte door){
        switch(door){
            case 0b1000: return 0b0010;
            case 0b0100: return 0b0001;
            case 0b0010: return 0b1000;
            case 0b0001: return 0b0100;
            default: return 0b0000;
        }
    }
    private RoomConfig createRoom(int id, Byte directionDoors = 0b0000, bool isChest = false){
        RoomConfig room = new RoomConfig();
        room.IdRoom = id;
        room.DirectionDoors = directionDoors;
        room.IsChest = isChest;
        return room;
    }
}
public class posXY{
    public posXY(int x, int y){
        this.x = x;
        this.y = y;
        this.directionDoor = 0b0000;
    }
    private int x;
    private int y;
    private Byte directionDoor; // 0000 cima, direita, baixo, esquerda
    public int X{
        get => x;
        set => x = value;
    }
    public int Y{
        get => y;
        set => y = value;
    }
    public Byte DirectionDoor{
        get => directionDoor;
        set => directionDoor = value;
    }
}
