using System;
using WindowsInput;
using WindowsInput.Native;

namespace ActiveDeskRoboSet
{
    public class ActiveDeskRobot
    {
        private void Demo()
        {
            //var elt = new UIElement();
            //Mouse.Click(elt, new Point(20, 11));

            //Keyboard.SendKeys(elt);

            var simu = new InputSimulator();
            // 2 keys needed and code like (windows key + )
            simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
            // 3 keys needed and code like this(windows key + ctrl + D):
            simu.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_D);

            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R).Sleep(1000)
                .TextEntry("notepad").Sleep(1000)
                .KeyPress(VirtualKeyCode.RETURN).Sleep(1000)
                .TextEntry("These...").TextEntry("and in 5 seconds.").Sleep(5000)
                .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE)
                .KeyPress(VirtualKeyCode.DOWN)
                .KeyPress(VirtualKeyCode.RETURN);
        }

        private static int Laps = 800;

        public static void ReportHeader(int laps, string name, string aHVNb, string period)
        {
            Laps = laps;

            // InputSimulator
            var sim = new InputSimulator();
            sim.Keyboard.TextEntry(name).Sleep(Laps).KeyPress(VirtualKeyCode.TAB).Sleep(Laps);

            // sim.Keyboard.TextEntry(aHVNb).KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
            SimKeyPressNumericString(sim, aHVNb, Laps);
            sim.Keyboard.KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
            
            sim.Keyboard.TextEntry(period).Sleep(Laps).KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
        }

        public static void SimKeyPressNumericString(InputSimulator sim, string numS, int laps)
        {
            foreach (var chr in numS.ToCharArray())
            {
                switch (chr)
                {
                    default:
                    case '0':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD0).Sleep(laps);
                        break;
                    case '1':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD1).Sleep(laps);
                        break;
                    case '2':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD2).Sleep(laps);
                        break;
                    case '3':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD3).Sleep(laps);
                        break;
                    case '4':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD4).Sleep(laps);
                        break;
                    case '5':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD5).Sleep(laps);
                        break;
                    case '6':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD6).Sleep(laps);
                        break;
                    case '7':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD7).Sleep(laps);
                        break;
                    case '8':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD8).Sleep(laps);
                        break;
                    case '9':
                        sim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9).Sleep(laps);
                        break;
                }
            }
        }


        public static void ReportEntry(int laps, DateTime date,
                string firmaAddressContact, string positionTitleLocation,
                bool positionAssigned, bool fullTime, bool partTime,
                bool postulationWritten, bool postulationPerso, bool postulationTelephon,
                bool open, bool interviewed, bool hired, bool fired,
                string hiringProcess_Notes)
        {
            Laps = laps;

            var sim = new InputSimulator();
            ReportString(sim, date.ToString("ddMM"));
            ReportString(sim, firmaAddressContact);
            ReportString(sim, positionTitleLocation);

            ReportBoolean(sim, positionAssigned);
            ReportBoolean(sim, fullTime);
            ReportBoolean(sim, partTime);
            ReportBoolean(sim, postulationWritten);
            ReportBoolean(sim, postulationPerso);
            ReportBoolean(sim, postulationTelephon);
            ReportBoolean(sim, open);
            ReportBoolean(sim, interviewed);
            ReportBoolean(sim, hired);
            ReportBoolean(sim, fired);

            ReportString(sim, hiringProcess_Notes);
        }

        public static void ReportString(InputSimulator sim, string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry))
            {
                sim.Keyboard.TextEntry(entry).Sleep(Laps).KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
            }
            else
                sim.Keyboard.KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
        }

        private static void ReportBoolean(InputSimulator sim, bool condition)
        {
            if (condition)
                sim.Keyboard.KeyPress(VirtualKeyCode.RETURN).KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
            else
                sim.Keyboard.KeyPress(VirtualKeyCode.TAB).Sleep(Laps);
        }
    }
}