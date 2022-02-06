///////////////////////////////////////////////////////////////////////////////
//
//  File:           IPersistant.cs
//
//  Facility:       Persistance.
//
//
//  Abstract:       ��������� ���������� ��������� ������� � �������� ������
//                  � �������������� �� ����.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  11-11-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
namespace GPS.Common
{
    /// 
    /// <summary>
    /// ��������� ���������� ��������� ������� � �������� ������ � 
    /// �������������� �� ����.
    /// </summary>
    /// 

    public interface IPersistant
    {
        /// 
        /// <summary>
        /// ���������� ����� � ��������� ���������.
        /// </summary>
        /// 

        void Reset ();

        /// 
        /// <summary>
        /// ��������� ��������� ������� � ����� stream.
        /// </summary>
        /// <param name="stream">�����, � ������� ����������� ����������.
        /// </param>
        /// <returns>true, ���� �������� ����������� �������.</returns>
        ///
 
        bool SaveGuts (Stream stream);

        /// 
        /// <summary>
        /// ��������������� �������� ������� �� ������ stream.
        /// </summary>
        /// <param name="stream">�����, �� ������� ����������� ��������������.
        /// </param>
        /// <returns>true, ���� �������� ����������� �������.</returns>
        ///

        bool RestoreGuts (Stream stream);
    }
}
