using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NT_Utils
{
    namespace UI
    {
        public class Utilities : MonoBehaviour
        {
            public static IEnumerator DisableButtonForSecs(Button button, float seconds)
            {
                button.interactable = false;
                yield return new WaitForSeconds(seconds);
                button.interactable = true;
            }
        }
    }
    namespace Conversion
    {
        public class NumberConversion : MonoBehaviour
        {
            public enum StartAbbrevation
            {
                M,
                B,
                t
            }

            private static readonly SortedDictionary<string, string> abbrevations = new SortedDictionary<string, string>
            {
                {"1000","K"},
                {"1000000", "M" },
                {"1000000000", "B" },
                {"1000000000000", "t" },
                {"1000000000000000", "q" },
                {"1000000000000000000", "Q" },
                {"1000000000000000000000", "s" },
                {"1000000000000000000000000", "S" },
                {"1000000000000000000000000000", "o" },
                {"1000000000000000000000000000000", "n" },
                {"1000000000000000000000000000000000", "d" },
                {"1000000000000000000000000000000000000", "U" },
                {"1000000000000000000000000000000000000000", "D" },
                {"1000000000000000000000000000000000000000000", "T" },
                {"1000000000000000000000000000000000000000000000", "Qt" },
                {"1000000000000000000000000000000000000000000000000", "Qd" },
                {"1000000000000000000000000000000000000000000000000000", "Sd" },
                {"1000000000000000000000000000000000000000000000000000000", "St" },
                {"1000000000000000000000000000000000000000000000000000000000", "O" },
                {"1000000000000000000000000000000000000000000000000000000000000", "N" },
                {"1000000000000000000000000000000000000000000000000000000000000000", "v" },
                {"1000000000000000000000000000000000000000000000000000000000000000000", "c" },
                {"1000000000000000000000000000000000000000000000000000000000000000000000", "e69" },
            };

            public static string AbbreviateNumber(double number)
            {
                number = Math.Round(number, 2);
                if (number.ToString().Length >= 70)
                {
                    return string.Format("{0:#.##e00}", number);
                }
                else
                {
                    for (int i = abbrevations.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, string> pair = abbrevations.ElementAt(i);
                        if (Math.Abs(number) >= double.Parse(pair.Key))
                        {
                            double roundedNumber = Math.Floor(number / double.Parse(pair.Key));
                            return roundedNumber.ToString() + pair.Value;
                        }
                    }
                    return number.ToString();
                }
            }

            public static string AbbreviateNumber(double number, int digits)
            {
                number = Math.Round(number, 2);
                if (number.ToString().Length >= 70)
                {
                    return string.Format("{0:#.##e00}", number);
                }
                else
                {
                    for (int i = abbrevations.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, string> pair = abbrevations.ElementAt(i);
                        if (Math.Abs(number) >= double.Parse(pair.Key))
                        {
                            double roundedNumber = Math.Round(number / double.Parse(pair.Key), digits);
                            return roundedNumber.ToString() + pair.Value;
                        }
                    }
                    return number.ToString();
                }
            }

            public static string AbbreviateNumber(double number, StartAbbrevation startAbbrevation)
            {
                number = Math.Round(number, 2);

                if (startAbbrevation == StartAbbrevation.M && number < 1000000)
                {
                    return number.ToString();
                }
                else if (startAbbrevation == StartAbbrevation.B && number < 1000000000)
                {
                    return number.ToString();
                }
                else if (startAbbrevation == StartAbbrevation.t && number < 1000000000000)
                {
                    return number.ToString();
                }

                if (number.ToString().Length >= 70)
                {
                    return string.Format("{0:#.##e00}", number);
                }
                else
                {
                    for (int i = abbrevations.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, string> pair = abbrevations.ElementAt(i);
                        if (Math.Abs(number) >= double.Parse(pair.Key))
                        {
                            double roundedNumber = Math.Floor(number / double.Parse(pair.Key));
                            return roundedNumber.ToString() + pair.Value;
                        }
                    }
                    return number.ToString();
                }
            }
        }

        public class TimeConversion
        {
            public static string AbbreviateTime(double timeInSeconds)
            {
                TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);

                if (timeInSeconds < 60)
                {
                    return Math.Round(timeInSeconds, 1).ToString() + "s";
                }
                else if(timeInSeconds >= 60 && timeInSeconds < 600)
                {
                    return string.Format("{0:D1}m{1:D1}s",
                        time.Minutes,
                        time.Seconds);
                }
                else if (timeInSeconds >= 600 && timeInSeconds < 3600)
                {
                    return string.Format("{0:D2}m{1:D2}s",
                        time.Minutes,
                        time.Seconds);
                }
                else if (timeInSeconds >= 3600 && timeInSeconds < 36000)
                {
                    return string.Format("{0:D1}h{1:D2}m{2:D2}s",
                        time.Hours,
                        time.Minutes,
                        time.Seconds);
                }
                else if (timeInSeconds >= 36000 && timeInSeconds < 86400)
                {
                    return string.Format("{0:D2}h{1:D2}m{2:D2}s",
                        time.Hours,
                        time.Minutes,
                        time.Seconds);
                }
                else if (timeInSeconds >= 86400 && timeInSeconds < 864000)
                {
                    return string.Format("{0:D1}d{1:D2}h{2:D2}m",
                        time.Days,
                        time.Hours,
                        time.Minutes);
                }
                else
                {
                    return string.Format("{0:D2}d{1:D2}h{2:D2}m",
                        time.Days,
                        time.Hours,
                        time.Minutes);
                }
            }

            public static string AbbreviateTime(double timeInSeconds, bool isForStatistics)
            {
                TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
                if (isForStatistics)
                {
                    return string.Format("{0:D1}D {1:D1}H {2:D1}M {3:D1}S",
                            time.Days,
                            time.Hours,
                            time.Minutes,
                            time.Seconds);
                }
                else
                {
                    if (timeInSeconds < 60)
                    {
                        return Math.Round(timeInSeconds, 1).ToString() + "s";
                    }
                    else if (timeInSeconds >= 60 && timeInSeconds < 600)
                    {
                        return string.Format("{0:D1}m{1:D1}s",
                            time.Minutes,
                            time.Seconds);
                    }
                    else if (timeInSeconds >= 600 && timeInSeconds < 3600)
                    {
                        return string.Format("{0:D2}m{1:D2}s",
                            time.Minutes,
                            time.Seconds);
                    }
                    else if (timeInSeconds >= 3600 && timeInSeconds < 36000)
                    {
                        return string.Format("{0:D1}h{1:D2}m{2:D2}s",
                            time.Hours,
                            time.Minutes,
                            time.Seconds);
                    }
                    else if (timeInSeconds >= 36000 && timeInSeconds < 86400)
                    {
                        return string.Format("{0:D2}h{1:D2}m{2:D2}s",
                            time.Hours,
                            time.Minutes,
                            time.Seconds);
                    }
                    else if (timeInSeconds >= 86400 && timeInSeconds < 864000)
                    {
                        return string.Format("{0:D1}d{1:D2}h{2:D2}m",
                            time.Days,
                            time.Hours,
                            time.Minutes);
                    }
                    else
                    {
                        return string.Format("{0:D2}d{1:D2}h{2:D2}m",
                            time.Days,
                            time.Hours,
                            time.Minutes);
                    }
                }
                
            }
        }
    }
}

