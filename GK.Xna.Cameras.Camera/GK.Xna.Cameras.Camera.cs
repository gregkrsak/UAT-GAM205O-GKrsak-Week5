

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

namespace GK.Xna.Cameras
{
    public class Camera
    {
        protected Vector3 _position;
        protected Vector3 _lookAt;
        protected Vector3 _up;

        protected Double _fovRadians;
        protected Double _aspectRatio;
        protected Double _nearPlaneDistance;
        protected Double _farPlaneDistance;

        protected Matrix _viewMatrix;
        protected Matrix _projectionMatrix;


        public Camera(Vector3 position, Vector3 lookAt, Vector3 up, Microsoft.Xna.Framework.Game targetGame,
                      Double FOVdegrees = 45.0,
                      Double nearPlaneDistance = 1.0,
                      Double farPlaneDistance = 10000.0)
        {
            this._position = position;
            this._lookAt = lookAt;
            this._up = up;

            this._fovRadians = MathHelper.ToRadians((float)FOVdegrees);
            this._aspectRatio = targetGame.GraphicsDevice.Viewport.AspectRatio;

            this._nearPlaneDistance = nearPlaneDistance;
            this._farPlaneDistance = farPlaneDistance;

            this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                                        (float)this._fovRadians,
                                        (float)this._aspectRatio,
                                        (float)this._nearPlaneDistance,
                                        (float)this._farPlaneDistance);
            this._viewMatrix = Matrix.CreateLookAt(
                                    this._position,
                                    this._lookAt,
                                    this._up);
        }


        public Camera(Matrix view, Matrix projection)
        {
            this._viewMatrix = view;
            this._projectionMatrix = projection;
        }


        public static GK.Xna.Cameras.Camera Orthographic(Vector2 lookAt)
        {
            Matrix view = Matrix.CreateLookAt(new Vector3(lookAt, 0.0f), new Vector3(lookAt, 1.0f), new Vector3(0.0f, -1.0f, 0.0f));
            Matrix projection = Matrix.CreateOrthographic(lookAt.X * 2.0f, lookAt.Y * 2.0f, -0.5f, 1.0f);
            return new GK.Xna.Cameras.Camera(view, projection);
        }


        public Matrix ProjectionMatrix
        {
            get
            {
                return this._projectionMatrix;
            }
        }


        public Matrix ViewMatrix
        {
            get
            {
                return this._viewMatrix;
            }
        }
    }
}
