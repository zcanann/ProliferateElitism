using Microsoft.Xna.Framework;
using System.ComponentModel;
using ProjectMercury;
using System;

namespace River
{
    public class EllipseEmitter : ProjectMercury.Emitters.Emitter
    {
        public float Radius;
        private bool Ring;

        public EllipseEmitter(float Radius, bool Ring = false)
            : base()
        {
            this.Radius = Radius;
            this.Ring = Ring;
        }

        public override ProjectMercury.Emitters.Emitter DeepCopy()
        {
            EllipseEmitter emitter = new EllipseEmitter(this.Radius, this.Ring);
            base.CopyBaseFields(emitter);
            return emitter;

        }

        protected override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 force)
        {
            float distX;
            float distY;

            if (!Ring)
            {
                distX = FastRand.NextSingle(0f, Radius);
                distY = FastRand.NextSingle(0f, Radius / 2);
            }
            else
            {
                distX = Radius;
                distY = Radius / 2;
            }
            
            FastRand.NextUnitVector(out force);

            offset = new Vector2(force.X * distX, force.Y * distY);
        }
    }
}
