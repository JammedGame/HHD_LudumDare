using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Engineer.Engine;
using Engineer.Mathematics;
using System.IO;
using System.ComponentModel;
using System.Runtime;

namespace Engineer.Draw
{
    public class DrawEngine
    {
        private BackgroundWorker _CurrentWorker;
        private MatrixTransformer _Matrix;
        private Renderer _CurrentRenderer;
        private MaterialTranslator _CurrentTranslator;
        public Renderer CurrentRenderer
        {
            get
            {
                return _CurrentRenderer;
            }

            set
            {
                _CurrentRenderer = value;
            }
        }
        public MaterialTranslator CurrentTranslator
        {
            get
            {
                return _CurrentTranslator;
            }

            set
            {
                _CurrentTranslator = value;
            }
        }
        public DrawEngine()
        {
            this._Matrix = new MatrixTransformer();
        }
        public DrawEngine(DrawEngine DE)
        {
            this._Matrix = new MatrixTransformer();
        }
        public void SetDefaults()
        {
            ShaderMaterialTranslator SMT = _CurrentTranslator as ShaderMaterialTranslator;
            if (this.CurrentTranslator.TranslateMaterial(Material.Default))
            {
                _CurrentRenderer.SetMaterial(new object[3] { new string[6] { "Default", SMT.VertexShaderOutput, SMT.FragmentShaderOutput, null, null, null }, null, null }, true);
            }
        }
        private void Preload2DSceneWork(object sender, DoWorkEventArgs e)
        {
            Scene2D CurrentScene = (Scene2D)e.Argument;
            if (CurrentScene == null) return;
            this._CurrentRenderer.Toggle(RenderEnableCap.Depth, false);
            this._CurrentRenderer.ClearColor(new float[4] {(CurrentScene.BackColor.R *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.G *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.B *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.A *1.0f + 1)/256});
            this._CurrentRenderer.Clear();
            this._Matrix.MatrixMode("Projection");
            this._Matrix.LoadIdentity();
            this._CurrentRenderer.SetProjectionMatrix(_Matrix.ProjectionMatrix);
            this._Matrix.MatrixMode("ModelView");
            this._Matrix.LoadIdentity();
            this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
            for (int i = 0; i < CurrentScene.Objects.Count; i++)
            {
                if (CurrentScene.Objects[i].Type == SceneObjectType.DrawnSceneObject)
                {
                    if (CurrentScene.Objects[i].Visual.Type == DrawObjectType.Sprite) this._CurrentRenderer.PreLoad2DMaterial(CurrentScene.Objects[i].Visual.ID, ((Sprite)CurrentScene.Objects[i].Visual).CollectiveLists()); 
                    if (CurrentScene.Objects[i].Visual.Type == DrawObjectType.Tile) this._CurrentRenderer.PreLoad2DMaterial(((Tile)CurrentScene.Objects[i].Visual).Collection.ID, ((Tile)CurrentScene.Objects[i].Visual).Collection.TileImages);
                }
                this._CurrentWorker.ReportProgress((i * 100) / CurrentScene.Objects.Count);
            }
        }
        public virtual void Preload2DScene(Scene2D CurrentScene, BackgroundWorker Worker)
        {
            if (Worker == null) Worker = new BackgroundWorker();
            this._CurrentWorker = Worker;
            Worker.DoWork += new DoWorkEventHandler(this.Preload2DSceneWork);
            Worker.RunWorkerAsync(CurrentScene);
        }
        private void Destroy2DSceneWork(object sender, DoWorkEventArgs e)
        {
            Scene2D CurrentScene = (Scene2D)e.Argument;
            for (int i = 0; i < CurrentScene.Objects.Count; i++)
            {
                if (CurrentScene.Objects[i].Type == SceneObjectType.DrawnSceneObject)
                {
                    if(CurrentScene.Objects[i].Visual.Type == DrawObjectType.Tile) this._CurrentRenderer.DestroyMaterial(((Tile)CurrentScene.Objects[i].Visual).Collection.ID);
                    else this._CurrentRenderer.DestroyMaterial(CurrentScene.Objects[i].Visual.ID);
                }
                this._CurrentWorker.ReportProgress(i / CurrentScene.Objects.Count);
            }
        }
        public virtual void Destroy2DScene(Scene2D CurrentScene, BackgroundWorker Worker)
        {
            Worker.DoWork += new DoWorkEventHandler(this.Destroy2DSceneWork);
            this._CurrentWorker = Worker;
            Worker.RunWorkerAsync(CurrentScene);
        }
        public virtual void Draw2DScene(Scene2D CurrentScene, int Width, int Height)
        {
            if (CurrentScene == null) return;
            this._CurrentRenderer.Toggle(RenderEnableCap.Depth, false);
            this._CurrentRenderer.SetViewport(Width, Height);
            this._CurrentRenderer.ClearColor(new float[4] {(CurrentScene.BackColor.R *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.G *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.B *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.A *1.0f + 1)/256});
            this._CurrentRenderer.Clear();
            this._Matrix.MatrixMode("Projection");
            this._Matrix.LoadIdentity();
            this._Matrix.Ortho2D(0, Width, Height, 0);
            this._CurrentRenderer.SetProjectionMatrix(_Matrix.ProjectionMatrix);
            this._Matrix.MatrixMode("ModelView");
            this._Matrix.LoadIdentity();
            this._Matrix.Translate(CurrentScene.Transformation.Translation.X, CurrentScene.Transformation.Translation.Y, CurrentScene.Transformation.Translation.Z);
            this._Matrix.Scale(CurrentScene.Transformation.Scale.X, CurrentScene.Transformation.Scale.Y, CurrentScene.Transformation.Scale.Z);

            this._Matrix.PushMatrix();
            this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
            if(this._CurrentRenderer.TargetType == RenderTargetType.Editor) this._CurrentRenderer.Render2DGrid();

            for (int i = 0; i < CurrentScene.Objects.Count; i++)
            {
                if (CurrentScene.Objects[i].Visual.Fixed) continue;
                if (CurrentScene.Objects[i].Visual == null) continue;
                if (CurrentScene.Objects[i].Visual.Type == DrawObjectType.Sprite) DrawSprite((Sprite)CurrentScene.Objects[i].Visual);
                if (CurrentScene.Objects[i].Visual.Type == DrawObjectType.Tile) DrawTile((Tile)CurrentScene.Objects[i].Visual);
                this._Matrix.ReadMatrix();
            }
            this._Matrix.PopMatrix();

            this._Matrix.LoadIdentity();
            this._Matrix.Scale(CurrentScene.Transformation.Scale.X, CurrentScene.Transformation.Scale.Y, CurrentScene.Transformation.Scale.Z);
            this._Matrix.PushMatrix();
            this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
            for (int i = 0; i < CurrentScene.Objects.Count; i++)
            {
                if (!CurrentScene.Objects[i].Visual.Fixed) continue;
                if (CurrentScene.Objects[i].Visual == null) continue;
                if (CurrentScene.Objects[i].Visual.Type == DrawObjectType.Sprite) DrawSprite((Sprite)CurrentScene.Objects[i].Visual);
                if (CurrentScene.Objects[i].Visual.Type == DrawObjectType.Tile) DrawTile((Tile)CurrentScene.Objects[i].Visual);
                this._Matrix.ReadMatrix();
            }
            this._Matrix.PopMatrix();
        }
        public virtual void Preload3DScene(Scene2D CurrentScene)
        {

        }
        public virtual void Draw3DScene(Scene3D CurrentScene, int Width, int Height)
        {
            bool GlobalUpdate = false;
            this._CurrentRenderer.Toggle(RenderEnableCap.Depth, true);
            List<Light> Lights = CurrentScene.Lights;
            List<Actor> Actors = CurrentScene.Actors;
            this._CurrentRenderer.SetViewport(Width, Height);
            this._CurrentRenderer.ClearColor(new float[4] {(CurrentScene.BackColor.R *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.G *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.B *1.0f + 1)/256,
                                                           (CurrentScene.BackColor.A *1.0f + 1)/256});
            this._CurrentRenderer.SetCameraPosition(CurrentScene.EditorCamera.Translation);
            this._CurrentRenderer.ResetLights();
            for (int k = 0; k < CurrentScene.Lights.Count; k++)
            {
                Vertex[] LightVertices = new Vertex[4];
                LightVertices[0] = VertexBuilder.FromRGB(CurrentScene.Lights[k].Color.R,
                                                         CurrentScene.Lights[k].Color.G,
                                                         CurrentScene.Lights[k].Color.B);
                LightVertices[1] = CurrentScene.Lights[k].Translation;
                LightVertices[2] = CurrentScene.Lights[k].Attenuation;
                if (CurrentScene.Lights[k].Active) LightVertices[3] = new Vertex(CurrentScene.Lights[k].Intensity, 0, 0);
                else LightVertices[3] = new Vertex(0, 0, 0);
                GlobalUpdate = GlobalUpdate || this._CurrentRenderer.SetViewLight(k, LightVertices);
            }
            this._CurrentRenderer.Clear();
            this._Matrix.MatrixMode("Projection");
            this._Matrix.LoadIdentity();
            this._Matrix.DefaultPerspective(Width, Height);
            this._CurrentRenderer.SetProjectionMatrix(_Matrix.ProjectionMatrix);
            this._Matrix.MatrixMode("ModelView");
            this._Matrix.LoadIdentity();
            Vertex Eye = new Vertex();
            this._Matrix.DefaultView(Eye, new Vertex(Eye.X, Eye.Y, Eye.Z - 1));
            this._Matrix.Rotate(CurrentScene.EditorCamera.Rotation.X, 1, 0, 0);
            this._Matrix.Rotate(CurrentScene.EditorCamera.Rotation.Y, 0, 1, 0);
            this._Matrix.Rotate(CurrentScene.EditorCamera.Rotation.Z, 0, 0, 1);
            this._Matrix.Translate(-CurrentScene.EditorCamera.Translation.X,
                                   -CurrentScene.EditorCamera.Translation.Y,
                                   -CurrentScene.EditorCamera.Translation.Z);
            this._Matrix.PushMatrix();
            this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix); 

