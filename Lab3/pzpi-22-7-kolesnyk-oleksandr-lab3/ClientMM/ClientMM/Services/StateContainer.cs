namespace ClientMM.Services
{
    public class StateContainer
    {
        private bool _showHeader = true;

        public bool ShowHeader
        {
            get => _showHeader;
            set
            {
                _showHeader = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private async Task NotifyStateChanged() => OnChange?.Invoke();
    }
}