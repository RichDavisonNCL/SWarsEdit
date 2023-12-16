using System.Collections.Generic;
using System.IO;
using SWars;

namespace SWars
{
    public class Vehicles
    {
        public static void LoadMeshes(string filename, ref VehicleMeshFile vehicleData)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                VehicleMeshFile vData = new VehicleMeshFile();

                vData.header = SWars.Functions.ByteToType<SWars.VehicleHeader>(reader);

                vData.vertices = new List<SWars.Vertex>();
                vData.tris = new List<SWars.Tri>();
                vData.quads = new List<SWars.Quad>();
                vData.meshes = new List<SWars.MeshDetails>();

                vData.quadTex = new List<SWars.QuadTextureInfo>();
                vData.triTex = new List<SWars.TriTextureInfo>();

                for (int i = 0; i < vData.header.numVerts; ++i)
                {
                    vData.vertices.Add(SWars.Functions.ByteToType<SWars.Vertex>(reader));
                }
                for (int i = 0; i < vData.header.numTris; ++i)
                {
                    vData.tris.Add(SWars.Functions.ByteToType<SWars.Tri>(reader));
                }
                for (int i = 0; i < vData.header.numQuads; ++i)
                {
                    vData.quads.Add(SWars.Functions.ByteToType<SWars.Quad>(reader));
                }
                for (int i = 0; i < vData.header.numMeshes; ++i)
                {
                    vData.meshes.Add(SWars.Functions.ByteToType<SWars.MeshDetails>(reader));
                }
                for (int i = 0; i < vData.header.numQuadUV; ++i)
                {
                    vData.quadTex.Add(SWars.Functions.ByteToType<SWars.QuadTextureInfo>(reader));
                }
                for (int i = 0; i < vData.header.numTriUV; ++i)
                {
                    vData.triTex.Add(SWars.Functions.ByteToType<SWars.TriTextureInfo>(reader));
                }
                vehicleData = vData;
            }
        }

        public static void SaveMeshes(string filename, VehicleMeshFile vData)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate)))
            {
                SWars.Functions.WriteType<SWars.VehicleHeader>(writer, vData.header);

                SWars.Functions.WriteData<SWars.Vertex>(writer, ref vData.vertices);

                SWars.Functions.WriteData<SWars.Tri>(writer, ref vData.tris);

                SWars.Functions.WriteData<SWars.Quad>(writer, ref vData.quads);

                SWars.Functions.WriteData<SWars.MeshDetails>(writer, ref vData.meshes);

                SWars.Functions.WriteData<SWars.QuadTextureInfo>(writer, ref vData.quadTex);

                SWars.Functions.WriteData<SWars.TriTextureInfo>(writer, ref vData.triTex);
            }
        }
    }
}