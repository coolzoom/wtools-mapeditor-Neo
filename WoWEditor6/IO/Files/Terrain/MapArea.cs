﻿using System;
using System.Collections.Generic;
using SharpDX;
using WoWEditor6.IO.Files.Models;
using WoWEditor6.Scene.Texture;

namespace WoWEditor6.IO.Files.Terrain
{
    abstract class MapArea : IDisposable
    {
        public int IndexX { get; protected set; }
        public int IndexY { get; protected set; }
        public string Continent { get; protected set; }
        public bool IsValid { get; protected set; }

        public List<M2Instance> DoodadInstances { get; private set; }

        public AdtVertex[] FullVertices { get; private set; }

        public List<string> TextureNames { get; private set; } 

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public BoundingBox BoundingBox { get; protected set; }
        public BoundingBox ModelBox { get; protected set; }

        protected IList<Graphics.Texture> mTextures = new List<Graphics.Texture>(); 

        protected MapArea()
        {
            IsValid = true;
            DoodadInstances = new List<M2Instance>();
            FullVertices = new AdtVertex[145 * 256];
            TextureNames = new List<string>();
        }

        public virtual void Dispose()
        {
            DoodadInstances.Clear();
        }

        public abstract void Save();

        public abstract void AsyncLoad();
        public abstract MapChunk GetChunk(int index);

        public abstract bool Intersect(ref Ray ray, out MapChunk chunk, out float distance);

        public abstract bool OnChangeTerrain(Editing.TerrainChangeParameters parameters);
        public abstract void OnUpdateModelPositions(Editing.TerrainChangeParameters parameters);
        public abstract void UpdateNormals();
        public abstract bool OnTextureTerrain(Editing.TextureChangeParameters parameters);

        public int GetOrAddTexture(string textureName)
        {
            for (var i = 0; i < TextureNames.Count; ++i)
            {
                if (string.Equals(TextureNames[i], textureName, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            }

            TextureNames.Add(textureName);
            mTextures.Add(TextureManager.Instance.GetTexture(textureName));
            return TextureNames.Count - 1;
        }

        public string GetTextureName(int index)
        {
            if (index >= TextureNames.Count)
                throw new IndexOutOfRangeException();

            return TextureNames[index];
        }

        public Graphics.Texture GetTexture(int index)
        {
            if (index >= mTextures.Count)
                throw new IndexOutOfRangeException();

            return mTextures[index];
        }
    }
}
