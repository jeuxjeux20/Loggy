using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        private Stopwatch St { get; } = new Stopwatch();
        private bool _c;
        /// <summary>
        /// The cooldown seconds that has been set to this instance.
        /// </summary>
        public int CooldownSeconds { get; set; }

        /// <summary>
        /// Returns true if the cooldown is finished
        /// </summary>
        public bool IsFinished
        {
            get
            {
                if (!_c)
                {
                    return St.Elapsed.Seconds > CooldownSeconds;
                }
                _c = false;
                return true;
            }
        }
        /// <summary>
        /// Returns the seconds left for the cooldown to be reached, if it is alerady, returns null
        /// </summary>
        public int? SecondsLeft
        {
            get
            {
                if (!IsFinished)
                    return CooldownSeconds - St.Elapsed.Seconds;
                return null;
            }
        }
        public static Cooldown operator +(Cooldown a, Cooldown b)
        {
            return new Cooldown(a.CooldownSeconds + b.CooldownSeconds);
        }
        /// <summary>
        /// Restarts the cooldown.
        /// </summary>
        public void Restart()
        {
            St.Restart();
        }

        /// <summary>
        /// Creates a new instance of Cooldown
        /// </summary>
        /// <param name="seconds">The seconds of the cooldown</param>
        /// <param name="isAleradyCompleted">ye m8 will the first time complete m8</param>
        public Cooldown(int seconds, bool isAleradyCompleted = true)
        {
            CooldownSeconds = seconds;
            St.Start();
            _c = isAleradyCompleted;
           
        }
        /// <summary>
        /// Resumes the cooldown into a string
        /// </summary>
        /// <returns>A string that resumes the cooldowns</returns>
        public override string ToString()
        {
            return $"Cooldown seconds : {CooldownSeconds}";
        }
        public class CooldownElapsedEventArgs : EventArgs
        {
            public CooldownElapsedEventArgs(int a)
            {
                CooldownLength = a;
            }
            public readonly int CooldownLength;
        }
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal sealed class YoutuberAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        // This is a positional argument
        public YoutuberAttribute(string positionalString)
        {
            PositionalString = positionalString;

            // TODO: Implement code here

          //  throw new NotImplementedException();
        }

        public string PositionalString { get; }

        // This is a named argument
        public int NamedInt { get; set; }
    }
    /// <summary>
    /// why did i created this
    /// </summary>
    [Youtuber("Electronicwiz1")]
    public sealed class Joolya : IComparable
    {
        public List<Computer> MyComputers = new List<Computer> { new Computer() };
        public int CompareTo(object obj)
        {
            var joolya = obj as Joolya;
            if (joolya != null)
            {
                Joolya j = joolya;
                if (j.MyComputers == MyComputers)
                {
                    return 0;
                }
            }
            return -1;
        }

        public string ToJoolya(string joo)
        {
            return joo + "...LOL!";
        }
    }
    public class Computer : IEnumerable<Computer.Wirus>
    {
        public Computer() { }
        public Computer(IEnumerable<Wirus> vs)
        {
            Wiruses = (HashSet<Wirus>)vs;
        }
        public class Wirus
        {
            public string Name { get; set; }
            public short Dangerousity { get; set; }
        }
        public HashSet<Wirus> Wiruses = new HashSet<Wirus>();
        public bool HasAVirus => Wiruses.Any();

        IEnumerator<Wirus> IEnumerable<Wirus>.GetEnumerator()
        {
            return ((IEnumerable<Wirus>)Wiruses).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Wirus>)Wiruses).GetEnumerator();
        }
    }
}
