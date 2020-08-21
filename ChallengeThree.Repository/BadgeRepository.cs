using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeThree.Repository
{
    public class BadgeRepository
    {
        private readonly Dictionary<int, List<string>> _badgeRepo = new Dictionary<int, List<string>>();

        //CREATE
        public void AddToDictionary(Badge badge)
        {
            badge.DoorNames.Sort();
            _badgeRepo.Add(badge.BadgeID, badge.DoorNames);
        }

        //READ
        public bool CheckExistence(int id)
        {
            if (_badgeRepo.ContainsKey(id))
            {
                return true;
            }
            return false;
        }

        public Dictionary<int, List<string>> GetAllPairs()
        {
            Dictionary<int, List<string>> fullDictionary = new Dictionary<int, List<string>>();
            for (int i = 0; i < _badgeRepo.Count; i++)
            {
                fullDictionary.Add(_badgeRepo.ElementAt(i).Key, _badgeRepo.ElementAt(i).Value);
            }
            return fullDictionary;
        }

        public List<int> GetAllBadgeIDs()
        {
            List<int> allBadgeIDs = new List<int>();
            for (int i = 0; i < _badgeRepo.Count; i++)
            {
                allBadgeIDs.Add(_badgeRepo.ElementAt(i).Key);
            }
            return allBadgeIDs;
        }

        public List<string> GetDoorsByID(int id)
        {
            if (CheckExistence(id))
            {
                return _badgeRepo[id];
            }
            return null;
        }

        public Dictionary<int, List<string>> GetAllBadgesWithDoor(string door, Dictionary<int, List<string>> dict)
        {
            Dictionary<int, List<string>> newDict = new Dictionary<int, List<string>>();
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict.ElementAt(i).Value.Contains(door))
                {
                    newDict.Add(dict.ElementAt(i).Key, dict.ElementAt(i).Value);
                }
            }
            return newDict;
        }

        //UPDATE
        public bool UpdateDoors(int id, List<string> newDoors)
        {
            if (CheckExistence(id))
            {
                newDoors.Sort();
                _badgeRepo[id] = newDoors;
                return true;
            }
            return false;
        }

        public string AddDoor(int id, string door)
        {
            if (CheckExistence(id))
            {
                door = door.ToUpper();
                if (!_badgeRepo[id].Contains(door))
                {
                    _badgeRepo[id].Add(door);
                    _badgeRepo[id].Sort();
                    return "Door added.";
                }
                return $"Badge {id} already has door {door}.";
            }
            return $"Badge {id} not found.";
        }

        public string AddSeveralDoors(int id, List<string> doors)
        {
            string check = "";
            foreach (string door in doors)
            {
                check = AddDoor(id, door);
            }
            if (check == "Door added.")
            {
                return "Doors added.";
            }
            else
            {
                return "Doors stayed the same.";
            }
        }

        public string RemoveDoor(int id, string door)
        {
            if (CheckExistence(id))
            {
                door = door.ToUpper();
                if (_badgeRepo[id].Contains(door))
                {
                    _badgeRepo[id].Remove(door);
                    return "Door removed.";
                }
                return $"Badge {id} doesn't have door {door}.";
            }
            return $"Badge {id} not found.";
        }

        //DELETE
        public bool DeleteBadge(int id)
        {
            if (CheckExistence(id))
            {
                _badgeRepo.Remove(id);
                return true;
            }
            return false;
        }

        public bool DeleteDoorsFromBadge(int id)
        {
            return UpdateDoors(id, new List<string>());
        }

        public bool DeleteDoorFromAllBadges(string door)
        {
            bool success = false;
            door = door.ToUpper();
            for (int i = 0; i < _badgeRepo.Count; i++)
            {
                if (_badgeRepo.ElementAt(i).Value.Contains(door))
                {
                    _badgeRepo.ElementAt(i).Value.Remove(door);
                    success = true;
                }
            }
            return success;
        }
    }
}
