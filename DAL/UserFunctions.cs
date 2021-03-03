using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DAB3.Models;
using DAB3.Services;
using MongoDB.Driver;

namespace DAB3.DAL
{
    public class UserFunctions
    {
        UsersService _usersService;
        private CirclesService _circlesService;


        public UserFunctions()
        {
            _usersService = new UsersService();
            _circlesService = new CirclesService();
        }

        public string CreatePost(string MyName, string content, List<string> CircleNamesList, string img)
        {
            Users MyUser = _usersService.FindSingleUserFromName(MyName);

            Posts post1 = new Posts
            {
                UserId = MyUser.Id,
                Text = content,
                Time = DateTime.Now,
                Id = DateTime.Now.ToLongTimeString(),
                Comments = new List<Comments>(),
                img = img
            };

            foreach (var circleName in CircleNamesList)
            {
                Circle myCircle = _circlesService.FindSingleCircleFromName(circleName, MyUser.Id);
                myCircle.Posts.Add(post1);
                _circlesService.Update(myCircle.Id, myCircle);
            }

            return "The post has been created";
        }


        /////////////////////// COMMENT//////////////////////////


        public string CreateComment(string comment, string MyName, string Postid)
        {
            Users MyUser = _usersService.FindSingleUserFromName(MyName);
            Circle myCircle = new Circle();
            Posts myPosts = new Posts();

            Comments comment1 = new Comments
            {
                Time = DateTime.Now,
                Text = comment,
                UserId = MyUser.Id
            };
            foreach (var CircleId in MyUser.MyCirclesId)
            {
                myCircle = _circlesService.Get(CircleId);

                foreach (var post in myCircle.Posts)
                {
                    if (post.Id == Postid)
                    {
                        post.Comments.Add(comment1);
                        _circlesService.Update(myCircle.Id, myCircle);
                        return "Comment has been created";
                    }
                }
            }
            return "Comment has not been created";
        }


        //////////////////////// BANLIST //////////////////////////

        public string AddUserToBanList(string myName, string banName)
        {
            Users _banUser = _usersService.FindSingleUserFromName(banName);

            Users _myUser = _usersService.FindSingleUserFromName(myName);

            _myUser.BlackListedUserId.Add(_banUser.Id);
            _usersService.Update(_myUser.Id, _myUser);

            return "User added to Ban list";

        }


        public string RemoveUserFromBanList(string myName, string banName)
        {
            Users _banUser = _usersService.FindSingleUserFromName(banName);
            Users _myUser = _usersService.FindSingleUserFromName(myName);

            _myUser.BlackListedUserId.Remove(_banUser.Id);
            _usersService.Update(_myUser.Id, _myUser);

            return "User removed from Ban list";
        }

        /////////////////// SUBSCRIBER /////////////////////////////
        public string SubcribeToUser(string myName, string OtherUserName)
        {
            Users MyUser = _usersService.FindSingleUserFromName(myName);
            Users OtherUser = _usersService.FindSingleUserFromName(OtherUserName);

            List<Circle> subsribeCircle = _circlesService.FindCircleFromName("Public", OtherUser.Id);
            subsribeCircle[0].UserIds.Add(MyUser.Id);
            _circlesService.Update(OtherUser.Id, subsribeCircle[0]);

            MyUser.SubscribedTo.Add(subsribeCircle[0].Id);
            _usersService.Update(MyUser.Id, MyUser);

            return "User added to subscribe list";
        }

        public string UnsubcribeToUser(string myName, string OtherUserName)
        {
            Users MyUser = _usersService.FindSingleUserFromName(myName);
            Users OtherUser = _usersService.FindSingleUserFromName(OtherUserName);

            List<Circle> subsribeCircle = _circlesService.FindCircleFromName("Public", OtherUser.Id);
            subsribeCircle[0].UserIds.Remove(MyUser.Id);
            _circlesService.Update(OtherUser.Id, subsribeCircle[0]);

            MyUser.SubscribedTo.Remove(subsribeCircle[0].Id);
            _usersService.Update(MyUser.Id, MyUser);

            return "User removed from subscribe list";
        }



