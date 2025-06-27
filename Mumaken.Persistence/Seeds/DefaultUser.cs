using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Persistence.Seeds
{
    public static class DefaultUser
    {
        public static List<ApplicationDbUser> IdentityBasicUserList()
        {
            var hasher = new PasswordHasher<ApplicationDbUser>();

            return new List<ApplicationDbUser>()
            {
                new ApplicationDbUser
                {
                    // ادمن لوحه التحكم
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UserName = "aait@aait.sa",
                    Email = "aait@aait.sa",
                    user_Name = "aait@aait.sa",
                    ShowPassword = "123456",
                    TypeUser =(int) UserType.Admin,
                    ActiveCode = true,
                    PublishDate = DateTime.Now,
                    IsActive = true,
                    ImgProfile = "images/Users/default.jpg",
                    NormalizedEmail = "aait@aait.sa",
                    NormalizedUserName = "Aait@Aait.sa",
                    PhoneCode="",
                    CityId=1

                },
                new ApplicationDbUser
                {
                    // يوزر لسرفس
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UserName = "Api@aait.sa",
                    Email = "Api@aait.sa",
                    user_Name = "Api@aait.sa",
                    ShowPassword = "123456",
                    TypeUser =(int) UserType.Client,
                    ActiveCode = true,
                    PublishDate = DateTime.Now,
                    IsActive = true,
                    ImgProfile = "images/Users/default.jpg",
                    NormalizedEmail = "Api@aait.sa",
                    NormalizedUserName = "Api@Aait.sa",
                    PhoneCode="",
                    CityId=1
                },
            };
        }
    }
}
