using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loggy
{
    /// <summary>
    /// A cooldown
    /// </summary>
    /// <seealso cref="Stopwatch"/>
    public sealed class Cooldown
    {
        /// <summary>
        /// The private stopwatch of this class, the main compenent
        /// </summary>
        private Stopwatch st { get; } = new Stopwatch();

        /// <summary>
        /// The cooldown seconds that has been set to this instance.
        /// </summary>
        public int cooldownSeconds { get; set; }
            public event EventHandler<CooldownElapsedEventArgs> CooldownElapsed;
        /// <summary>
        /// Returns true if the cooldown is finished
        /// </summary>
        public bool isFinished
        {
            get
            {
                return st.Elapsed.Seconds > cooldownSeconds;
            }
        }
        /// <summary>
        /// Returns the seconds left for the cooldown to be reached, if it is alerady, returns null
        /// </summary>
        public int? secondsLeft
        {
            get
            {
                if (!isFinished)
                    return cooldownSeconds - st.Elapsed.Seconds;
                else
                    return null;
            }
        }       
        public static Cooldown operator +(Cooldown a,Cooldown b)
        {
            return new Cooldown(a.cooldownSeconds + b.cooldownSeconds);
        }
        /// <summary>
        /// Restarts the cooldown.
        /// </summary>
        public void Restart()
        {
            st.Restart();
        }
        /// <summary>
        /// Creates a new instance of Cooldown
        /// </summary>
        /// <param name="seconds">The seconds of the cooldown</param>
        public Cooldown(int seconds)
        {
            
            cooldownSeconds = seconds;
            st.Start();
            
            new Task(async () =>
            {
                while (true)
                {
                    if (isFinished && CooldownElapsed != null)
                        CooldownElapsed(this,new CooldownElapsedEventArgs(cooldownSeconds));
                    await Task.Delay(secondsLeft ?? 10/ 2);
                }
            }).Start();
        }
        /// <summary>
        /// Resumes the cooldown into a string
        /// </summary>
        /// <returns>A string that resumes the cooldowns</returns>
        public override string ToString()
        {
            return $"Cooldown seconds : {cooldownSeconds}";
        }
        public class CooldownElapsedEventArgs : EventArgs
        {
            public CooldownElapsedEventArgs(int a)
            {
                cooldownLength = a;
            }
            public readonly int cooldownLength;
        }
    }
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class YoutuberAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;

        // This is a positional argument
        public YoutuberAttribute(string positionalString)
        {
            this.positionalString = positionalString;

            // TODO: Implement code here
                    
            throw new NotImplementedException();
        }

        public string PositionalString
        {
            get { return positionalString; }
        }

        // This is a named argument
        public int NamedInt { get; set; }
    }
    /// <summary>
    /// why did i created this
    /// </summary>
    [Youtuber("Electronicwiz1")]
    public sealed class Joolya : IComparable
    {
        public List<Computer> myComputers = new List<Computer> { new Computer() };
        public int CompareTo(object obj)
        {
            
            if(obj is Joolya)
            {
                Joolya j = (Joolya)obj;
                if (j.myComputers == myComputers)
                {
                    return 0;
                }
            }
            return -1;
        }

        public string ToJoolya(string joo)
        {
            return joo += "...LOL!";
        }
    }
    public class Computer : IEnumerable<Computer.Wirus>
    {
        public Computer() { }
        public Computer(IEnumerable<Wirus> vs)
        {
            wiruses = (HashSet<Wirus>)vs;
        }
        public class Wirus
        {
            public string Name { get; set; }
            public short Dangerousity { get; set; }
        }
        public HashSet<Wirus> wiruses = new HashSet<Wirus>();
        public bool HasAVirus { get { return wiruses.Any(); } }

        IEnumerator<Wirus> IEnumerable<Wirus>.GetEnumerator()
        {
            return ((IEnumerable<Wirus>)wiruses).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Wirus>)wiruses).GetEnumerator();
        }
    }
}
