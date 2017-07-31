using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public class TileMap : Tile
    {
        private int _FieldSize;
        private Point _MapSize;
        private int[,] _MapMatrix;
        private TileCollection _MapCollection;
        public int FieldSize { get => _FieldSize; set => _FieldSize = value; }
        public Point MapSize { get => _MapSize; }
        public int[,] MapMatrix { get => _MapMatrix; }
        public TileCollection MapCollection { get => _MapCollection; set => _MapCollection = value; }
        public TileMap() : base()
        {
            this._FieldSize = 100;
            this._MapCollection = new TileCollection();
        }
        public TileMap(TileMap TM) : base(TM)
        {
            this._FieldSize = TM._FieldSize;
            this._MapSize = new Point(TM._MapSize.X, TM._MapSize.Y);
            this._MapMatrix = new int[this._MapSize.X, this._MapSize.Y];
            for (int i = 0; i < this._MapSize.X; i++)
            {
                for (int j = 0; j < this._MapSize.Y; j++)
                {
                    this._MapMatrix[i, j] = TM._MapMatrix[i, j];
                }
            }
            this._MapCollection = new TileCollection(TM._MapCollection);
        }
        public bool SetMap(Point Size, int[,] Indices)
        {
            if (this._MapCollection == null) return false;
            this._MapSize = Size;
            this._MapMatrix = Indices;
            for (int i = 0; i < this._MapSize.X; i++)
            {
                for (int j = 0; j < this._MapSize.Y; j++)
                {
                    if (this._MapMatrix[i, j] >= this._MapCollection.TileImages.Count) return false;
                }
            }
            this.GenerateMap();
            return true;
        }
        private void GenerateMap()
        {
            this.Scale = new Mathematics.Vertex(this._MapSize.X * this._FieldSize, this._MapSize.Y * this._FieldSize, 1);
            Bitmap B = new Bitmap(this._MapSize.X * this._FieldSize, this._MapSize.Y * this._FieldSize);
            Graphics G = Graphics.FromImage(B);
            for (int i = 0; i < this._MapSize.X; i++)
            {
                for (int j = 0; j < this._MapSize.Y; j++)
                {
                    if (this._MapMatrix[i, j] < 0) continue;
                    G.DrawImage(this._MapCollection.TileImages[this._MapMatrix[i, j]], i * this._FieldSize, j * this._FieldSize, this._FieldSize, this._FieldSize);
                }
            }
            G.Dispose();
            this.Collection.TileImages.Add(B);
        }
    }
}