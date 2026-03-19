# AspNetBenchmark
AspNetBenchmark is a benchmark developed by MSFT ASPNET team, based on open source benchmark TechEmpower.  
This workload supports both single-machine and multi-role (client-server) configurations. The server part runs as an ASP.NET service using Kestrel. The client uses either Bombardier or Wrk to send HTTP requests to the server.

* [AspNetBenchmarks Github](https://github.com/aspnet/benchmarks)
* [Bombardier Github](https://github.com/codesenberg/bombardier)
* [Bombardier Release](https://github.com/codesenberg/bombardier/releases/tag/v1.2.5)
* [Wrk Github](https://github.com/wg/wrk)

## Workload Metrics
The following metrics are examples of those captured by the Virtual Client when running the AspNetBenchmark workload.

[Bombardier output example](https://github.com/codesenberg/bombardier#examples)

The following metrics are examples of those captured during the operations of the AspNetBench workload.

| Name                     | Example            | Unit        | Description                            |
|--------------------------|--------------------|-------------|----------------------------------------|
| Latency Max               | 178703                | microsecond | ASP.NET Web Request latency (max) |
| Latency Average           | 8270.807963429836  | microsecond | ASP.NET Web Request latency (avg) |
| Latency Stddev           | 6124.356473307014  | microsecond | ASP.NET Web Request latency (standard deviation) |
| Latency P50               | 6058                | microsecond | ASP.NET Web Request latency (P50) |
| Latency P75                  | 10913                | microsecond | ASP.NET Web Request latency (P75) |
| Latency P90                  | 17949                | microsecond | ASP.NET Web Request latency (P90) |
| Latency P95                  | 23318                | microsecond | ASP.NET Web Request latency (P95) |
| Latency P99               | 35856                | microsecond | ASP.NET Web Request latency (P99) |
| RequestPerSecond Max     | 61221.282458945345 | Reqs/sec      | ASP.NET Web Request per second (max) |
| RequestPerSecond Average | 31211.609987720527 | Reqs/sec    | ASP.NET Web Request per second (avg) |
| RequestPerSecond Stddev  | 6446.822354105378  | Reqs/sec    | ASP.NET Web Request per second (standard deviation) |
| RequestPerSecond P50     | 31049.462844       | Reqs/sec    | ASP.NET Web Request per second (P50) |
| RequestPerSecond P75     | 35597.436614       | Reqs/sec    | ASP.NET Web Request per second (P75) |
| RequestPerSecond P90     | 39826.205746       | Reqs/sec    | ASP.NET Web Request per second (P90) |
| RequestPerSecond P95     | 41662.542962       | Reqs/sec    | ASP.NET Web Request per second (P95) |
| RequestPerSecond P99     | 48600.556224       | Reqs/sec    | ASP.NET Web Request per second (P99) |

## CPU Core Affinity
Core affinity binds a process to run exclusively on specified CPU cores. This eliminates OS scheduler interference
where the server and client processes compete for the same cores on single-VM deployments. On a 16+ core machine,
pinning the server to one half and the client to the other produces results comparable to a two-machine setup.

### Linux
Uses `numactl -C <cores>` to wrap the process command. numactl must be installed on the system.

### Windows
Uses the Windows processor affinity bitmask API applied to the process after it starts.

### Core Specification Format

| Format | Example | Meaning |
|--------|---------|---------|
| Range | `0-7` | Cores 0 through 7 |
| List | `0,2,4,6` | Specific cores |
| Mixed | `0-3,8-11` | Cores 0-3 and 8-11 |

### Choosing Core Assignments
- Ensure server and client core sets do **not overlap**.
- Use `lscpu` (Linux) or Task Manager (Windows) to check available cores and NUMA topology.
- For NUMA-aware pinning, keep each workload within a single NUMA node.

## Packaging and Setup
The following section covers how to create the custom Virtual Client dependency packages required to execute the workload and toolset(s).

### Architecture

The workload now uses a **decoupled architecture** with separate executors:

**Server Executor:**
- `AspNetServerExecutor` - Builds and runs the ASP.NET Benchmarks application

**Client Executors:**
- `BombardierExecutor` - HTTP load testing client (cross-platform)
- `WrkExecutor` - HTTP load testing client (Linux only)

### Single Machine Setup

For single-machine testing, both server and client run on the same system using multi-role pattern:

1. VC installs .NET SDK
2. VC clones AspNetBenchmarks GitHub repo
3. VC builds the Benchmarks project using .NET SDK
4. `AspNetServerExecutor` starts the Kestrel server (Server role)
5. `BombardierExecutor` or `WrkExecutor` sends requests (Client role)

**Server Command:**
```bash
dotnet Benchmarks.dll --nonInteractive true --scenarios json --urls http://*:9876 \
  --server Kestrel --kestrelTransport Sockets --protocol http \
  --header "Accept: application/json,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7" \
  --header "Connection: keep-alive"
```

**Client Command (Bombardier):**
```bash
bombardier --duration 15s --connections 256 --timeout 10s --fasthttp --insecure \
  -l http://localhost:9876/json --print r --format json
```

**Client Command (Wrk):**
```bash
wrk -t 256 -c 256 -d 15s --timeout 10s http://localhost:9876/json \
  --header "Accept: application/json,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7"
```

### Multi-Machine Setup

For distributed testing, server and client run on separate machines:

1. **Server Machine**: Runs `AspNetServerExecutor` with `Role: Server`
2. **Client Machine(s)**: Run `BombardierExecutor` or `WrkExecutor` with `Role: Client`
3. Client automatically discovers server IP via Virtual Client API

**Benefits:**
- Eliminates resource contention between server and client
- Better represents real-world scenarios
- Can scale to multiple client machines
- Mix and match different client tools
