using System;
using System.IO;
using System.Text;

namespace KeyGen
{
    class Program
    {
        static void Main (string [] args)
        {
            Console.WriteLine (args.Length);
            if (args.Length < 6)
            {
                Console.WriteLine ("Программе требуется следующие аргументы:");
                Console.WriteLine ("    - имя файла с аппаратным идентификатором устройства,");
                Console.WriteLine ("    - имя владельца лицензии,");
                Console.WriteLine ("    - имя дистрибъютора,");
                Console.WriteLine ("    - область дистрибъютора,");
                Console.WriteLine ("    - номер лицензии,");
                Console.WriteLine ("    - имя файла, в который будет записана лицензия.");

                return;
            }

            string strPresetId = string.Empty;
            string strPlatformId = string.Empty;
            try
            {
                StreamReader sr = new StreamReader (args [0]);
                strPresetId = sr.ReadLine ();
                strPlatformId = sr.ReadLine ();

                sr.Close ();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine (e.ToString ());
                Console.WriteLine ("Не удалось прочитать аппаратный идентификатор устройства из файла");
                Console.WriteLine (args [0]);
                Console.WriteLine ("Описание ошибки: " + e.Message);

                return;
            }

            string strCustomKey = "MiP 1.5: Собственность компании OOO \"ЛайтКом\"";

            LightCom.WinCE.HardwareKey key = new LightCom.WinCE.HardwareKey ();
            key.PlatformId = strPlatformId;
            key.PresetId = strPresetId;
            key.LicenseOwner = args [1];
            key.DistributorName = args [2];
            key.DistributorArea = args [3];
            key.Number = args [4];

            if (!key.SaveLicense (args [5], strCustomKey))
            {
                Console.WriteLine ("Не удалось записать лицензию в файл " + args [2]);
                return;
            }

            //            LightCom.WinCE.HardwareKey key1 = new LightCom.WinCE.HardwareKey ();
            //            key1.LoadLicense (args [5], strCustomKey);

            Console.WriteLine ("Лицензия успешно записана в файл " + args [2]);
        }
    }
}
