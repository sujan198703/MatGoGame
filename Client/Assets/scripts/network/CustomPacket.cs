using FreeNet;
using System;
using System.Text;
using UnityEngine;

[Serializable]
public class CustomPacket : MonoBehaviour
{
    public IPeer owner
    {
        get;
        private set;
    }

    public byte[] buffer
    {
        get;
        private set;
    }

    public int position
    {
        get;
        private set;
    }

    public short protocol_id
    {
        get;
        private set;
    }

    public static CPacket create(short protocol_id)
    {
        CPacket cPacket = CPacketBufferManager.pop();
        cPacket.set_protocol(protocol_id);
        return cPacket;
    }

    public static void destroy(CPacket packet)
    {
        CPacketBufferManager.push(packet);
    }

 //  public CPacket(byte[] buffer, IPeer owner)
 //  {
 //      this.buffer = buffer;
 //      position = Defines.HEADERSIZE;
 //      this.owner = owner;
 //  }

 //   public CPacket()
 //   {
 //       buffer = new byte[1024];
 //   }

    public short pop_protocol_id()
    {
        return pop_int16();
    }

    public void copy_to(CPacket target)
    {
        target.set_protocol(protocol_id);
        target.overwrite(buffer, position);
    }

    public void overwrite(byte[] source, int position)
    {
        Array.Copy(source, buffer, source.Length);
        this.position = position;
    }

    public byte pop_byte()
    {
        byte result = (byte)BitConverter.ToInt16(buffer, position);
        position++;
        return result;
    }

    public short pop_int16()
    {
        short result = BitConverter.ToInt16(buffer, position);
        position += 2;
        return result;
    }

    public int pop_int32()
    {
        int result = BitConverter.ToInt32(buffer, position);
        position += 4;
        return result;
    }

    public string pop_string()
    {
        short num = BitConverter.ToInt16(buffer, position);
        position += 2;
        string @string = Encoding.UTF8.GetString(buffer, position, num);
        position += num;
        return @string;
    }

    public void set_protocol(short protocol_id)
    {
        this.protocol_id = protocol_id;
       // position = Defines.HEADERSIZE;
        push_int16(protocol_id);
    }

    public void record_size()
    {
       // short value = (short)(position - Defines.HEADERSIZE);
        //byte[] bytes = BitConverter.GetBytes(value);
      //  bytes.CopyTo(buffer, 0);
    }

    public void push_int16(short data)
    {
        byte[] bytes = BitConverter.GetBytes(data);
        bytes.CopyTo(buffer, position);
        position += bytes.Length;
    }

    public void push(byte data)
    {
        byte[] bytes = BitConverter.GetBytes(data);
        bytes.CopyTo(buffer, position);
        position++;
    }

    public void push(short data)
    {
        byte[] bytes = BitConverter.GetBytes(data);
        bytes.CopyTo(buffer, position);
        position += bytes.Length;
    }

    public void push(int data)
    {
        byte[] bytes = BitConverter.GetBytes(data);
        bytes.CopyTo(buffer, position);
        position += bytes.Length;
    }

    public void push(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        short value = (short)bytes.Length;
        byte[] bytes2 = BitConverter.GetBytes(value);
        bytes2.CopyTo(buffer, position);
        position += 2;
        bytes.CopyTo(buffer, position);
        position += bytes.Length;
    }
}
