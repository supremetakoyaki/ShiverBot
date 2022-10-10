namespace ShiverBot.Forms
{
    public partial class GearSeedFinderResultsForm : Form
    {
        public GearSeedFinderResultsForm()
        {
            InitializeComponent();
        }

        public void ShowResults(List<(long, uint)> results)
        {
            foreach ((long, uint) tuple in results)
            {
                resultsDataGridView.Rows.Add($"{tuple.Item1:X}", $"0x{tuple.Item2:X}");
            }
        }
    }
}
