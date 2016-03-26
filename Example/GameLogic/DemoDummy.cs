﻿/*
 *  RailgunNet - A Client/Server Network State-Synchronization Layer for Games
 *  Copyright (c) 2016 - Alexander Shoulson - http://ashoulson.com
 *
 *  This software is provided 'as-is', without any express or implied
 *  warranty. In no event will the authors be held liable for any damages
 *  arising from the use of this software.
 *  Permission is granted to anyone to use this software for any purpose,
 *  including commercial applications, and to alter it and redistribute it
 *  freely, subject to the following restrictions:
 *  
 *  1. The origin of this software must not be misrepresented; you must not
 *     claim that you wrote the original software. If you use this software
 *     in a product, an acknowledgment in the product documentation would be
 *     appreciated but is not required.
 *  2. Altered source versions must be plainly marked as such, and must not be
 *     misrepresented as being the original software.
 *  3. This notice may not be removed or altered from any source distribution.
*/

using System;
using System.Collections;
using System.Collections.Generic;

using Railgun;
using UnityEngine;

[RegisterEntity(typeof(DemoState))]
public class DemoDummy : RailEntity<DemoState>
{
  public static System.Random random = new System.Random();

  public event Action Frozen;
  public event Action Unfrozen;

  private float startX;
  private float startY;
  private float distance;
  private float angle;
  private float speed;

  protected override void OnStart()
  {
    DemoEvents.OnDummyAdded(this);

    this.startX = this.State.X;
    this.startY = this.State.Y;
    this.angle = 0.0f;

    this.distance = 1.0f + ((float)DemoDummy.random.NextDouble() * 2.0f);
    this.speed = 1.0f + ((float)DemoDummy.random.NextDouble() * 2.0f);

    if (DemoDummy.random.NextDouble() > 0.5f)
      this.speed *= -1.0f;
  }

  protected override void OnSimulate()
  {
    this.angle += RailConfig.FIXED_DELTA_TIME * this.speed;

    float adjustedX = this.startX + this.distance;
    float adjustedY = this.startY;

    float newX = (float)(this.startX + (adjustedX - this.startX) * System.Math.Cos(this.angle) - (adjustedY - this.startY) * System.Math.Sin(this.angle));
    float newY = (float)(this.startY + (adjustedX - this.startX) * System.Math.Sin(this.angle) + (adjustedY - this.startY) * System.Math.Cos(this.angle));

    this.State.X = newX;
    this.State.Y = newY;
  }

  protected override void OnFrozen()
  {
    if (this.Frozen != null)
      this.Frozen.Invoke();
  }

  protected override void OnUnfrozen()
  {
    if (this.Unfrozen != null)
      this.Unfrozen.Invoke();
  }
}
