using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Engineer.Engine
{
    public class SoundSceneObject : SceneObject
    {
        private bool _Playing;
        private bool _Looped;
        private string _Path;
        private MediaPlayer _Player;
        private EventHandler _LoopHandler;
        public bool Playing { get => _Playing; }
        public int Volume { get => (int)(_Player.Volume * 100); set => _Player.Volume = value / 100.0; }
        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                _Path = value;
                this._Player = new System.Windows.Media.MediaPlayer();
                this._Player.Open(new Uri(Path, UriKind.Relative));
            }
        }
        public SoundSceneObject() : base()
        {
            this.Type = SceneObjectType.SoundSceneObject;
            this._Looped = false;
            this._LoopHandler = new EventHandler(this.Ended);
        }
        public SoundSceneObject(string Path, string Name) : base(Name)
        {
            this.Type = SceneObjectType.SoundSceneObject;
            this.Name = Name;
            this._Looped = false;
            this._Player = new System.Windows.Media.MediaPlayer();
            this._Player.Open(new Uri(Path, UriKind.Relative));
            this._LoopHandler = new EventHandler(this.Ended);
        }
        public SoundSceneObject(SoundSceneObject SSO, Scene ParentScene) : base(SSO, ParentScene)
        {
            this.Type = SceneObjectType.SoundSceneObject;
            this._Looped = SSO._Looped;
            this._Player = SSO._Player;
            this._LoopHandler = new EventHandler(this.Ended);
        }
        public void Play()
        {
            this._Player.Stop();
            this._Playing = true;
            this._Player.Play();
            this._Looped = false;
        }
        public void PlayLooped()
        {
            this._Player.Stop();
            this._Playing = true;
            this._Player.Play();
            this._Looped = true;
            this._Player.MediaEnded += this._LoopHandler;
        }
        public void Stop()
        {
            this._Playing = false;
            this._Player.Stop();
        }
        private void Ended(object sender, EventArgs e)
        {
            if(!this._Looped)
            {
                this._Playing = false;
                this._Player.MediaEnded -= this._LoopHandler;
            }
            this._Player.Stop();
            this._Player.Play();
        }
    }
}
