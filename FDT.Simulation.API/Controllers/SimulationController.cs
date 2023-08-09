using FDT.Simulation.API.SyncDataServices;
using FDT.Simulation.Simulators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FDT.Simulation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimulationController : ControllerBase
    {

        private readonly ILogger<SimulationController> _logger;
        private readonly IManagementDataClient managementClient;
        private readonly IMemoryCache cache;

        public SimulationController(ILogger<SimulationController> logger, 
            IManagementDataClient managementClient,
            IMemoryCache cache)
        {
            _logger = logger;
            this.managementClient = managementClient;
            this.cache = cache;
        }

        [HttpPost("{digitalTwinId}/{simulatorId}")]
        public async Task<IActionResult> RunSimulationForDigitalTwin(int digitalTwinId, int simulatorId)
        {
            try
            {
                var digitalTwin = await managementClient.GetDigitalTwinById(digitalTwinId);
                var rotorDiameter = double.Parse(digitalTwin.Properties
                                        .FirstOrDefault(p => p.PropertyName == "RotorDiameter").PropertyValue);

                // TODO: Load the simulator with the specified ID from your data source

                var simulatedData = await WindTurbineSimpleSimulation.Simulate(rotorDiameter);
                var windSpeeds = simulatedData.Select(t => t.Item1).ToArray();
                var powerOutputs = simulatedData.Select(t => t.Item2).ToArray();



                return Ok(new { message = "Simulation started successfully", windSpeeds, powerOutputs });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while starting the simulation.");
            }
        }

        [HttpPost("continuous/start/{digitalTwinId}/{simulatorId}")]
        public async Task<IActionResult> StartContinuous(int digitalTwinId, int simulatorId)
        {
            try
            {
                var digitalTwin = await managementClient.GetDigitalTwinById(digitalTwinId);
                var rotorDiameter = double.Parse(digitalTwin.Properties
                                        .FirstOrDefault(p => p.PropertyName == "RotorDiameter").PropertyValue);

                // TODO: Load the simulator with the specified ID from your data source

                var simulatedData = await WindTurbineSimpleSimulation.Simulate(rotorDiameter);
                var simulationId = Guid.NewGuid();

                cache.Set(simulationId.ToString(), simulatedData);

                return Ok(new { message = "Simulation started successfully", simulationId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while starting the simulation.");
            }
        }


        [HttpGet("continuous/data/{simulationId}/{index}")]
        public IActionResult GetContinuousData(Guid simulationId, int index)
        {
            if (!cache.TryGetValue(simulationId.ToString(), out var simulatedDataObj) || simulatedDataObj == null)
            {
                return BadRequest("Invalid simulation ID or missing simulated data.");
            }

            if (!(simulatedDataObj is IEnumerable<Tuple<double, double>> simulatedData))
            {
                return BadRequest("Invalid simulated data type.");
            }

            if (index < 0 || index >= simulatedData.Count())
            {
                return BadRequest("Invalid index.");
            }

            var simData = simulatedData.ToList()[index];
            return Ok(new { windSpeed = simData.Item1, powerOutput = simData.Item2 });
        }
    }
}