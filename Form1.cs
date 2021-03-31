using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Particle2D
{
    public partial class Form1 : Form
    {
        Thread GameThread;
        Graphics g;

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            GameThread = new Thread(MainLoop);
            //GameThread.Start();
        }

        #region thread controls
        private void button1_Click(object sender, EventArgs e)
        {
            GameThread.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GameThread.Abort();
            GameThread = null;
            GameThread = new Thread(MainLoop);

        }
        #endregion

        public void UpdateDisplay(List<Particle> particles)
        {
            //Updates the content that is being displayed.
            g.Clear(Color.White);
            foreach (Particle particle in particles)
            {
                SolidBrush p = new SolidBrush(Color.Red);
                g.FillEllipse(p, particle.x, particle.y, 5, 5); //Brush, x, y, widthX, widthY
                Console.WriteLine("Completed an iteration");
            }
        }

        private void MainLoop() 
        {
            int currentFrame = 0; // current frame in the process
            List<Particle> particles = new List<Particle>();

            int particlesToGenerate = 1000;
            int currentParticles = 0;
            Random rand = new Random();

            while (currentParticles != particlesToGenerate)
            {
                Particle newParticle = new Particle();
                newParticle.TTL = rand.Next(3, 100);
                newParticle.x = 400;
                newParticle.y = 300;

                newParticle.velX = rand.Next(-20, 20);
                newParticle.velY = rand.Next(-20, 20);

                particles.Add(newParticle);
                Console.WriteLine("Generated Particle " + currentParticles);
                currentParticles++;
            }
            bool ff = true; //debugging line
            while (ff == true)
            {
                //Thread.Sleep(1000/50); //Stop for a sec

                List<Particle> nextIterationParticles = new List<Particle>();

                //Make them move
                foreach(Particle particle in particles)
                {
                    //Check ttl
                    if(particle.TTL == currentFrame)
                    {
                        continue;
                    }
                    //If the particle should still be alive, then it continues through here.
                    particle.x = particle.x + particle.velX;
                    particle.y = particle.y + particle.velY;
                    nextIterationParticles.Add(particle); //Allow the particle into the new generation.
                }
                particles.Clear();
                
                foreach(Particle part in nextIterationParticles)
                {
                    particles.Add(part);
                }

                if (particles.Count == 0)
                {
                    MessageBox.Show("No Particles Remaining.");
                    GameThread.Abort();
                }
                else
                {
                    //Apply new stuff
                    this.Invoke((MethodInvoker)delegate { UpdateDisplay(particles); });
                    Console.WriteLine("Iteration Complete: " + currentFrame);
                    //Go to next frame
                    currentFrame++;
                    //ff = false;  //Uncomment to stop after frame 0
                }
            }
        }
    }
}
