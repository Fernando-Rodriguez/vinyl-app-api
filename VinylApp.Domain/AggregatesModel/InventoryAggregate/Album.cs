namespace VinylApp.Domain.AggregatesModel.InventoryAggregate
{
    public class Album
    {
        public Album(
            string name,
            string artist)
        {
            Name = name;
            Artist = artist;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Artist { get; private set; }

        public string ImageUrl { get; private set; }

        public int Rating { get; private set; } = 0;

        public void SetRating(int rating)
        {
            if (rating > 5 || rating < 1) return;

            Rating = rating;
        }

        public void UpdateAlbumName(string newName)
        {
            Name = newName;
        }

        public void UpdateArtistName(string newArtist)
        {
            Artist = newArtist;
        }

        public void SetImageUrl(string url)
        {
            ImageUrl = url;
        }
    }
}
