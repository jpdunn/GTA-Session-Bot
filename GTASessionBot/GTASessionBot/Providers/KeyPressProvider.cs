using InputManager;


namespace GTASessionBot.Providers {
    public class KeyPressProvider {

        /// <summary>
        /// Sends the given key code to the Grand Theft Auto process.
        /// </summary>
        /// <param name="keyCode">The key code to send.</param>
        public static void SendKeyEvent(System.Windows.Forms.Keys keyCode) {
            Keyboard.KeyPress(keyCode, 10);
        }
    }
}
