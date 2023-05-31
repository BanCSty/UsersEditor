using UsersEditor.Models;

namespace UsersEditor.Repository
{
    //Service...
    public class UserRepository
    {
        private static List<User> users = new List<User>()
            {
                new User()
                {
                    Guid = Guid.NewGuid(),
                    Login = "Ban@exaple.com",
                    Password = "&u*eVFG95%",
                    Name = "Ban",
                    Gender = 1,
                    Birthday = new DateTime(2001,3, 15),
                    Admin = true,
                    CreatedOn = new DateTime(2023, 5, 24, 18, 30, 25),
                    CreateBy = "",
                    ModifiedOn = DateTime.MinValue,
                    ModifiedBy = "",
                    RevokedOn = DateTime.MinValue,
                    RevokedBy = "",
                },
                new User()
                {
                    Guid = Guid.NewGuid(),
                    Login = "Alex@exaple.com",
                    Password = "&u*eVFG95%",
                    Name = "Alex",
                    Gender = 1,
                    Birthday = new DateTime(2000,3, 15),
                    Admin = false,
                    CreatedOn = new DateTime(2023, 6, 21, 18, 30, 25),
                    CreateBy = "",
                    ModifiedOn = DateTime.MinValue,
                    ModifiedBy = "",
                    RevokedOn = DateTime.MinValue,
                    RevokedBy = "",
                },
            };

        //Возвращаем экземпляр пользователя без пароля
        public static async Task<User> Find(string username)
        {
            var task = Task.Run(() => users.FirstOrDefault(user => user.Login.ToLower() == username.ToLower()));
            var user = await task;
            var newUser = new User()
            {
                Guid = user.Guid,
                Login = user.Login,
                Password = default,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreateBy = user.CreateBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
                RevokedOn = user.RevokedOn,
                RevokedBy = user.RevokedBy,
            };
            return newUser;
        }

        public static async Task<User> Find(string username, string password) => await
            Task.Run(() => users.FirstOrDefault(user => user.Login.ToLower() == username.ToLower() && user.Password == password));



        //#1
        public static async Task CreateUser(string adminLogin, UserCreateDTO user)
        {
            var task = Task.Run(() => users.Any(u => u.Login == user.Login));
            var ressult = await task;
            if (ressult)
                throw new ArgumentException("Invalid `login`: A user with this login address already exists.");

            var addUser = Task.Run(() => users.Add(
                new User()
                {
                    Guid = Guid.NewGuid(),
                    Login = user.Login,
                    Password = user.Password,
                    Name = user.Name,
                    Gender = user.Gender,
                    Birthday = user.Birthday,
                    Admin = user.Admin,
                    CreatedOn = DateTime.Now,
                    CreateBy = adminLogin,
                    ModifiedOn = DateTime.MinValue,
                    ModifiedBy = default,
                    RevokedOn = DateTime.MinValue,
                    RevokedBy = default
                }
            ));
        }

        //#2
        public static async Task<User> UpdateName(User user, string name, int gender, DateTime birthday)
        {
            var task = Task.Run(() => users.FirstOrDefault(x => x.Login == user.Login));

            var usr = await task;
            if (usr == null) throw new ArgumentException("User not  fo");

            usr.Name = name;
            usr.Gender = gender;
            usr.Birthday = birthday;

            return usr;
        }

        //#3
        public static async Task<User> UpdatePassword(User user, string password)
        {
            var task = Task.Run(() => users.FirstOrDefault(x => x.Login == user.Login));
            var usr = await task;

            if (usr == null) return null;

            usr.Password = password;

            return usr;
        }

        //#4
        public static async Task<User> UpdateLogin(User user, string login)
        {
            var task = Task.Run(() => users.FirstOrDefault(x => x.Login == user.Login));
            var usr = await task;

            if (usr == null) return null;

            var loginValidator = Task.Run(() => users.Any(u => u.Login == login));

            if (loginValidator.Result)
                throw new ArgumentException("Invalid `login`: A user with this login address already exists.");

            usr.Login = login;

            return usr;
        }
        //#5
        public static async Task<List<User>> GetAllUser() => await
            Task.Run(() => users.Where(u => u.RevokedOn == DateTime.MinValue).OrderBy(u => u.CreatedOn).ToList());

        //#8
        public static async Task<List<User>> GetOlderUsers(int age) => await
            Task.Run(() => users.Where(u => (DateTime.Today.Year - u.Birthday.Year) > age).ToList());

        //#9
        public static async Task<int> HardDeleteUser(string login) => await
            Task.Run(() => users.RemoveAll(u => u.Login.ToLower() == login.ToLower()));

        public static async Task SoftDeleteUser(string adminLogin, string login)
        {
            var task = Task.Run(() => users.FirstOrDefault(u => u.Login.ToLower() == login.ToLower()));

            var user = await task;
            if (user == null) throw new ArgumentException("User not found");

            user.RevokedOn = DateTime.Now;
            user.RevokedBy = adminLogin;
        }

        //#10
        public static async Task RecoveryUser(string adminLogin, string login)
        {
            var task = Task.Run(() => users.FirstOrDefault(u => u.Login.ToLower() == login.ToLower()));

            var user = await task;

            if (user == null) throw new ArgumentException("User not found");

            user.RevokedOn = DateTime.MinValue;
            user.RevokedBy = default;

            user.ModifiedBy = adminLogin;
            user.ModifiedOn = DateTime.Now;
        }
    }
}
