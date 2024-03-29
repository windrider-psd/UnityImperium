﻿using Imperium.Misc;
using System;

namespace Imperium.MapObjects
{
    public class BulletFactory : Singleton<BulletFactory>
    {
        private BulletFactory()
        {
        }

        public Bullet CreateBullet(BulletType type)
        {
            switch (type)
            {
                case BulletType.Test:
                    return new Bullet(20f, 10, "BulletTestPrefab", type);

                default:
                    throw new Exception("Invalid bullet type");
            }
        }
    }
}