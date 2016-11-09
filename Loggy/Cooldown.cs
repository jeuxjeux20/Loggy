using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        /// <summary>
        /// An optional event when the Cooldown got Elapsed.
        /// </summary>
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
            new Task(() =>
            {
                while (true)
                {
                    if (isFinished)
                        CooldownElapsed(this,new CooldownElapsedEventArgs(this.cooldownSeconds));
                    while (!isFinished) ;
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
    /// <summary>
    /// why did i created this
    /// </summary>
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
