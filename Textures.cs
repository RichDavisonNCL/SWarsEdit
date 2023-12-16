using System.Collections.Generic;
using System.IO;

namespace SWars
{
    public class Textures
    {
        public static void ReadPaletteFile(string filename, ColourFunc cFunc)
        {
            string paletteFile = filename;
            byte[] palette = null;
            using (BinaryReader reader = new BinaryReader(File.Open(paletteFile, FileMode.Open)))
            {
                palette = reader.ReadBytes(256 * 3);
            }

            for (int i = 0; i < 256; ++i)
            {
                float r = palette[(i * 3) + 0] / 255.0f;
                float g = palette[(i * 3) + 1] / 255.0f;
                float b = palette[(i * 3) + 2] / 255.0f;
                float a = i == 0 ? 0.0f : 1.0f;
                cFunc(r, g, b, a);
            }
        }

        public static void ReadTABEntries(string filename, ref List<TABFileEntry> list)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    list.Add(Functions.ByteToType<TABFileEntry>(reader));
                }
            }
        }

        public static void ReadDATFile(string filename, int width, int height,TextureFunc tFunc)
        {
            byte[] texData = null;

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                texData = reader.ReadBytes(width * height);
            }
            tFunc(0, width, height, texData);
        }

        public static void ReadDATFile(string filename, ref List<TABFileEntry> inputList, TextureFunc tFunc)
        {
            int loadedCount = 0;

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                foreach (SWars.TABFileEntry e in inputList)
                {
                    reader.BaseStream.Seek(e.offset, SeekOrigin.Begin);
                    int xPos = 0;
                    int yPos = e.height - 1;

                    int dataSize = e.width * e.height;

                    byte[] data = new byte[dataSize];

                    bool valid = true;

                    while (valid && yPos >= 0)
                    {
                        byte b = reader.ReadByte();
                        sbyte c = unchecked((sbyte)b);

                        if (c == 0)
                        {
                            xPos = 0;
                            yPos--;
                        }
                        else if (c < 0)
                        {
                            xPos -= c;
                        }
                        else
                        {
                            for (int i = 0; i < c; ++i)
                            {
                                int at = (e.width * yPos) + xPos;

                                if (at >= data.Length)
                                {
                                    valid = false;
                                    break;
                                }
                                if (at >= reader.BaseStream.Length)
                                {
                                    valid = false;
                                    break;
                                }
                                data[at] = reader.ReadByte();
                                xPos++;
                            }
                        }
                    }
                    tFunc(loadedCount++, e.width, e.height, data);
                }
            }
        }
    }
}
