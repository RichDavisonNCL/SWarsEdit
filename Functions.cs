using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

namespace SWars
{
    public delegate void VertexFunc(float x, float y, float z);
    public delegate void IndexFunc(uint i);

    public delegate void ColourFunc(float r, float g, float b, float a);

    public delegate void TextureFunc(int i, int width, int height, byte[] data);

    public class Functions
    {
        public static T ByteToType<T>(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        public static T ByteToType<T>(BinaryReader reader, int forcedSize)
        {
            byte[] bytes = reader.ReadBytes(forcedSize);

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        public static void WriteType<T>(BinaryWriter writer, T type, int forceSize, bool forceBlank = false)
        {
            if (forceSize > Marshal.SizeOf(type))
            {
                bool a = true;
            }
            int dataSize = forceSize;
            IntPtr ptr = Marshal.AllocHGlobal(dataSize);
            byte[] dataBuff = new byte[dataSize];
            if (!forceBlank)
            {
                Marshal.StructureToPtr(type, ptr, true);
                Marshal.Copy(ptr, dataBuff, 0, dataSize);
            }

            writer.Write(dataBuff);
        }

        public static void WriteType<T>(BinaryWriter writer, T type, bool forceBlank = false)
        {
            int dataSize = Marshal.SizeOf(type);
            IntPtr ptr = Marshal.AllocHGlobal(dataSize);
            byte[] dataBuff = new byte[dataSize];
            if (!forceBlank)
            {
                Marshal.StructureToPtr(type, ptr, true);
                Marshal.Copy(ptr, dataBuff, 0, dataSize);
            }

            writer.Write(dataBuff);
        }


        public static void ReadUnknownData(BinaryReader source, ref List<short> dest, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                dest.Add(source.ReadInt16()); //???
            }
        }

        public static void WriteData<T>(BinaryWriter writer, ref List<T> type, bool forceBlank = false)
        {
            foreach (T data in type)
            {
                WriteType<T>(writer, data, forceBlank);
            }
        }

        public static void ReadData<T>(BinaryReader source, ref List<T> dest, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                dest.Add(ByteToType<T>(source));
            }
        }

        public static void WriteTag(BinaryWriter writer, short tag)
        {
            writer.Write(tag);
        }

        public static bool VerifyTag(BinaryReader reader, short expectedValue)
        {
            int tag = reader.ReadInt16();
            if (tag != expectedValue)
            {
                UnityEngine.Debug.LogError("Fine failed to read tag " + expectedValue);
                return false;
            }
            return true;
        }

        public static SWars.Tri GetTri(int index, ref List<SWars.Tri> tris)
        {
            if (index >= tris.Count)
            {
                index = tris.Count - 1;
            }
            return tris[index];
        }
        public static SWars.Quad GetQuad(int index, ref List<SWars.Quad> quads)
        {
            if (index >= quads.Count)
            {
                index = quads.Count - 1;
            }
            return quads[index];
        }

        public static SWars.TriTextureInfo GetTriTexture(int index, ref List<SWars.TriTextureInfo> tris)
        {
            if (index >= tris.Count)
            {
                index = tris.Count - 1;
            }
            return tris[index];
        }
        public static SWars.QuadTextureInfo GetQuadTexture(int index, ref List<SWars.QuadTextureInfo> quads)
        {
            if (index >= quads.Count)
            {
                index = quads.Count - 1;
            }
            return quads[index];
        }

        public static void SwarsMissionObjectPosition(SWars.BaseObjectData o, out ushort x, out ushort y, out ushort z)
        {
            x = (ushort)(o.x1 + (o.x2 << 8));
            y = (ushort)(o.y1 + (o.y2 << 8));
            z = (ushort)(o.z1 + (o.z2 << 8));
        }

        public static bool BlockLineIsQuad(SWars.NPCBlockLine b)
        {
            return (b.primIndex) < 0;
        }

        public static bool BlockLineIsTri(SWars.NPCBlockLine b)
        {
            return (b.primIndex) >= 0;
        }

        public static int BlockLineTriIndex(SWars.NPCBlockLine b)
        {
            if (!BlockLineIsTri(b))
            {
                return -1;
            }
            return b.primIndex;
        }

        public static int BlockLineQuadIndex(SWars.NPCBlockLine b)
        {
            if (!BlockLineIsQuad(b))
            {
                return -1;
            }
            return -b.primIndex;
        }

        public static string SpriteNumToName(int nameID)
        {
            switch (nameID)
            {
                case (990): return "UnknownEntity";           //701				//in london
                case (992): return "UnknownEntity";           //701				//in london
                case (993): return "UnknownEntity";            //701				//in london		//these all appear to be nothing
                case (999): return "StreetlightC";             //701				//in london	//This should be the tall streetlight, needs gluing together
                case (1000): return "Dustbin";             //675				//london
                case (1001): return "TreeB";           //1035-337			//london
                case (1004): return "StreetlightF";            //687				//london
                case (1005): return "FlowerA";             //687				//london
                case (1006): return "FlowerB";             //688				//london
                case (1007): return "InvLight";            //unknown			//This is strange, they only appear inside buildings, and NOT by the doorways?
                case (1008): return "StreetlightE";
                case (1009): return "TheBurningMan";
                case (1025): return "UnknownEntity";           //Seems to be nothing?
                case (1026): return "GrayBarrel";
                case (1027): return "YellowBarrel";
                case (1029): return "LargeMineA";
                case (1032): return "StreetlightE";            //unknown
                case (1034): return "TreeD";           //unknown
                case (1035): return "TreeA";           //unknown
                case (1036): return "LargeMineB";          //unknown
                case (1037): return "StreetlightB";            //unknown
                case (1038): return "StreetlightA";            //unknown
                case (1047): return "TreeF";           //unknown
                case (1049): return "TreeE";           //unknown
                case (1050): return "StreetlightC";            //unknown
                case (1052): return "Cashpoint";           //unknown
                case (1054): return "UnknownEntity";           //unknown		//seemingly nothing, or perhaps 'anti light'?
                case (1062): return "TreeG";           //unknown
                case (1064): return "TreeC";           //728				//london
                case (1085): return "BushA";           //unknown
                case (65535): return "Emitter";
                default: return "Unknown Sprite";       //Cant have enough dustbins
            };
        }

    }
}