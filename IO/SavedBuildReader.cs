namespace ShiverBot.IO
{
    internal class SavedBuildReader
    {
        private Dictionary<string, SavedBuild> savedBuilds;

        internal SavedBuildReader()
        {
            savedBuilds = new();
            foreach (string fileName in Directory.GetFiles("addresses/"))
            {
                string[] contents = File.ReadAllLines(fileName);
                string buildId = Path.GetFileNameWithoutExtension(fileName);
                string? version = null;
                string? moneyBase = null;
                string? chunkBase = null;
                string? foodTicketBase = null;
                string? drinkTicketBase = null;
                string? tableTurfBase = null;

                foreach (string line in contents)
                {
                    string[] data = line.Split('=');

                    if (data.Length != 2)
                    {
                        MessageBox.Show($"error: invalid build file {buildId}");
                        break;
                    }
                    else
                    {
                        switch (data[0])
                        {
                            case "version":
                                version = data[1];
                                break;

                            case "moneyBase":
                                moneyBase = data[1];
                                break;

                            case "chunkBase":
                                chunkBase = data[1];
                                break;

                            case "foodTicketBase":
                                foodTicketBase = data[1];
                                break;

                            case "drinkTicketBase":
                                drinkTicketBase = data[1];
                                break;

                            case "tableTurfBase":
                                tableTurfBase = data[1];
                                break;
                        }
                    }
                }

                if (version == null || moneyBase == null || chunkBase == null || foodTicketBase == null || drinkTicketBase == null || tableTurfBase == null)
                {
                    MessageBox.Show("error: build file {buildId} is incomplete.");
                }
                else
                {
                    savedBuilds.Add(buildId, new(buildId, version, moneyBase, chunkBase, foodTicketBase, drinkTicketBase, tableTurfBase));
                }
            }

            if (savedBuilds.Count == 0)
            {
                MessageBox.Show("error: there were no valid address files.");
            }
        }

        internal SavedBuild? GetBuild(string buildId)
        {
            if (savedBuilds.TryGetValue(buildId, out SavedBuild? build))
            {
                return build;
            }

            return null;
        }
    }
}