        /////////////////////// CIRCLE ///////////////////////
        public string CreateCircle(string myName, string circleName)
        {
            Users user1 = _usersService.FindSingleUserFromName(myName);
            List<string> users = new List<string>();

            users.Add(user1.Id);
            Circle circle1 = new Circle
            {
                UserIds = users,
                CircleOwner = user1.Id,
                CircleName = circleName
            };

            var circle = _circlesService.Create(circle1);

            user1.MyCirclesId.Add(_circlesService.Get(circle1.Id).Id);
            _usersService.Update(user1.Id, user1);

            return "Circle created <br/> Name: " + circleName + "<br/>" + "Circle Owner: " + myName;
        }

        public List<string> ShowMyCircles(string myName)
        {
            Users MyUser = _usersService.FindSingleUserFromName(myName);
            List<Circle> myCircles = new List<Circle>();
            List<string> CircleStrings = new List<string>();

            foreach (var circleID in MyUser.MyCirclesId)
            {
                CircleStrings.Add(_circlesService.Get(circleID).CircleName);
            }

            return CircleStrings;
        }

        public Circle ShowCircle(string myName, string circleName)
        {
            Users MyUser = _usersService.FindSingleUserFromName(myName);
            Circle MyCircle = _circlesService.FindSingleCircleFromName(circleName, MyUser.Id);

            Circle Chosen = new Circle();

            foreach (var circleID in MyUser.MyCirclesId)
            {
                if (MyCircle.Id == circleID)
                {
                    Chosen = MyCircle;
                }

            }

            return Chosen;
        }

        public string DeleteCircle(string myName, string circleName)
        {
            Users user1 = _usersService.FindSingleUserFromName(myName);

            Circle circle1 = _circlesService.FindSingleCircleFromName(circleName, user1.Id);
            if (circle1.CircleOwner != user1.Id)
            {
                return "You are not the owner of this circle";
            }

            foreach (var userId in circle1.UserIds)
            {
                Users OtherCircleUser = _usersService.Get(userId);
                OtherCircleUser.MyCirclesId.Remove(circle1.Id);
            }

            _circlesService.Remove(circle1.Id);
            return "The circle has been deleted";
        }

        public string AddUserToCircle(string myName, string otherUserName, string circleName)
        {
            var myUser = _usersService.FindSingleUserFromName(myName);
            var OtherUser = _usersService.FindSingleUserFromName(otherUserName);

            Circle circle = _circlesService.FindSingleCircleFromName(circleName, myUser.Id);

            circle.UserIds.Add(OtherUser.Id);
            _circlesService.Update(circle.Id, circle);

            OtherUser.MyCirclesId.Add(circle.Id);
            _usersService.Update(OtherUser.Id, OtherUser);
            return "User added to Circle";
        }


        public string RemoveUserFromCircle(string myName, string otherUserName, string circleName)
        {
            var myUser = _usersService.FindSingleUserFromName(myName);
            var OtherUser = _usersService.FindSingleUserFromName(otherUserName);

            Circle circle = _circlesService.FindSingleCircleFromName(circleName, myUser.Id);

            if (circle.CircleOwner != myUser.Id)
            {
                return "Unable to remove user from circle due not owning the circle";
            }

            circle.UserIds.Remove(OtherUser.Id);
            _circlesService.Update(circle.Id, circle);

            OtherUser.MyCirclesId.Remove(circle.Id);
            _usersService.Update(OtherUser.Id, OtherUser);
            return "User removed to Circle";
        }



