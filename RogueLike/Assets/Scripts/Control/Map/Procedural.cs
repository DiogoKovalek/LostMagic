using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Procedural
{
    private RoomConfig[,] mapGenerateRooms;
    public RoomConfig[,] generateMapProcedural(int sizeX, int sizeY, int numberOfRooms, int numberOfRoomsList){
        // cria o array
        mapGenerateRooms = new RoomConfig[sizeX,sizeY];
        // retorna o array central
        int pointx = (int) sizeX/2;
        int pointy = (int) sizeY/2;
        
        mapGenerateRooms[pointx,pointy] = createRoom(1); // A sala 1 será a sala que que o player ira iniciar
        // A sala 1 tera 4 portas de saida, depois disso devese criar entao 4 ramificacoes de caminhos aleatorios
        // op 1 : ambas sao criadas ao mesmo tempo

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
                    pos.DirectionDoor += 0b1000;
                    emptySpace.Add(pos);
                }
                if(posx + 1 <= (sizeX - 1) && mapGenerateRooms[posx + 1, posy] == null){ // Direita
                    posXY pos = new posXY(posx + 1, posy);
                    pos.DirectionDoor += 0b0100;
                    emptySpace.Add(pos);
                }
                if(posy + 1 <= (sizeY - 1) && mapGenerateRooms[posx, posy + 1] == null){ // Baixo
                    posXY pos = new posXY(posx, posy + 1);
                    pos.DirectionDoor += 0b0010;
                    emptySpace.Add(pos);
                }
                if(posx - 1 >= 0 && mapGenerateRooms[posx - 1, posy] == null){ // Esquerda
                    posXY pos = new posXY(posx - 1, posy);
                    pos.DirectionDoor += 0b0001;
                    emptySpace.Add(pos);
                }
                if(emptySpace.Count == 0) break; // nao encontrou caminho nenhum

                #endregion

                #region Add doors
                // Decide o caminho
                int chosenPath = UnityEngine.Random.Range(0,emptySpace.Count);
                // Adiciona a porta ao antigo ponto
                mapGenerateRooms[seeds[j].X, seeds[j].Y].DirectionDoors += emptySpace[chosenPath].DirectionDoor; 
                // subistitui para a porta contraria
                switch(emptySpace[chosenPath].DirectionDoor){
                    case 0b1000:
                        emptySpace[chosenPath].DirectionDoor = 0b0010;
                        break;
                    case 0b0100:
                        emptySpace[chosenPath].DirectionDoor = 0b0001;
                        break;
                    case 0b0010:
                        emptySpace[chosenPath].DirectionDoor = 0b1000;
                        break;
                    case 0b0001:
                        emptySpace[chosenPath].DirectionDoor = 0b0100;
                        break;
                    default:
                        break;
                }
                seeds[j] = emptySpace[chosenPath];
                #endregion
                
                #region Create Room
                int RoomNumber = UnityEngine.Random.Range(1,numberOfRoomsList);
                mapGenerateRooms[seeds[j].X, seeds[j].Y] = createRoom(RoomNumber, seeds[j].DirectionDoor);
                #endregion
            }
        }
        /*
        String mapTest = "";
        String mapBit = "";
        for(int i = 0; i < mapGenerateRooms.GetLength(0); i++){
            mapTest += "[";
            mapBit += "[";
            for(int j = 0; j < mapGenerateRooms.GetLength(1); j++){
                if(mapGenerateRooms[i,j] == null){
                    mapTest += 0;
                    mapBit += "0000";
                }else{
                    mapTest += mapGenerateRooms[i,j].IdRoom;
                    String bits = Convert.ToString(mapGenerateRooms[i,j].DirectionDoors, 2);
                    if(bits.Length < 4){
                        bits = new String('0', 4 - bits.Length) + bits;
                    }
                    mapBit += bits;
                }
                if(j+1 != mapGenerateRooms.GetLength(1)){
                    mapTest +=",";
                    mapBit +=",";
                }
            }
            mapTest += "]";
            mapBit += "]";
        }
        Debug.Log(mapTest);
        Debug.Log(mapBit);
        */
        return mapGenerateRooms;
    }

    private RoomConfig createRoom(int id, int directionDoors = 0b0000, bool isChest = false){
        RoomConfig room = new RoomConfig();
        room.IdRoom = id;
        room.DirectionDoors = directionDoors;
        room.IsChest = isChest;
        return room;
    }

    private IEnumerator DebugDelay(){ // Deletar depois
        yield return new WaitForSeconds(7);
    }
}
public class posXY{
    public posXY(int x, int y){
        this.x = x;
        this.y = y;
        this.directionDoor = 0;
    }
    private int x;
    private int y;
    private int directionDoor; // 0000 cima, direita, baixo, esquerda
    public int X{
        get => x;
        set => x = value;
    }
    public int Y{
        get => y;
        set => y = value;
    }
    public int DirectionDoor{
        get => directionDoor;
        set => directionDoor = value;
    }
}
