using System.Collections.Generic;
using System.IO;

namespace SWars
{
    public class UIFunctions
    {
        static public void CreateMapMesh(string inputFile, VertexFunc vFunc, IndexFunc iFunc)
        {
            List<UIMapCoordinate> loadedCoords = new List<UIMapCoordinate>();

            using (BinaryReader reader = new BinaryReader(File.Open(SWars.FilePath.Get() + inputFile, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    loadedCoords.Add(Functions.ByteToType<SWars.UIMapCoordinate>(reader));
                }
            }

            uint previousIndex = 0;
            uint latestIndex = 0;
            bool reset = true;

            uint vertsAdded = 0;

            foreach (UIMapCoordinate c in loadedCoords)
            {
                if (c.x == 0 && c.y == 0) //End the loop!
                {
                    reset = true;
                }
                else
                {
                    vFunc(c.x, -c.y, 0);
                    vertsAdded++;

                    previousIndex = latestIndex;
                    latestIndex = vertsAdded - 1;

                    if (reset)
                    {
                        reset = false;
                    }
                    else
                    {
                        iFunc(previousIndex);
                        iFunc(latestIndex);
                    }
                }
            }
        }

        public static void LoadCityInfo(string inputFile, ref List<UICityData> loadedCities)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(SWars.FilePath.Get() + inputFile, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    loadedCities.Add(SWars.Functions.ByteToType<SWars.UICityData>(reader));
                }
            }
        }
    }
}
