using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataPedanaHandler
{
    public sealed class DatiPedanaFileHandler : IDatiPedanaFileHandler
    {
        //private string test = "\r\n5880    \t33.894  \t0.000   \t332.389 \t-0.228  \t0.220   \t-75.704 \t5.880   \r\n

       // string x = "\tLoad   \r\n0  \tEncoder \tForce   \tSpeed   \tDispl   \tPower   \tTime    \r\n\r\n0       \t44.586  \t0.000   \t437.237 \t0.000   \t0.000   \t0.000   \t0.000   \r\n1       "
        private string DatiPedanaRowAsFileLine(DatiPedanaRow row)
            => string.Join(
                '\t',
                EightCharacter($"{row.Index}"),
                EightCharacter(row.Load.ToString("F3", CultureInfo.InvariantCulture)),
                EightCharacter(row.Encoder.ToString("F3", CultureInfo.InvariantCulture)),
                EightCharacter(row.Force.ToString("F3", CultureInfo.InvariantCulture)),
                EightCharacter(row.Speed.ToString("F3", CultureInfo.InvariantCulture)),
                EightCharacter(row.Displ.ToString("F3", CultureInfo.InvariantCulture)),
                EightCharacter(row.Power.ToString("F3", CultureInfo.InvariantCulture)),
                EightCharacter(row.Time.ToString("F3", CultureInfo.InvariantCulture))
            );

        private string EightCharacter(string text)
        {
            if(text.Length < 8)
                return string.Concat(text, new String(' ', 8 - text.Length));
            return text;
        }

        public void CreateCopy(string sourceFile, string destinationFile, DatiPedanaRow[] PlatformA, DatiPedanaRow[] PlatofrmB)
        {
            if (File.Exists(destinationFile))
                File.Delete(destinationFile);

            var sbHeaderPlatformA = new StringBuilder();
            var sbHeaderPlatformB = new StringBuilder();
            sbHeaderPlatformB.AppendLine(Environment.NewLine);
            sbHeaderPlatformB.AppendLine(Environment.NewLine);

            bool headerPlatformACompleted = false;
            bool headerPlatformBStarted = false;
            bool headerPlatformBCompleted = false;

            foreach (var line in File.ReadAllLines(sourceFile))
            {
                if(!headerPlatformACompleted)
                {
                    sbHeaderPlatformA.AppendLine(line);
                    headerPlatformACompleted = line.Contains("idx", StringComparison.InvariantCultureIgnoreCase);
                    continue;
                }

                if (!headerPlatformBStarted)
                {
                    headerPlatformBStarted= line.Contains("Platform B", StringComparison.InvariantCultureIgnoreCase);
                    if (headerPlatformBStarted)
                        sbHeaderPlatformB.AppendLine(line);
                }
                else {
                    sbHeaderPlatformB.AppendLine(line);
                    headerPlatformBCompleted = line.Contains("idx", StringComparison.InvariantCultureIgnoreCase);
                    if (headerPlatformBCompleted)
                        break;
                }
            }

            sbHeaderPlatformB.AppendLine(String.Empty);
            sbHeaderPlatformB.AppendLine(string.Empty);

            foreach (var itemA in PlatformA)
                sbHeaderPlatformA.AppendLine(DatiPedanaRowAsFileLine(itemA));
            foreach (var itemB in PlatofrmB)
                sbHeaderPlatformB.AppendLine(DatiPedanaRowAsFileLine(itemB));

            File.WriteAllText(destinationFile, sbHeaderPlatformA.Append(sbHeaderPlatformB).ToString());
        }

        public IEnumerable<DatiPedanaRow> ReadPlatformA(string filePath)
        {
            foreach (var row in File.ReadAllLines(filePath).Skip(25).TakeWhile(r => !string.IsNullOrWhiteSpace(r) && !r.Contains("Platform B", StringComparison.InvariantCultureIgnoreCase)))
            {
                var fields = row.Split('\t');

                yield return new DatiPedanaRow
                {
                    Index = int.Parse(fields[0]),
                    Load = GetFloatValue(fields[1]),
                    Encoder = GetFloatValue(fields[2]),
                    Force = GetFloatValue(fields[3]),
                    Speed = GetFloatValue(fields[4]),
                    Displ = GetFloatValue(fields[5]),
                    Power = GetFloatValue(fields[6]),
                    Time = GetFloatValue(fields[7]),
                    TimePrecision = GetFloatValue(fields[7])
                };
            }
        }

        public IEnumerable<DatiPedanaRow> ReadPlatformB(string filePath)
        {
            bool platformBReached = false;
            bool headerReached = false;

            foreach (var row in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(row)) continue;

                if (!platformBReached)
                {
                    platformBReached = row.Contains("Platform B", StringComparison.InvariantCultureIgnoreCase);
                    continue;
                }

                if (!headerReached)
                {
                    headerReached = row.Contains("idx", StringComparison.InvariantCultureIgnoreCase);
                    continue;
                }

                var fields = row.Split("\t");

                yield return new DatiPedanaRow
                {
                    Index = int.Parse(fields[0]),
                    Load = GetFloatValue(fields[1]),
                    Encoder = GetFloatValue(fields[2]),
                    Force = GetFloatValue(fields[3]),
                    Speed = GetFloatValue(fields[4]),
                    Displ = GetFloatValue(fields[5]),
                    Power = GetFloatValue(fields[6]),
                    Time = GetFloatValue(fields[7]),
                    TimePrecision = GetFloatValue(fields[7])
                };
            }
        }

        private float GetFloatValue(string text)
        {
            return float.Parse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture);
        }
    }

    public sealed class DatiPedanaDecimator
    {
        public enum Riduzione { Decimo, Centesimo }

        public IEnumerable<DatiPedanaRow> ReduceTimePrecision(IEnumerable<DatiPedanaRow> items, Riduzione riduzione)
        {
            int newIndex = 0;

            float scalaDiRiduzione = 100;
            if (riduzione == Riduzione.Decimo)
                scalaDiRiduzione = 10;

            var reduced = items
                .Select(i => i with { TimePrecision = (float)(Math.Truncate(i.TimePrecision * scalaDiRiduzione) / scalaDiRiduzione) })
                .ToArray(); ;
            foreach (var group in reduced.GroupBy(i => i.TimePrecision))
            {
                yield return new DatiPedanaRow
                {
                    Index = newIndex,
                    Load = group.Average(i => i.Load),
                    Encoder = group.Average(i => i.Encoder),
                    Force = group.Average(i => i.Force),
                    Speed = group.Average(i => i.Speed),
                    Displ = group.Average(i => i.Displ),
                    Power = group.Average(i => i.Power),
                    Time = group.Key,
                    TimePrecision = group.Key
                };
                newIndex++;
            }
        }
    }

    public record DatiPedanaRow
    {
        public int Index { get; init; }
        public float Load { get; init; }
        public float Encoder { get; init; }
        public float Force { get; init; }
        public float Speed { get; init; }
        public float Displ { get; init; }
        public float Power { get; init; }
        public float Time { get; init; }
        public float TimePrecision { get; set; }
    }
}
