using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    [XmlInclude(typeof(TileCollection))]
    public class Tile : DrawObject
    {
        private bool _Modified;
        private bool _IsMap;
        private int _CurrentIndex;
        private Color _Paint;
        private TileCollection _Collection;
        private List<Tile> _SubTiles;
        public bool Modified
        {
            get
            {
                return _Modified;
            }

            set
            {
                _Modified = value;
            }
        }
        public Color Paint { get => _Paint; set => _Paint = value; }
        [XmlIgnore]
        public TileCollection Collection { get => _Collection; set => _Collection = value; }
        public List<Tile> SubTiles { get => _SubTiles; set => _SubTiles = value; }
        public bool IsMap { get => _IsMap; set => _IsMap = value; }
        public Tile() : base()
        {
            this._IsMap = false;
            this._CurrentIndex = 0;
            this.Type = DrawObjectType.Tile;
            this.Scale = new Mathematics.Vertex(100, 100, 1);
            this.Collection = new TileCollection();
            this._SubTiles = new List<Tile>();
            this._Paint = Color.White;
        }
        public Tile(Tile T) : base(T)
        {
            this._IsMap = T._IsMap;
            this._CurrentIndex = 0;
            this.Collection = new TileCollection(T.Collection);
            this.Collection = new TileCollection(T.Collection);
            this._SubTiles = new List<Tile>();
            for (int i = 0; i < T._SubTiles.Count; i++) this._SubTiles.Add(new Tile(T._SubTiles[i]));
            this._Paint = T._Paint;
        }
        public void SetIndex(int Index)
        {
            if (Index < this.Collection.TileImages.Count) this._CurrentIndex = Index;
        }
        public bool InCollision(DrawObject Collider, Collision2DType Type)
        {
            if (Collider.ID == this.ID) return false;
            return Collision2D.Check(this.Translation, this.Scale, Collider.Translation, Collider.Scale, Type);
        }
        public int Index()
        {
            return _CurrentIndex;
        }
    }
    public class TileCollection
    {
        private string _ID;
        private List<Bitmap> _TileImages;
        [XmlIgnore]
        public List<Bitmap> TileImages
        {
            get
            {
                return _TileImages;
            }

            set
            {
                _TileImages = value;
            }
        }
        public string ID { get => _ID; set => _ID = value; }
        public TileCollection()
        {
            this._ID = Guid.NewGuid().ToString();
            this._TileImages = new List<Bitmap>();
        }
        public TileCollection(Bitmap TileImage)
        {
            this._ID = Guid.NewGuid().ToString();
            this._TileImages = new List<Bitmap>();
            this._TileImages.Add(TileImage);
        }
        public TileCollection(TileCollection TC)
        {
            this._ID = Guid.NewGuid().ToString();
            this._TileImages = new List<Bitmap>(TC._TileImages);
        }
    }
}
