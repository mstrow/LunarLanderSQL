using System;
using System.Collections.Generic;
using System.Data;

namespace LanderConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            LanderSession session = new LanderSession("Server=localhost;Port=3306;Database=LanderGame;Uid=devuser;password=password;");
            session.Login("Admin","Pass");
            
            session.ListLanders();
            session.PrintMaps();
            
            session.NewMatch("Apollo", "moon");
            //session.PrintCurrentMap();
            session.ControlRocket(-20.0f,true);
            session.UpdateShip();
            session.ControlRocket(-10.0f,true);
            session.UpdateShip();
            session.ControlRocket(+12.0f,false);
            session.UpdateShip();
            session.ControlRocket(-20.0f,true);
            session.UpdateShip();
            session.ControlRocket(+12.0f,false);
            session.UpdateShip();
            session.EndMatch();
            session.SendChat("Message");
            session.PrintChat();
            User bogo = new User("Bogo", "bozo", false, 0, 0, false);
            session.NewUserAdmin(bogo);
            session.ListUsers();
            session.DeleteUserAdmin("bogo");
            User existingUser = session.GetUserByNameAdmin("tworcs2k");
            existingUser.HighScore += 500;
            session.EditUserAdmin(existingUser);
            session.Logout();
        }
        
    }
}
