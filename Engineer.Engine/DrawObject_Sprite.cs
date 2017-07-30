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
    [XmlInclude(typeof(SpriteSet))]
    public class Sprite : DrawObject
    {
        private bool _Modified;
        private bool _Flipped;
        private int _CurrentIndex;
        private int _CurrentSpriteSet;
        private int _BackUpSpriteSet;
        private Color _Paint;
        private List<SpriteSet> _SpriteSets;
        private List<Sprite> _SubSprites;
        private List<Bitmap> _Collectives;
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
        public bool Flipped
        {
            get
            {
                return _Flipped;
            }

            set
            {
                _Flipped = value;
                _Modified = true;
            }
        }
        [XmlIgnore]
        public int BackUpSpriteSet { get => _BackUpSpriteSet; set => _BackUpSpriteSet = value; }
        public int CurrentSpriteSet { get => _CurrentSpriteSet; }
        public Color Paint { get => _Paint; set => _Paint = value; }
        [XmlIgnore]
        public List<SpriteSet> SpriteSets
        {
            get
            {
                return _SpriteSets;
            }

            set
            {
                _SpriteSets = value;
            }
        }
        [XmlIgnore]
        public List<Sprite> SubSprites
        {
            get
            {
                return _SubSprites;
            }

            set
            {
                _SubSprites = value;
            }
        }
        public Sprite() : base()
        {
            this._CurrentIndex = 0;
            this._Flipped = false;
            this.Type = DrawObjectType.Sprite;
            this._SpriteSets = new List<SpriteSet>();
            this.Scale = new Mathematics.Vertex(100,100,1);
            this._SubSprites = new List<Sprite>();
            this._Paint = Color.White;
        }
        public Sprite(Sprite S) : base(S)
        {
            this._CurrentIndex = 0;
            this._Flipped = S._Flipped;
            this._SpriteSets = new List<SpriteSet>();
            for (int i = 0; i < S._SpriteSets.Count; i++) this._SpriteSets.Add(new SpriteSet(S._SpriteSets[i]));
            this._SubSprites = new List<Sprite>();
            for(int i = 0; i <S.SubSprites.Count; i++)
            {
                _SubSprites.Add(new Sprite(S.SubSprites[i]));
            }
            this._Paint = S._Paint;
        }
        public List<Bitmap> CollectiveLists()
        {
            if (this._Collectives != null && this._Modified) return this._Collectives;
            List<Bitmap> Lists = new List<Bitmap>();
            for(int i = 0; i < _SpriteSets.Count; i++)
            {
                Lists.AddRange(_SpriteSets[i].Sprite);
            }
            this._Collectives = Lists;
            return Lists;
        }
        public void RaiseIndex()
        {
            _CurrentIndex++;
            if (_SpriteSets.Count <= 0) _CurrentIndex = -1;
            else if (_CurrentIndex >= _SpriteSets[CurrentSpriteSet].Sprite.Count)
            {
                if (this._BackUpSpriteSet != -1)
                {
                    this._CurrentSpriteSet = this._BackUpSpriteSet;
                    this._BackUpSpriteSet = -1;
                }
                _CurrentIndex = 0;
            }
        }
        public void SetSpriteSet(int Index)
        {
            if (Index >= _SpriteSets.Count) return;
            this._CurrentSpriteSet = Index;
            this._CurrentIndex = 0;
        }
        public void SetSpriteSet(string Name)
        {
            for(int i = 0; i < this._SpriteSets.Count; i++)
            {
                if (this._SpriteSets[i].Name == Name) this.SetSpriteSet(i);
            }
        }
        public void UpdateSpriteSet(int Index)
        {
            if (Index != CurrentSpriteSet) SetSpriteSet(Index);
        }
        public void UpdateSpriteSet(string Name)
        {
            for (int i = 0; i < this._SpriteSets.Count; i++)
            {
                if (this._SpriteSets[i].Name == Name) this.UpdateSpriteSet(i);
            }
        }
        public bool InCollision(DrawObject Collider, Collision2DType Type)
        {
            if (Collider.ID == this.ID) return false;
            return Collision2D.Check(this.Translation, this.Scale, Collider.Translation, Collider.Scale, Type);
        }
        public int Index()
        {
            int Index = 0;
            for(int i = 0; i < CurrentSpriteSet; i++)
            {
                Index += _SpriteSets[i].Sprite.Count;
            }
            Index += _CurrentIndex;
            return Index;
        }
    }
    public class SpriteSet
    {
        private int _Seed;
        private string _ID;
        private string _Name;
        private List<Bitmap> _Sprites;
        public int Seed { get => _Seed; set => _Seed = value; }
        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
        [XmlIgnore]
        public List<Bitmap> Sprite
        {
            get
            {
                return _Sprites;
            }

            set
            {
                _Sprites = value;
            }
        }
        public SpriteSet()
        {
            this._Seed = -1;
            this._ID = Guid.NewGuid().ToString();
            this._Name = this._ID;
            this._Sprites = new List<Bitmap>();
        }
        public SpriteSet(string Name)
        {
            this._Seed = -1;
            this._ID = Guid.NewGuid().ToString();
            this._Name = Name;
            this._Sprites = new List<Bitmap>();
        }
        public SpriteSet(string Name, Bitmap SpriteImage)
        {
            this._Seed = -1;
            this._ID = Guid.NewGuid().ToString();
            this._Name = Name;
            this._Sprites = new List<Bitmap>();
            this._Sprites.Add(SpriteImage);
        }
        public SpriteSet(SpriteSet SS)
        {
            this._Seed = SS._Seed;
            this._ID = SS._ID;
            this._Name = SS._Name;
            this._Sprites = new List<Bitmap>(SS._Sprites);
        }
    }
}
