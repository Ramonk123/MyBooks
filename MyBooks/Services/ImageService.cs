using Data.Data;

namespace MyBooks.Services
{
    public class ImageService
    {
        private readonly MyBooksDbContext _context;

        public ImageService(MyBooksDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetImage(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            var user = await _context.Users.FindAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.Base64ProfileImage))
                return null;

            return $"data:image/jpeg;base64,{user.Base64ProfileImage}";
        }


        public async Task<bool> SaveImage(string userId, Stream imageStream)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            if (imageStream == null || imageStream.Length == 0)
                throw new ArgumentException("Image data cannot be null or empty", nameof(imageStream));

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                user.Base64ProfileImage = Convert.ToBase64String(imageBytes);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}