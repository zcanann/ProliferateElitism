using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using ProjectMercury.Modifiers;
using ProjectMercury;

namespace River
{
    public sealed class ControlledFade : ProjectMercury.Modifiers.Modifier
    {
        public override Modifier DeepCopy()
        {
            ControlledFade Copy = new ControlledFade();
            Copy.SetSpeed(Speed);
            Copy.SetInitial(Initial);
            return Copy;
           // throw new NotImplementedException();
        }

        private float Speed = 1f;
        private float Initial = 0f;

        public void SetSpeed(float Speed)
        {
            this.Speed = Speed;
        }

        public void SetInitial(float Initial)
        {
            this.Initial = Initial;
        }

       protected  override unsafe void Process(float dt, Particle* particleArray, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Particle* currentParticle = (particleArray + i);

                currentParticle->Colour.W = (this.Initial - this.Initial * currentParticle->Age * Speed);

                //if (currentParticle->Age > 2f)
                //    currentParticle->Colour.W = Initial;
            }
        }


    }
}
