namespace ShiverBot.IO
{
    internal record SavedBuild(string BuildId, string GameVersion, string MoneyAddress, string ChunkBaseAddress, string FoodTicketBase, string DrinkTicketBase, string GearBase, string TableTurfRankBase, SavedBuildTableTurf TableTurf);
}
