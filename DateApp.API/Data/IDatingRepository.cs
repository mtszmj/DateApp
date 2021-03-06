using System.Collections.Generic;
using System.Threading.Tasks;
using DateApp.API.Helpers;
using DateApp.API.Models;

namespace DateApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<User> GetUserWithUnapprovedPhotos(int id);
        Task<PagedList<User>> GetUsersWithUnapprovedPhotos(UserParams userParams);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Like> GetLike(int userId, int recipentId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}