using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MagicBank {
    private static Dictionary<Magic, IMagic> magics;
    public static IMagic GetMagicFromEnum(Magic e) {
        if (magics.TryGetValue(e, out var m)) return m;
        else return null;
    }
    public static void InitMagics() {
        ProjectileBank.IntiProjectileBank();

        Dictionary<Magic, IMagic> dict;
        dict = new Dictionary<Magic, IMagic> {
            { Magic.Teste, new MagicTest()},
            {Magic.Fire, new Fire()},
            {Magic.FireAutoRemote, new FireAutoRemote()},
            {Magic.FireAllDirection, new FireAllDirection()},
            {Magic.Water, new Water()},
            {Magic.WaterAutoRemote, new WaterAutoRemote()},
            {Magic.WaterAllDirection, new WaterAllDirection()},
            {Magic.Wind, new Wind()},
            {Magic.WindAutoRemote, new WindAutoRemote()},
            {Magic.WindAllDirection, new WindAllDirection()},
            {Magic.Earth, new Earth()},
            {Magic.EarthAutoRemote, new EarthAutoRemote()},
            {Magic.EarthAllDirection, new EarthAllDirection()}
        };
        magics = dict;
    }
}

public enum Magic {
    Teste,
    Fire,
    FireAutoRemote,
    FireAllDirection,
    Water,
    WaterAutoRemote,
    WaterAllDirection,
    Wind,
    WindAutoRemote,
    WindAllDirection,
    Earth,
    EarthAutoRemote,
    EarthAllDirection,
}
