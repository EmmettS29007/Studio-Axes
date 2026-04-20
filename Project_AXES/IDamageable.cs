using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public interface IDamageable
    {
        int Health
        {
            get;
            set;
        }

        /*
        /// <summary>
        /// Lowers health by the damage taken.
        /// </summary>
        /// <param name="damage">The amount of damage to be taken</param>
        void TakeDamage(int damage)
        {
            Health -= damage;
            // If the object reaches zero health, it will die
            if (Health <= 0)
            {
                // Die();
            }
        }

        /// <summary>
        /// Method that handles incoming "Death" logic.
        /// Ideally requires IDamageable objects to die
        /// if their health reaches below zero.
        /// </summary>
        void Die();
        */
    }
}