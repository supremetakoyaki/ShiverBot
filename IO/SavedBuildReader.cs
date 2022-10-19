using ShiverBot.Network;

namespace ShiverBot.IO
{
    internal class SavedBuildReader
    {
        private readonly Dictionary<string, SavedBuild> _savedBuilds;

        internal SavedBuildReader()
        {
            _savedBuilds = new();
            foreach (string fileName in Directory.GetFiles("addresses/"))
            {
                string[] contents = File.ReadAllLines(fileName);
                string buildId = Path.GetFileNameWithoutExtension(fileName)[..16];
                string? language = null;
                string? version = null;
                string? moneyBase = null;
                string? chunkBase = null;
                string? foodTicketBase = null;
                string? drinkTicketBase = null;
                string? gearBase = null;
                string? tableTurfRankBase = null;

                // table turf dynamic addressing
                string? tableTurfSpecialBase = null;
                string? tableTurfSpecialStep = null;
                string? tableTurfPointBase = null;
                string? tableTurfPointStep = null;

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

                            case "language":
                                language = data[1];
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

                            case "gearBase":
                                gearBase = data[1];
                                break;

                            case "tableTurfRankBase":
                                tableTurfRankBase = data[1];
                                break;

                            case "tableTurfSpecialBase":
                                tableTurfSpecialBase = data[1];
                                break;

                            case "tableTurfSpecialStep":
                                tableTurfSpecialStep = data[1];
                                break;

                            case "tableTurfPointBase":
                                tableTurfPointBase = data[1]; 
                                break;

                            case "tableTurfPointStep":
                                tableTurfPointStep = data[1];
                                break;
                        }
                    }
                }

                if (version == null || moneyBase == null || chunkBase == null || foodTicketBase == null || drinkTicketBase == null || gearBase == null || tableTurfRankBase == null || tableTurfSpecialBase == null || tableTurfSpecialStep == null || tableTurfPointBase == null || tableTurfPointStep == null)
                {
                    MessageBox.Show("error: build file {buildId} is incomplete.");
                }
                else
                {
                    List<MemoryStep> specialSteps = new();
                    foreach (string sStep in tableTurfSpecialStep.Split(','))
                    {
                        specialSteps.Add(new(sStep[0], Convert.ToInt64(sStep[1..], 16)));
                    }

                    List<MemoryStep> pointSteps = new();
                    foreach (string pStep in tableTurfPointStep.Split(','))
                    {
                        pointSteps.Add(new(pStep[0], Convert.ToInt64(pStep[1..], 16)));
                    }

                    if (language == "1")
                    {
                        _savedBuilds.Add(buildId, new(buildId, version, moneyBase, chunkBase, foodTicketBase, drinkTicketBase, gearBase, tableTurfRankBase, new(tableTurfSpecialBase, specialSteps, tableTurfPointBase, pointSteps)));
                    }
                    else
                    {
                        _savedBuilds.Add($"{buildId}.{language}", new($"{buildId}.{language}", version, moneyBase, chunkBase, foodTicketBase, drinkTicketBase, gearBase, tableTurfRankBase, new(tableTurfSpecialBase, specialSteps, tableTurfPointBase, pointSteps)));
                    }
                }
            }

            if (_savedBuilds.Count == 0)
            {
                MessageBox.Show("error: there were no valid address files.");
            }
        }

        internal SavedBuild? GetBuild(string buildId)
        {
            if (_savedBuilds.TryGetValue(buildId, out SavedBuild? build))
            {
                return build;
            }

            return null;
        }
    }
}
