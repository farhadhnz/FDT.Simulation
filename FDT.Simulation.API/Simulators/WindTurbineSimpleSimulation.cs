namespace FDT.Simulation.Simulators
{
    public static class WindTurbineSimpleSimulation
    {
        public static async Task<IEnumerable<Tuple<double, double>>> Simulate(double rotorDiameter)
        {
            // Read wind speed data from CSV file
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string csvFilePath = Path.Combine(baseDirectory, "weather.csv");
            var windSpeeds = await ReadWindSpeedDataFromCSV(csvFilePath, "WindSpeed9am");

            // Calculate swept area of the rotor
            double rotorRadius = rotorDiameter / 2.0;
            double sweptArea = Math.PI * Math.Pow(rotorRadius, 2.0);

            var simulationResults = new List<Tuple<double, double>>();

            // Perform simulation for each time step
            Console.WriteLine("Wind Turbine Performance Simulation");
            Console.WriteLine("----------------------------------");
            for (int i = 0; i < windSpeeds.Length; i++)
            {
                double windSpeed = windSpeeds[i];
                double powerOutput = CalculatePowerOutput(windSpeed, sweptArea);
                Console.WriteLine("Time Step: {0}, Wind Speed: {1} m/s, Power Output: {2} kW", i + 1, windSpeed, powerOutput);
                simulationResults.Add(new Tuple<double, double>(windSpeed, powerOutput));
            }

            return simulationResults;
        }

        private static async Task<double[]> ReadWindSpeedDataFromCSV(string filePath, string columnName)
        {
            List<double> windSpeeds = new List<double>();

            try
            {
                string[] lines = await File.ReadAllLinesAsync(filePath);
                if (lines.Length > 1)
                {
                    string[] headers = lines[0].Split(',');
                    int columnIndex = Array.IndexOf(headers, columnName);

                    if (columnIndex != -1)
                    {
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] fields = lines[i].Split(',');
                            if (fields.Length > columnIndex && double.TryParse(fields[columnIndex], out double windSpeed))
                            {
                                windSpeeds.Add(windSpeed);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Column '{0}' not found in the CSV file.", columnName);
                    }
                }
                else
                {
                    Console.WriteLine("CSV file does not contain data.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading the CSV file: {0}", ex.Message);
            }

            return windSpeeds.ToArray();
        }

        private static double CalculatePowerOutput(double windSpeed, double sweptArea)
        {
            // Simplified power output calculation based on wind speed and power coefficient
            double Cp = CalculatePowerCoefficient(windSpeed);
            return Cp * 0.5 * sweptArea * Math.Pow(windSpeed, 3.0);
        }

        private static double CalculatePowerCoefficient(double windSpeed)
        {
            // Simplified power coefficient calculation based on wind speed
            if (windSpeed < 3.0)
            {
                return 0.0;
            }
            else if (windSpeed >= 3.0 && windSpeed <= 15.0)
            {
                return 0.4;
            }
            else
            {
                return 0.0;
            }
        }
    }
}
