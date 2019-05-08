using System.Collections.Generic;

namespace GTASessionBot.Providers {
    public class PermissionManager {


        private Dictionary<ulong, int> deniedCommands;

        public PermissionManager() {
            deniedCommands = new Dictionary<ulong, int>();
        }


        public void AddOrIncrementUserFailure(ulong userID) {
            if (!deniedCommands.ContainsKey(userID)) {

                deniedCommands.Add(userID, 0);
            }

            deniedCommands[userID]++;
        }


        public int UserCommandFailures(ulong userID) {
            if (deniedCommands.ContainsKey(userID)) {
                return deniedCommands[userID];
            }

            return 0;
        }


        public Dictionary<ulong, int> getNaughtyList() {
            return deniedCommands;
        }


    }
}
