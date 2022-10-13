using ShiverBot.Network;

namespace ShiverBot.IO
{
    internal record struct SavedBuildTableTurf(string SpecialBaseAddress, List<MemoryStep> SpecialMemorySteps, string PointsBaseAddress, List<MemoryStep> PointsMemorySteps);
}