            for (int i = 0; i < CurrentScene.Actors.Count; i++)
            {
                if(CurrentScene.Actors[i].Active)
                {
                    this._Matrix.Scale(CurrentScene.Actors[i].Scale.X, CurrentScene.Actors[i].Scale.Y, CurrentScene.Actors[i].Scale.Z);
                    this._Matrix.Translate(CurrentScene.Actors[i].Translation.X,
                                           CurrentScene.Actors[i].Translation.Y,
                                           CurrentScene.Actors[i].Translation.Z);
                    this._Matrix.Rotate(CurrentScene.Actors[i].Rotation.X, 1, 0, 0);
                    this._Matrix.Rotate(CurrentScene.Actors[i].Rotation.Y, 0, 1, 0);
                    this._Matrix.Rotate(CurrentScene.Actors[i].Rotation.Z, 0, 0, 1);

                    this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
                    for (int j = 0; j < CurrentScene.Actors[i].Geometries.Count; j++)
                    {
                        if (!this._CurrentRenderer.IsMaterialReady(CurrentScene.Actors[i].Materials[CurrentScene.Actors[i].GeometryMaterialIndices[j]].ID) || CurrentScene.Actors[i].Materials[CurrentScene.Actors[i].GeometryMaterialIndices[j]].Modified || GlobalUpdate)
                        {
                            this._CurrentRenderer.UpdateMaterial();
                            ShaderMaterialTranslator SMT = _CurrentTranslator as ShaderMaterialTranslator;
                            if (this.CurrentTranslator.TranslateMaterial(CurrentScene.Actors[i].Materials[CurrentScene.Actors[i].GeometryMaterialIndices[j]]))
                            {
                                _CurrentRenderer.SetMaterial(new object[3] { new string[6] { CurrentScene.Actors[i].Materials[CurrentScene.Actors[i].GeometryMaterialIndices[j]].ID, SMT.VertexShaderOutput, SMT.FragmentShaderOutput, null, null, null }, SMT.TexturesNumber, SMT.Textures }, true);
                            }
                            else _CurrentRenderer.SetMaterial(new object[3] { new string[6] { "Default", null, null, null, null, null }, null, null }, false);
                            CurrentScene.Actors[i].Materials[CurrentScene.Actors[i].GeometryMaterialIndices[j]].Modified = false;
                        }
                        else _CurrentRenderer.SetMaterial(new object[3] { new string[6] { CurrentScene.Actors[i].Materials[CurrentScene.Actors[i].GeometryMaterialIndices[j]].ID, null, null, null, null, null }, null, null }, false);

                        this._CurrentRenderer.UpdateMaterial();
                        this._CurrentRenderer.RenderGeometry(CurrentScene.Actors[i].Geometries[j].Vertices,
                                                             CurrentScene.Actors[i].Geometries[j].Normals,
                                                             CurrentScene.Actors[i].Geometries[j].TexCoords,
                                                             CurrentScene.Actors[i].Geometries[j].Faces,
                                                             CurrentScene.Actors[i].Modified);
                        
                    }
                    this._Matrix.PopMatrix();
                }
            }
        }
        public virtual void DrawSprite(Sprite CurrentSprite)
        {
            this._Matrix.PushMatrix();
            if (CurrentSprite.Active)
            {
                this._Matrix.Translate(CurrentSprite.Translation.X, CurrentSprite.Translation.Y, CurrentSprite.Translation.Z);
                this._Matrix.Scale(CurrentSprite.Scale.X, CurrentSprite.Scale.Y, CurrentSprite.Scale.Z);
                this._Matrix.Rotate(CurrentSprite.Rotation.X, 1, 0, 0);
                this._Matrix.Rotate(CurrentSprite.Rotation.Y, 0, 1, 0);
                this._Matrix.Rotate(CurrentSprite.Rotation.Z, 0, 0, 1);
                float[] PaintColor = { (CurrentSprite.Paint.R * 1.0f + 1) / 256, (CurrentSprite.Paint.G * 1.0f + 1) / 256, (CurrentSprite.Paint.B * 1.0f + 1) / 256, (CurrentSprite.Paint.A * 1.0f + 1) / 256 };
                this._CurrentRenderer.SetSurface(PaintColor);
                this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
                this._CurrentRenderer.RenderImage(CurrentSprite.ID, CurrentSprite.CollectiveLists(), (CurrentSprite.CollectiveLists().Count > 0) ? CurrentSprite.Index() : -1, CurrentSprite.Modified, CurrentSprite.Flipped);
                CurrentSprite.Modified = false;
                for (int i = 0; i < CurrentSprite.SubSprites.Count; i++)
                {
                    this._Matrix.ReadMatrix();
                    this._Matrix.Translate(CurrentSprite.Translation.X, CurrentSprite.Translation.Y, CurrentSprite.Translation.Z);
                    this._Matrix.Translate(CurrentSprite.SubSprites[i].Translation.X, CurrentSprite.SubSprites[i].Translation.Y, CurrentSprite.SubSprites[i].Translation.Z);
                    this._Matrix.Scale(CurrentSprite.SubSprites[i].Scale.X, CurrentSprite.SubSprites[i].Scale.Y, CurrentSprite.SubSprites[i].Scale.Z);
                    this._Matrix.Rotate(CurrentSprite.Rotation.X, 1, 0, 0);
                    this._Matrix.Rotate(CurrentSprite.SubSprites[i].Rotation.X, 1, 0, 0);
                    this._Matrix.Rotate(CurrentSprite.Rotation.Y, 0, 1, 0);
                    this._Matrix.Rotate(CurrentSprite.SubSprites[i].Rotation.Y, 0, 1, 0);
                    this._Matrix.Rotate(CurrentSprite.Rotation.Z, 0, 0, 1);
                    this._Matrix.Rotate(CurrentSprite.SubSprites[i].Rotation.Z, 0, 0, 1);
                    PaintColor = new float[] { (CurrentSprite.SubSprites[i].Paint.R * 1.0f + 1) / 256, (CurrentSprite.SubSprites[i].Paint.G * 1.0f + 1) / 256, (CurrentSprite.SubSprites[i].Paint.B * 1.0f + 1) / 256, (CurrentSprite.SubSprites[i].Paint.A * 1.0f + 1) / 256 };
                    this._CurrentRenderer.SetSurface(PaintColor);
                    this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
                    this._CurrentRenderer.RenderImage(CurrentSprite.SubSprites[i].ID, CurrentSprite.SubSprites[i].CollectiveLists(), (CurrentSprite.SubSprites[i].CollectiveLists().Count > 0) ? CurrentSprite.Index() : -1, CurrentSprite.SubSprites[i].Modified, CurrentSprite.Flipped);
                    CurrentSprite.SubSprites[i].Modified = false;
                }
            }
            this._Matrix.PopMatrix();
        }
        public virtual void DrawTile(Tile CurrentTile)
        {
            this._Matrix.PushMatrix();
            if (CurrentTile.Active)
            {
                this._Matrix.Translate(CurrentTile.Translation.X, CurrentTile.Translation.Y, CurrentTile.Translation.Z);
                this._Matrix.Scale(CurrentTile.Scale.X, CurrentTile.Scale.Y, CurrentTile.Scale.Z);
                this._Matrix.Rotate(CurrentTile.Rotation.X, 1, 0, 0);
                this._Matrix.Rotate(CurrentTile.Rotation.Y, 0, 1, 0);
                this._Matrix.Rotate(CurrentTile.Rotation.Z, 0, 0, 1);
                float[] PaintColor = { (CurrentTile.Paint.R * 1.0f + 1) / 256, (CurrentTile.Paint.G * 1.0f + 1) / 256, (CurrentTile.Paint.B * 1.0f + 1) / 256, (CurrentTile.Paint.A * 1.0f + 1) / 256 };
                this._CurrentRenderer.SetSurface(PaintColor);
                this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
                this._CurrentRenderer.RenderImage(CurrentTile.Collection.ID, CurrentTile.Collection.TileImages, (CurrentTile.Collection.TileImages.Count > 0) ? CurrentTile.Index() : -1, CurrentTile.Modified);
                CurrentTile.Modified = false;
                for (int i = 0; i < CurrentTile.SubTiles.Count; i++)
                {
                    this._Matrix.ReadMatrix();
                    this._Matrix.Translate(CurrentTile.Translation.X, CurrentTile.Translation.Y, CurrentTile.Translation.Z);
                    this._Matrix.Translate(CurrentTile.SubTiles[i].Translation.X, CurrentTile.SubTiles[i].Translation.Y, CurrentTile.SubTiles[i].Translation.Z);
                    this._Matrix.Scale(CurrentTile.SubTiles[i].Scale.X, CurrentTile.SubTiles[i].Scale.Y, CurrentTile.SubTiles[i].Scale.Z);
                    this._Matrix.Rotate(CurrentTile.Rotation.X, 1, 0, 0);
                    this._Matrix.Rotate(CurrentTile.SubTiles[i].Rotation.X, 1, 0, 0);
                    this._Matrix.Rotate(CurrentTile.Rotation.Y, 0, 1, 0);
                    this._Matrix.Rotate(CurrentTile.SubTiles[i].Rotation.Y, 0, 1, 0);
                    this._Matrix.Rotate(CurrentTile.Rotation.Z, 0, 0, 1);
                    this._Matrix.Rotate(CurrentTile.SubTiles[i].Rotation.Z, 0, 0, 1);
                    PaintColor = new float[] { (CurrentTile.SubTiles[i].Paint.R * 1.0f + 1) / 256, (CurrentTile.SubTiles[i].Paint.G * 1.0f + 1) / 256, (CurrentTile.SubTiles[i].Paint.B * 1.0f + 1) / 256, (CurrentTile.SubTiles[i].Paint.A * 1.0f + 1) / 256 };
                    this._CurrentRenderer.SetSurface(PaintColor);
                    this._CurrentRenderer.SetModelViewMatrix(_Matrix.ModelViewMatrix);
                    this._CurrentRenderer.RenderImage(CurrentTile.SubTiles[i].Collection.ID, CurrentTile.SubTiles[i].Collection.TileImages, (CurrentTile.SubTiles[i].Collection.TileImages.Count > 0) ? CurrentTile.SubTiles[i].Index() : -1, CurrentTile.SubTiles[i].Modified);
                    CurrentTile.SubTiles[i].Modified = false;
                }
            }
            this._Matrix.PopMatrix();
        }
    }
}
