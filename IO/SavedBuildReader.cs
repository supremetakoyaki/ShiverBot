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
                string? gearBase = null;


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

                            case "gearBase":
                                gearBase = data[1];
                                break;
                        }
                    }
                }

                if (version == null || gearBase == null)
                {
                    MessageBox.Show("error: build file {buildId} is incomplete.");
                }
                else
                {
                    if (language == "1")
                    {
                        _savedBuilds.Add(buildId, new(buildId, version, gearBase));
                    }
                    else
                    {
                        _savedBuilds.Add($"{buildId}.{language}", new(buildId, version, gearBase));
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
