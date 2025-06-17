using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Modules.Scp127;
using MEC;
using UnityEngine;

using Random = System.Random;

namespace Taser
{
    public class TaserItem : CustomItem
    {
        static Random random = new Random();
        public CoroutineHandle _coroutine;
        public override uint Id { get; set; } = 69;
        public override string Name { get; set; } = "Тайзер";
        public override string Description { get; set; } = "При попадании в человека/scp-049 трясет его камеру";
        public override float Weight { get; set; } = 0.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.GunCOM15;
        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (item is Firearm f)
            {
                f.MaxMagazineAmmo = 1;
                f.MagazineAmmo = 1;
            }
            base.OnAcquired(player, item, displayMessage);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Attacker.CurrentItem)) return;
            if (ev.Player.IsScp && ev.Player.Role.Type != PlayerRoles.RoleTypeId.Scp049) return;
            if (ev.Player == null || ev.Attacker == null) return;

            ev.IsAllowed = false;
            ev.Player.EnableEffect(Exiled.API.Enums.EffectType.Slowness, 50, 10);
            ev.Player.EnableEffect(Exiled.API.Enums.EffectType.Flashed, 1, 1);

            Timing.RunCoroutine(ShakeCamera(ev.Player));
        }

        private IEnumerator<float> ShakeCamera(Player player)
        {
            for (int i = 0; i < 40; i++)
            {
                player.Rotation = new Quaternion(random.Next(0, 361), random.Next(0, 361), random.Next(0, 361), random.Next(0, 361));
                yield return Timing.WaitForSeconds(0.25f);
            }
        }
    }
}