        /////////// LIST ///////////////////////////
        public List<Posts> Feed(string Logged_In_UserName)
        {
            List<Posts> Feed = new List<Posts>();
            List<Users> _userList = _usersService.FindUserFromName(Logged_In_UserName);
            Users _loggedInUser = _userList[0];


            // SUBSCRIBED TO
            if (_loggedInUser.SubscribedTo == null)
            {

            }
            foreach (var subscription in _loggedInUser.SubscribedTo)
            {
                var provider = _usersService.Get(_loggedInUser.Id);

                // Check for BannedUser
                if (provider.BlackListedUserId.Contains(_loggedInUser.Id))
                {
                    continue;
                }

                var publicCircle = _circlesService.Get(subscription);

                // Get the 3 latest post from Subscribee's Public Circle
                for (int i = 0; i < 3; i++)
                {
                    Feed.Add(publicCircle.Posts[publicCircle.Posts.Count - i]);
                }
            }

            //Circles
            foreach (string CircleId in _loggedInUser.MyCirclesId)
            {
                var circle = _circlesService.Get(CircleId);

                var circleOwner = _usersService.Get(circle.CircleOwner);

                // Check for BannedUser
                if (circleOwner.BlackListedUserId.Contains(_loggedInUser.Id))
                {
                    continue;
                }
                var privateCircle = _circlesService.Get(CircleId);

                // Get the 3 latest post from Subscribee's Public Circle
                if (privateCircle.Posts.Count != 0)
                {
                    int count = privateCircle.Posts.Count - 1;
                    int countMax = count - 3;
                    if (countMax < 0)
                    {
                        countMax = 0;
                    }
                    for (int i = count; i > countMax; i--)
                    {
                        Feed.Add(privateCircle.Posts[i]);
                    }
                }


            }

            //SORT BY DATE & TIME
            Feed.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
            // ENTEN ELLER
            //Feed = Feed.OrderBy(x => x.Time).ToList();

            return Feed;
        }

        public string FormatFeedListToHTML(List<Posts> Feed)
        {
            string initString = "" +
                                "<html>";
            string endString = "</html>";

            string bodystring = "";
            foreach (var post in Feed)
            {
                bodystring += "<p>" + "Text: " + post.Text + "<br/>";

                if (post.img != "null")
                {
                    bodystring += "<img src='" + post.img + "' height='10%' width='10%'>" + "<br/>";
                }

                foreach (var comment in post.Comments)
                {
                    bodystring += " Comment: " + comment.Text + " -------- By: " + _usersService.Get(comment.UserId).UserName + "<br/>";
                }

                bodystring += " Posted: " + post.Time + "<br/>" +
                              " Post id: " + post.Id +
                              "<p/>"
                              + "<br/>";
            }

            return initString + bodystring + endString;
        }

        /// //////////////////////// WALL ///////////////////////

        public List<Posts> Wall(string VisitorName, string HostName)
        {
            List<Users> _VisitorList = _usersService.FindUserFromName(VisitorName);
            Users VisitorUser = _VisitorList[0];

            List<Users> _HostList = _usersService.FindUserFromName(HostName);
            Users HostUser = _HostList[0];


            List<Posts> Wall = new List<Posts>();


            if (HostUser.BlackListedUserId.Contains(VisitorUser.Id))
            {
                return null;
            }

            foreach (var CircleId in HostUser.MyCirclesId)
            {
                var circle = _circlesService.Get(CircleId);

                if (!circle.UserIds.Contains(VisitorUser.Id))
                {
                    continue;
                }

                List<Posts> FromUser = new List<Posts>();

                foreach (var post in circle.Posts)
                {
                    if (post.UserId == HostUser.Id)
                    {
                        FromUser.Add(post);

                    }
                }

                //SORT BY DATE & TIME
                FromUser.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
                // Get the 3 latest post from Subscribee's Public Circle
                if (circle.Posts.Count != 0)
                {
                    int count = circle.Posts.Count;
                    if (count >= 1)
                    {
                        count--;
                    }
                    int countMax = count - 3;
                    if (countMax < 0)
                    {
                        countMax = 0;
                    }
                    for (int i = count; i >= countMax; i--)
                    {
                        Wall.Add(circle.Posts[i]);
                    }
                }
            }

            //SORT BY DATE & TIME
            Wall.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
            // ENTEN ELLER

            return Wall;
        }

        public string FormatWallListToHTML(List<Posts> Wall)
        {
            string initString = "" +
                                "<html>";
            string endString = "</html>";

            string bodystring = "";

            foreach (var post in Wall)
            {
                bodystring += "<p>" + "Text: " + post.Text + "<br/>";

                if (post.img != "null")
                {
                    bodystring += "<img src='" + post.img + "' height='10%' width='10%'>" + "<br/>";
                }

                foreach (var comment in post.Comments)
                {
                    bodystring += " Comment: " + comment.Text + " -------- By: " + _usersService.Get(comment.UserId).UserName + "<br/>";
                }

                bodystring += " Posted: " + post.Time + "<br/>" +
                              " Post id: " + post.Id +
                              "<p/>"
                              + "<br/>";
            }

            return initString + bodystring + endString;
        }
    }
}
