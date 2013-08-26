

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GK.Xna.Models
{
    public class Model
    {
        protected Microsoft.Xna.Framework.Graphics.Model _data;
        protected Vector3 _position;
        protected Quaternion _rotation;


        public Model(String contentName, Vector3 worldPosition, Game targetGame)
        {
            this._data = targetGame.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(contentName);
            this._position = worldPosition;
            this._rotation = Quaternion.Identity;
        }


        public void Draw(GK.Xna.Cameras.Camera targetCamera)
        {
            Matrix[] transforms = new Matrix[this._data.Bones.Count];
            this._data.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix position = Matrix.CreateTranslation(this._position);
            Matrix rotation = Matrix.CreateFromQuaternion(this._rotation);

            foreach (ModelMesh mesh in this._data.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.PreferPerPixelLighting = false;

                    effect.World = transforms[mesh.ParentBone.Index] * position * rotation;

                    effect.Projection = targetCamera.ProjectionMatrix;
                    effect.View = targetCamera.ViewMatrix;
                }
                mesh.Draw();
            }
        }


        public Vector3 Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }


        public Quaternion Rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                this._rotation = value;
            }
        }
    }
}
