using System.Collections.Generic;
using System.Linq;

namespace CSGSI.Nodes
{
    /// <summary>
    /// A node containing a collection of <see cref="WeaponNode"/>s.
    /// </summary>
    public class WeaponsNode : NodeBase
    {
        /// <summary>
        /// The list of all weapons in this node.
        /// </summary>
        public List<WeaponNode> Weapons { get; set; } = new List<WeaponNode>();

        /// <summary>
        /// Gets the list of <see cref="WeaponNode"/>s.
        /// </summary>
        public IEnumerable<WeaponNode> WeaponList => Weapons;

        /// <summary>
        /// The amount of weapons this node contains.
        /// </summary>
        public int Count => Weapons.Count;

        /// <summary>
        /// The weapon/equipment the player has currently pulled out.
        /// </summary>
        public WeaponNode ActiveWeapon
        {
            get
            {
                foreach (var weapon in Weapons.Where(weapon => weapon.State == WeaponState.Active || weapon.State == WeaponState.Reloading))
                {
                    return weapon;
                }

                return new WeaponNode("");
            }
        }

        internal WeaponsNode(string json)
            : base(json)
        {
            foreach (var weapon in _data.Children())
            {
                if (weapon.First != null)
                {
                    Weapons.Add(new WeaponNode(weapon.First.ToString()));
                }
            }
        }

        /// <summary>
        /// Gets the weapon with index &lt;index&gt;
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WeaponNode this[int index] => index > Weapons.Count - 1 ? new WeaponNode("") : Weapons[index];
    }
}