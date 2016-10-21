using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace AFAR_Field
{
    class Program
    {
        static string constantsFilePath = "config.ini";
        static IniFiles constantsFile = new IniFiles(Environment.CurrentDirectory.ToString() + "/" + constantsFilePath);
        

        static string[] constatns = readConstatns(constantsFile);
        static double wavelength = Convert.ToDouble(constatns[0]);
        static int phaseShiftNumber = Convert.ToInt32(constatns[1]);
        static int elementsNumber = Convert.ToInt32(constatns[2]);
        static string elementsCoordFilename = constatns[3];
        static string elementsAmpPhaseFilename = constatns[4];
        static string scriptFilename = constatns[5];
        static List<string> outputFileList = new List<string>();

        static double[,] elementsCoordinates = readElements(elementsCoordFilename);
        static double[,] elementsAmpPhase = readElements(elementsAmpPhaseFilename);
        static int[] phaseShifterStates = new int[elementsNumber];
        static string phaseShifterStatesString;
        

        static void Main(string[] args)
        {
            
            string constantsFilePath = "config.ini";
            IniFiles constantsFile = new IniFiles(Environment.CurrentDirectory.ToString() + "/" + constantsFilePath);
            

          
            addToLogFile("Считывание констант прошло успешно");
            Console.WriteLine("Считывание констант прошло успешно");

            

            
            addToLogFile("Считывание координат элементов прошло успешно");
            Console.WriteLine("Считывание координат элементов прошло успешно");

            
            addToLogFile("Считывание амплитуд и фаз элементов прошло успешно");
            Console.WriteLine("Считывание амплитуд и фаз элементов прошло успешно");
           
            for (int i = 0; i<elementsNumber-1; i++)
            {
                phaseShifterStatesString += "00."; 
            }
            phaseShifterStatesString += "00";
            scriptProceeding();

            Console.WriteLine("Программы успешно закончила выполнение.");
            addToLogFile("Программы успешно закончила выполнение.");
            Console.WriteLine("Нажмите Enter, чтобы выйти");
            Console.ReadLine();
        }

        
        static string[] readConstatns(IniFiles constantsFile)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"log.txt", true))
            {
                file.WriteLine("===============================================================================");
            }
            addToLogFile("Запуск программы");
            string[] constants = new string[6];
            constants[0] = constantsFile.ReadINI("Constants", "wavelength");


            if (constants[0] != "")
            {

                addToLogFile("Длина волны: " + constants[0]);
                Console.WriteLine("Длина волны: " + constants[0]);
            }
            else
            {
                addToLogFile("Ошибка при считывании значения длины волны");
                Console.WriteLine("Ошибка при считывании значения длины волны");
                closeApplication();
            }

            constants[1] = constantsFile.ReadINI("Constants", "phaseShiftNumber");

            if (constants[1] != "")
            {
                addToLogFile("Количество состояний фазовращателей: " + constants[1]);
                Console.WriteLine("Количество состояний фазовращателей: " + constants[1]);

            }
            else
            {
                addToLogFile("Ошибка при считывании количества состояний фазовращателей");
                Console.WriteLine("Ошибка при считывании количества состояний фазовращателей");
                closeApplication();

            }

            constants[2] = constantsFile.ReadINI("Constants", "elementsNumber");

            if (constants[2] != "")
            {
                addToLogFile("Количество элементов: " + constants[2]);
                Console.WriteLine("Количество элементов: " + constants[2]);

            }
            else
            {
                addToLogFile("Ошибка при считывании количества элементов");
                Console.WriteLine("Ошибка при считывании элементов");
                closeApplication();
            }

            constants[3] = constantsFile.ReadINI("Constants", "elementsCoordinateFilename");

            if (constants[3] != "")
            {
                addToLogFile("Файл координат элементов: " + constants[3]);
                Console.WriteLine("Файл координат элементов: " + constants[3]);

            }
            else
            {
                addToLogFile("Ошибка при считывании файла координат элементов");
                Console.WriteLine("Ошибка при считывании файла координат элементов");
                closeApplication();
            }

            constants[4] = constantsFile.ReadINI("Constants", "elementsAmpPhaseFilename");

            if (constants[4] != "")
            {
                addToLogFile("Файл cобственных амплитуд и фаз элементов: " + constants[4]);
                Console.WriteLine("Файл cобственных амплитуд и фаз элементов: " + constants[4]);

            }
            else
            {
                addToLogFile("Ошибка при считывании файла координат элементов");
                Console.WriteLine("Ошибка при считывании файла координат элементов");
                closeApplication();
            }

            constants[5] = constantsFile.ReadINI("Constants", "scriptFilename");

            if (constants[5] != "")
            {
                addToLogFile("Файл скрипта: " + constants[5]);
                Console.WriteLine("Файл координат элементов: " + constants[5]);

            }
            else
            {
                addToLogFile("Ошибка при считывании файла скрипта");
                Console.WriteLine("Ошибка при считывании файла скрипта");
                closeApplication();
            }

            return constants;
        }
        static double[,] readElements(string elementsFilename)
        {
            double[,] coordinates = new double[elementsNumber, 2];
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(elementsFilename))
                {
                    string line;
                    StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
                    char[] separator = null;
                    int i = 0;
                    while ((line = file.ReadLine()) != null)
                    {

                        string[] coord = line.Split(separator, options);
                        if (coord.Length != 0)
                        {
                            coordinates.SetValue(Convert.ToDouble(coord[0]), i, 0);
                            try
                            {
                                coordinates.SetValue(Convert.ToDouble(coord[1]), i, 1);
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                addToLogFile("Ошибка! Неверный формат файла " + elementsFilename);
                                Console.WriteLine("Ошибка! Неверный формат файла " + elementsFilename);
                                closeApplication();

                            }
                            i++;
                        }
                    }
                    if (i < elementsNumber)
                    {
                        addToLogFile("Ошибка! Количество элементов в файле " + elementsFilename + " меньше количества элементов, заданного в файле конфигурации!");
                        Console.WriteLine("Ошибка! Количество элементов в файле " + elementsFilename + " меньше количества элементов, заданного в файле конфигурации!");
                        closeApplication();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Файл " + elementsFilename + " не найден!");
                addToLogFile("Файл " + elementsFilename + " не найден!");
                closeApplication();
            }
            return coordinates;
        }
        static void scriptProceeding()
        {
            
            List<string> scriptList = new List<string>();
            
            using (System.IO.StreamReader file = new System.IO.StreamReader(scriptFilename))
            {
                string line;
                StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
                char[] commandSeparator = null;
                while ((line = file.ReadLine()) != null)
                {
                    scriptList.Add(line);
                }
                if (scriptList.Count == 0)
                {
                    addToLogFile("Файл скрипта не содержит ни одной команды");
                    Console.WriteLine("Файл скрипта не содержит ни одной команды");
                    closeApplication();
                }

                char lineSeparator = ';';
                for (int i = 0; i < scriptList.Count; i++)
                {
                    string[] lineCommands = scriptList.ElementAt(i).Split(lineSeparator);
                    for (int j = 0; j < lineCommands.Length; j++)
                    {

                        if (lineCommands[j] == null)
                            wrongScriptCommand(lineCommands[j]);
                        string[] commandContent = lineCommands[j].Split(commandSeparator, options);
                      
                        
                        if (commandContent.Length!=0)
                        switch (commandContent[0])
                        {
                            #region Команды изменения состояний
                            case "WHSTATES":
                                if (commandContent.Length != 2)
                                {
                                    Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=2). Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto case "WHSTATES";
                                }
                                else
                                {
                                    phaseShifterStates = setStates(commandContent[1], phaseShifterStates, phaseShiftNumber);
                                    
                                    string statesLine ="";
                                    for (int elem = 0; elem<phaseShifterStates.Length; elem++)
                                    {
                                        statesLine += phaseShifterStates[elem].ToString("X2") + ".";
                                    }
                                    phaseShifterStatesString = statesLine;
                                    Console.WriteLine("Успешно установлены следующие состояния фазовращателей: "+statesLine);
                                    addToLogFile("Успешно установлены следующие состояния фазовращателей: "+statesLine);
                                }
                                break;

                            case "CHSTATE":
                                if (commandContent.Length != 3)
                                {
                                    Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=3). Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto case "CHSTATE";
                                }
                                else
                                {
                                    try
                                    {
                                        if (Convert.ToInt32(commandContent[1]) > elementsNumber)
                                        {
                                            Console.WriteLine("Задан неправильный номер элемента!");
                                            addToLogFile("Задан неправильный номер элемента!");
                                            closeApplication();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        wrongScriptCommand(lineCommands[j]);
                                    }
                                    phaseShifterStates = setStates(commandContent[2], phaseShifterStates, phaseShiftNumber, Convert.ToInt32(commandContent[1])-1);
                                    string statesLine = "";
                                    for (int elem = 0; elem < phaseShifterStates.Length; elem++)
                                    {
                                        statesLine += phaseShifterStates[elem].ToString("X2") + ".";

                                    }
                                    phaseShifterStatesString = statesLine;
                                    Console.WriteLine("Успешно установлены следующие состояния фазовращателей: " + statesLine);
                                    addToLogFile("Успешно установлены следующие состояния фазовращателей: " + statesLine);
                                }
                                break;
                            #endregion
                            #region Команды вычисления поля
                            case "FIELD_UV":
                                if (commandContent.Length != 4)
                                {
                                    Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=4). Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto case "FIELD_UV";
                                }
                                else
                                {
                                    try
                                    {
                                        double u = Convert.ToDouble(commandContent[1]);
                                        double v = Convert.ToDouble(commandContent[2]);
                                        outputFileList.Add(evaluateField(commandContent, u, v));
                                        Console.WriteLine("Расчет поля в направлении U=" + commandContent[1] + " V=" + commandContent[2] + " успешно выполнен");
                                        addToLogFile("Расчет поля в направлении U=" + commandContent[1] + " V=" + commandContent[2] + " успешно выполнен");
                                    }
                                    catch (Exception e)
                                    {
                                        wrongScriptCommand(lineCommands[j]);
                                    }
                                }
                                break;
                            case "FIELD_AE":
                                if (commandContent.Length != 4)
                                {
                                    Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=4). Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto case "FIELD_AE";
                                }
                                else
                                {

                                    try
                                    {
                                        double u = Math.Cos(Convert.ToDouble(commandContent[2])*Math.PI/180) * Math.Cos(Convert.ToDouble(commandContent[1]) * Math.PI / 180);
                                        double v = Math.Sin(Convert.ToDouble(commandContent[2]) * Math.PI / 180);
                                        outputFileList.Add(evaluateField(commandContent, u, v));
                                        Console.WriteLine("Расчет поля в направлении A=" + commandContent[1] + " E=" + commandContent[2] + " успешно выполнен");
                                        addToLogFile("Расчет поля в направлении A=" + commandContent[1] + " E=" + commandContent[2] + " успешно выполнен");
                                     }
                                    catch (Exception e)
                                    {
                                        wrongScriptCommand(lineCommands[j]);
                                    }
                                }
                                break;
                            case "FIELD_QF":
                                if (commandContent.Length != 4)
                                {
                                    Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=4). Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto case "FIELD_AE";
                                }
                                else
                                {
                                    try
                                    {
                                        double u = Math.Sin(Convert.ToDouble(commandContent[1]) * Math.PI / 180) * Math.Cos(Convert.ToDouble(commandContent[2]) * Math.PI / 180);
                                        double v = Math.Sin(Convert.ToDouble(commandContent[1]) * Math.PI / 180) * Math.Sin(Convert.ToDouble(commandContent[2]) * Math.PI/180);
                                        outputFileList.Add(evaluateField(commandContent, u, v));
                                        Console.WriteLine("Расчет поля в направлении Q=" + commandContent[1] + " F=" + commandContent[2] + " успешно выполнен");
                                        addToLogFile("Расчет поля в направлении Q=" + commandContent[1] + " F=" + commandContent[2] + " успешно выполнен");
                                     }
                                    catch (Exception e)
                                    {
                                        wrongScriptCommand(lineCommands[j]);
                                    }
                                   
                                }
                                break;
                            #endregion
                            #region Комынды сохранеия/удаления
                            case "SAVETO":
                                if (commandContent.Length != 2)
                                {
                                    Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=2). Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto case "SAVETO";
                                }
                                else
                                {
                                    saveToFile(commandContent[1],"output");
                                }
                                break;
                            case "CLROUT":
                                    if (commandContent.Length != 1)
                                    {
                                        Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=1). Игнорировать и продолжить выполнение программы?(Y/N)");
                                        string input = Console.ReadLine();
                                        if (input == "Y")
                                        {
                                            Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                        }
                                        if (input == "N")
                                            closeApplication();
                                        else
                                            if (input != "Y")
                                            goto case "SAVETO";
                                    }
                                    else
                                    {
                                        clearOutputFileList();
                                    }
                                    break;
                            case "SAVESTATES":
                                    if (commandContent.Length != 2)
                                    {
                                        Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=2). Игнорировать и продолжить выполнение программы?(Y/N)");
                                        string input = Console.ReadLine();
                                        if (input == "Y")
                                        {
                                            Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                        }
                                        if (input == "N")
                                            closeApplication();
                                        else
                                            if (input != "Y")
                                            goto case "SAVESTATES";
                                    }
                                    else
                                    {
                                        saveToFile(commandContent[1], "states");
                                    }
                                    break;
                            case "SAVEPHASE":
                                    if (commandContent.Length != 2)
                                    {
                                        Console.WriteLine("Команда " + commandContent[0] + " имеет неверное число параметров(!=2). Игнорировать и продолжить выполнение программы?(Y/N)");
                                        string input = Console.ReadLine();
                                        if (input == "Y")
                                        {
                                            Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                        }
                                        if (input == "N")
                                            closeApplication();
                                        else
                                            if (input != "Y")
                                            goto case "SAVEPHASE";
                                    }
                                    else
                                    {
                                        saveToFile(commandContent[1], "phase");
                                    }
                                    break;
                            #endregion
                            default:
                                {
                                    Console.WriteLine("Неизвестная команда " + commandContent[0] + ". Игнорировать и продолжить выполнение программы?(Y/N)");
                                    string input = Console.ReadLine();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("Игнорирую команду " + commandContent[0] + " и считываю следующую команду");

                                    }
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        if (input != "Y")
                                        goto default;

                                    break;
                                }

                        }


                    }




                }
            }
            
        }
       
        static int[] setStates(string command, int[] elementsPhaseShifterStates, int phaseShiftNumber)
        {
            int[] phaseShifterStates = elementsPhaseShifterStates;
            string[] states = command.Split('.');
            int limit = Math.Min(states.Length, elementsPhaseShifterStates.Length);
            for (int i = 0; i < limit; i++)
            {
                switch (states[i])
                {
                    case "NN":
                        if (phaseShifterStates[i] + 1 > phaseShiftNumber)
                            phaseShifterStates[i] = 0;
                        else
                            phaseShifterStates[i]++;
                        break;
                    case "PP":
                        if (phaseShifterStates[i] - 1 < 0)
                            phaseShifterStates[i] = phaseShiftNumber;
                        else
                            phaseShifterStates[i]--;
                        break;
                    case "II":
                        if (phaseShifterStates[i] + phaseShiftNumber / 2 > phaseShiftNumber)
                            phaseShifterStates[i] = phaseShifterStates[i] - phaseShiftNumber / 2;
                        else
                            phaseShifterStates[i] = phaseShifterStates[i] + phaseShiftNumber / 2;
                        break;
                    case "SS":
                        break;
                    default:

                        if (Convert.ToInt32(states[i], 16) <= phaseShiftNumber)
                        {
                            phaseShifterStates[i] = Convert.ToInt32(states[i], 16);
                            break;
                        }
                        else

                        {/*
                                Console.WriteLine("Заданное в команде состояние фазовращателя элемента " + (i + 1).ToString() + " превышает количество возможных состояний фазовращателя.");
                                Console.WriteLine("Чтобы оставить значение фазовращателя неизменным введите SS, чтобы завершить работу програмы нажмите N");
                                string input = Console.ReadLine();
                                if (input == "SS")
                                    goto case "SS";
                                else
                                {
                                    if (input == "N")
                                        closeApplication();
                                    else
                                        goto default;
                                }

                            }
                        }
                        catch(Exception e)
                        {*/
                            Console.WriteLine("Неправильный формат команды скрипта!");
                            addToLogFile("Неправильный формат команды скрипта!");
                            closeApplication();
                            /*}*/
                            break;
                        }
                }
            }
            return phaseShifterStates;
        }
        static int[] setStates(string command, int[] elementsPhaseShifterStates, int phaseShiftNumber, int element)
        {
            int[] phaseShifterStates = elementsPhaseShifterStates;
            int i = element;
                switch (command)
                {
                    case "NN":
                        if (phaseShifterStates[i] + 1 > phaseShiftNumber)
                            phaseShifterStates[i] = 0;
                        else
                            phaseShifterStates[i]++;
                        break;
                    case "PP":
                        if (phaseShifterStates[i] - 1 < 0)
                            phaseShifterStates[i] = phaseShiftNumber;
                        else
                            phaseShifterStates[i]--;
                        break;
                    case "II":
                        if (phaseShifterStates[i] + phaseShiftNumber / 2 > phaseShiftNumber)
                            phaseShifterStates[i] = phaseShifterStates[i] - phaseShiftNumber / 2;
                        else
                            phaseShifterStates[i] = phaseShifterStates[i] + phaseShiftNumber / 2;
                        break;
                    case "SS":
                        break;
                    default:
                        if (Convert.ToInt32(command, 16) <= phaseShiftNumber)
                        {
                            phaseShifterStates[i] = Convert.ToInt32(command, 16);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат команды скрипта!");
                            addToLogFile("Неправильный формат команды скрипта!");
                            closeApplication();
                            /*}*/
                            break;
                        }
                }
                
            
            return phaseShifterStates;


        }

        static string evaluateField(string[] command, double u, double v)
        {
            string outputLine = "";
            Complex field = new Complex();
            double eField;

            for (int i = 0; i < elementsNumber; i++)
            {
                field += elementsAmpPhase[i, 1] * Complex.Exp(Complex.ImaginaryOne * elementsAmpPhase[i, 0]) * Complex.Exp(Complex.ImaginaryOne * phaseShifterStates[i]*360/phaseShiftNumber) * Complex.Exp(-Complex.ImaginaryOne * (u * elementsCoordinates[i, 0] + v * elementsCoordinates[i, 1]) * 2 * Math.PI / wavelength);
            }
            if (field != 0)
                eField = 20 * Math.Log10(Complex.Abs(field));
            else
                eField = -50;
            switch (command[3])
            {
                case "N":
                    outputLine = command[1] + " " + command[2] + " " + eField.ToString("0.####") + " " + field.Magnitude.ToString("0.####") + " " + field.Phase.ToString("0.####") + " " + field.Phase.ToString("0.####") + " " + field.Imaginary.ToString("0.####");
                    break;
                case "Y":
                    outputLine = command[1] + " " + command[2] + " " + eField.ToString("0.####") + " " + field.Magnitude.ToString("0.####") + " " + field.Phase.ToString("0.####") + " " + field.Phase.ToString("0.####") + " " + field.Imaginary.ToString("0.####") + " "+phaseShifterStatesString;
                    break;
                default:
                    string wrongCommand = "";
                    for (int i = 0; i<command.Length-1; i++)
                        wrongCommand += command[i] + " ";
                    wrongCommand += wrongCommand[command.Length];
                    wrongScriptCommand(wrongCommand);
                    break;
            }
            return outputLine;
        }
        static void saveToFile(string path, string type)
        {
            switch (type)
            {
                case "output":
                    try
                     {
                     using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                    {
                            for (int i = 0; i < outputFileList.Count; i++) 
                                file.WriteLine(outputFileList[i]);
                    }
                    }
                    catch (Exception e)
                    {
                        wrongScriptCommand("SAVETO");
                    }
                    break;
                case "phase":
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                        {
                            for (int i = 0; i < elementsNumber; i++)
                                file.WriteLine(elementsCoordinates[i, 0].ToString() + " " + elementsCoordinates[i, 1].ToString() + " " + phaseShifterStates[i].ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        wrongScriptCommand("SAVEPHASE");
                    }
                    break;
                case "states":
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                        {
                            for (int i = 0; i < elementsNumber; i++)
                            {
                                double phase = phaseShifterStates[i] * 360 / phaseShiftNumber;
                                file.WriteLine(elementsCoordinates[i, 0].ToString() + " " + elementsCoordinates[i, 1].ToString() + " " + phase.ToString());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        wrongScriptCommand("SAVESTATES");
                    }
                    break;
                

            }
        }
        static void clearOutputFileList()
        {
            outputFileList.Clear();
        }

        static void addToLogFile(string text)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("log.txt", true))
            {
                DateTime localDate = DateTime.Now;
                file.WriteLine('[' + localDate.ToString() + ']' + text);
            }
        }
        static void closeApplication()
        {
            Console.WriteLine("Нажмите Enter для выхода из программы");
            Console.ReadLine();
            Environment.Exit(0);
        }
        static void wrongScriptCommand(string command)
        {
            Console.WriteLine("Неверный формат команды! "+"("+command+")");
            addToLogFile("Неверный формат команды! " + "(" + command + ")");
            closeApplication();
        }
        


    }
}

