# AspNetBench Workload Profiles
The following profiles run customer-representative or benchmarking scenarios using the AspNetBench workload.

* [Workload Details](./aspnetbench.md)  

## PERF-ASPNETBENCH.json
Runs the AspNetBench benchmark workload using Bombardier client to assess the performance of an ASP.NET Server. This profile uses a **multi-role architecture** with separate server and client executors running on the same machine.

**Executors Used:**
- `AspNetServerExecutor` (Server role) - Builds and runs ASP.NET Benchmarks
- `BombardierExecutor` (Client role) - Sends HTTP load to the server

* [Workload Profile](https://github.com/microsoft/VirtualClient/blob/main/src/VirtualClient/VirtualClient.Main/profiles/PERF-ASPNETBENCH.json) 

* **Dependencies**  
  The dependencies defined in the 'Dependencies' section of the profile itself are required in order to run the workload operations effectively.
  * Internet connection.
  * .NET SDK
  * AspNetBenchmarks repository
  * Bombardier package

  Additional information on components that exist within the 'Dependencies' section of the profile can be found in the following locations:
  * [Installing Dependencies](https://microsoft.github.io/VirtualClient/docs/category/dependencies/)

  | Parameter                 | Purpose                                                           | Default Value |
  |---------------------------|-------------------------------------------------------------------|---------------|
  | DotNetVersion             | Optional. The version of the [.NET SDK to download and install](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks). | 8.0.204       |
  | TargetFramework           | Optional. The [.NET target framework](https://learn.microsoft.com/en-us/dotnet/standard/frameworks) to run (e.g. net8.0, net9.0). | net8.0        |
  | ServerPort                | Optional. The port number for the ASP.NET server. | 9876        |

* **Profile Runtimes**  
  See the 'Metadata' section of the profile for estimated runtimes. These timings represent the length of time required to run a single round of profile 
  The following section provides a few basic examples of how to use the workload profile.

  ``` bash
  # Execute the workload profile (single machine with multi-role pattern)
  VirtualClient.exe --profile=PERF-ASPNETBENCH.json --system=Demo --timeout=1440 --packageStore="{BlobConnectionString|SAS Uri}"

  # Override the profile default parameters to use a different .NET SDK version
  VirtualClient.exe --profile=PERF-ASPNETBENCH.json --system=Demo --timeout=1440 --packageStore="{BlobConnectionString|SAS Uri}" --parameters="DotNetVersion=9.0.101,TargetFramework=net9.0"
  
  # Run on distributed systems (separate server and client machines)
  # On Server machine:
  VirtualClient.exe --profile=PERF-ASPNETBENCH.json --system=Demo --timeout=1440 --clientId=Server --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  
  # On Client machine:
  VirtualClient.exe --profile=PERF-ASPNETBENCH.json --system=Demo --timeout=1440 --clientId=Client --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  ```

## PERF-ASPNETBENCH-MULTI.json
Runs the AspNetBench benchmark workload using Wrk client in a multi-role configuration. This profile demonstrates the flexibility of using different HTTP clients with the same ASP.NET server.

**Executors Used:**
- `AspNetServerExecutor` (Server role) - Builds and runs ASP.NET Benchmarks
- `WrkExecutor` (Client role) - Sends HTTP load using Wrk (with warm-up and benchmark phases)

* [Workload Profile](https://github.com/microsoft/VirtualClient/blob/main/src/VirtualClient/VirtualClient.Main/profiles/PERF-ASPNETBENCH-MULTI.json) 

* **Supported Platform/Architectures**
  * linux-x64
  * linux-arm64

* **Profile Parameters**  

  | Parameter                           | Purpose                                                           | Default Value |
  |-------------------------------------|-------------------------------------------------------------------|---------------|
  | DotNetVersion                       | Optional. The version of the .NET SDK. | 8.0.204       |
  | TargetFramework                     | Optional. The .NET target framework. | net8.0        |
  | AspNetCoreThreadCount               | Optional. ASPNETCORE_threadCount environment variable. | 1        |
  | DotNetSystemNetSocketsThreadCount   | Optional. DOTNET_SYSTEM_NET_SOCKETS_THREAD_COUNT environment variable. | 1        |

* **Usage Examples**  

  ``` bash
  # Execute on distributed systems (recommended for accurate results)
  # On Server machine:
  VirtualClient.exe --profile=PERF-ASPNETBENCH-MULTI.json --system=Demo --timeout=1440 --clientId=Server --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  
  # On Client machine:
  VirtualClient.exe --profile=PERF-ASPNETBENCH-MULTI.json --system=Demo --timeout=1440 --clientId=Client --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  ```

## PERF-ASPNET-TEJSON-WRK.json
Runs ASP.NET JSON serialization benchmarks using Wrk with configurable thread and connection counts.

**Executors Used:**
- `AspNetServerExecutor` (Server role)
- `WrkExecutor` (Client role) - With warm-up phase

* [Workload Profile](https://github.com/microsoft/VirtualClient/blob/main/src/VirtualClient/VirtualClient.Main/profiles/PERF-ASPNET-TEJSON-WRK.json) 

* **Supported Platform/Architectures**
  * linux-x64
  * linux-arm64
  * win-x64
  * win-arm64

* **Profile Parameters**  

  | Parameter                 | Purpose                                                           | Default Value |
  |---------------------------|-------------------------------------------------------------------|---------------|
  | DotNetVersion             | Optional. The version of the .NET SDK. | 9.0.101       |
  | TargetFramework           | Optional. The .NET target framework. | net9.0        |
  | ServerPort                | Optional. The port number for the ASP.NET server. | 9876        |
  | TestDuration              | Optional. Duration of each test run. | 00:00:15        |
  | Timeout                   | Optional. Timeout for operations. | 00:10:00        |
  | EmitLatencySpectrum       | Optional. Whether to emit detailed latency metrics. | false        |

* **Usage Examples**  

  ``` bash
  # Execute on distributed systems
  # On Server machine:
  VirtualClient.exe --profile=PERF-ASPNET-TEJSON-WRK.json --system=Demo --timeout=1440 --clientId=Server --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  
  # On Client machine:
  VirtualClient.exe --profile=PERF-ASPNET-TEJSON-WRK.json --system=Demo --timeout=1440 --clientId=Client --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  ```

## PERF-ASPNETBENCH-AFFINITY.json
Runs ASP.NET JSON serialization benchmark with CPU core affinity pinning. Server and client processes
are isolated to separate core sets, enabling reproducible single-VM benchmarking.

**Executors Used:**
- `AspNetServerExecutor` (Server role) - Builds and runs ASP.NET Benchmarks pinned to ServerCoreAffinity
- `BombardierExecutor` (Client role) - Sends HTTP load pinned to ClientCoreAffinity

* [Workload Profile](https://github.com/microsoft/VirtualClient/blob/main/src/VirtualClient/VirtualClient.Main/profiles/PERF-ASPNETBENCH-AFFINITY.json)

* **Supported Platform/Architectures**
  * linux-x64
  * linux-arm64
  * win-x64
  * win-arm64

* **Supports Disconnected Scenarios**
  * Yes. The workload packages can be pre-installed on the system.

* **Dependencies**
  The dependencies defined in the 'Dependencies' section of the profile itself are required in order to run the workload operations effectively.
  * .NET SDK
  * AspNetBenchmarks repository
  * Bombardier package

  Additional information on components that exist within the 'Dependencies' section of the profile can be found in the following locations:
  * [Installing Dependencies](https://microsoft.github.io/VirtualClient/docs/category/dependencies/)

* **Profile Parameters**
  The following parameters can be optionally supplied on the command line to modify the behaviors of the workload.

  | Parameter                 | Purpose                                                           | Default Value |
  |---------------------------|-------------------------------------------------------------------|---------------|
  | DotNetVersion             | Optional. The version of the [.NET SDK to download and install](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks). | 8.0.204       |
  | TargetFramework           | Optional. The [.NET target framework](https://learn.microsoft.com/en-us/dotnet/standard/frameworks) to run (e.g. net8.0, net9.0). | net8.0        |
  | ServerPort                | Optional. The port number for the ASP.NET server. | 9876        |
  | ServerCoreAffinity        | Optional. CPU cores to bind the server process to (e.g., "0-7"). | 0-7           |
  | ClientCoreAffinity        | Optional. CPU cores to bind the client process to (e.g., "8-15"). | 8-15          |

* **Profile Runtimes**
  See the 'Metadata' section of the profile for estimated runtimes. These timings represent the length of time required to run a single round of profile
  actions. These timings can be used to determine minimum required runtimes for the Virtual Client in order to get results.

* **Usage Examples**
  The following section provides a few basic examples of how to use the workload profile.

  ``` bash
  # Single VM with default core assignments (0-7 server, 8-15 client)
  VirtualClient.exe --profile=PERF-ASPNETBENCH-AFFINITY.json --system=Demo --timeout=60 --packageStore="{BlobConnectionString|SAS Uri}"

  # Override core assignments for a 32-core machine
  VirtualClient.exe --profile=PERF-ASPNETBENCH-AFFINITY.json --system=Demo --timeout=60 --packageStore="{BlobConnectionString|SAS Uri}" --parameters="ServerCoreAffinity=0-15,,,ClientCoreAffinity=16-31"

  # Run on distributed systems
  # On Server machine:
  VirtualClient.exe --profile=PERF-ASPNETBENCH-AFFINITY.json --system=Demo --timeout=60 --clientId=Server --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"

  # On Client machine:
  VirtualClient.exe --profile=PERF-ASPNETBENCH-AFFINITY.json --system=Demo --timeout=60 --clientId=Client --layoutPath=layout.json --packageStore="{BlobConnectionString|SAS Uri}"
  ```
