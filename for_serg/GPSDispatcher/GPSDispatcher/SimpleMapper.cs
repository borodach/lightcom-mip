////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           IPositionMapper.cs
//
//  Facility:       �������������� ���������.
//
//
//  Abstract:       ������� �������������� ���������, ������������ �������������� 
//                  ���������� �������� ������ ���� ����� � �������������� 
//                  ������� ������ �������.
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
//
//  Creation Date:  30/10/2005
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace GPS.Common
{
/// 
/// <summary>
/// ������ �������������� ���������, ������������ �������������� ���������� 
/// �������� ������ ���� ����� � �������������� ������� ������ �������.
/// </summary>
///

public class SimpleMapper : IPositionMapper
{
//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// �����������.
/// </summary>
///
//////////////////////////////////////////////////////////////////////////////// 

public SimpleMapper ()
{
    this.m_MapX = 0;
    this.m_MapY = 0;
    this.m_dx = 0;
    this.m_dy = 0;
}

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// �����������.
/// </summary>
///
//////////////////////////////////////////////////////////////////////////////// 

public SimpleMapper (double mapX, double mapY, double dx, double dy)
{
    this.m_MapX = mapX;
    this.m_MapY = mapY;
    this.m_dx = dx;
    this.m_dy = dy;
}

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// ����������� �������������� ���������� � ���������� �� �����.
/// </summary>
/// <param name="global">�������������� ����������.</param>
/// <param name="map">���������� �� �����.</param>
/// <returns>true, ���� �������������� ��������� �������.</returns>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public bool GlobalToMap (GlobalPoint global, MapPoint map)
{
    if (0 == this.m_dx || 0 == this.m_dy) return false;
    try
    {
        map.x = (int) Math.Round ((global.x - this.m_MapX) / this.m_dx);
        map.y = (int) Math.Round ((global.y - this.m_MapY) / this.m_dy);
    }
    catch (Exception)
    {
        return false;
    }

    return true;    
}

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// ����������� ���������� �� ����� � �������������� ���������� .
/// </summary>
/// <param name="map">���������� �� �����.</param>
/// <param name="global">�������������� ����������.</param>
/// <returns>true, ���� �������������� ��������� �������.</returns>
///
//////////////////////////////////////////////////////////////////////////////// 

public bool MapToGlobal (MapPoint map, GlobalPoint global)
{
    global.x = this.m_MapX + this.m_dx * map.x;
    global.y = this.m_MapY + this.m_dy * map.y;

    return true;
}

/// 
/// <summary>
/// �������������� ���������� �������� ������ ���� �����.
/// </summary>
/// 

public double MapX { get {return m_MapX;} set {m_MapX = value;}}
protected double m_MapX;

public double MapY { get {return m_MapY;} set {m_MapY = value;}}
protected double m_MapY;

/// 
/// <summary>
/// ��������������������� �������.
/// </summary>
///

public double dx { get {return m_dx;} set {m_dx = value;}}
protected double m_dx;

public double dy { get {return m_dy;} set {m_dy = value;}}
protected double m_dy;}
}