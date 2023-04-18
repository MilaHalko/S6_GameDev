using Items.Data;
using StatsSystem;

namespace Items.Core
{
    public class Potion : Item
    {
        private readonly StatsController _statsController;
        private StatChangingItemDescriptor _itemDescriptor;
        private int _amount;
        public override int Amount => _amount;

        public Potion(ItemDescriptor descriptor, StatsController statsController) : base(descriptor)
        {
            _itemDescriptor = descriptor as StatChangingItemDescriptor;
            _statsController = statsController;
            _amount = 1;
        }

        public override void Use()
        {
            _amount--;
            foreach (var stat in _itemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }

            if (_amount <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public void AddToStack(int amount)
        {
            _amount += amount;
        }
    }
}