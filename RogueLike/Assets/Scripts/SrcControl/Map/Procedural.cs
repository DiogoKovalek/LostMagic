using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

public class Procedural {
    private RoomConfig[,] mapGenerateRooms;

    private float chanceForCreateTrapRoom = 30.0f; // number Between 0 and 100
    private float chanceForCreateDoubleTrapRoom = 15.0f;
    private float chanceForAddNewDoorInRoom = 70.0f;
    private float chanceForAddExtraNewDoorInRoom = 30.0f;
    public RoomConfig[,] generateMapProcedural(int size2D, int numberOfRooms, int numberOfRoomsList) {
        // cria o array
        mapGenerateRooms = new RoomConfig[size2D, size2D];
        // retorna o array central
        int pointx = (int)size2D / 2;
        int pointy = (int)size2D / 2;

        mapGenerateRooms[pointx, pointy] = createRoom(0, 0b0000, TypeRoom.initial); // A sala 0 será a sala que que o player ira iniciar
        // A sala 1 tera 4 portas de saida, depois disso devese criar entao 4 ramificacoes de caminhos aleatorios
        // init branch
        List<posXY> seeds = new List<posXY>(); // cada ponto representa uma ramificacao
        for (int i = 0; i < 4; i++) { // Cria 4 sementes de ramificacao
            seeds.Add(new posXY(pointx, pointy));
        }
        for (int i = 0; i < numberOfRooms; i++) { // Para cada sala maxima da seed
            for (int j = 0; j < seeds.Count; j++) { // para cada seed

                #region Posible positions

                List<posXY> emptySpace = new List<posXY>();
                int posx = seeds[j].X;
                int posy = seeds[j].Y;
                // Possibilidades de movimento
                if (posy - 1 >= 0 && mapGenerateRooms[posx, posy - 1] == null) { // Cima
                    posXY pos = new posXY(posx, posy - 1);
                    pos.DirectionDoor |= 0b1000;
                    emptySpace.Add(pos);
                }
                if (posx + 1 <= (size2D - 1) && mapGenerateRooms[posx + 1, posy] == null) { // Direita
                    posXY pos = new posXY(posx + 1, posy);
                    pos.DirectionDoor |= 0b0100;
                    emptySpace.Add(pos);
                }
                if (posy + 1 <= (size2D - 1) && mapGenerateRooms[posx, posy + 1] == null) { // Baixo
                    posXY pos = new posXY(posx, posy + 1);
                    pos.DirectionDoor |= 0b0010;
                    emptySpace.Add(pos);
                }
                if (posx - 1 >= 0 && mapGenerateRooms[posx - 1, posy] == null) { // Esquerda
                    posXY pos = new posXY(posx - 1, posy);
                    pos.DirectionDoor |= 0b0001;
                    emptySpace.Add(pos);
                }
                if (emptySpace.Count == 0) { // nao encontrou caminho nenhum
                    continue;
                    // break para manter todas as seeds com mesmo tamanho, continue pra deixar cada uma com tamanho diferente
                }

                #endregion

                #region Add doors
                // Decide o caminho
                int chosenPath = UnityEngine.Random.Range(0, emptySpace.Count);
                // Adiciona a porta ao antigo ponto
                mapGenerateRooms[seeds[j].X, seeds[j].Y].DirectionDoors |= emptySpace[chosenPath].DirectionDoor;
                // subistitui para a porta contraria
                emptySpace[chosenPath].DirectionDoor = getOpositeDoor(emptySpace[chosenPath].DirectionDoor);
                posXY previousRoom = seeds[j]; // para alteracoes futuras no algoritimo
                seeds[j] = emptySpace[chosenPath];
                emptySpace.Remove(emptySpace[chosenPath]);
                #endregion

                #region Create new Room
                int roomNumber = UnityEngine.Random.Range(0, numberOfRoomsList);
                TypeRoom tyr = sortTypeRoom(roomNumber, true, numberOfRooms - 1 - i);
                mapGenerateRooms[seeds[j].X, seeds[j].Y] = createRoom(roomNumber, seeds[j].DirectionDoor, tyr);
                //Debug.Log($"Room new({seeds[j].X},{seeds[j].Y}) Doors: {Convert.ToString(mapGenerateRooms[seeds[j].X,seeds[j].Y].DirectionDoors, 2).PadLeft(4, '0')}");
                #endregion

                #region If Trap Room 
                if (i != 0 && UnityEngine.Random.Range(0.0f, 100.0f) <= chanceForCreateTrapRoom && emptySpace.Count > 0) { // Nao pode ter trapRoom na primeira sala, e como está no final tambem nao pode ter na ultima, e tambem dever ter salas para adicionar
                    byte maxOfTrapRooms = 1;
                    if (emptySpace.Count == 2 && UnityEngine.Random.Range(0.0f, 100.0f) <= chanceForCreateDoubleTrapRoom) { // Se for duas salas, roda o loop
                        maxOfTrapRooms = 2;
                    }
                    for (byte s = 0; s < maxOfTrapRooms; s++) {
                        int numRoomChange = UnityEngine.Random.Range(0, emptySpace.Count);
                        posXY roomChange = emptySpace[numRoomChange];
                        mapGenerateRooms[previousRoom.X, previousRoom.Y].DirectionDoors |= roomChange.DirectionDoor;// Atualiza a antiga sala
                        roomNumber = UnityEngine.Random.Range(0, numberOfRoomsList);
                        tyr = sortTypeRoom(roomNumber, false, numberOfRooms - 1 - i);
                        mapGenerateRooms[roomChange.X, roomChange.Y] = createRoom(roomNumber, getOpositeDoor(roomChange.DirectionDoor), tyr); // Cria a nova sala
                        emptySpace.Remove(emptySpace[numRoomChange]);
                    }
                }
                #endregion

                #region Check if have a room near to have chance add door
                if (UnityEngine.Random.Range(0.0f, 100.0f) <= chanceForAddNewDoorInRoom) {
                    List<posXY> roomsNear = new List<posXY>();
                    if (seeds[j].Y - 1 >= 0 && mapGenerateRooms[seeds[j].X, seeds[j].Y - 1] != null && seeds[j].Y - 1 != previousRoom.Y) { // Cima
                        posXY pos = new posXY(seeds[j].X, seeds[j].Y - 1);
                        pos.DirectionDoor |= 0b1000;
                        roomsNear.Add(pos);
                    }
                    if (seeds[j].X + 1 < (size2D - 1) && mapGenerateRooms[seeds[j].X + 1, seeds[j].Y] != null && seeds[j].X + 1 != previousRoom.X) { // Direita
                        posXY pos = new posXY(seeds[j].X + 1, seeds[j].Y);
                        pos.DirectionDoor |= 0b0100;
                        roomsNear.Add(pos);
                    }
                    if (seeds[j].Y + 1 < (size2D - 1) && mapGenerateRooms[seeds[j].X, seeds[j].Y + 1] != null && seeds[j].Y + 1 != previousRoom.Y) { // Baixo 
                        posXY pos = new posXY(seeds[j].X, seeds[j].Y + 1);
                        pos.DirectionDoor |= 0b0010;
                        roomsNear.Add(pos);
                    }
                    if (seeds[j].X - 1 >= 0 && mapGenerateRooms[seeds[j].X - 1, seeds[j].Y] != null && seeds[j].X - 1 != previousRoom.X) { // Esquerda
                        posXY pos = new posXY(seeds[j].X - 1, seeds[j].Y);
                        pos.DirectionDoor |= 0b0001;
                        roomsNear.Add(pos);
                    }

                    while (roomsNear.Count > 0) {
                        int numRoomChange = UnityEngine.Random.Range(0, roomsNear.Count);
                        posXY roomChange = roomsNear[numRoomChange];
                        mapGenerateRooms[seeds[j].X, seeds[j].Y].DirectionDoors |= roomChange.DirectionDoor;
                        mapGenerateRooms[roomChange.X, roomChange.Y].DirectionDoors |= getOpositeDoor(roomChange.DirectionDoor);
                        if (UnityEngine.Random.Range(0.0f, 100.0f) >= chanceForAddExtraNewDoorInRoom) {
                            break;
                        }
                    }
                }
                #endregion
            }
        }

        #region Add Portal next level
        posXY roomFinal = seeds[Random.Range(0, seeds.Count)];
        mapGenerateRooms[roomFinal.X, roomFinal.Y].TypeRoom = TypeRoom.portalFinal;
        mapGenerateRooms[roomFinal.X, roomFinal.Y].IdRoom = 0;
        #endregion

        return mapGenerateRooms;
    }
    private TypeRoom sortTypeRoom(int roomNumber, bool isPrincipalSeed, int countRemainigRooms) {
        List<TypeRoom> possibleRooms = new List<TypeRoom> { TypeRoom.basic, TypeRoom.basicWithChest, TypeRoom.bonusChest };
        TypeRoom sorted;
        if (isPrincipalSeed) {
            possibleRooms.Remove(TypeRoom.bonusChest);
        }
        sorted = sortElementList<TypeRoom>(possibleRooms);
        return sorted;
    }
    private T sortElementList<T>(List<T> list) {
        int num = Random.Range(0, list.Count);
        return list[num];
    }
    private byte getOpositeDoor(byte door) {
        switch (door) {
            case 0b1000: return 0b0010;
            case 0b0100: return 0b0001;
            case 0b0010: return 0b1000;
            case 0b0001: return 0b0100;
            default: return 0b0000;
        }
    }
    private RoomConfig createRoom(int id, byte directionDoors = 0b0000, TypeRoom typeRoom = TypeRoom.initial) {
        RoomConfig room = new RoomConfig();
        room.IdRoom = id;
        room.DirectionDoors = directionDoors;
        room.TypeRoom = typeRoom;
        return room;
    }
}
public class posXY {
    public posXY(int x, int y) {
        this.x = x;
        this.y = y;
        this.directionDoor = 0b0000;
    }
    private int x;
    private int y;
    private byte directionDoor; // 0000 cima, direita, baixo, esquerda
    public int X {
        get => x;
        set => x = value;
    }
    public int Y {
        get => y;
        set => y = value;
    }
    public byte DirectionDoor {
        get => directionDoor;
        set => directionDoor = value;
    }
}
