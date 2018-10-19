﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
using Imperium.Misc;
namespace Imperium.Combat.Turret
{
    public class BulletFactory : Singleton<BulletFactory>
    {
        private BulletFactory() { }

        public Bullet CreateBullet(BulletType type)
        {
            switch (type)
            {
                case BulletType.Test:
                    return new Bullet(5f, 10, "BulletTestPrefab");
                default:
                    throw new Exception("Invalid bullet type");
            }
        }
    }
}
